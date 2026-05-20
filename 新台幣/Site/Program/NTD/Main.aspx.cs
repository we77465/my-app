using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// 新臺幣定期存款專案利率申請 - 待處理 / 我的單列表。
/// 經辦進此頁新增申請；其他職等亦可在此看到本分行的單。
/// </summary>
public partial class NtdMain : Page
{
    protected UserInfo Me { get { return UserInfo.Current; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        LblUser.Text = string.Format("{0} {1} ({2} {3}) [職等 {4}]",
            Me.EmpID, Me.EmpName, Me.BranchCode, Me.BranchName, Me.TitleLevel);

        if (!IsPostBack) BindApplyList();
    }

    /// <summary>頂端導覽列 URL，附帶 SID (本機測試用)。</summary>
    protected string NavUrl(string relativeUrl)
    {
        return Me.AppendSid(relativeUrl);
    }

    /// <summary>
    /// 列出列表。
    /// 規則：經辦只看自己建的；其他職等看本分行 + 自己建的單。
    /// </summary>
    private void BindApplyList()
    {
        int filterStatus;
        int.TryParse(DdlStatus.SelectedValue, out filterStatus);

        var sqlBuilder = new System.Text.StringBuilder(@"
            SELECT  m.ApplyNo, m.ApplyDate, m.BranchCode,
                    ISNULL(b.BranchName, '')           AS BranchName,
                    m.CustomerName, m.TotalAmount, m.Status, m.CreateUser,
                    ISNULL(e.EmpName, m.CreateUser)    AS CreateUserName
            FROM    dbo.NTD_Main    m
            LEFT JOIN dbo.BranchOnly b ON b.BranchCode = m.BranchCode
            LEFT JOIN dbo.EMPLOYEE   e ON e.EmpID      = m.CreateUser
            WHERE   1 = 1 ");

        var parameters = new List<SqlParameter>();

        // 經辦只看自己；其他職位看本分行 + 自己建的
        if (Me.TitleLevel == 1)
        {
            sqlBuilder.Append(" AND m.CreateUser = @MyEmpID ");
            parameters.Add(P("@MyEmpID", Me.EmpID));
        }
        else
        {
            sqlBuilder.Append(" AND (m.BranchCode = @MyBranch OR m.CreateUser = @MyEmpID) ");
            parameters.Add(P("@MyBranch", Me.BranchCode));
            parameters.Add(P("@MyEmpID",  Me.EmpID));
        }

        if (filterStatus >= 0)
        {
            sqlBuilder.Append(" AND m.Status = @Status ");
            parameters.Add(P("@Status", filterStatus));
        }

        sqlBuilder.Append(" ORDER BY m.ApplyDate DESC, m.ApplyNo DESC ");

        GvApplyList.DataSource = Query(sqlBuilder.ToString(), parameters.ToArray());
        GvApplyList.DataBind();
    }

    /// <summary>此筆是否可編輯/刪除：自己建的 + 狀態為 0 (草稿/退回)。</summary>
    protected bool CanEditRow(object status, object creator)
    {
        int statusValue = Convert.ToInt32(status);
        string creatorEmpID = Convert.ToString(creator);
        return statusValue == 0
            && string.Equals(creatorEmpID, Me.EmpID, StringComparison.OrdinalIgnoreCase);
    }

    protected void DdlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindApplyList();
    }

    protected void BtnNew_Click(object sender, EventArgs e)
    {
        // §2 權限設定：填報角色僅限「經辦」
        if (!Me.CanCreateApply)
        {
            ShowMessage("僅經辦可新增申請。", "err");
            return;
        }
        Response.Redirect(Me.AppendSid("Edit.aspx"), true);
    }

    protected void GvApplyList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string applyNo = Convert.ToString(e.CommandArgument);
        if (string.IsNullOrEmpty(applyNo)) return;

        switch (e.CommandName)
        {
            case "ViewItem":
                Response.Redirect(Me.AppendSid("View.aspx?no=" + applyNo), true);
                break;
            case "EditItem":
                Response.Redirect(Me.AppendSid("Edit.aspx?no=" + applyNo), true);
                break;
            case "DeleteItem":
                DeleteApply(applyNo);
                break;
        }
    }

    /// <summary>
    /// 刪除申請單；僅限自己建的且仍在草稿/退回狀態 (Status = 0)。
    /// 一併刪除明細與簽核紀錄 (用同一個交易確保一致性)。
    /// </summary>
    private void DeleteApply(string applyNo)
    {
        using (var conn = new SqlConnection(ConnStr))
        {
            conn.Open();
            using (var transaction = conn.BeginTransaction())
            {
                using (var cmd = new SqlCommand("DELETE FROM dbo.NTD_Detail WHERE ApplyNo = @ApplyNo", conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@ApplyNo", applyNo);
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = new SqlCommand("DELETE FROM dbo.NTD_Log WHERE ApplyNo = @ApplyNo", conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@ApplyNo", applyNo);
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = new SqlCommand(@"
                    DELETE FROM dbo.NTD_Main
                    WHERE ApplyNo = @ApplyNo AND Status = 0 AND CreateUser = @MyEmpID", conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@ApplyNo", applyNo);
                    cmd.Parameters.AddWithValue("@MyEmpID", Me.EmpID);
                    int affected = cmd.ExecuteNonQuery();
                    transaction.Commit();
                    ShowMessage(affected > 0
                        ? "申請單 " + applyNo + " 已刪除。"
                        : "此單已送出，無法刪除。", affected > 0 ? "ok" : "warn");
                }
            }
        }
        BindApplyList();
    }

    private void ShowMessage(string text, string level)
    {
        LblMessage.Visible  = true;
        LblMessage.CssClass = "alert alert-" + level;
        LblMessage.Text     = Server.HtmlEncode(text);
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
