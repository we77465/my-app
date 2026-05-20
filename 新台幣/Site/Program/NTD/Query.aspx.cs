using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// 新臺幣定期存款專案利率申請 - 查詢頁。
/// 提供條件查詢、分頁、Excel 匯出。
/// </summary>
public partial class NtdQuery : Page
{
    protected UserInfo Me { get { return UserInfo.Current; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        LblUser.Text = string.Format("{0} {1} ({2} {3}) [職等 {4}]",
            Me.EmpID, Me.EmpName, Me.BranchCode, Me.BranchName, Me.TitleLevel);

        if (IsPostBack) return;

        BindBranchDropDown();
        TxtDateFrom.Text = DateTime.Today.AddMonths(-3).ToString("yyyy-MM-dd");
        TxtDateTo.Text   = DateTime.Today.ToString("yyyy-MM-dd");
        DoQuery();
    }

    protected string NavUrl(string relativeUrl) { return Me.AppendSid(relativeUrl); }

    /// <summary>
    /// 載入單位下拉。分行人員預設只看自己分行；總行人員預設全部。
    /// 對應 §2 權限設定。
    /// </summary>
    private void BindBranchDropDown()
    {
        DdlBranch.Items.Clear();
        DdlBranch.Items.Add(new ListItem("全部", ""));

        var branches = Query("SELECT BranchCode, BranchName FROM dbo.BranchOnly ORDER BY BranchCode");
        foreach (DataRow row in branches.Rows)
        {
            DdlBranch.Items.Add(new ListItem(
                row["BranchCode"] + " " + row["BranchName"],
                Convert.ToString(row["BranchCode"])));
        }

        if (!Me.IsHeadOffice) DdlBranch.SelectedValue = Me.BranchCode;
    }

    protected void BtnQuery_Click(object sender, EventArgs e)
    {
        GvResult.PageIndex = 0;
        DoQuery();
    }

    protected void BtnReset_Click(object sender, EventArgs e)
    {
        TxtDateFrom.Text          = "";
        TxtDateTo.Text            = "";
        TxtApplyNoKeyword.Text    = "";
        TxtCustomerKeyword.Text   = "";
        DdlStatus.SelectedValue   = "-1";
        DdlBranch.SelectedValue   = Me.IsHeadOffice ? "" : Me.BranchCode;
        GvResult.PageIndex = 0;
        DoQuery();
    }

    protected void GvResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvResult.PageIndex = e.NewPageIndex;
        DoQuery();
    }

