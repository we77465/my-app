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
    protected UserInfo Me { get { return UserInfo.Current; } }

    /// <summary>URL 上的申請單號 (?no=)，空字串代表新增。</summary>
    private string ApplyNo { get { return Request.QueryString["no"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        LblUser.Text = string.Format("{0} {1} ({2} {3}) [職等 {4}]",
            Me.EmpID, Me.EmpName, Me.BranchCode, Me.BranchName, Me.TitleLevel);

        // §2 權限設定：僅經辦可填報
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
            // 新增模式：給一列空白明細
            RptDetail.DataSource = new List<DetailRow>
            {
                new DetailRow { PeriodMonth = 12, Amount = 0, ProposedRate = 0, Memo = "" }
            };
            RptDetail.DataBind();
        }
        else
        {
            LoadExistingApply(ApplyNo);
        }
    }

    protected string NavUrl(string relativeUrl) { return Me.AppendSid(relativeUrl); }

    /// <summary>載入既有申請單。僅在自己建的 + 狀態 0 (草稿/退回) 時才能編輯。</summary>
    private void LoadExistingApply(string applyNo)
    {
        var mainTable = Query(@"
            SELECT  m.*, ISNULL(b.BranchName, '') AS BranchName
            FROM    dbo.NTD_Main    m
            LEFT JOIN dbo.BranchOnly b ON b.BranchCode = m.BranchCode
            WHERE   m.ApplyNo = @ApplyNo",
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
        TxtCustomerName.Text  = Convert.ToString(row["CustomerName"]);
        TxtCustomerID.Text    = Convert.ToString(row["CustomerID"]);
        TxtTotalAmount.Text   = (row["TotalAmount"] == DBNull.Value)
            ? "" : Convert.ToDecimal(row["TotalAmount"]).ToString("0.##");
        TxtReason.Text        = Convert.ToString(row["Reason"]);
        LblStatus.Text        = StatusName(currentStatus);

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
        TxtCustomerName.Enabled = false;
        TxtCustomerID.Enabled   = false;
        TxtTotalAmount.Enabled  = false;
        TxtReason.Enabled       = false;
        TxtApplyDate.Enabled    = false;
        BtnSaveDraft.Visible    = false;
        BtnSubmit.Visible       = false;
    }

    protected void BtnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(Me.AppendSid("Main.aspx"), true);
    }

    /// <summary>儲存為草稿 (Status 維持 0)。</summary>
    protected void BtnSaveDraft_Click(object sender, EventArgs e) { SaveApply(false); }

    /// <summary>送出申請進入簽核流程 (Status -> 1，等襄理放行)。</summary>
    protected void BtnSubmit_Click(object sender, EventArgs e) { SaveApply(true); }

    /// <summary>
    /// 儲存申請主檔 + 明細；submitToWorkflow=true 時送入簽核流程。
    /// 主檔/明細/簽核紀錄三者於同一交易完成。
    /// </summary>
    private void SaveApply(bool submitToWorkflow)
    {
        if (string.IsNullOrWhiteSpace(TxtCustomerName.Text))
        { ShowMessage("請填寫客戶名稱。", "err"); return; }

        decimal totalAmount;
        if (!decimal.TryParse(TxtTotalAmount.Text, out totalAmount) || totalAmount < 0)
        { ShowMessage("申請總金額格式錯誤。", "err"); return; }

        if ((TxtReason.Text ?? "").Length > 3000)
        { ShowMessage("申請理由超過 3,000 字。", "err"); return; }

        var detailRows = ReadDetailFromForm();
        if (detailRows.Count == 0)
        { ShowMessage("至少要有一筆利率明細。", "err"); return; }

        DateTime applyDate;
        if (!DateTime.TryParse(TxtApplyDate.Text, out applyDate)) applyDate = DateTime.Today;

        using (var conn = new SqlConnection(ConnStr))
        {
            conn.Open();
            using (var transaction = conn.BeginTransaction())
            {
                string applyNo   = ApplyNo;
                bool   isNew     = string.IsNullOrEmpty(applyNo);
                int    fromStatus = 0;
                int    toStatus   = submitToWorkflow ? 1 : 0;

                if (isNew)
                {
                    applyNo = GenerateApplyNo(conn, transaction);
                    InsertMain(conn, transaction, applyNo, applyDate, totalAmount, toStatus);
                }
                else
                {
                    fromStatus = ReadStatusForEdit(conn, transaction, applyNo);
                    UpdateMain(conn, transaction, applyNo, applyDate, totalAmount, toStatus);
                    DeleteDetailsForRewrite(conn, transaction, applyNo);
                }

                InsertDetailRows(conn, transaction, applyNo, detailRows);
                InsertLog(conn, transaction, applyNo,
                          isNew ? "CREATE" : (submitToWorkflow ? "SUBMIT" : "EDIT"),
                          fromStatus, toStatus,
                          submitToWorkflow ? "送出申請" : "存草稿");

                transaction.Commit();
            }
        }
        Response.Redirect(Me.AppendSid("View.aspx?no=" + (ApplyNo ?? "")), true);
    }

    /// <summary>產生申請單號：NTD + yyyyMMdd + 4 碼流水 (同一天內遞增)。</summary>
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
                            string applyNo, DateTime applyDate, decimal totalAmount, int initialStatus)
    {
        using (var cmd = new SqlCommand(@"
            INSERT INTO dbo.NTD_Main
                (ApplyNo, BranchCode, ApplyDate, CustomerName, CustomerID,
                 TotalAmount, Reason, Status, CreateUser, CreateTime)
            VALUES
                (@ApplyNo, @BranchCode, @ApplyDate, @CustomerName, @CustomerID,
                 @TotalAmount, @Reason, @Status, @CreateUser, GETDATE())", conn, transaction))
        {
            cmd.Parameters.AddWithValue("@ApplyNo",      applyNo);
            cmd.Parameters.AddWithValue("@BranchCode",   Me.BranchCode);
            cmd.Parameters.AddWithValue("@ApplyDate",    applyDate);
            cmd.Parameters.AddWithValue("@CustomerName", TxtCustomerName.Text.Trim());
            cmd.Parameters.AddWithValue("@CustomerID",   (object)TxtCustomerID.Text.Trim() ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TotalAmount",  totalAmount);
            cmd.Parameters.AddWithValue("@Reason",       TxtReason.Text ?? "");
            cmd.Parameters.AddWithValue("@Status",       initialStatus);
            cmd.Parameters.AddWithValue("@CreateUser",   Me.EmpID);
            cmd.ExecuteNonQuery();
        }
    }

    /// <summary>讀取現況並驗證可編輯：必須 Status=0 且為自己建的。</summary>
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
                            string applyNo, DateTime applyDate, decimal totalAmount, int newStatus)
    {
        using (var cmd = new SqlCommand(@"
            UPDATE dbo.NTD_Main
            SET    ApplyDate    = @ApplyDate,
                   CustomerName = @CustomerName,
                   CustomerID   = @CustomerID,
                   TotalAmount  = @TotalAmount,
                   Reason       = @Reason,
                   Status       = @Status,
                   UpdateUser   = @UpdateUser,
                   UpdateTime   = GETDATE()
            WHERE  ApplyNo      = @ApplyNo", conn, transaction))
        {
            cmd.Parameters.AddWithValue("@ApplyNo",      applyNo);
            cmd.Parameters.AddWithValue("@ApplyDate",    applyDate);
            cmd.Parameters.AddWithValue("@CustomerName", TxtCustomerName.Text.Trim());
            cmd.Parameters.AddWithValue("@CustomerID",   (object)TxtCustomerID.Text.Trim() ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TotalAmount",  totalAmount);
            cmd.Parameters.AddWithValue("@Reason",       TxtReason.Text ?? "");
            cmd.Parameters.AddWithValue("@Status",       newStatus);
            cmd.Parameters.AddWithValue("@UpdateUser",   Me.EmpID);
            cmd.ExecuteNonQuery();
        }
    }

    private void DeleteDetailsForRewrite(SqlConnection conn, SqlTransaction transaction, string applyNo)
    {
        // 編輯時明細是「整批覆寫」策略，比起逐筆比對代碼簡單且明細量少
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
                INSERT INTO dbo.NTD_Detail (ApplyNo, SeqNo, PeriodMonth, Amount, ProposedRate, Memo)
                VALUES (@ApplyNo, @SeqNo, @PeriodMonth, @Amount, @ProposedRate, @Memo)", conn, transaction))
            {
                cmd.Parameters.AddWithValue("@ApplyNo",      applyNo);
                cmd.Parameters.AddWithValue("@SeqNo",        seqNo++);
                cmd.Parameters.AddWithValue("@PeriodMonth",  detail.PeriodMonth);
                cmd.Parameters.AddWithValue("@Amount",       detail.Amount);
                cmd.Parameters.AddWithValue("@ProposedRate", detail.ProposedRate);
                cmd.Parameters.AddWithValue("@Memo",         (object)detail.Memo ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }
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

    /// <summary>
    /// 從 Request.Form 讀出動態增刪的明細列。
    /// 欄位命名於前端 JS 為 Period_N / Amount_N / Rate_N / Memo_N (N 為列序)。
    /// </summary>
    private List<DetailRow> ReadDetailFromForm()
    {
        var rows = new List<DetailRow>();
        for (int index = 0; index < 100; index++)
        {
            string period = Request.Form["Period_" + index];
            string amount = Request.Form["Amount_" + index];
            string rate   = Request.Form["Rate_"   + index];
            string memo   = Request.Form["Memo_"   + index];

            // 整列空白就跳過
            if (string.IsNullOrWhiteSpace(period)
                && string.IsNullOrWhiteSpace(amount)
                && string.IsNullOrWhiteSpace(rate)) continue;

            int     periodMonth;
            decimal amountValue, rateValue;
            int.TryParse(period,     NumberStyles.Integer, CultureInfo.InvariantCulture, out periodMonth);
            decimal.TryParse(amount, NumberStyles.Number,  CultureInfo.InvariantCulture, out amountValue);
            decimal.TryParse(rate,   NumberStyles.Number,  CultureInfo.InvariantCulture, out rateValue);

            rows.Add(new DetailRow
            {
                PeriodMonth  = periodMonth,
                Amount       = amountValue,
                ProposedRate = rateValue,
                Memo         = memo
            });
        }
        return rows;
    }

    private void ShowMessage(string text, string level)
    {
        LblMessage.Visible  = true;
        LblMessage.CssClass = "alert alert-" + level;
        LblMessage.Text     = Server.HtmlEncode(text);
    }

    /// <summary>單列明細的記憶體模型 (用於 Repeater 綁定 + 表單解析)。</summary>
    public class DetailRow
    {
        public int     PeriodMonth  { get; set; }
        public decimal Amount       { get; set; }
        public decimal ProposedRate { get; set; }
        public string  Memo         { get; set; }
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
