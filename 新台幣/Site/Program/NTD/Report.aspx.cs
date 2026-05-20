using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// 新臺幣定期存款專案利率申請 - 報表頁。
/// 對應「需求確認書 §6 報表規格」之彙整表與稽催表。
/// </summary>
/// <remarks>
/// 彙整表：以分行彙總申請件數、金額、結案/處理中/退回件數。
/// 稽催表：列出所有非結案 (Status &lt;&gt; 9) 的滯留案件，含滯留天數。
/// </remarks>
public partial class NtdReport : Page
{
    protected UserInfo Me { get { return UserInfo.Current; } }

    private const string ReportTypeAudit = "AUDIT";

    protected void Page_Load(object sender, EventArgs e)
    {
        LblUser.Text = string.Format("{0} {1} ({2} {3}) [職等 {4}]",
            Me.EmpID, Me.EmpName, Me.BranchCode, Me.BranchName, Me.TitleLevel);

        if (IsPostBack) return;

        BindBranchDropDown();
        TxtBaseDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
        RunReport();
    }

    protected string NavUrl(string relativeUrl) { return Me.AppendSid(relativeUrl); }

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

    protected void DdlReportType_SelectedIndexChanged(object sender, EventArgs e) { RunReport(); }
    protected void BtnRun_Click(object sender, EventArgs e)                       { RunReport(); }

    private void RunReport()
    {
        if (DdlReportType.SelectedValue == ReportTypeAudit)
        {
            LitReportTitle.Text = "稽催表 (滯留中案件)";
            PnlSummary.Visible  = false;
            PnlAudit.Visible    = true;

            var table = BuildAuditTable();
            LblResultCount.Text = table.Rows.Count.ToString("N0");
            GvAudit.DataSource  = table;
            GvAudit.DataBind();
        }
        else
        {
            LitReportTitle.Text = "彙整表";
            PnlSummary.Visible  = true;
            PnlAudit.Visible    = false;

            var table = BuildSummaryTable();
            LblResultCount.Text  = table.Rows.Count.ToString("N0");
            GvSummary.DataSource = table;
            GvSummary.DataBind();
        }
    }