    /// <summary>
    /// 依畫面條件組查詢 SQL；分行人員強制限制只能看本分行 + 自己的單。
    /// </summary>
    private DataTable BuildResultTable()
    {
        var sqlBuilder = new StringBuilder(@"
            SELECT  m.ApplyNo, m.ApplyDate, m.BranchCode,
                    ISNULL(b.BranchName, '')        AS BranchName,
                    m.CustomerName, m.TotalAmount, m.Status, m.RateTypeCode,
                    m.CreateUser,
                    ISNULL(e.EmpName, m.CreateUser) AS CreateUserName
            FROM    dbo.NTD_Main    m
            LEFT JOIN dbo.BranchOnly b ON b.BranchCode = m.BranchCode
            LEFT JOIN dbo.EMPLOYEE   e ON e.EmpID      = m.CreateUser
            WHERE   1 = 1 ");
        var parameters = new List<SqlParameter>();

        // §2 權限設定：分行人員只能看本分行 + 自己的單；總行人員不設限
        if (!Me.IsHeadOffice)
        {
            sqlBuilder.Append(" AND (m.BranchCode = @MyBranch OR m.CreateUser = @MyEmpID) ");
            parameters.Add(P("@MyBranch", Me.BranchCode));
            parameters.Add(P("@MyEmpID",  Me.EmpID));
        }

        DateTime dateFrom, dateTo;
        if (DateTime.TryParse(TxtDateFrom.Text, out dateFrom))
        {
            sqlBuilder.Append(" AND m.ApplyDate >= @DateFrom ");
            parameters.Add(P("@DateFrom", dateFrom));
        }
        if (DateTime.TryParse(TxtDateTo.Text, out dateTo))
        {
            sqlBuilder.Append(" AND m.ApplyDate < DATEADD(d, 1, @DateTo) ");
            parameters.Add(P("@DateTo", dateTo));
        }

        if (!string.IsNullOrEmpty(DdlBranch.SelectedValue))
        {
            sqlBuilder.Append(" AND m.BranchCode = @Branch ");
            parameters.Add(P("@Branch", DdlBranch.SelectedValue));
        }

        int filterStatus;
        if (int.TryParse(DdlStatus.SelectedValue, out filterStatus) && filterStatus >= 0)
        {
            sqlBuilder.Append(" AND m.Status = @Status ");
            parameters.Add(P("@Status", filterStatus));
        }

        if (!string.IsNullOrWhiteSpace(TxtCustomerKeyword.Text))
        {
            sqlBuilder.Append(" AND m.CustomerName LIKE @CustomerKeyword ");
            parameters.Add(P("@CustomerKeyword", "%" + TxtCustomerKeyword.Text.Trim() + "%"));
        }

        if (!string.IsNullOrWhiteSpace(TxtApplyNoKeyword.Text))
        {
            sqlBuilder.Append(" AND m.ApplyNo LIKE @ApplyNoKeyword ");
            parameters.Add(P("@ApplyNoKeyword", "%" + TxtApplyNoKeyword.Text.Trim() + "%"));
        }

        sqlBuilder.Append(" ORDER BY m.ApplyDate DESC, m.ApplyNo DESC ");
        return Query(sqlBuilder.ToString(), parameters.ToArray());
    }

    private void DoQuery()
    {
        var resultTable = BuildResultTable();
        LblResultCount.Text = resultTable.Rows.Count.ToString("N0");
        GvResult.DataSource = resultTable;
        GvResult.DataBind();
    }

    /// <summary>
    /// 匯出查詢結果為 Excel (Tab 分隔 + UTF-8 BOM，Excel 可直接開啟)。
    /// 對應 §3 功能畫面 - 匯出 EXCEL。
    /// </summary>
    protected void BtnExport_Click(object sender, EventArgs e)
    {
        var resultTable = BuildResultTable();
        var builder = new StringBuilder();
        builder.AppendLine("申請單號\t申請日期\t單位代號\t單位名稱\t客戶名稱\t申請金額\t流程狀態\t利率別\t建單人");
        foreach (DataRow row in resultTable.Rows)
        {
            builder.AppendFormat("{0}\t{1:yyyy-MM-dd}\t{2}\t{3}\t{4}\t{5:N0}\t{6}\t{7}\t{8}\r\n",
                row["ApplyNo"],
                Convert.ToDateTime(row["ApplyDate"]),
                row["BranchCode"],
                row["BranchName"],
                row["CustomerName"],
                row["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["TotalAmount"]),
                StatusName(Convert.ToInt32(row["Status"])),
                row["RateTypeCode"],
                row["CreateUserName"]);
        }

        Response.Clear();
        Response.Charset         = "utf-8";
        Response.ContentEncoding = Encoding.UTF8;
        Response.ContentType     = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition",
            "attachment;filename=NTD_Query_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls");
        Response.BinaryWrite(new byte[] { 0xEF, 0xBB, 0xBF });
        Response.Write(builder.ToString());
        Response.End();
    }

    // ===== 資料存取 / 流程狀態 helper (本頁專用) =====

    /// <summary>chb_iom 連線字串 (Web.config)。</summary>
    private static string ConnStr
    {
        get { return ConfigurationManager.ConnectionStrings["chb_iom"].ConnectionString; }
    }

    /// <summary>執行 SELECT，回傳 DataTable。</summary>
    private static DataTable Query(string sql, params SqlParameter[] parameters)
    {
        using (var conn = new SqlConnection(ConnStr))
        using (var cmd  = new SqlCommand(sql, conn))
        {
            if (parameters != null) cmd.Parameters.AddRange(parameters);
            conn.Open();
            using (var adapter = new SqlDataAdapter(cmd))
            {
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }
    }

    /// <summary>建立 SqlParameter；null 自動轉 DBNull。</summary>
    private static SqlParameter P(string name, object value)
    {
        return new SqlParameter(name, value ?? DBNull.Value);
    }

    /// <summary>簽核流程狀態代碼 → 中文名稱 (對應「需求確認書 §1 簽核流程」)。</summary>
    protected static string StatusName(int status)
    {
        switch (status)
        {
            case 0: return "草稿/退回";
            case 1: return "待襄理放行";
            case 2: return "待經理放行";
            case 4: return "待總行經辦審核";
            case 6: return "待總行科長決行";
            case 9: return "結案";
            default: return "未知(" + status + ")";
        }
    }
}
