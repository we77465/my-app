using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

public partial class Program_LargeMoney_DataView_G5310 : System.Web.UI.Page
{
    private string _connectionString = ConfigurationManager.ConnectionStrings["bee"].ConnectionString;

    // 使用者資訊
    private string empid = "";
    private string brno = "";
    private string managerLevel = "0000";
    private string empName = "";

    // 當前檢視的資料ID
    private int _viewId = 0;

    // 當前資料狀態
    private int _currentStatus = 0;

    // 是否為主管 (05, 09)
    private bool IsManager
    {
        get { return (managerLevel == "05" || managerLevel == "09"); }
    }

    // 幣別對照字典
    private static Dictionary<string, string> _currencyDict;

    protected void Page_Load(object sender, EventArgs e)
    {
        //empid = UserInfo.getUserId(Request.QueryString["sid"], Request);
        empid = "150128";
        //取得登入單位代號
        string sInfo = UserInfo.getUserBrNoAndLevel(empid);
        Char delimiter = ',';
        String[] arrInfo = sInfo.Split(delimiter);
        if (arrInfo.Length == 3)
        {
            brno = arrInfo[0];
            empName = arrInfo[1];
            managerLevel = arrInfo[2];
        }

        // 測試用資料
        //empid = "203704";
        //brno = "0501";
        //managerLevel = "05";  // 改成其他值測試一般人員，"05" 或 "09" 測試主管
        //empName = "測試人員";

        if (!IsPostBack)
        {
            // 載入幣別對照
            LoadCurrencyDict();

            // 檢查是否有傳入ID
            int id;
            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out id))
            {
                _viewId = id;
                ViewState["ViewId"] = id;
                LoadData(id);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error",
                    "alert('未指定資料ID'); window.history.back();", true);
            }
        }
        else
        {
            // 從 ViewState 恢復
            if (ViewState["ViewId"] != null)
            {
                _viewId = (int)ViewState["ViewId"];
            }
            if (ViewState["CurrentStatus"] != null)
            {
                _currentStatus = (int)ViewState["CurrentStatus"];
            }
        }
    }

    /// <summary>
    /// 載入幣別對照字典
    /// </summary>
    private void LoadCurrencyDict()
    {
        _currencyDict = new Dictionary<string, string>();

        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT CURCD, CurrencyName FROM tbl_Currency_Code";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string code = reader["CURCD"].ToString().Trim();
                        string name = reader["CurrencyName"].ToString();
                        if (!_currencyDict.ContainsKey(code))
                        {
                            _currencyDict.Add(code, name);
                        }
                    }
                }
            }
        }
        catch
        {
            // 使用預設值
            _currencyDict = new Dictionary<string, string>();
            _currencyDict.Add("01", "新台幣");
            _currencyDict.Add("02", "美元");
            _currencyDict.Add("09", "日圓");
            _currencyDict.Add("16", "歐元");
            _currencyDict.Add("20", "人民幣");
        }
    }

    /// <summary>
    /// 載入資料
    /// </summary>
    private void LoadData(int id)
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
                            // 基本資訊
                            lblSrno.Text = reader["SRNO"].ToString();
                            lblApType.Text = reader["APTYPE"].ToString().Trim();
                            lblReportDate.Text = Convert.ToDateTime(reader["REPORT_DATE"]).ToString("yyyy/MM/dd");
                            lblStday.Text = Convert.ToDateTime(reader["STDAY"]).ToString("yyyy/MM/dd");

                            // 功能
                            int kind = Convert.ToInt32(reader["KIND"]);
                            switch (kind)
                            {
                                case 1:
                                    lblKind.Text = "新增";
                                    break;
                                case 2:
                                    lblKind.Text = "修改";
                                    break;
                                case 3:
                                    lblKind.Text = "刪除";
                                    break;
                                default:
                                    lblKind.Text = kind.ToString();
                                    break;
                            }

                            // 狀態 (0=待放行, 1=已放行)
                            _currentStatus = Convert.ToInt32(reader["Status"]);
                            ViewState["CurrentStatus"] = _currentStatus;

                            switch (_currentStatus)
                            {
                                case 0:
                                    lblStatus.Text = "待放行";
                                    lblStatus.CssClass = "status-pending";
                                    break;
                                case 1:
                                    lblStatus.Text = "已放行";
                                    lblStatus.CssClass = "status-released";
                                    break;
                            }

                            // 客戶資訊
                            lblUnino.Text = reader["UNINO"].ToString().Trim();
                            lblCustomerName.Text = reader["Customer_Name"] != DBNull.Value ?
                                reader["Customer_Name"].ToString() : "";

                            // 交易資訊
                            lblIbrno.Text = reader["IBRNO"].ToString().Trim();
                            lblAcbrno.Text = reader["ACBRNO"].ToString().Trim();

                            // 幣別
                            string curcd = reader["CURCD"].ToString().Trim();
                            lblCurcd.Text = GetCurrencyDisplay(curcd);

                            string bkcur = reader["BKCUR"] != DBNull.Value ?
                                reader["BKCUR"].ToString().Trim() : "";
                            lblBkcur.Text = string.IsNullOrEmpty(bkcur) ? "" : GetCurrencyDisplay(bkcur);

                            lblBksrno.Text = reader["BKSRNO"] != DBNull.Value ?
                                reader["BKSRNO"].ToString().Trim() : "";

                            // 匯出/匯入
                            int iocd = Convert.ToInt32(reader["IOCD"]);
                            lblIocd.Text = (iocd == 1) ? "匯入" : "匯出";

                            // 金額
                            decimal amt = Convert.ToDecimal(reader["AMT"]);
                            lblAmt.Text = amt.ToString("N2");
                            lblAmt.CssClass = (iocd == 1) ? "amount-in" : "amount-out";

                            // 資金說明
                            lblAmtwhere.Text = reader["AMTWHERE"].ToString();
                            lblReason.Text = reader["REASON"].ToString();

                            // 經辦人員
                            lblEmpno.Text = reader["EMPNO"].ToString().Trim();
                            lblEmpnm.Text = reader["EMPNM"].ToString().Trim();

                            // 系統資訊
                            lblCreatedBy.Text = reader["Created_By"] != DBNull.Value ?
                                reader["Created_By"].ToString() : "";
                            lblCreatedTime.Text = reader["Created_Time"] != DBNull.Value ?
                                Convert.ToDateTime(reader["Created_Time"]).ToString("yyyy/MM/dd HH:mm:ss") : "";

                            lblModifiedBy.Text = reader["Modified_By"] != DBNull.Value ?
                                reader["Modified_By"].ToString() : "";
                            lblModifiedTime.Text = reader["Modified_Time"] != DBNull.Value ?
                                Convert.ToDateTime(reader["Modified_Time"]).ToString("yyyy/MM/dd HH:mm:ss") : "";

                            lblApprovedBy.Text = reader["Approved_By"] != DBNull.Value ?
                                reader["Approved_By"].ToString() : "";
                            lblApprovedTime.Text = reader["Approved_Time"] != DBNull.Value ?
                                Convert.ToDateTime(reader["Approved_Time"]).ToString("yyyy/MM/dd HH:mm:ss") : "";
                            // 主機回傳資訊
                            lblHostSrno.Text = reader["Host_SRNO"] != DBNull.Value ?
                                reader["Host_SRNO"].ToString() : "";

                            string hostStatus = reader["Host_STATUS"] != DBNull.Value ?
                                reader["Host_STATUS"].ToString() : "";
                            lblHostStatus.Text = hostStatus == "1" ? "有效" : (hostStatus == "2" ? "無效" : "");
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                                "alert('找不到該筆資料'); window.history. back();", true);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                "alert('載入資料錯誤: " + ex.Message.Replace("'", "\\'") + "');", true);
        }
    }

    /// <summary>
    /// 取得幣別顯示文字
    /// </summary>
    private string GetCurrencyDisplay(string curcd)
    {
        if (string.IsNullOrEmpty(curcd)) return "";

        if (_currencyDict != null && _currencyDict.ContainsKey(curcd))
        {
            return string.Format("{0} ({1})", _currencyDict[curcd], curcd);
        }
        return curcd;
    }

   

    /// <summary>
    /// 返回查詢按鈕
    /// </summary>
    protected void BtnBack_Click(object sender, EventArgs e)
    {
        //string redirectUrl = "DataQuery_G5310.aspx";
        //if (Request.QueryString["sid"] != null)
        //    redirectUrl += "?sid=" + Request.QueryString["sid"];
        //// 1. 取得並驗證 redirectUrl 只允許相對路徑（不允許外部網址）



        // 1. 取得並驗證 redirectUrl 只允許相對路徑（不允許外部網址）
        string redirectUrl = "DataQuery_G5310.aspx";

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
    /// 編輯按鈕
    /// </summary>
    //protected void BtnEdit_Click(object sender, EventArgs e)
    //{
    //    if (ViewState["ViewId"] == null) return;

    //    int id = (int)ViewState["ViewId"];
    //    string sid = Request.QueryString["sid"] ?? "";
    //    Response.Redirect(string.Format("DataAdd_G5310.aspx?id={0}&sid={1}", id, sid));
    //}

    /// <summary>
    /// 放行按鈕 - 發送 EAI 電文
    /// </summary>
    private void ReleaseData(int id)
    {
        try
        {
            if (!IsManager)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error",
                    "alert('您沒有放行權限，僅主管可執行放行作業');", true);
                return;
            }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // 1. 取得完整資料
                string getSql = @"SELECT * FROM tbl_LargeMoney_G5310 
                             WHERE ID = @ID AND IsDeleted = 0";

                int kind = 0;
                DateTime reportDate = DateTime.Now;
                int srno = 0;
                string unino = "";
                DateTime stday = DateTime.Now;
                string ibrno = "";
                string acbrno = "";
                string curcd = "";
                string bkcur = "";
                string bksrno = "";
                decimal amt = 0;
                int iocd = 0;
                string amtwhere = "";
                string reason = "";
                string empno = "";
                string empnm = "";

                using (SqlCommand getCmd = new SqlCommand(getSql, conn))
                {
                    getCmd.Parameters.AddWithValue("@ID", id);
                    using (SqlDataReader reader = getCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int currentStatus = Convert.ToInt32(reader["Status"]);
                            if (currentStatus != 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "error",
                                    "alert('只有待放行狀態的資料才能放行');", true);
                                return;
                            }

                            kind = Convert.ToInt32(reader["KIND"]);
                            reportDate = Convert.ToDateTime(reader["REPORT_DATE"]);
                            srno = Convert.ToInt32(reader["SRNO"]);
                            unino = reader["UNINO"].ToString().Trim();
                            stday = Convert.ToDateTime(reader["STDAY"]);
                            ibrno = reader["IBRNO"].ToString().Trim();
                            acbrno = reader["ACBRNO"].ToString().Trim();
                            curcd = reader["CURCD"].ToString().Trim();
                            bkcur = reader["BKCUR"] != DBNull.Value ? reader["BKCUR"].ToString().Trim() : "";
                            bksrno = reader["BKSRNO"] != DBNull.Value ? reader["BKSRNO"].ToString().Trim() : "";
                            amt = Convert.ToDecimal(reader["AMT"]);
                            iocd = Convert.ToInt32(reader["IOCD"]);
                            amtwhere = reader["AMTWHERE"].ToString();
                            reason = reader["REASON"].ToString();
                            empno = reader["EMPNO"].ToString().Trim();
                            empnm = reader["EMPNM"].ToString().Trim();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                                "alert('找不到該筆資料');", true);
                            return;
                        }
                    }
                }

                // ★★★ 2. 資料驗證 ★★★
                // 統編必須至少 10 碼
                if (unino.Length < 10)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error",
                        string.Format("alert('統編長度不足，至少需要 10 碼。\\n目前統編：{0} ({1}碼)');", unino, unino.Length), true);
                    return;
                }

                // ★★★ 3. 組裝統編欄位 ★★★
                string cifkey;
                string unino11;
                string ciferr;

                if (unino.Length >= 11)
                {
                    cifkey = unino.Substring(0, 10);
                    unino11 = unino.Substring(0, 11);
                    ciferr = unino.Substring(10, 1);
                }
                else // unino.Length == 10
                {
                    cifkey = unino;
                    unino11 = unino + "0";
                    ciferr = "0";
                }

                // ★★★ 4. 處理 BKCUR (存匯行幣別) ★★★
                // 如果沒有值或是 00，使用主幣別
                if (string.IsNullOrEmpty(bkcur) || bkcur == "00" || bkcur == "0")
                {
                    bkcur = curcd;
                }

                // 5. 發送 EAI 電文
                G5310 g5310 = new G5310(empid, "LargeMoney_G5310");

                // EAI Header
                g5310.dic_Rq.Add("KINBR", ibrno.PadLeft(4, '0'));
                g5310.dic_Rq.Add("ACFLG", "0");

                // EAI Body (按成功電文順序)
                g5310.dic_Rq.Add("KIND", kind.ToString());
                g5310.dic_Rq.Add("APTYPE", "NE");
                g5310.dic_Rq.Add("DATE", reportDate.ToString("yyyyMMdd"));
                g5310.dic_Rq.Add("SRNO", "0000");  // 新增時固定 0000
                g5310.dic_Rq.Add("CIFKEY", cifkey);
                g5310.dic_Rq.Add("UNINO", unino11);
                g5310.dic_Rq.Add("CIFERR", ciferr);
                g5310.dic_Rq.Add("STDAY", stday.ToString("yyyyMMdd"));
                g5310.dic_Rq.Add("IBRNO", ibrno.PadLeft(4, '0'));
                g5310.dic_Rq.Add("ACBRNO", acbrno.PadLeft(4, '0'));
                g5310.dic_Rq.Add("CURCD", curcd.PadLeft(2, '0'));
                g5310.dic_Rq.Add("BKCUR", curcd.PadLeft(2, '0'));//改成跟幣別一樣
                g5310.dic_Rq.Add("BKSRNO", string.IsNullOrEmpty(bksrno) ? "000" : bksrno.PadLeft(3, '0'));
                g5310.dic_Rq.Add("AMT", FormatAmount(amt));
                g5310.dic_Rq.Add("IOCD", iocd.ToString());
                g5310.dic_Rq.Add("AMTWHERE", PadRightByByte(amtwhere, 40));
                g5310.dic_Rq.Add("REASON", PadRightByByte(reason, 80));
                g5310.dic_Rq.Add("EMPNO", PadRightByByte(empno, 6));
                g5310.dic_Rq.Add("EMPNM", PadRightByByte(empnm, 10));
                g5310.dic_Rq.Add("END", "$");

                g5310.GetRS();

                // 6. 檢查 EAI 結果
                if (!g5310.isSuccess || g5310.RspCode != "G5310")
                {
                    StringBuilder errDetail = new StringBuilder();
                    errDetail.AppendFormat("isSuccess: {0}\\n", g5310.isSuccess);
                    errDetail.AppendFormat("RspCode: {0}\\n", g5310.RspCode ?? "(null)");
                    if (!string.IsNullOrEmpty(g5310.Desc))
                        errDetail.AppendFormat("Desc:  {0}\\n", g5310.Desc);
                    if (!string.IsNullOrEmpty(g5310.STATUS))
                        errDetail.AppendFormat("STATUS: {0}\\n", g5310.STATUS);

                    // 顯示送出的關鍵欄位，方便除錯
                    errDetail.AppendFormat("\\n--- 送出的資料 ---\\n");
                    errDetail.AppendFormat("CURCD: {0}\\n", curcd);
                    errDetail.AppendFormat("BKCUR:  {0}\\n", bkcur);
                    errDetail.AppendFormat("UNINO: {0}\\n", unino11);

                    ScriptManager.RegisterStartupScript(this, GetType(), "error",
                        string.Format("alert('EAI電文發送失敗：\\n{0}');",
                            errDetail.ToString().Replace("'", "\\'")), true);
                    return;
                }

                // 7. 檢查主機回傳狀態
                if (g5310.STATUS == "2")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error",
                        "alert('主機回傳失敗：狀態為無效');", true);
                    return;
                }

                // 8. 取得主機回傳資料
                string hostSrno = g5310.ReturnSRNO;
                string hostStatus = g5310.STATUS;

                // 9. 更新資料庫狀態
                string updateSql = @"UPDATE tbl_LargeMoney_G5310 
                                SET Status = 1, 
                                    Approved_By = @Approved_By, 
                                    Approved_Time = @Approved_Time,
                                    Modified_By = @Modified_By,
                                    Modified_Time = @Modified_Time,
                                    Host_SRNO = @Host_SRNO,
                                    Host_STATUS = @Host_STATUS
                                WHERE ID = @ID AND IsDeleted = 0 AND Status = 0";

                using (SqlCommand updateCmd = new SqlCommand(updateSql, conn))
                {
                    updateCmd.Parameters.AddWithValue("@ID", id);
                    updateCmd.Parameters.AddWithValue("@Approved_By", empName);
                    updateCmd.Parameters.AddWithValue("@Approved_Time", DateTime.Now);
                    updateCmd.Parameters.AddWithValue("@Modified_By", empName);
                    updateCmd.Parameters.AddWithValue("@Modified_Time", DateTime.Now);
                    updateCmd.Parameters.AddWithValue("@Host_SRNO", hostSrno ?? "");
                    updateCmd.Parameters.AddWithValue("@Host_STATUS", hostStatus ?? "");

                    int affected = updateCmd.ExecuteNonQuery();

                    if (affected > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "success",
                            string.Format("alert('放行成功！\\n主機案件編號：{0}');", hostSrno), true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "error",
                            "alert('放行失敗，資料可能已被其他人處理');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                "alert('放行失敗:  " + ex.Message.Replace("'", "\\'") + "');", true);
        }
    }

    // 需要新增這兩個輔助方法
    private string FormatAmount(decimal amount)
    {
        long intPart = (long)Math.Truncate(Math.Abs(amount));
        int decPart = (int)(Math.Abs(amount - Math.Truncate(amount)) * 100);
        return intPart.ToString().PadLeft(12, '0') + decPart.ToString().PadLeft(2, '0');
    }

    private string PadRightByByte(string value, int byteLength)
    {
        if (string.IsNullOrEmpty(value))
            return new string(' ', byteLength);

        int currentByteLen = System.Text.Encoding.Default.GetByteCount(value);

        if (currentByteLen >= byteLength)
        {
            int charLen = 0;
            int byteCount = 0;
            foreach (char c in value)
            {
                int charBytes = System.Text.Encoding.Default.GetByteCount(c.ToString());
                if (byteCount + charBytes > byteLength)
                    break;
                byteCount += charBytes;
                charLen++;
            }
            return value.Substring(0, charLen);
        }

        return value + new string(' ', byteLength - currentByteLen);
    }

}