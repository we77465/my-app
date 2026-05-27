using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Program_LargeMoney_DataAdd : System.Web.UI.Page
{
    // 資料庫連接字串
    private string _connectionString = ConfigurationManager.ConnectionStrings["bee"].ConnectionString;
    private int? _editId = null;

    // 使用者資訊
    string empid = "203704";
    string brno = "0501";
    string managerLevel = "10";
    string empName = "欄位錯誤";


    // ===== 存匯行代號資料 =====
    private static readonly Dictionary<string, string> BankCodeData = new Dictionary<string, string>
{
    {"001", "001"}, {"002", "002"}, {"004", "004"},
    {"005", "005"}, {"006", "006"}, {"007", "007"},
    {"015", "015"}, {"016", "016"}, {"017", "017"},
    {"100", "100"}, {"101", "101"}, {"102", "102"},
    {"112", "112"}, {"115", "115"}, {"117", "117"},
    {"118", "118"}, {"126", "126"}, {"162", "162"},
    {"202", "202"}, {"204", "204"}, {"207", "207"},
    {"208", "208"}, {"209", "209"}, {"212", "212"},
    {"226", "226"}, {"231", "231"}, {"241", "241"},
    {"281", "281"}, {"291", "291"},
    {"300", "300"}, {"303", "303"},
    {"505", "505"}, {"506", "506"},
    {"602", "602"}, {"603", "603"},
    {"800", "800"}, {"805", "805"},
    {"821", "821"}, {"861", "861"}, {"871", "871"},
    {"880", "880"}, {"886", "886"},
    {"887", "887"}, {"889", "889"},
    {"911", "911"},
    {"922", "922"}, {"923", "923"}, {"926", "926"},
    {"927", "927"}, {"928", "928"},
    {"931", "931"}, {"953", "953"}
};

    // ===== 幣別與存匯行對應 =====
    private static readonly Dictionary<string, string[]> CurrencyBankMapping = new Dictionary<string, string[]>
{
    {"01", new[] {"001", "002", "004", "005", "006", "007", "015", "016", "017", "202", "204", "207", "208", "209", "212", "226", "231", "241", "281", "291"}},
    {"02", new[] {"300", "303"}},
    {"04", new[] {"241", "505", "506"}},
    {"05", new[] {"100", "101", "102"}},
    {"06", new[] {"800", "805"}},
    {"07", new[] {"602", "603"}},
    {"08", new[] {"821"}},
    {"14", new[] {"861"}},
    {"15", new[] {"871"}},
    {"16", new[] {"241", "880", "886", "887", "889"}},
    {"18", new[] {"911"}},
    {"19", new[] {"241", "922", "923", "926", "927", "928", "953"}},
    {"20", new[] {"931"}},
    {"22", new[] {"112", "115", "117", "118", "126", "162", "241"}}
};

    // ===== 需要提醒的幣別（匯出時）=====
    private static readonly HashSet<string> ReminderCurrencies = new HashSet<string> { "04", "05", "06", "18", "16", "20", "22" };

    protected void Page_Load(object sender, EventArgs e)
    {
        empid = UserInfo.getUserId(Request.QueryString["sid"].ToString(), Request);
        // brno = UserInfo.getUserBrNoAndLevel(empid). Split(',')[0];
        // managerLevel = UserInfo.getUserBrNoAndLevel(empid).Split(',')[1];
        // empName = UserInfo.getUserBrNoAndLevel(empid). Split(',')[2];
        string sInfo = UserInfo.getUserBrNoAndLevel(empid);
        Char delimiter = ',';
        String[] arrInfo = sInfo.Split(delimiter);
        if (arrInfo.Length == 3)
        {
            brno = arrInfo[0];
            empName = arrInfo[1];
            managerLevel = arrInfo[2];
        }

        if (!IsPostBack)
        {
            // 初始化下拉選單
            LoadBranchDropDown();
            LoadCurrencyDropDown();

            // 設定預設值
            txtEmpno.Text = empid;
            txtEmpnm.Text = empName;

            // ★★★ 修正：使用 DropDownList 而非 TextBox ★★★
            ddlIbrno.SelectedValue = brno;

            txtApType.Text = "NE";
            //txtReportDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            SetDefaultSearchDate();
            LoadBankCodeDropDown("");
            // 根據權限設定功能選項
            SetKindOptionsByPermission();

            // 檢查是否為編輯模式
            int id;
            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out id))
            {
                _editId = id;
                ViewState["EditId"] = id;
                LoadDataForEdit(id);
            }
        }
        else
        {
            // 從 ViewState 恢復 EditId
            if (ViewState["EditId"] != null)
            {
                _editId = (int)ViewState["EditId"];
            }
        }
    }

    private void SetDefaultSearchDate()
    {
        txtStday.Text = DateTime.Now.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// 載入存匯行代號下拉選單（根據幣別）
    /// </summary>
    private void LoadBankCodeDropDown(string curcd)
    {
        ddlBankCode.Items.Clear();
        ddlBankCode.Items.Add(new ListItem("---請選擇---", ""));

        // 新台幣不需要存匯行
        if (curcd == "00")
        {
            ddlBankCode.Enabled = false;
            spanBankCodeRequired.Visible = false;
            return;
        }

        ddlBankCode.Enabled = true;
        spanBankCodeRequired.Visible = true;

        // 根據幣別載入對應的存匯行
        if (!string.IsNullOrEmpty(curcd) && CurrencyBankMapping.ContainsKey(curcd))
        {
            foreach (string bankCode in CurrencyBankMapping[curcd])
            {
                string displayText = BankCodeData.ContainsKey(bankCode) ? BankCodeData[bankCode] : bankCode;
                ddlBankCode.Items.Add(new ListItem(displayText, bankCode));
            }
        }
    }

    /// <summary>
    /// 幣別變更事件
    /// </summary>
    protected void ddlCurcd_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadBankCodeDropDown(ddlCurcd.SelectedValue);
        CheckAndShowReminder();
    }

    /// <summary>
    /// 匯出/匯入變更事件
    /// </summary>
    protected void ddlIocd_SelectedIndexChanged(object sender, EventArgs e)
    {
        CheckAndShowReminder();
    }

    /// <summary>
    /// 檢查並顯示提醒訊息
    /// </summary>
    private void CheckAndShowReminder()
    {
        string curcd = ddlCurcd.SelectedValue;
        string iocd = ddlIocd.SelectedValue;

        // 匯出(2) + 需要提醒的幣別
        if (iocd == "2" && ReminderCurrencies.Contains(curcd))
        {
            string currencyName = ddlCurcd.SelectedItem.Text;
            string message = string.Format(
                "【提醒】\\n\\n幣別：{0}\\n\\nVALUE DATE 可否改為次一營業日？\\n如需當日，請通知財務處資金科。",
                currencyName);
            ScriptManager.RegisterStartupScript(this, GetType(), "reminder",
                string.Format("alert('{0}');", message), true);
        }
    }
    /// <summary>
    /// 載入幣別下拉選單
    /// </summary>
    private void LoadCurrencyDropDown()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT CURCD, CurrencyName, CurrencyCode FROM tbl_Currency_Code ORDER BY CURCD";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    ddlCurcd.Items.Clear();
                    ddlCurcd.Items.Add(new ListItem("---請選擇---", ""));

                    ddlBkcur.Items.Clear();
                    ddlBkcur.Items.Add(new ListItem("---請選擇---", ""));

                    while (reader.Read())
                    {
                        string value = reader["CURCD"].ToString();
                        string text = string.Format("{0}-{1}({2})",
                            reader["CURCD"], reader["CurrencyName"], reader["CurrencyCode"]);

                        ddlCurcd.Items.Add(new ListItem(text, value));
                        ddlBkcur.Items.Add(new ListItem(text, value));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // 若資料表不存在，使用預設選項
            ddlCurcd.Items.Clear();
            ddlCurcd.Items.Add(new ListItem("---請選擇---", ""));
            ddlCurcd.Items.Add(new ListItem("01-新台幣(TWD)", "01"));
            ddlCurcd.Items.Add(new ListItem("02-美元(USD)", "02"));
            ddlCurcd.Items.Add(new ListItem("16-歐元(EUR)", "16"));
            ddlCurcd.Items.Add(new ListItem("09-日圓(JPY)", "09"));
            ddlCurcd.Items.Add(new ListItem("20-人民幣(CNY)", "20"));

            ddlBkcur.Items.Clear();
            ddlBkcur.Items.Add(new ListItem("---請選擇---", ""));
            ddlBkcur.Items.Add(new ListItem("01-新台幣(TWD)", "01"));
            ddlBkcur.Items.Add(new ListItem("02-美元(USD)", "02"));
            ddlBkcur.Items.Add(new ListItem("16-歐元(EUR)", "16"));
            ddlBkcur.Items.Add(new ListItem("09-日圓(JPY)", "09"));
            ddlBkcur.Items.Add(new ListItem("20-人民幣(CNY)", "20"));
        }
    }

    /// <summary>
    /// 根據權限設定功能選項
    /// </summary>
    private void SetKindOptionsByPermission()
    {
        bool isFinanceDept = (managerLevel == "05" || managerLevel == "09");

        if (!isFinanceDept)
        {
            foreach (ListItem item in ddlKind.Items)
            {
                if (item.Value == "2" || item.Value == "3")
                {
                    item.Enabled = false;
                }
            }
        }
    }

    private void LoadBranchDropDown()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "USE chb_pub SELECT BRNO, BRNAME FROM BRANCH ORDER BY BRNO";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    ddlIbrno.Items.Clear();
                    ddlIbrno.Items.Add(new ListItem("---請選擇---", ""));

                    ddlAcbrno.Items.Clear();
                    ddlAcbrno.Items.Add(new ListItem("---請選擇---", ""));

                    while (reader.Read())
                    {
                        string brnoValue = reader["BRNO"].ToString().Trim();
                        string brname = reader["BRNAME"].ToString().Trim();
                        string displayText = brnoValue + " - " + brname;

                        ddlIbrno.Items.Add(new ListItem(displayText, brnoValue));
                        ddlAcbrno.Items.Add(new ListItem(displayText, brnoValue));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ddlIbrno.Items.Clear();
            ddlIbrno.Items.Add(new ListItem("---請選擇---", ""));
            ddlIbrno.Items.Add(new ListItem("0501 - 總行營業部", "0501"));
            ddlIbrno.Items.Add(new ListItem("5012 - 台北分行", "5012"));

            ddlAcbrno.Items.Clear();
            ddlAcbrno.Items.Add(new ListItem("---請選擇---", ""));
            ddlAcbrno.Items.Add(new ListItem("0501 - 總行營業部", "0501"));
            ddlAcbrno.Items.Add(new ListItem("5012 - 台北分行", "5012"));
        }
    }

    /// <summary>
    /// 載入資料供編輯
    /// </summary>
    /// <summary>
    /// 載入資料供編輯
    /// </summary>
    private void LoadDataForEdit(int id)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = @"SELECT * FROM tbl_LargeMoney_G5310 
                          WHERE ID = @ID AND IsDeleted = 0";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // 填充表單資料
                            ddlKind.SelectedValue = reader["KIND"].ToString();
                            txtApType.Text = reader["APTYPE"].ToString().Trim();
                            txtReportDate.Text = Convert.ToDateTime(reader["REPORT_DATE"]).ToString("yyyy-MM-dd");
                            txtSrno.Text = reader["SRNO"].ToString();
                            txtUnino.Text = reader["UNINO"].ToString().Trim();
                            txtCustomerName.Text = reader["Customer_Name"].ToString() ?? "";
                            txtStday.Text = Convert.ToDateTime(reader["STDAY"]).ToString("yyyy-MM-dd");

                            // ★★★ 修正：使用 DropDownList ★★★
                            ddlIbrno.SelectedValue = reader["IBRNO"].ToString().Trim();
                            ddlAcbrno.SelectedValue = reader["ACBRNO"].ToString().Trim();

                            ddlCurcd.SelectedValue = reader["CURCD"].ToString().Trim();

                            if (reader["BKCUR"] != DBNull.Value)
                                ddlBkcur.SelectedValue = reader["BKCUR"].ToString().Trim();


                            LoadBankCodeDropDown(ddlCurcd.SelectedValue);
                            ddlBankCode.SelectedValue = reader["BKSRNO"].ToString().Trim();
                            txtAmt.Text = Convert.ToDecimal(reader["AMT"]).ToString("F2");
                            ddlIocd.SelectedValue = reader["IOCD"].ToString();
                            txtAmtwhere.Text = reader["AMTWHERE"].ToString();
                            txtReason.Text = reader["REASON"].ToString();
                            txtEmpno.Text = reader["EMPNO"].ToString().Trim();
                            txtEmpnm.Text = reader["EMPNM"].ToString().Trim();

                            // 取得 Status 值
                            string status = reader["Status"].ToString();

                            // 修改按鈕文字和標題
                            bool isFinanceDept = (managerLevel == "05" || managerLevel == "09");
                            string edit = "";
                            if (Request.QueryString["Edit"] != null)
                            {
                                edit = Request.QueryString["Edit"];
                            }
                            if (status == "1" || edit == "false") //如果已經放行了，或是點檢視，則不能操作
                            {
                                litTitle.Text = "大額資金通報(G5310)-檢視";
                                btnSubmit.Visible = false;  // 隱藏編輯/送出按鈕
                                btnReset.Visible = false;   // 隱藏清空按鈕

                                // 可選：將所有輸入欄位設為唯讀
                                SetFormReadOnly(true);
                            }
                            else if (isFinanceDept)
                            {
                                btnSubmit.Text = "編輯";
                                litTitle.Text = "大額資金通報(G5310)-編輯";
                                btnSubmit.Visible = true;
                                btnReset.Visible = true;
                            }
                            else
                            {
                                btnSubmit.Text = "更新";
                                litTitle.Text = "大額資金通報(G5310)-編輯";
                                btnSubmit.Visible = true;
                                btnReset.Visible = true;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                                "alert('找不到該筆資料'); window.history.back();", true);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                string.Format("alert('載入資料錯誤: {0}');", ex.Message.Replace("'", "\\'")), true);
        }
    }

    /// <summary>
    /// 設定表單為唯讀模式（可選）
    /// </summary>
    private void SetFormReadOnly(bool readOnly)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "setDisplayReadonly",
    string.Format("var d=document.getElementById('txtAmtDisplay'); if(d) d.disabled={0};",
    readOnly ? "true" : "false"), true);
        ddlKind.Enabled = !readOnly;
        txtReportDate.Enabled = !readOnly;
        txtUnino.Enabled = !readOnly;
        btnQueryCustomer.Enabled = !readOnly;  // 新增：控制查詢按鈕 txtCustomerName 保持唯讀，不需要額外處理
        txtStday.Enabled = !readOnly;
        ddlIbrno.Enabled = !readOnly;
        ddlAcbrno.Enabled = !readOnly;
        ddlCurcd.Enabled = !readOnly;
        ddlBkcur.Enabled = !readOnly;
        ddlBankCode.Enabled = !readOnly;
        txtAmt.Enabled = !readOnly;
        ddlIocd.Enabled = !readOnly;
        txtAmtwhere.Enabled = !readOnly;
        txtReason.Enabled = !readOnly;
    }

    /// <summary>
    /// 送出按鈕點擊事件
    /// </summary>
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsValid)
                return;

            // 驗證日期邏輯
            //DateTime reportDate = DateTime.Parse(txtReportDate.Text);
            DateTime reportDate = DateTime.Now;
            DateTime stday = DateTime.Parse(txtStday.Text);

            // 驗證金額
            decimal amount;
            if (!decimal.TryParse(txtAmt.Text.Trim(), out amount) || amount <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error",
                    "alert('金額必須為有效的正數');", true);
                return;
            }
            if (ddlCurcd.SelectedValue != "00" && string.IsNullOrEmpty(ddlBankCode.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error",
                    "alert('請選擇存匯行代號');", true);
                return;
            }
            // 金額超過 5 億，資金來源/去處必填
            decimal threshold = 500000000M;
            if (amount >= threshold)
            {
                if (string.IsNullOrWhiteSpace(txtAmtwhere.Text))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error",
                        "alert('金額達 5 億元以上，【資金來源/去處】為必填欄位');", true);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtReason.Text))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error",
                        "alert('金額達 5 億元以上，【變動原因內容】為必填欄位');", true);
                    return;
                }
            }

            // 判斷是新增還是更新
            bool isSuccess = false;
            string message = "";

            if (ViewState["EditId"] != null)
            {
                int editId = (int)ViewState["EditId"];
                isSuccess = UpdateData(editId, reportDate, stday, amount);
                message = isSuccess ? "更新成功" : "更新失敗，請稍後重試";
            }
            else
            {
                isSuccess = InsertData(reportDate, stday, amount);
                message = isSuccess ? "新增成功" : "新增失敗，請稍後重試";
            }

            if (isSuccess)
            {
                string redirectUrl = "DataQuery_G5310.aspx";
                if (Request.QueryString["sid"] != null)
                    redirectUrl += "?sid=" + Request.QueryString["sid"];

                ScriptManager.RegisterStartupScript(this, GetType(), "success",
                    string.Format("alert('{0}'); window.location='{1}';", message, redirectUrl), true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error",
                    string.Format("alert('{0}');", message), true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                string.Format("alert('發生錯誤: {0}');", ex.Message.Replace("'", "\\'")), true);
        }
    }

    /// <summary>
    /// 查詢客戶按鈕點擊事件 - 根據統一編號查詢客戶名稱
    /// </summary>
    protected void BtnQueryCustomer_Click(object sender, EventArgs e)
    {
        try
        {
            string unino = txtUnino.Text.Trim();

            // 驗證統一編號是否已輸入
            if (string.IsNullOrEmpty(unino))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error",
                    "alert('請先輸入統一編號');", true);
                return;
            }

            // 驗證統一編號格式 (8-11碼英數字)
            if (unino.Length < 8 || unino.Length > 11)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error",
                    "alert('統一編號格式錯誤，必須為8-11碼');", true);
                return;
            }

            // 呼叫 F0101 查詢客戶資料
            F0101 f0101rs = RiskUtils.GetF0101Rs("System", unino, "LargeMoney", Convert.ToString(brno).Substring(0, 4));

            if (f0101rs.isSuccess && !string.IsNullOrEmpty(f0101rs.CNAME))
            {
                // 查詢成功，帶入中文戶名（客戶名稱）
                txtCustomerName.Text = f0101rs.CNAME;

                ScriptManager.RegisterStartupScript(this, GetType(), "success",
                    "alert('查詢成功，已帶入客戶名稱');", true);
            }
            else
            {
                // 查詢失敗或找不到資料
                txtCustomerName.Text = "";

                ScriptManager.RegisterStartupScript(this, GetType(), "notfound",
                    "alert('查無此統一編號對應的客戶資料，請確認統一編號是否正確');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                string.Format("alert('查詢客戶資料發生錯誤: {0}');", ex.Message.Replace("'", "\\'")), true);
        }
    }

    /// <summary>
    /// 插入資料到資料庫
    /// </summary>
    private bool InsertData(DateTime reportDate, DateTime stday, decimal amount)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // 取得新案件編號
                int newSrno = 0;
                using (SqlCommand cmdSeq = new SqlCommand("sp_GetNextSRNO", conn))
                {
                    cmdSeq.CommandType = CommandType.StoredProcedure;
                    cmdSeq.Parameters.AddWithValue("@ReportDate", reportDate);
                    SqlParameter outputParam = new SqlParameter("@NewSRNO", SqlDbType.Int);
                    outputParam.Direction = ParameterDirection.Output;
                    cmdSeq.Parameters.Add(outputParam);
                    cmdSeq.ExecuteNonQuery();
                    newSrno = (int)outputParam.Value;
                }

                string sql = @"INSERT INTO tbl_LargeMoney_G5310
                            (KIND, APTYPE, REPORT_DATE, SRNO, UNINO, CIFKEY, STDAY, 
                             IBRNO, ACBRNO, CURCD, BKCUR, BKSRNO, AMT, IOCD, 
                             AMTWHERE, REASON, EMPNO, EMPNM, Customer_Name,
                             Created_By, Created_Time, IsDeleted,BRNO)
                            VALUES
                            (@KIND, @APTYPE, @REPORT_DATE, @SRNO, @UNINO, @CIFKEY, @STDAY, 
                             @IBRNO, @ACBRNO, @CURCD, @BKCUR, @BKSRNO, @AMT, @IOCD, 
                             @AMTWHERE, @REASON, @EMPNO, @EMPNM, @Customer_Name,
                             @Created_By, @Created_Time, 0,@BRNO)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@KIND", int.Parse(ddlKind.SelectedValue));
                    cmd.Parameters.AddWithValue("@APTYPE", txtApType.Text.Trim());
                    cmd.Parameters.AddWithValue("@REPORT_DATE", reportDate);
                    cmd.Parameters.AddWithValue("@SRNO", newSrno);
                    cmd.Parameters.AddWithValue("@UNINO", txtUnino.Text.Trim());
                    cmd.Parameters.AddWithValue("@CIFKEY", txtUnino.Text.Trim().Length >= 10 ?
                        txtUnino.Text.Trim().Substring(0, 10) : txtUnino.Text.Trim());
                    cmd.Parameters.AddWithValue("@STDAY", stday);

                    // ★★★ 修正：使用 DropDownList. SelectedValue ★★★
                    cmd.Parameters.AddWithValue("@IBRNO", ddlIbrno.SelectedValue);
                    cmd.Parameters.AddWithValue("@ACBRNO", ddlAcbrno.SelectedValue);

                    cmd.Parameters.AddWithValue("@CURCD", ddlCurcd.SelectedValue);
                    cmd.Parameters.AddWithValue("@BKCUR", string.IsNullOrEmpty(ddlBkcur.SelectedValue) ?
                        (object)DBNull.Value : ddlBkcur.SelectedValue);

                    cmd.Parameters.AddWithValue("@BKSRNO", ddlCurcd.SelectedValue == "00" || string.IsNullOrEmpty(ddlBankCode.SelectedValue) ? (object)DBNull.Value : ddlBankCode.SelectedValue);
                    cmd.Parameters.AddWithValue("@AMT", amount);
                    cmd.Parameters.AddWithValue("@IOCD", int.Parse(ddlIocd.SelectedValue));
                    cmd.Parameters.AddWithValue("@AMTWHERE", txtAmtwhere.Text.Trim());
                    cmd.Parameters.AddWithValue("@REASON", txtReason.Text.Trim());
                    cmd.Parameters.AddWithValue("@EMPNO", txtEmpno.Text.Trim());
                    cmd.Parameters.AddWithValue("@EMPNM", txtEmpnm.Text.Trim());
                    cmd.Parameters.AddWithValue("@Customer_Name", txtCustomerName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Created_By", empName);
                    cmd.Parameters.AddWithValue("@Created_Time", DateTime.Now);
                    cmd.Parameters.AddWithValue("@BRNO", brno);
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("新增資料錯誤: " + ex.Message);
        }
    }

    /// <summary>
    /// 更新資料到資料庫
    /// </summary>
    private bool UpdateData(int id, DateTime reportDate, DateTime stday, decimal amount)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = @"UPDATE tbl_LargeMoney_G5310 
                              SET KIND = @KIND,
                                  REPORT_DATE = @REPORT_DATE,
                                  UNINO = @UNINO,
                                  CIFKEY = @CIFKEY,
                                  STDAY = @STDAY,
                                  IBRNO = @IBRNO,
                                  ACBRNO = @ACBRNO,
                                  CURCD = @CURCD,
                                  BKCUR = @BKCUR,
                                  BKSRNO = @BKSRNO,
                                  AMT = @AMT,
                                  IOCD = @IOCD,
                                  AMTWHERE = @AMTWHERE,
                                  REASON = @REASON,
                                  Customer_Name = @Customer_Name,
                                  Modified_By = @Modified_By,
                                  Modified_Time = @Modified_Time
                              WHERE ID = @ID AND IsDeleted = 0";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@KIND", int.Parse(ddlKind.SelectedValue));
                    cmd.Parameters.AddWithValue("@REPORT_DATE", reportDate);
                    cmd.Parameters.AddWithValue("@UNINO", txtUnino.Text.Trim());
                    cmd.Parameters.AddWithValue("@CIFKEY", txtUnino.Text.Trim().Length >= 10 ?
                        txtUnino.Text.Trim().Substring(0, 10) : txtUnino.Text.Trim());
                    cmd.Parameters.AddWithValue("@STDAY", stday);
                    cmd.Parameters.AddWithValue("@IBRNO", ddlIbrno.SelectedValue);
                    cmd.Parameters.AddWithValue("@ACBRNO", ddlAcbrno.SelectedValue);

                    cmd.Parameters.AddWithValue("@CURCD", ddlCurcd.SelectedValue);
                    cmd.Parameters.AddWithValue("@BKCUR", string.IsNullOrEmpty(ddlBkcur.SelectedValue) ?
                        (object)DBNull.Value : ddlBkcur.SelectedValue);
                    cmd.Parameters.AddWithValue("@BKSRNO", ddlCurcd.SelectedValue == "00" || string.IsNullOrEmpty(ddlBankCode.SelectedValue) ? (object)DBNull.Value : ddlBankCode.SelectedValue);
                    cmd.Parameters.AddWithValue("@AMT", amount);
                    cmd.Parameters.AddWithValue("@IOCD", int.Parse(ddlIocd.SelectedValue));
                    cmd.Parameters.AddWithValue("@AMTWHERE", txtAmtwhere.Text.Trim());
                    cmd.Parameters.AddWithValue("@REASON", txtReason.Text.Trim());
                    cmd.Parameters.AddWithValue("@Customer_Name", txtCustomerName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Modified_By", empName);
                    cmd.Parameters.AddWithValue("@Modified_Time", DateTime.Now);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("更新資料錯誤: " + ex.Message);
        }
    }

    /// <summary>
    /// 清空按鈕點擊事件
    /// </summary>
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        if (ViewState["EditId"] != null)
        {
            LoadDataForEdit((int)ViewState["EditId"]);
        }
        else
        {
            ClearForm();
        }
    }

    /// <summary>
    /// 返回按鈕點擊事件
    /// </summary>
    protected void BtnBack_Click(object sender, EventArgs e)
    {
        string redirectUrl = "DataQuery_G5310.aspx";
        //if (Request.QueryString["sid"] != null)
        //    redirectUrl += "?sid=" + Request.QueryString["sid"];


        if (!string.IsNullOrEmpty(redirectUrl) && Uri.IsWellFormedUriString(redirectUrl, UriKind.Relative))
        {
            // 安全：只允許相對路徑
            redirectUrl += "?sid=" + Server.UrlEncode(Request.QueryString["sid"]);
            Response.Redirect(redirectUrl);
        }
        else
        {
            // 不合法時導回首頁或預設頁面
            Response.Redirect("~/Default.aspx");
        }
    }


    /// <summary>
    /// 交割日期不能小於今天（後端驗證）
    /// </summary>
    protected void cvStday_ServerValidate(object source, ServerValidateEventArgs args)
    {
        DateTime stday;
        if (DateTime.TryParse(args.Value, out stday))
        {
            args.IsValid = stday.Date >= DateTime.Today;
        }
        else
        {
            args.IsValid = false;
        }
    }

    /// <summary>
    /// 清空表單
    /// </summary>
    private void ClearForm()
    {
        ddlKind.SelectedIndex = 0;
        txtApType.Text = "NE";
        //txtReportDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtSrno.Text = string.Empty;
        txtUnino.Text = string.Empty;
        txtCustomerName.Text = string.Empty;
        txtStday.Text = string.Empty;

        // ★★★ 修正：使用 DropDownList ★★★
        ddlIbrno.SelectedValue = brno;  // 預設為使用者所屬單位
        ddlAcbrno.SelectedIndex = 0;    // 重設為「請選擇」

        ddlCurcd.SelectedIndex = 0;
        ddlBkcur.SelectedIndex = 0;
        // 先載入幣別對應的存匯行選項
        //LoadBankCodeDropDown(reader["CURCD"].ToString().Trim());
        LoadBankCodeDropDown("");

        //// 設定存匯行代號
        //if (reader["BKSRNO"] != DBNull.Value)
        //{
        //    string bksrnoValue = reader["BKSRNO"].ToString().Trim();
        //    if (ddlBankCode.Items.FindByValue(bksrnoValue) != null)
        //    {
        //        ddlBankCode.SelectedValue = bksrnoValue;
        //    }
        //}
        txtAmt.Text = string.Empty;
        ddlIocd.SelectedIndex = 0;
        txtAmtwhere.Text = string.Empty;
        txtReason.Text = string.Empty;
    }
}