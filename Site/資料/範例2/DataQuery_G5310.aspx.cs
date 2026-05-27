using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Emit;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Program_LargeMoney_DataQuery_G5310 : System.Web.UI.Page
{
    private string _connectionString = ConfigurationManager.ConnectionStrings["bee"].ConnectionString;

    // 使用者資訊
    private string empid = "";
    private string brno = "";
    private string managerLevel = "";
    private string empName = "";

    // 是否為主管 (05, 09)
    private bool IsManager
    {
        get { return (managerLevel == "05" || managerLevel == "09"); }
    }

    // 幣別對照字典
    private static Dictionary<string, string> _currencyDict;
    private static Dictionary<string, string> _branchDict;
    protected void Page_Load(object sender, EventArgs e)
    {

        
        // brno = UserInfo.getUserBrNoAndLevel(empid). Split(',')[0];
        // managerLevel = UserInfo.getUserBrNoAndLevel(empid).Split(',')[1];
        // empName = UserInfo.getUserBrNoAndLevel(empid). Split(',')[2];
        //string soInfo = UserInfo.getUserBrNoAndLevel(empid);
        //Char delimiter = ',';
        //String[] arrInfo = soInfo.Split(delimiter);
        //if (arrInfo.Length == 3)
        //{
        //    brno = arrInfo[0];
        //    empName = arrInfo[1];
        //    managerLevel = arrInfo[2];
        //}



        // 取得使用者資訊
        try
        {
            string sid = Request.QueryString["SID"];

            if (!string.IsNullOrEmpty(sid))
            {
                empid = UserInfo.getUserId(Request.QueryString["sid"].ToString(), Request);
                //empid = UserInfo.getUserId(sid, Request);
                //empid = "150128";
                if (!string.IsNullOrEmpty(empid))
                {
                    string sInfo = UserInfo.getUserBrNoAndLevel(empid);

                    if (!string.IsNullOrEmpty(sInfo))
                    {
                        string[] arrInfo = sInfo.Split(',');

                        if (arrInfo.Length >= 3)
                        {
                            brno = arrInfo[0].Trim();
                            empName = arrInfo[1].Trim();
                            managerLevel = arrInfo[2].Trim();
                        }
                        else
                        {
                            // 資料不完整，記錄錯誤
                            System.Diagnostics.Debug.WriteLine(
                                string.Format("getUserBrNoAndLevel 回傳資料不完整: empid={0}, sInfo={1}", empid, sInfo));
                        }
                    }
                    else
                    {
                        // 查無員工資料
                        System.Diagnostics.Debug.WriteLine(
                            string.Format("查無員工資料: empid={0}", empid));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("取得使用者資訊錯誤: " + ex.Message);
        }

        LoadBranchDict();
        LoadCurrencyDict();
        if (!IsPostBack)
        {
            DisplayUserInfo();
            LoadCurrencyDropDown();
            LoadBranchDropDown();
            SetDefaultSearchDate();
        }
    }


    private void LoadCurrencyDict()
    {
        if (_currencyDict != null && _currencyDict.Count > 0)
            return;

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
                        string value = reader["CURCD"].ToString().Trim();
                        string name = reader["CurrencyName"].ToString();

                        if (!_currencyDict.ContainsKey(value))
                            _currencyDict.Add(value, name);
                    }
                }
            }
        }
        catch
        {
            _currencyDict.Add("01", "新台幣");
            _currencyDict.Add("02", "美元");
            _currencyDict.Add("09", "日圓");
            _currencyDict.Add("16", "歐元");
            _currencyDict.Add("20", "人民幣");
        }
    }
    private void LoadBranchDict()
    {
        if (_branchDict != null && _branchDict.Count > 0)
            return;

        _branchDict = new Dictionary<string, string>();

        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "USE chb_pub SELECT BRNO, BRNAME FROM BRANCH";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string brnoValue = reader["BRNO"].ToString().Trim();
                        string brname = reader["BRNAME"].ToString().Trim();

                        if (!_branchDict.ContainsKey(brnoValue))
                            _branchDict.Add(brnoValue, brname);
                    }
                }
            }
        }
        catch
        {
            _branchDict.Add("0501", "總行營業部");
        }
    }

    private void DisplayUserInfo()
    {
        lblEmpId.Text = empid;
        lblEmpName.Text = empName;
        lblBrno.Text = brno;

        if (IsManager)
        {
            lblRole.Text = "主管";
            lblRole.CssClass = "role-manager";
        }
        else
        {
            lblRole.Text = "一般人員";
            lblRole.CssClass = "role-normal";
        }
    }

    private void LoadCurrencyDropDown()
    {
        _currencyDict = new Dictionary<string, string>();

        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT CURCD, CurrencyName, CurrencyCode FROM tbl_Currency_Code ORDER BY CURCD";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    ddlSearchCurcd.Items.Clear();
                    ddlSearchCurcd.Items.Add(new ListItem("全部", ""));

                    while (reader.Read())
                    {
                        string value = reader["CURCD"].ToString().Trim();
                        string name = reader["CurrencyName"].ToString();
                        string text = string.Format("{0}-{1}", value, name);

                        ddlSearchCurcd.Items.Add(new ListItem(text, value));

                        if (!_currencyDict.ContainsKey(value))
                            _currencyDict.Add(value, name);
                    }
                }
            }
        }
        catch
        {
            ddlSearchCurcd.Items.Clear();
            ddlSearchCurcd.Items.Add(new ListItem("全部", ""));
            ddlSearchCurcd.Items.Add(new ListItem("01-新台幣", "01"));
            ddlSearchCurcd.Items.Add(new ListItem("02-美元", "02"));
            ddlSearchCurcd.Items.Add(new ListItem("09-日圓", "09"));
            ddlSearchCurcd.Items.Add(new ListItem("16-歐元", "16"));
            ddlSearchCurcd.Items.Add(new ListItem("20-人民幣", "20"));

            _currencyDict = new Dictionary<string, string>();
            _currencyDict.Add("01", "新台幣");
            _currencyDict.Add("02", "美元");
            _currencyDict.Add("09", "日圓");
            _currencyDict.Add("16", "歐元");
            _currencyDict.Add("20", "人民幣");
        }
    }

    private void SetDefaultSearchDate()
    {
        txtReportDateFrom.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
        txtReportDateTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
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
                    ddlSearchIbrno.Items.Clear();
                    ddlSearchIbrno.Items.Add(new ListItem("全部", ""));

                    ddlSearchAcbrno.Items.Clear();
                    ddlSearchAcbrno.Items.Add(new ListItem("全部", ""));

                    while (reader.Read())
                    {
                        string brnoValue = reader["BRNO"].ToString().Trim();
                        string brname = reader["BRNAME"].ToString().Trim();
                        string displayText = brnoValue + " - " + brname;

                        ddlSearchIbrno.Items.Add(new ListItem(displayText, brnoValue));
                        ddlSearchAcbrno.Items.Add(new ListItem(displayText, brnoValue));
                    }
                }
            }
        }
        catch
        {
            ddlSearchIbrno.Items.Clear();
            ddlSearchIbrno.Items.Add(new ListItem("全部", ""));
            ddlSearchIbrno.Items.Add(new ListItem("0501 - 總行營業部", "0501"));

            ddlSearchAcbrno.Items.Clear();
            ddlSearchAcbrno.Items.Add(new ListItem("全部", ""));
            ddlSearchAcbrno.Items.Add(new ListItem("0501 - 總行營業部", "0501"));
        }
    }

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        BindGridData();
    }

    private void BindGridData()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                StringBuilder sql = new StringBuilder();
                sql.Append(@"SELECT ID, KIND, APTYPE, REPORT_DATE, SRNO, UNINO, Customer_Name,
                STDAY, IBRNO, ACBRNO, CURCD, BKCUR, BKSRNO, AMT, IOCD,
                AMTWHERE, REASON, EMPNO, EMPNM, Status, Created_Time,
                Host_SRNO, Created_By,BRNO
                FROM tbl_LargeMoney_G5310 
                WHERE IsDeleted = 0 ");

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;

                    if (!string.IsNullOrEmpty(txtReportDateFrom.Text))
                    {
                        sql.Append(" AND REPORT_DATE >= @ReportDateFrom ");
                        cmd.Parameters.AddWithValue("@ReportDateFrom", DateTime.Parse(txtReportDateFrom.Text));
                    }
                    if (!string.IsNullOrEmpty(txtReportDateTo.Text))
                    {
                        sql.Append(" AND REPORT_DATE <= @ReportDateTo ");
                        cmd.Parameters.AddWithValue("@ReportDateTo", DateTime.Parse(txtReportDateTo.Text));
                    }
                    if (!string.IsNullOrEmpty(txtStdayFrom.Text))
                    {
                        sql.Append(" AND STDAY >= @StdayFrom ");
                        cmd.Parameters.AddWithValue("@StdayFrom", DateTime.Parse(txtStdayFrom.Text));
                    }
                    if (!string.IsNullOrEmpty(txtStdayTo.Text))
                    {
                        sql.Append(" AND STDAY <= @StdayTo ");
                        cmd.Parameters.AddWithValue("@StdayTo", DateTime.Parse(txtStdayTo.Text));
                    }
                    if (!string.IsNullOrEmpty(txtSearchUnino.Text.Trim()))
                    {
                        sql.Append(" AND UNINO LIKE @Unino ");
                        cmd.Parameters.AddWithValue("@Unino", "%" + txtSearchUnino.Text.Trim() + "%");
                    }
                    if (!string.IsNullOrEmpty(txtSearchCustomerName.Text.Trim()))
                    {
                        sql.Append(" AND Customer_Name LIKE @CustomerName ");
                        cmd.Parameters.AddWithValue("@CustomerName", "%" + txtSearchCustomerName.Text.Trim() + "%");
                    }
                    if (!string.IsNullOrEmpty(txtSearchSrno.Text.Trim()))
                    {
                        int ID;
                        if (int.TryParse(txtSearchSrno.Text.Trim(), out ID))
                        {
                            sql.Append(" AND ID = @ID ");
                            cmd.Parameters.AddWithValue("@ID", ID);
                        }
                    }
                    if (!string.IsNullOrEmpty(ddlSearchIbrno.SelectedValue))
                    {
                        sql.Append(" AND IBRNO = @Ibrno ");
                        cmd.Parameters.AddWithValue("@Ibrno", ddlSearchIbrno.SelectedValue);
                    }
                    if (!string.IsNullOrEmpty(ddlSearchAcbrno.SelectedValue))
                    {
                        sql.Append(" AND ACBRNO = @Acbrno ");
                        cmd.Parameters.AddWithValue("@Acbrno", ddlSearchAcbrno.SelectedValue);
                    }
                    if (!string.IsNullOrEmpty(ddlSearchCurcd.SelectedValue))
                    {
                        sql.Append(" AND CURCD = @Curcd ");
                        cmd.Parameters.AddWithValue("@Curcd", ddlSearchCurcd.SelectedValue);
                    }
                    if (!string.IsNullOrEmpty(ddlSearchIocd.SelectedValue))
                    {
                        sql.Append(" AND IOCD = @Iocd ");
                        cmd.Parameters.AddWithValue("@Iocd", int.Parse(ddlSearchIocd.SelectedValue));
                    }
                    if (!string.IsNullOrEmpty(ddlSearchStatus.SelectedValue))
                    {
                        sql.Append(" AND Status = @Status ");
                        cmd.Parameters.AddWithValue("@Status", int.Parse(ddlSearchStatus.SelectedValue));
                    }

                    sql.Append(" ORDER BY REPORT_DATE DESC, SRNO DESC ");

                    cmd.CommandText = sql.ToString();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        gvData.DataSource = dt;
                        gvData.DataBind();

                        lblResultInfo.Text = string.Format("共查詢到 {0} 筆資料", dt.Rows.Count);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblResultInfo.Text = "查詢發生錯誤";
        }
    }

    protected void GvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            // 取得狀態值
            int status = 0;
            if (drv["Status"] != DBNull.Value)
            {
                status = Convert.ToInt32(drv["Status"]);
            }

            // 取得建立者
            string EMPNO = "";
            string BRNO = "";
            if (drv["EMPNO"] != DBNull.Value)
            {
                EMPNO = drv["EMPNO"].ToString().Trim();
            }
            if (drv["BRNO"] != DBNull.Value)
            {
                BRNO = drv["BRNO"].ToString().Trim();
            }

            // 判斷是否為建立者
            bool isCreator = (EMPNO == empid);

            // ★★★ 加回：處理狀態欄位 ★★★
            System.Web.UI.WebControls.Label lblStatus = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblStatus");
            if (lblStatus != null)
            {
                switch (status)
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
            }

            // 按鈕控制
            LinkButton lnkView = (LinkButton)e.Row.FindControl("lnkView");
            LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");
            LinkButton lnkRelease = (LinkButton)e.Row.FindControl("lnkRelease");
            LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");

            if (lnkView != null)
                lnkView.Visible = true;

            if (lnkEdit != null)
                lnkEdit.Visible = (status == 0 && isCreator);

            if (lnkRelease != null)
                lnkRelease.Visible = (status == 0 && IsManager&&BRNO== brno); //BRNO為資料庫的 brno為現在登入者

            if (lnkDelete != null)
                lnkDelete.Visible = (status == 0 && isCreator);

            // 處理通報行欄位
            System.Web.UI.WebControls.Label lblIbrno = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblIbrno");
            if (lblIbrno != null && drv["IBRNO"] != DBNull.Value)
            {
                string ibrnoCode = drv["IBRNO"].ToString().Trim();
                lblIbrno.Text = (_branchDict != null && _branchDict.ContainsKey(ibrnoCode))
                    ? _branchDict[ibrnoCode]
                    : ibrnoCode;
            }

            // 處理掛帳行欄位
            System.Web.UI.WebControls.Label lblAcbrno = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblAcbrno");
            if (lblAcbrno != null && drv["ACBRNO"] != DBNull.Value)
            {
                string acbrnoCode = drv["ACBRNO"].ToString().Trim();
                lblAcbrno.Text = (_branchDict != null && _branchDict.ContainsKey(acbrnoCode))
                    ? _branchDict[acbrnoCode]
                    : acbrnoCode;
            }

            // ★★★ 處理幣別欄位 (索引改為 7) ★★★
            if (drv["CURCD"] != DBNull.Value)
            {
                string curcd = drv["CURCD"].ToString().Trim();
                if (_currencyDict != null && _currencyDict.ContainsKey(curcd))
                {
                    e.Row.Cells[7].Text = _currencyDict[curcd];
                }
            }

            // 處理金額欄位
            System.Web.UI.WebControls.Label lblAmt = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblAmt");
            if (lblAmt != null && drv["AMT"] != DBNull.Value)
            {
                decimal amt = Convert.ToDecimal(drv["AMT"]);
                lblAmt.Text = amt.ToString("N2");

                if (drv["IOCD"] != DBNull.Value)
                {
                    int iocd = Convert.ToInt32(drv["IOCD"]);
                    lblAmt.CssClass = (iocd == 1) ? "amount-in" : "amount-out";
                }
            }

            // ★★★ 處理匯出/匯入欄位 (索引改為 9) ★★★
            if (drv["IOCD"] != DBNull.Value)
            {
                int iocd = Convert.ToInt32(drv["IOCD"]);
                e.Row.Cells[9].Text = (iocd == 1) ? "匯入" : "匯出";
            }
        }
    }

    protected void GvData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Page") return;

        // ★ 直接從 CommandArgument 取得 ID，不用再透過 DataKeys
        int id = Convert.ToInt32(e.CommandArgument);
        string sid = Request.QueryString["sid"] ?? "";

        switch (e.CommandName)
        {
            case "ViewDetail":
                Response.Redirect(string.Format("DataAdd_G5310.aspx?id={0}&sid={1}&Edit=false", id, sid));
                break;

            case "EditData":
                Response.Redirect(string.Format("DataAdd_G5310.aspx?id={0}&sid={1}", id, sid));
                break;

            case "ReleaseData":
                ReleaseData(id);
                BindGridData();
                break;

            case "DeleteData":
                DeleteData(id);
                BindGridData();
                break;
        }
    }

    /// <summary>
    /// 放行資料 - 發送 EAI 電文
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
                //if (unino.Length < 10)
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "error",
                //        string.Format("alert('統編長度不足，至少需要 10 碼。\\n目前統編：{0} ({1}碼)');", unino, unino.Length), true);
                //    return;
                //}

                // ★★★ 3. 組裝統編欄位 ★★★
                string cifkey;
                string unino11;
                string ciferr = "";

                if (unino.Length >= 11)
                {
                    cifkey = unino.Substring(0, 10);
                    unino11 = unino.Substring(0, 11);
                }
                else // unino.Length == 10
                {
                    cifkey = unino;
                    unino11 = unino + "0";
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
                g5310.dic_Rq.Add("BKCUR", bkcur.PadLeft(2, '0'));
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
                                    Host_STATUS = @Host_STATUS,
                                    REPORT_DATE = @Report_Date
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
                    updateCmd.Parameters.AddWithValue("@Report_Date", DateTime.Now);

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

    private void DeleteData(int id)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // ★★★ 檢查狀態和建立者 ★★★
                string checkSql = "SELECT Status, EMPNO FROM tbl_LargeMoney_G5310 WHERE ID = @ID AND IsDeleted = 0";
                using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@ID", id);
                    using (SqlDataReader reader = checkCmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                                "alert('找不到該筆資料');", true);
                            return;
                        }

                        int currentStatus = Convert.ToInt32(reader["Status"]);
                        string createdBy = reader["EMPNO"].ToString().Trim() ?? "";

                        if (currentStatus != 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                                "alert('只有待放行狀態的資料才能刪除');", true);
                            return;
                        }

                        // ★★★ 驗證是否為建立者 ★★★
                        if (createdBy != empid)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                                "alert('只有資料建立者才能刪除此筆資料');", true);
                            return;
                        }
                    }
                }

                // 執行刪除
                string sql = @"UPDATE tbl_LargeMoney_G5310 
                          SET IsDeleted = 1, 
                              Modified_By = @Modified_By, 
                              Modified_Time = @Modified_Time 
                          WHERE ID = @ID AND Status = 0 AND EMPNO = @EMPNO";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Modified_By", empid);
                    cmd.Parameters.AddWithValue("@Modified_Time", DateTime.Now);
                    cmd.Parameters.AddWithValue("@EMPNO", empid);

                    int affected = cmd.ExecuteNonQuery();

                    if (affected > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "success",
                            "alert('刪除成功');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "error",
                            "alert('刪除失敗');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                "alert('刪除失敗:  " + ex.Message.Replace("'", "\\'") + "');", true);
        }
    }


    protected void BtnClear_Click(object sender, EventArgs e)
    {
        txtReportDateFrom.Text = "";
        txtReportDateTo.Text = "";
        txtStdayFrom.Text = "";
        txtStdayTo.Text = "";
        txtSearchUnino.Text = "";
        txtSearchCustomerName.Text = "";
        txtSearchSrno.Text = "";
        ddlSearchIbrno.SelectedIndex = 0;
        ddlSearchAcbrno.SelectedIndex = 0;
        ddlSearchCurcd.SelectedIndex = 0;
        ddlSearchIocd.SelectedIndex = 0;
        ddlSearchStatus.SelectedIndex = 0;

        gvData.DataSource = null;
        gvData.DataBind();
        lblResultInfo.Text = "請輸入查詢條件後按查詢按鈕";
    }

    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        string redirectUrl = "DataAdd_G5310.aspx";
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

    protected void BtnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvData.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "alert('請先查詢資料後再匯出');", true);
                return;
            }

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
                "attachment;filename=LargeMoney_G5310_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write("<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />");

            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    gvData.AllowPaging = false;
                    BindGridData();

                    foreach (GridViewRow row in gvData.Rows)
                    {
                        row.Cells[0].Visible = false;
                    }
                    if (gvData.HeaderRow != null)
                    {
                        gvData.HeaderRow.Cells[0].Visible = false;
                    }

                    gvData.RenderControl(hw);

                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (System.Threading.ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                "alert('匯出失敗: " + ex.Message.Replace("'", "\\'") + "');", true);
        }
        finally
        {
            gvData.AllowPaging = true;
            BindGridData();
        }
    }

    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvData.PageIndex = e.NewPageIndex;
        BindGridData();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    private string FormatAmount(decimal amount)
    {
        long intPart = (long)Math.Truncate(Math.Abs(amount));
        int decPart = (int)(Math.Abs(amount - Math.Truncate(amount)) * 100);

        if (decPart == 0)
        {
            // 例如 800000.00 → 000000800000
            return intPart.ToString().PadLeft(12, '0');
        }
        else
        {
            // 例如 800000.12 → 000800000.12
            return intPart.ToString().PadLeft(12, '0') + "." + decPart.ToString().PadLeft(2, '0');
        }
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