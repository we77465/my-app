using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// 新臺幣定期存款專案利率申請 - 簽核 / 決行頁。
/// 依登入者職等只顯示其可處理的單，動作分「放行」與「退回」。
/// </summary>
/// <remarks>
/// 簽核流程 (§1)：
///   經辦送出(Status=1) → 襄理放行(→2) → 經理放行(→4) → 總行經辦放行(→6) → 總行科長決行(→9)
/// 退回 (任一關)：Status -> 0 (回經辦)
/// 總行科長決行 (Status=6 -> 9) 強制必填「利率別代號」。
/// </remarks>
public partial class NtdApprove : Page
{
    protected UserInfo Me { get { return UserInfo.Current; } }

    /// <summary>依登入者職等決定可處理的來源狀態 (沒有對應職位回 -1)。</summary>
    private int MyHandlingStatus
    {
        get
        {
            switch (Me.TitleLevel)
            {
                case 2: return 1;   // 襄理 處理 status = 1
                case 3: return 2;   // 經理 處理 status = 2
                case 4: return 4;   // 總行經辦 處理 status = 4
                case 5: return 6;   // 總行科長 處理 status = 6 (決行)
                default: return -1;
            }
        }
    }

    /// <summary>放行後的下一個狀態。</summary>
    private static int NextStatusOnApprove(int fromStatus)
    {
        switch (fromStatus)
        {
            case 1: return 2;
            case 2: return 4;
            case 4: return 6;
            case 6: return 9;   // 決行 -> 結案
            default: return fromStatus;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        LblUser.Text = string.Format("{0} {1} ({2} {3}) [職等 {4}]",
            Me.EmpID, Me.EmpName, Me.BranchCode, Me.BranchName, Me.TitleLevel);

        if (MyHandlingStatus < 0)
        {
            ShowMessage("您的職位不負責簽核作業。", "warn");
            MvApprove.ActiveViewIndex = 0;
            return;
        }

        if (IsPostBack) return;

        string applyNo = Request.QueryString["no"];
        if (string.IsNullOrEmpty(applyNo)) BindPendingList();
        else                               ShowHandleView(applyNo);
    }

    protected string NavUrl(string relativeUrl) { return Me.AppendSid(relativeUrl); }

    protected string GetMyHandlingStatusName()
    {
        return MyHandlingStatus < 0 ? "(無)" : StatusName(MyHandlingStatus);
    }

    /// <summary>列出可處理的單。分行職位 (襄理/經理) 只看本分行；總行職位看全行。</summary>
    private void BindPendingList()
    {
        MvApprove.ActiveViewIndex = 0;

        string sql = @"
            SELECT  m.ApplyNo, m.ApplyDate, m.BranchCode,
                    ISNULL(b.BranchName, '')           AS BranchName,
                    m.CustomerName, m.TotalAmount, m.Status, m.CreateUser,
                    ISNULL(e.EmpName, m.CreateUser)    AS CreateUserName
            FROM    dbo.NTD_Main    m
            LEFT JOIN dbo.BranchOnly b ON b.BranchCode = m.BranchCode
            LEFT JOIN dbo.EMPLOYEE   e ON e.EmpID      = m.CreateUser
            WHERE   m.Status = @Status ";

        var parameters = new List<SqlParameter> { P("@Status", MyHandlingStatus) };
        if (!Me.IsHeadOffice)
        {
            sql += " AND m.BranchCode = @MyBranch ";
            parameters.Add(P("@MyBranch", Me.BranchCode));
        }
        sql += " ORDER BY m.ApplyDate, m.ApplyNo ";

        GvPendingList.DataSource = Query(sql, parameters.ToArray());
        GvPendingList.DataBind();
    }

    /// <summary>切換到處理頁；同時驗證該單仍在我可處理範圍。</summary>
    private void ShowHandleView(string applyNo)
    {
        var table = Query(
            "SELECT Status, BranchCode FROM dbo.NTD_Main WHERE ApplyNo = @ApplyNo",
            P("@ApplyNo", applyNo));

        if (table.Rows.Count == 0)
        {
            ShowMessage("申請單不存在：" + applyNo, "err");
            BindPendingList();
            return;
        }

        int    currentStatus = Convert.ToInt32(table.Rows[0]["Status"]);
        string branchCode    = Convert.ToString(table.Rows[0]["BranchCode"]);

        if (currentStatus != MyHandlingStatus)
        {
            ShowMessage("此單狀態為 " + StatusName(currentStatus) + "，非您可處理範圍。", "warn");
            BindPendingList();
            return;
        }
        if (!Me.IsHeadOffice && branchCode != Me.BranchCode)
        {
            ShowMessage("此單非本分行案件。", "warn");
            BindPendingList();
            return;
        }

        UcApplyDetail.LoadApply(applyNo);
        ViewState["ApplyNo"]    = applyNo;
        ViewState["FromStatus"] = currentStatus;

        // 總行科長決行 (Status=6) 必填利率別代號
        if (currentStatus == 6)
        {
            PhRateType.Visible = true;
            BindRateTypeDropDown();
        }
        else
        {
            PhRateType.Visible = false;
        }

        MvApprove.ActiveViewIndex = 1;
    }

    private void BindRateTypeDropDown()
    {
        DdlRateType.Items.Clear();
        DdlRateType.Items.Add(new ListItem("-- 請選擇 --", ""));

        var rateTypes = Query(
            "SELECT RateCode, RateName FROM dbo.NTD_RateType WHERE IsActive = 1 ORDER BY RateCode");
        foreach (DataRow row in rateTypes.Rows)
        {
            DdlRateType.Items.Add(new ListItem(
                row["RateCode"] + " - " + row["RateName"],
                Convert.ToString(row["RateCode"])));
        }
    }

    protected void GvPendingList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "HandleItem")
            ShowHandleView(Convert.ToString(e.CommandArgument));
    }

    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(Me.AppendSid("Approve.aspx"), true);
    }

    /// <summary>放行至下一關。Status=6 -> 9 的「決行」動作需指定利率別代號。</summary>
    protected void BtnApprove_Click(object sender, EventArgs e)
    {
        string applyNo    = Convert.ToString(ViewState["ApplyNo"]);
        int    fromStatus = Convert.ToInt32(ViewState["FromStatus"]);
        int    toStatus   = NextStatusOnApprove(fromStatus);

        string rateTypeCode = null;
        if (fromStatus == 6)
        {
            if (string.IsNullOrEmpty(DdlRateType.SelectedValue))
            {
                ShowMessage("總行科長決行必須選擇『利率別代號』。", "err");
                ShowHandleView(applyNo);
                return;
            }
            rateTypeCode = DdlRateType.SelectedValue;
        }

        using (var conn = new SqlConnection(ConnStr))
        {
            conn.Open();
            using (var transaction = conn.BeginTransaction())
            {
                // 更新主檔：使用 Status 條件確保 race condition 下不會誤改
                string updateSql = @"
                    UPDATE dbo.NTD_Main
                    SET    Status     = @ToStatus,
                           UpdateUser = @MyEmpID,
                           UpdateTime = GETDATE() ";
                if (rateTypeCode != null) updateSql += ", RateTypeCode = @RateTypeCode ";
                updateSql += " WHERE ApplyNo = @ApplyNo AND Status = @FromStatus ";

                using (var cmd = new SqlCommand(updateSql, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@ToStatus",   toStatus);
                    cmd.Parameters.AddWithValue("@MyEmpID",    Me.EmpID);
                    cmd.Parameters.AddWithValue("@ApplyNo",    applyNo);
                    cmd.Parameters.AddWithValue("@FromStatus", fromStatus);
                    if (rateTypeCode != null)
                        cmd.Parameters.AddWithValue("@RateTypeCode", rateTypeCode);

                    int affected = cmd.ExecuteNonQuery();
                    if (affected == 0)
                    {
                        ShowMessage("狀態已被他人變更，請重新整理。", "err");
                        transaction.Rollback();
                        BindPendingList();
                        return;
                    }
                }

                InsertLog(conn, transaction, applyNo,
                    fromStatus == 6 ? "DECIDE" : "APPROVE",
                    fromStatus, toStatus, TxtComment.Text);

                transaction.Commit();
            }
        }

        ShowMessage("申請單 " + applyNo + " 已放行至 " + StatusName(toStatus) + "。", "ok");
        BindPendingList();
    }

    /// <summary>退回經辦修改 (Status -> 0)。退回時必須填寫意見。</summary>
    protected void BtnReject_Click(object sender, EventArgs e)
    {
        string applyNo    = Convert.ToString(ViewState["ApplyNo"]);
        int    fromStatus = Convert.ToInt32(ViewState["FromStatus"]);

        if (string.IsNullOrWhiteSpace(TxtComment.Text))
        {
            ShowMessage("退回必須填寫意見。", "err");
            ShowHandleView(applyNo);
            return;
        }

        using (var conn = new SqlConnection(ConnStr))
        {
            conn.Open();
            using (var transaction = conn.BeginTransaction())
            {
                using (var cmd = new SqlCommand(@"
                    UPDATE dbo.NTD_Main
                    SET    Status = 0, UpdateUser = @MyEmpID, UpdateTime = GETDATE()
                    WHERE  ApplyNo = @ApplyNo AND Status = @FromStatus", conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@MyEmpID",    Me.EmpID);
                    cmd.Parameters.AddWithValue("@ApplyNo",    applyNo);
                    cmd.Parameters.AddWithValue("@FromStatus", fromStatus);
                    int affected = cmd.ExecuteNonQuery();
                    if (affected == 0)
                    {
                        ShowMessage("狀態已被他人變更，請重新整理。", "err");
                        transaction.Rollback();
                        BindPendingList();
                        return;
                    }
                }

                InsertLog(conn, transaction, applyNo, "REJECT", fromStatus, 0, TxtComment.Text);
                transaction.Commit();
            }
        }

        ShowMessage("申請單 " + applyNo + " 已退回經辦修改。", "ok");
        BindPendingList();
    }

    private void InsertLog(SqlConnection conn, SqlTransaction transaction,
                           string applyNo, string actionType,
                           int fromStatus, int toStatus, string comment)
    {
        using (var cmd = new SqlCommand(@"
            INSERT INTO dbo.NTD_Log
                (ApplyNo, ActionType, FromStatus, ToStatus,
                 ActionUser, ActionUserName, Comment)
            VALUES
                (@ApplyNo, @ActionType, @FromStatus, @ToStatus,
                 @ActionUser, @ActionUserName, @Comment)", conn, transaction))
        {
            cmd.Parameters.AddWithValue("@ApplyNo",        applyNo);
            cmd.Parameters.AddWithValue("@ActionType",     actionType);
            cmd.Parameters.AddWithValue("@FromStatus",     fromStatus);
            cmd.Parameters.AddWithValue("@ToStatus",       toStatus);
            cmd.Parameters.AddWithValue("@ActionUser",     Me.EmpID);
            cmd.Parameters.AddWithValue("@ActionUserName", Me.EmpName);
            cmd.Parameters.AddWithValue("@Comment",        comment ?? "");
            cmd.ExecuteNonQuery();
        }
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
