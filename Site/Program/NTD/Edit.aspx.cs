using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;

/// <summary>
/// 新臺幣定期存款專案利率申請 - 新增/編輯申請單。
/// 僅「經辦」可進入；申請單在 Status = 0 (草稿/退回) 時才可修改。
/// </summary>
public partial class NtdEdit : Page
{
    protected NtdCurrentUser Me { get { return NtdCurrentUser.Current; } }

    /// <summary>URL 上的申請單號 (?no=)，空字串代表新增。</summary>
    private string ApplyNo { get { return Request.QueryString["no"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        LblUser.Text = string.Format("{0} {1} ({2} {3}) [職等 {4}]",
            Me.EmpID, Me.EmpName, Me.BranchCode, Me.BranchName, Me.TitleLevel);

        if (!Me.CanCreateApply)
        {
            ShowMessage("您的職位無法新增/編輯申請。", "err");
            DisableForm();
            return;
        }

        if (IsPostBack) return;

        TxtApplyDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
        LblBranch.Text    = Me.BranchCode + " " + Me.BranchName;
        LblStatus.Text    = StatusName(0);

        if (string.IsNullOrEmpty(ApplyNo))
        {
            RptDetail.DataSource = new List<DetailRow>
            {
                new DetailRow { PeriodMonth = 12 }
            };
            RptDetail.DataBind();
        }
        else
        {
            LoadExistingApply(ApplyNo);
        }
    }

    protected string NavUrl(string relativeUrl) { return Me.AppendSid(relativeUrl); }

    // ===== 統編查詢客戶名稱 (UpdatePanel 局部更新) =====

    protected void BtnQueryCustomer_Click(object sender, EventArgs e)
    {
        LblQueryMsg.Text = "";
        string customerID = TxtCustomerID.Text.Trim();

        if (string.IsNullOrEmpty(customerID))
        {
            LblQueryMsg.Text = "請先輸入統一編號";
            return;
        }
        if (customerID.Length < 8 || customerID.Length > 11)
        {
            LblQueryMsg.Text = "統一編號格式錯誤（8-11碼）";
            return;
        }

        try
        {
            F0101 result = RiskUtils.GetF0101Rs("System", customerID, "NTD",
                Me.BranchCode.Length >= 4 ? Me.BranchCode.Substring(0, 4) : Me.BranchCode);

            if (result.isSuccess && !string.IsNullOrEmpty(result.CNAME))
            {
                TxtCustomerName.Text = result.CNAME;
            }
            else
            {
                TxtCustomerName.Text = "";
                LblQueryMsg.Text = "查無此統一編號對應的客戶資料";
            }
        }
        catch (Exception ex)
        {
            LblQueryMsg.Text = "查詢失敗：" + ex.Message;
        }
    }

    // ===== 載入既有申請單 =====

    private void LoadExistingApply(string applyNo)
    {
        var mainTable = Query(@"
            SELECT  ntd.*, ISNULL(branch.BRNAME, '') AS BranchName
            FROM    dbo.NTD_Main    ntd
            LEFT JOIN chb_pub.dbo.BRANCH branch ON branch.BRNO = ntd.BranchCode
            WHERE   ntd.ApplyNo = @ApplyNo",
            P("@ApplyNo", applyNo));

        if (mainTable.Rows.Count == 0)
        {
            ShowMessage("找不到申請單：" + applyNo, "err");
            DisableForm();
            return;
        }

        var row = mainTable.Rows[0];
        int currentStatus = Convert.ToInt32(row["Status"]);

        LitTitle.Text         = "編輯申請";
        LblApplyNo.Text       = applyNo;
        LblBranch.Text        = row["BranchCode"] + " " + row["BranchName"];
        TxtApplyDate.Text     = Convert.ToDateTime(row["ApplyDate"]).ToString("yyyy-MM-dd");
        TxtGroupName.Text     = Convert.ToString(row["GroupName"]);
        TxtCustomerID.Text    = Convert.ToString(row["CustomerID"]);
        TxtCustomerName.Text  = Convert.ToString(row["CustomerName"]);
        TxtTotalAmount.Text   = (row["TotalAmount"] == DBNull.Value)
            ? "" : Convert.ToDecimal(row["TotalAmount"]).ToString("0.##");
        TxtReason.Text        = Convert.ToString(row["Reason"]);
        TxtBranchNote.Text    = Convert.ToString(row["BranchNote"]);
        LblStatus.Text        = StatusName(currentStatus);

        if (row["IsRelated"] != DBNull.Value)
            RblIsRelated.SelectedValue = Convert.ToString(row["IsRelated"]);

        if (row["Contrib3M"]  != DBNull.Value) TxtContrib3M.Text  = Convert.ToDecimal(row["Contrib3M"]).ToString("0.##");
        if (row["Contrib6M"]  != DBNull.Value) TxtContrib6M.Text  = Convert.ToDecimal(row["Contrib6M"]).ToString("0.##");
        if (row["Contrib1Y"]  != DBNull.Value) TxtContrib1Y.Text  = Convert.ToDecimal(row["Contrib1Y"]).ToString("0.##");
        if (row["RelatedAmt"] != DBNull.Value) TxtRelatedAmt.Text = Convert.ToDecimal(row["RelatedAmt"]).ToString("0.##");

        bool isEditableNow = currentStatus == 0
            && string.Equals(Convert.ToString(row["CreateUser"]), Me.EmpID,
                             StringComparison.OrdinalIgnoreCase);

        if (!isEditableNow)
        {
            ShowMessage("此申請單目前狀態無法編輯。", "warn");
            DisableForm();
        }

        var detailTable = Query(
            "SELECT * FROM dbo.NTD_Detail WHERE ApplyNo = @ApplyNo ORDER BY SeqNo",
            P("@ApplyNo", applyNo));
        RptDetail.DataSource = detailTable;
        RptDetail.DataBind();

        var logTable = Query(
            "SELECT * FROM dbo.NTD_Log WHERE ApplyNo = @ApplyNo ORDER BY LogID DESC",
            P("@ApplyNo", applyNo));
        GvLog.DataSource = logTable;
        GvLog.DataBind();
        PnlLog.Visible = logTable.Rows.Count > 0;
    }

    private void DisableForm()
    {
        TxtGroupName.Enabled    = false;
        TxtCustomerID.Enabled   = false;
        TxtCustomerName.Enabled = false;
        BtnQueryCustomer.Enabled= false;
        TxtTotalAmount.Enabled  = false;
        TxtReason.Enabled       = false;
        TxtBranchNote.Enabled   = false;
        TxtApplyDate.Enabled    = false;
        TxtContrib3M.Enabled    = false;
        TxtContrib6M.Enabled    = false;
        TxtContrib1Y.Enabled    = false;
        TxtRelatedAmt.Enabled   = false;
        RblIsRelated.Enabled    = false;
        BtnSaveDraft.Visible    = false;
        BtnSubmit.Visible       = false;
    }

    protected void BtnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(Me.AppendSid("Main.aspx"), true);
    }

    protected void BtnSaveDraft_Click(object sender, EventArgs e) { SaveApply(false); }
    protected void BtnSubmit_Click(object sender, EventArgs e)    { SaveApply(true);  }

    private void SaveApply(bool submitToWorkflow)
    {
        // 必填驗證
        if (string.IsNullOrWhiteSpace(TxtCustomerID.Text))
        { ShowMessage("請填寫客戶統編/身分證。", "err"); return; }

        if (string.IsNullOrWhiteSpace(TxtCustomerName.Text))
        { ShowMessage("請先點「查詢客戶」帶入客戶名稱。", "err"); return; }

        if (string.IsNullOrEmpty(RblIsRelated.SelectedValue))
        { ShowMessage("請選擇是否屬利害關係人。", "err"); return; }

        decimal totalAmount;
        if (!decimal.TryParse(TxtTotalAmount.Text, out totalAmount) || totalAmount < 0)
        { ShowMessage("申請總金額格式錯誤。", "err"); return; }

        if ((TxtReason.Text ?? "").Length > 3000)
        { ShowMessage("申請理由超過 3,000 字。", "err"); return; }

        if ((TxtBranchNote.Text ?? "").Length > 3000)
        { ShowMessage("申請單位說明超過 3,000 字。", "err"); return; }

        var detailRows = ReadDetailFromForm();
        if (detailRows.Count == 0)
        { ShowMessage("至少要有一筆明細。", "err"); return; }

        foreach (var detail in detailRows)
        {
            if (string.IsNullOrEmpty(detail.DepositType))
            { ShowMessage("明細列的存款種類為必填項目。", "err"); return; }
        }

        DateTime applyDate;
        if (!DateTime.TryParse(TxtApplyDate.Text, out applyDate)) applyDate = DateTime.Today;

        decimal? contrib3M  = ParseNullableDecimal(TxtContrib3M.Text);
        decimal? contrib6M  = ParseNullableDecimal(TxtContrib6M.Text);
        decimal? contrib1Y  = ParseNullableDecimal(TxtContrib1Y.Text);
        decimal? relatedAmt = ParseNullableDecimal(TxtRelatedAmt.Text);

        string savedApplyNo = ApplyNo;
        using (var conn = new SqlConnection(ConnStr))
        {
            conn.Open();
            using (var transaction = conn.BeginTransaction())
            {
                bool isNew      = string.IsNullOrEmpty(savedApplyNo);
                int  fromStatus = 0;
                int  toStatus   = submitToWorkflow ? 1 : 0;

                if (isNew)
                {
                    savedApplyNo = GenerateApplyNo(conn, transaction);
                    InsertMain(conn, transaction, savedApplyNo, applyDate, totalAmount, toStatus,
                               contrib3M, contrib6M, contrib1Y, relatedAmt);
                }
                else
                {
                    fromStatus = ReadStatusForEdit(conn, transaction, savedApplyNo);
                    UpdateMain(conn, transaction, savedApplyNo, applyDate, totalAmount, toStatus,
                               contrib3M, contrib6M, contrib1Y, relatedAmt);
                    DeleteDetailsForRewrite(conn, transaction, savedApplyNo);
                }

                InsertDetailRows(conn, transaction, savedApplyNo, detailRows);
                InsertLog(conn, transaction, savedApplyNo,
                          isNew ? "CREATE" : (submitToWorkflow ? "SUBMIT" : "EDIT"),
                          fromStatus, toStatus,
                          submitToWorkflow ? "送出申請" : "存草稿");

                transaction.Commit();
            }
        }
        Response.Redirect(Me.AppendSid("View.aspx?no=" + savedApplyNo), true);
    }

    private static string GenerateApplyNo(SqlConnection conn, SqlTransaction transaction)
    {
        string prefix = "NTD" + DateTime.Now.ToString("yyyyMMdd");
        using (var cmd = new SqlCommand(@"
            SELECT ISNULL(MAX(CAST(RIGHT(ApplyNo, 4) AS INT)), 0) + 1
            FROM   dbo.NTD_Main
            WHERE  ApplyNo LIKE @Prefix + '%'", conn, transaction))
        {
            cmd.Parameters.AddWithValue("@Prefix", prefix);
            int nextSeq = Convert.ToInt32(cmd.ExecuteScalar());
            return prefix + nextSeq.ToString("D4");
        }
    }

    private void InsertMain(SqlConnection conn, SqlTransaction transaction,
                            string applyNo, DateTime applyDate, decimal totalAmount, int initialStatus,
                            decimal? contrib3M, decimal? contrib6M, decimal? contrib1Y, decimal? relatedAmt)
    {
        using (var cmd = new SqlCommand(@"
            INSERT INTO dbo.NTD_Main
                (ApplyNo, BranchCode, ApplyDate, CustomerName, CustomerID, GroupName,
                 TotalAmount, Reason, IsRelated,
                 Contrib3M, Contrib6M, Contrib1Y, RelatedAmt, BranchNote,
                 Status, CreateUser, CreateTime)
            VALUES
                (@ApplyNo, @BranchCode, @ApplyDate, @CustomerName, @CustomerID, @GroupName,
                 @TotalAmount, @Reason, @IsRelated,
                 @Contrib3M, @Contrib6M, @Contrib1Y, @RelatedAmt, @BranchNote,
                 @Status, @CreateUser, GETDATE())", conn, transaction))
        {
            cmd.Parameters.AddWithValue("@ApplyNo",      applyNo);
            cmd.Parameters.AddWithValue("@BranchCode",   Me.BranchCode);
            cmd.Parameters.AddWithValue("@ApplyDate",    applyDate);
            cmd.Parameters.AddWithValue("@CustomerName", TxtCustomerName.Text.Trim());
            cmd.Parameters.AddWithValue("@CustomerID",   (object)TxtCustomerID.Text.Trim() ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@GroupName",    (object)TxtGroupName.Text.Trim()  ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TotalAmount",  totalAmount);
            cmd.Parameters.AddWithValue("@Reason",       TxtReason.Text ?? "");
            cmd.Parameters.AddWithValue("@IsRelated",    RblIsRelated.SelectedValue);
            cmd.Parameters.AddWithValue("@Contrib3M",    (object)contrib3M  ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Contrib6M",    (object)contrib6M  ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Contrib1Y",    (object)contrib1Y  ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@RelatedAmt",   (object)relatedAmt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BranchNote",   TxtBranchNote.Text ?? "");
            cmd.Parameters.AddWithValue("@Status",       initialStatus);
            cmd.Parameters.AddWithValue("@CreateUser",   Me.EmpID);
            cmd.ExecuteNonQuery();
        }
    }

    private int ReadStatusForEdit(SqlConnection conn, SqlTransaction transaction, string applyNo)
    {
        using (var cmd = new SqlCommand(
            "SELECT Status, CreateUser FROM dbo.NTD_Main WHERE ApplyNo = @ApplyNo", conn, transaction))
        {
            cmd.Parameters.AddWithValue("@ApplyNo", applyNo);
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read())
                    throw new InvalidOperationException("申請單不存在：" + applyNo);

                int    currentStatus = Convert.ToInt32(reader["Status"]);
                string createUser    = Convert.ToString(reader["CreateUser"]);
                bool   isMine = string.Equals(createUser, Me.EmpID, StringComparison.OrdinalIgnoreCase);
                if (currentStatus != 0 || !isMine)
                    throw new InvalidOperationException("此單目前狀態無法編輯。");

                return currentStatus;
            }
        }
    }

    private void UpdateMain(SqlConnection conn, SqlTransaction transaction,
                            string applyNo, DateTime applyDate, decimal totalAmount, int newStatus,
                            decimal? contrib3M, decimal? contrib6M, decimal? contrib1Y, decimal? relatedAmt)
    {
        using (var cmd = new SqlCommand(@"
            UPDATE dbo.NTD_Main
            SET    ApplyDate    = @ApplyDate,
                   CustomerName = @CustomerName,
                   CustomerID   = @CustomerID,
                   GroupName    = @GroupName,
                   TotalAmount  = @TotalAmount,
                   Reason       = @Reason,
                   IsRelated    = @IsRelated,
                   Contrib3M    = @Contrib3M,
                   Contrib6M    = @Contrib6M,
                   Contrib1Y    = @Contrib1Y,
                   RelatedAmt   = @RelatedAmt,
                   BranchNote   = @BranchNote,
                   Status       = @Status,
                   UpdateUser   = @UpdateUser,
                   UpdateTime   = GETDATE()
            WHERE  ApplyNo      = @ApplyNo", conn, transaction))
        {
            cmd.Parameters.AddWithValue("@ApplyNo",      applyNo);
            cmd.Parameters.AddWithValue("@ApplyDate",    applyDate);
            cmd.Parameters.AddWithValue("@CustomerName", TxtCustomerName.Text.Trim());
            cmd.Parameters.AddWithValue("@CustomerID",   (object)TxtCustomerID.Text.Trim() ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@GroupName",    (object)TxtGroupName.Text.Trim()  ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TotalAmount",  totalAmount);
            cmd.Parameters.AddWithValue("@Reason",       TxtReason.Text ?? "");
            cmd.Parameters.AddWithValue("@IsRelated",    RblIsRelated.SelectedValue);
            cmd.Parameters.AddWithValue("@Contrib3M",    (object)contrib3M  ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Contrib6M",    (object)contrib6M  ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Contrib1Y",    (object)contrib1Y  ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@RelatedAmt",   (object)relatedAmt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BranchNote",   TxtBranchNote.Text ?? "");
            cmd.Parameters.AddWithValue("@Status",       newStatus);
            cmd.Parameters.AddWithValue("@UpdateUser",   Me.EmpID);
            cmd.ExecuteNonQuery();
        }
    }

    private void DeleteDetailsForRewrite(SqlConnection conn, SqlTransaction transaction, string applyNo)
    {
        using (var cmd = new SqlCommand("DELETE FROM dbo.NTD_Detail WHERE ApplyNo = @ApplyNo", conn, transaction))
        {
            cmd.Parameters.AddWithValue("@ApplyNo", applyNo);
            cmd.ExecuteNonQuery();
        }
    }

    private void InsertDetailRows(SqlConnection conn, SqlTransaction transaction,
                                  string applyNo, List<DetailRow> rows)
    {
        int seqNo = 1;
        foreach (var detail in rows)
        {
            using (var cmd = new SqlCommand(@"
                INSERT INTO dbo.NTD_Detail
                    (ApplyNo, SeqNo, DepositType, PeriodMonth, Amount, ProposedRate,
                     StartDate, EndDate, NewAmount, RenewAmount, Memo)
                VALUES
                    (@ApplyNo, @SeqNo, @DepositType, @PeriodMonth, @Amount, @ProposedRate,
                     @StartDate, @EndDate, @NewAmount, @RenewAmount, @Memo)", conn, transaction))
            {
                cmd.Parameters.AddWithValue("@ApplyNo",      applyNo);
                cmd.Parameters.AddWithValue("@SeqNo",        seqNo++);
                cmd.Parameters.AddWithValue("@DepositType",  (object)detail.DepositType  ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PeriodMonth",  detail.PeriodMonth);
                cmd.Parameters.AddWithValue("@Amount",       detail.Amount);
                cmd.Parameters.AddWithValue("@ProposedRate", detail.ProposedRate);
                cmd.Parameters.AddWithValue("@StartDate",    (object)detail.StartDate    ?? D