    /// <summary>
    /// 彙整表 SQL：以 BranchOnly 為左主表 LEFT JOIN，
    /// 讓沒有任何申請的分行也能列出 0 件。
    /// </summary>
    private DataTable BuildSummaryTable()
    {
        var sqlBuilder = new StringBuilder(@"
            SELECT  b.BranchCode, b.BranchName,
                    COUNT(m.ApplyNo)                                       AS ApplyCount,
                    ISNULL(SUM(m.TotalAmount), 0)                          AS TotalAmount,
                    SUM(CASE WHEN m.Status = 9          THEN 1 ELSE 0 END) AS DoneCount,
                    SUM(CASE WHEN m.Status IN (1,2,4,6) THEN 1 ELSE 0 END) AS PendingCount,
                    SUM(CASE WHEN m.Status = 0          THEN 1 ELSE 0 END) AS RejectCount
            FROM    dbo.BranchOnly  b
            LEFT JOIN dbo.NTD_Main  m
                   ON m.BranchCode = b.BranchCode
                  AND m.ApplyDate <= @BaseDate ");

        var parameters = new List<SqlParameter>();
        DateTime baseDate;
        if (!DateTime.TryParse(TxtBaseDate.Text, out baseDate)) baseDate = DateTime.Today;
        parameters.Add(P("@BaseDate", baseDate));

        int filterStatus;
        if (int.TryParse(DdlStatus.SelectedValue, out filterStatus) && filterStatus >= 0)
        {
            // 把狀態條件放在 JOIN 的 ON 上而非 WHERE，這樣沒符合的分行仍會以 0 顯示
            sqlBuilder.Append(" AND m.Status = @Status ");
            parameters.Add(P("@Status", filterStatus));
        }

        sqlBuilder.Append(" WHERE 1 = 1 ");
        if (!string.IsNullOrEmpty(DdlBranch.SelectedValue))
        {
            sqlBuilder.Append(" AND b.BranchCode = @Branch ");
            parameters.Add(P("@Branch", DdlBranch.SelectedValue));
        }
        sqlBuilder.Append(" GROUP BY b.BranchCode, b.BranchName ORDER BY b.BranchCode ");

        return Query(sqlBuilder.ToString(), parameters.ToArray());
    }

    /// <summary>
    /// 稽催表 SQL：列出所有非結案案件，
    /// 滯留天數 = 基準日 - (UpdateTime 或 CreateTime)。
    /// </summary>
    private DataTable BuildAuditTable()
    {
        var sqlBuilder = new StringBuilder(@"
            SELECT  m.ApplyNo, m.ApplyDate, m.BranchCode,
                    ISNULL(b.BranchName, '')           AS BranchName,
                    m.CustomerName, m.TotalAmount, m.Status, m.CreateUser,
                    ISNULL(e.EmpName, m.CreateUser)    AS CreateUserName,
                    CASE WHEN m.UpdateTime IS NULL
                         THEN DATEDIFF(d, m.CreateTime, @BaseDate)
                         ELSE DATEDIFF(d, m.UpdateTime, @BaseDate) END AS StayDays
            FROM    dbo.NTD_Main    m
            LEFT JOIN dbo.BranchOnly b ON b.BranchCode = m.BranchCode
            LEFT JOIN dbo.EMPLOYEE   e ON e.EmpID      = m.CreateUser
            WHERE   m.Status <> 9
              AND   m.ApplyDate <= @BaseDate ");

        var parameters = new List<SqlParameter>();
        DateTime baseDate;
        if (!DateTime.TryParse(TxtBaseDate.Text, out baseDate)) baseDate = DateTime.Today;
        parameters.Add(P("@BaseDate", baseDate));

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

        sqlBuilder.Append(" ORDER BY StayDays DESC, m.ApplyNo ");
        return Query(sqlBuilder.ToString(), parameters.ToArray());
    }

    /// <summary>
    /// 匯出當前報表為 Excel (Tab 分隔 + UTF-8 BOM)。
    /// 依報表類型寫不同欄位。
    /// </summary>
    protected void BtnExport_Click(object sender, EventArgs e)
    {
        string reportType = DdlReportType.SelectedValue;
        var builder = new StringBuilder();

        if (reportType == ReportTypeAudit)
        {
            var table = BuildAuditTable();
            builder.AppendLine("申請單號\t申請日期\t單位代號\t單位名稱\t客戶名稱\t申請金額\t目前狀態\t滯留天數\t建單人");
            foreach (DataRow row in table.Rows)
            {
                builder.AppendFormat("{0}\t{1:yyyy-MM-dd}\t{2}\t{3}\t{4}\t{5:N0}\t{6}\t{7}\t{8}\r\n",
                    row["ApplyNo"], Convert.ToDateTime(row["ApplyDate"]),
                    row["BranchCode"], row["BranchName"], row["CustomerName"],
                    row["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["TotalAmount"]),
                    StatusName(Convert.ToInt32(row["Status"])),
                    row["StayDays"], row["CreateUserName"]);
            }
        }
        else
        {
            var table = BuildSummaryTable();
            builder.AppendLine("單位代號\t單位名稱\t申請件數\t申請總金額\t結案件數\t處理中\t退回");
            foreach (DataRow row in table.Rows)
            {
                builder.AppendFormat("{0}\t{1}\t{2:N0}\t{3:N0}\t{4:N0}\t{5:N0}\t{6:N0}\r\n",
                    row["BranchCode"], row["BranchName"],
                    Convert.ToInt32(row["ApplyCount"]),
                    Convert.ToDecimal(row["TotalAmount"]),
                    Convert.ToInt32(row["DoneCount"]),
                    Convert.ToInt32(row["PendingCount"]),
                    Convert.ToInt32(row["RejectCount"]));
            }
        }

        Response.Clear();
        Response.Charset         = "utf-8";
        Response.ContentEncoding = Encoding.UTF8;
        Response.ContentType     = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition",
            "attachment;filename=NTD_" + reportType + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls");
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
