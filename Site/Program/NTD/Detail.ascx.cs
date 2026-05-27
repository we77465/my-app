using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

/// <summary>
/// 新臺幣定期存款專案利率申請 - 申請單明細顯示 UserControl。
/// 同時顯示主檔欄位、利率明細與簽核紀錄；由 View / Approve 共用。
/// </summary>
public partial class NtdApplyDetail : UserControl
{
    /// <summary>目前載入的申請單號 (供呼叫端讀取)。</summary>
    public string ApplyNo { get; private set; }

    /// <summary>載入並顯示指定申請單。傳入空字串時不執行任何動作。</summary>
    public void LoadApply(string applyNo)
    {
        ApplyNo = applyNo;
        if (string.IsNullOrEmpty(applyNo)) return;

        // 公司分行資料在 chb_pub.dbo.BRANCH（欄位 BRNO/BRNAME），以 BRNO 對應 BranchCode
        // 公司分行資料在 chb_pub.dbo.BRANCH（欄位 BRNO/BRNAME），CreateName/UpdateName 透過 getUserInfo 取得
        var mainTable = Query(@"
            SELECT  ntd.*,
                    ISNULL(branch.BRNAME, '') AS BranchName,
                    ntd.CreateUser            AS CreateName,
                    ntd.UpdateUser            AS UpdateName,
                    rt.RateName
            FROM    dbo.NTD_Main    ntd
            LEFT JOIN chb_pub.dbo.BRANCH   branch ON branch.BRNO = ntd.BranchCode
            LEFT JOIN dbo.NTD_RateType     rt     ON rt.RateCode = ntd.RateTypeCode
            WHERE   ntd.ApplyNo = @ApplyNo",
            P("@ApplyNo", applyNo));

        NtdCurrentUser.FillEmpNames(mainTable, "CreateName", "UpdateName");

        if (mainTable.Rows.Count == 0)
        {
            LblApplyNo.Text = "(查無此單 " + applyNo + ")";
            return;
        }

        var row = mainTable.Rows[0];
        int currentStatus = Convert.ToInt32(row["Status"]);

        LblApplyNo.Text      = Convert.ToString(row["ApplyNo"]);
        LblBranch.Text       = row["BranchCode"] + " " + row["BranchName"];
        LblApplyDate.Text    = Convert.ToDateTime(row["ApplyDate"]).ToString("yyyy-MM-dd");
        LblGroupName.Text    = Server.HtmlEncode(Convert.ToString(row["GroupName"]));
        LblCustomerID.Text   = Server.HtmlEncode(Convert.ToString(row["CustomerID"]));
        LblCustomerName.Text = Server.HtmlEncode(Convert.ToString(row["CustomerName"]));
        LblTotalAmount.Text  = (row["TotalAmount"] == DBNull.Value)
            ? "" : Convert.ToDecimal(row["TotalAmount"]).ToString("N0");
        LblIsRelated.Text    = Convert.ToString(row["IsRelated"]) == "Y" ? "是"
                             : Convert.ToString(row["IsRelated"]) == "N" ? "否" : "";
        LblReason.Text       = Server.HtmlEncode(Convert.ToString(row["Reason"])).Replace("\n", "<br/>");
        LblStatus.Text       = "<span class='status status-" + currentStatus + "'>"
                              + StatusName(currentStatus) + "</span>";

        string rateCode = Convert.ToString(row["RateTypeCode"]);
        LblRateType.Text = string.IsNullOrEmpty(rateCode)
            ? "(未填)"
            : rateCode + " " + Convert.ToString(row["RateName"]);

        LblCreate.Text = row["CreateUser"] + " " + Convert.ToString(row["CreateName"])
            + " @ " + Convert.ToDateTime(row["CreateTime"]).ToString("yyyy-MM-dd HH:mm");
        LblUpdate.Text = (row["UpdateTime"] == DBNull.Value)
            ? "(未異動)"
            : row["UpdateUser"] + " " + Convert.ToString(row["UpdateName"])
              + " @ " + Convert.ToDateTime(row["UpdateTime"]).ToString("yyyy-MM-dd HH:mm");

        LblContrib3M.Text  = row["Contrib3M"]  == DBNull.Value ? "" : Convert.ToDecimal(row["Contrib3M"]).ToString("N2");
        LblContrib6M.Text  = row["Contrib6M"]  == DBNull.Value ? "" : Convert.ToDecimal(row["Contrib6M"]).ToString("N2");
        LblContrib1Y.Text  = row["Contrib1Y"]  == DBNull.Value ? "" : Convert.ToDecimal(row["Contrib1Y"]).ToString("0.##") + "%";
        LblRelatedAmt.Text = row["RelatedAmt"] == DBNull.Value ? "" : Convert.ToDecimal(row["RelatedAmt"]).ToString("N2");
        LblBranchNote.Text = Server.HtmlEncode(Convert.ToString(row["BranchNote"])).Replace("\n", "<br/>");
        PnlBranchNote.Visible = !string.IsNullOrWhiteSpace(Convert.ToString(row["BranchNote"]));

        var detailTable = Query(
            "SELECT * FROM dbo.NTD_Detail WHERE ApplyNo = @ApplyNo ORDER BY SeqNo",
            P("@ApplyNo", applyNo));
        GvDetail.DataSource = detailTable;
        GvDetail.DataBind();

        var logTable = Query(
            "SELECT * FROM dbo.NTD_Log WHERE ApplyNo = @ApplyNo ORDER BY LogID DESC",
            P("@ApplyNo", applyNo));
        GvLog.DataSource = logTable;
        GvLog.DataBind();
        PnlLog.Visible = logTable.Rows.Count > 0;
    }

    // ===== 資料存取 / 流程狀態 helper (本控制項專用) =====

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
            default: return "未知狀態";
        }
    }
}
