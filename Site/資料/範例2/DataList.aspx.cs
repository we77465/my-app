
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Program_LargeMoney_DataList : System.Web.UI.Page
{
   
    // 使用 bee 資料庫的連接字符串
    private string _connectionString = ConfigurationManager.ConnectionStrings["bee"].ConnectionString;
    private string empid = "";
    private string brno = "";
    private string managerLevel = "0000";
    private string name = "測試中";
    protected void Page_Load(object sender, EventArgs e)
    {
        empid = UserInfo.getUserId(Request.QueryString["sid"].ToString(), Request);//員編
        brno = UserInfo.getUserBrNoAndLevel(empid).Split(',')[0];//BRNO 
        //managerLevel = UserInfo.getUserBrNoAndLevel(empid).Split(',')[1];// Maneger Level
        //name = UserInfo.getUserBrNoAndLevel(empid).Split(',')[2];//name
        if (!IsPostBack)
        {
            BindGridView();
        }
    }

    /// <summary>
    /// 綁定 GridView 數據
    /// </summary>
    private void BindGridView()
    {
        try
        {
            DataTable dt = GetLargeMoneyData();
            gvLargeMoneyData.DataSource = dt;
            gvLargeMoneyData.DataBind();

            // 顯示記錄數量
            if (dt != null && dt.Rows.Count > 0)
            {
                lblRecordCount.Text = string.Format("共查詢到 {0} 筆資料", dt.Rows.Count);
            }
            else
            {
                lblRecordCount.Text = "查無資料";
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    /// <summary>
    /// 從資料庫獲取數據
    /// </summary>
    private DataTable GetLargeMoneyData()
    {
        DataTable dt = new DataTable();

        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                StringBuilder sql = new StringBuilder();
                sql.Append(@"SELECT ID, Tax_ID_Number, Customer_Name, Notify_Date, Delivery_Date, 
                            Notify_Bank, Account_Bank, Currency, Bank_Code, Amount, 
                            Export_Import, Funds_Sources_Destinations, Change_Reason, 
                            Created_By, Created_Time 
                            FROM tbl_LargeMoney_DataAdd 
                            WHERE IsDeleted = 0");

                // 動態添加查詢條件
                if (!string.IsNullOrWhiteSpace(txtSearchTaxID.Text))
                {
                    sql.Append(" AND Tax_ID_Number LIKE @Tax_ID_Number");
                }

                if (!string.IsNullOrWhiteSpace(txtSearchCustomerName.Text))
                {
                    sql.Append(" AND Customer_Name LIKE @Customer_Name");
                }

                if (!string.IsNullOrWhiteSpace(txtSearchCurrency.Text))
                {
                    sql.Append(" AND Currency = @Currency");
                }

                if (!string.IsNullOrWhiteSpace(ddlSearchExportImport.SelectedValue))
                {
                    sql.Append(" AND Export_Import = @Export_Import");
                }

                if (!string.IsNullOrWhiteSpace(txtSearchNotifyDateStart.Text))
                {
                    sql.Append(" AND Notify_Date >= @Notify_Date_Start");
                }

                if (!string.IsNullOrWhiteSpace(txtSearchNotifyDateEnd.Text))
                {
                    sql.Append(" AND Notify_Date <= @Notify_Date_End");
                }

                if (!string.IsNullOrWhiteSpace(txtSearchDeliveryDateStart.Text))
                {
                    sql.Append(" AND Delivery_Date >= @Delivery_Date_Start");
                }

                if (!string.IsNullOrWhiteSpace(txtSearchDeliveryDateEnd.Text))
                {
                    sql.Append(" AND Delivery_Date <= @Delivery_Date_End");
                }

                sql.Append(" ORDER BY Created_Time DESC");

                using (SqlCommand cmd = new SqlCommand(sql.ToString(), conn))
                {
                    // 參數綁定
                    if (!string.IsNullOrWhiteSpace(txtSearchTaxID.Text))
                    {
                        cmd.Parameters.AddWithValue("@Tax_ID_Number", "%" + txtSearchTaxID.Text.Trim() + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(txtSearchCustomerName.Text))
                    {
                        cmd.Parameters.AddWithValue("@Customer_Name", "%" + txtSearchCustomerName.Text.Trim() + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(txtSearchCurrency.Text))
                    {
                        cmd.Parameters.AddWithValue("@Currency", txtSearchCurrency.Text.Trim());
                    }

                    if (!string.IsNullOrWhiteSpace(ddlSearchExportImport.SelectedValue))
                    {
                        cmd.Parameters.AddWithValue("@Export_Import", ddlSearchExportImport.SelectedValue);
                    }

                    if (!string.IsNullOrWhiteSpace(txtSearchNotifyDateStart.Text))
                    {
                        cmd.Parameters.AddWithValue("@Notify_Date_Start", DateTime.Parse(txtSearchNotifyDateStart.Text));
                    }

                    if (!string.IsNullOrWhiteSpace(txtSearchNotifyDateEnd.Text))
                    {
                        cmd.Parameters.AddWithValue("@Notify_Date_End", DateTime.Parse(txtSearchNotifyDateEnd.Text));
                    }

                    if (!string.IsNullOrWhiteSpace(txtSearchDeliveryDateStart.Text))
                    {
                        cmd.Parameters.AddWithValue("@Delivery_Date_Start", DateTime.Parse(txtSearchDeliveryDateStart.Text));
                    }

                    if (!string.IsNullOrWhiteSpace(txtSearchDeliveryDateEnd.Text))
                    {
                        cmd.Parameters.AddWithValue("@Delivery_Date_End", DateTime.Parse(txtSearchDeliveryDateEnd.Text));
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return dt;
    }

    /// <summary>
    /// 查詢按鈕點擊事件
    /// </summary>
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        gvLargeMoneyData.PageIndex = 0; // 重置到第一頁
        BindGridView();
    }

    /// <summary>
    /// 清除查詢條件
    /// </summary>
    protected void BtnClearSearch_Click(object sender, EventArgs e)
    {
        txtSearchTaxID.Text = string.Empty;
        txtSearchCustomerName.Text = string.Empty;
        txtSearchCurrency.Text = string.Empty;
        ddlSearchExportImport.SelectedIndex = 0;
        txtSearchNotifyDateStart.Text = string.Empty;
        txtSearchNotifyDateEnd.Text = string.Empty;
        txtSearchDeliveryDateStart.Text = string.Empty;
        txtSearchDeliveryDateEnd.Text = string.Empty;

        gvLargeMoneyData.PageIndex = 0;
        BindGridView();
    }

    /// <summary>
    /// 新增資料按鈕
    /// </summary>
    protected void BtnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("DataAdd.aspx?sid="+ Request.QueryString["sid"].ToString());
    }

    /// <summary>
    /// GridView 分頁事件
    /// </summary>
    protected void GvLargeMoneyData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLargeMoneyData.PageIndex = e.NewPageIndex;
        BindGridView();
    }

    /// <summary>
    /// GridView 行命令事件
    /// </summary>
    protected void GvLargeMoneyData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRow")
            {
                // 跳轉到編輯頁面
                Response.Redirect("DataAdd.aspx?id=" + id + "&sid=" + Request.QueryString["sid"].ToString());
            }
            else if (e.CommandName == "DeleteRow")
            {
                // 執行刪除（軟刪除）
                if (DeleteLargeMoneyData(id))
                {
                    ShowMessage("刪除成功");
                    BindGridView();
                }
                else
                {
                    ShowMessage("刪除失敗");
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    /// <summary>
    /// GridView 行數據綁定事件
    /// </summary>
    protected void GvLargeMoneyData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // 根據匯出/匯入類型設置金額顏色
            string exportImport = DataBinder.Eval(e.Row.DataItem, "Export_Import").ToString();
            TableCell amountCell = e.Row.Cells[9]; // 金額欄位

            if (exportImport == "Export")
            {
                amountCell.CssClass = "amount-export";
            }
            else if (exportImport == "Import")
            {
                amountCell.CssClass = "amount-import";
            }

            // 轉換匯出/匯入顯示文字
            TableCell exportImportCell = e.Row.Cells[10];
            if (exportImport == "Export")
            {
                exportImportCell.Text = "匯出";
            }
            else if (exportImport == "Import")
            {
                exportImportCell.Text = "匯入";
            }
        }
    }

    /// <summary>
    /// 刪除數據（軟刪除）
    /// </summary>
    private bool DeleteLargeMoneyData(int id)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = @"UPDATE tbl_LargeMoney_DataAdd 
                              SET IsDeleted = 1, 
                                  --Modified_By = @Modified_By, 
                                  Modified_Time = @Modified_Time 
                              WHERE ID = @ID";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Modified_By", GetCurrentUserName());
                    cmd.Parameters.AddWithValue("@Modified_Time", DateTime.Now);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// 顯示提示訊息
    /// </summary>
    private void ShowMessage(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "message",
            string.Format("alert('{0}');", message), true);
    }

    /// <summary>
    /// 獲取當前登入用戶名
    /// </summary>
    private string GetCurrentUserName()
    {
        return name;
    }
}