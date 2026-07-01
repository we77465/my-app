using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Program_EF_HQSet : System.Web.UI.Page
{
    // 欄位索引：前 4 欄為識別欄（表單編號/分行代號/往來金融機構/帳號），第 9 欄(index 8)為上傳對帳單
    private const int ColIdxAmtStart   = 4;    // 本行帳面餘額
    private const int ColIdxAmtB       = 5;    // 未達帳務(+)
    private const int ColIdxAmtC       = 6;    // 未達帳務(-)
    private const int ColIdxAmtD       = 7;    // 未達帳金額
    private const int ColIdxAmtE       = 8;    // 往來對帳單餘額（明細用 Amt2）
    private const int ColIdxUpload     = 9;    // 上傳對帳單
    private const int TotalColumnCount = 10;
    private const int IdentifierColumnCount = 4;

    // 合計累積（從 RowDataBound 逐列加總）
    private decimal _sumA = 0;
    private decimal _sumB = 0;
    private decimal _sumC = 0;
    private decimal _sumD = 0;

    private const string KhDetailSql = @"
SELECT
    a.[DocNo]      AS [表單編號],
    a.[Brno]       AS [分行代號],
    a.[PARTY_NAME] AS [往來金融機構],
    a.[AccNbr]     AS [帳號],
    a.[Amt1]       AS [本行帳面餘額],
    ISNULL(SUM(CASE WHEN RIGHT(n.[Type], 2) = N'加項' THEN n.[Amt] ELSE 0 END), 0) AS [未達帳務(+)],
    ISNULL(SUM(CASE WHEN RIGHT(n.[Type], 2) = N'減項' THEN n.[Amt] ELSE 0 END), 0) AS [未達帳務(-)],
    ISNULL(SUM(CASE WHEN RIGHT(n.[Type], 2) = N'加項' THEN n.[Amt] ELSE 0 END), 0)
        - ISNULL(SUM(CASE WHEN RIGHT(n.[Type], 2) = N'減項' THEN n.[Amt] ELSE 0 END), 0) AS [未達帳金額],
    a.[Amt2] AS [往來對帳單餘額],
    CASE WHEN a.[Upload_File] = N'上傳檔案' THEN 'O' ELSE 'X' END AS [上傳對帳單]
FROM [KHAmt] a
LEFT JOIN [KHAmtNever] n ON a.[DocNo] = n.[DocNo]
WHERE a.[HasDetail] = N'有'
    AND CAST(a.[YYYYMM] AS INT) = CAST(@YYMM AS INT) + 191100
GROUP BY a.[DocNo], a.[Brno], a.[PARTY_NAME], a.[AccNbr], a.[Amt1], a.[Amt2], a.[Upload_File]
ORDER BY a.[Brno], a.[DocNo]";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["HQBrnoU"] = "0208";
            string minusMonth = "0";
            ViewState["HQRole"] = "IntraMaintain";
            ViewState["Period"] = "m";
            ViewState["QuerySQL2"] = "";
            HFDocType.Value = "KH"; //Request.QueryString["DocType"];
            //ViewState["UserId"] = UserInfo.getUserId(Request.QueryString["sid"].ToString(), Request);
            ViewState["UserId"] = "";
            ViewState["Brno"] = "5050"; //UserInfo.getUserBrNoMergeU((string)ViewState["UserId"]);
            ViewState["Vertical"] = "n";
            ViewState["Vertical"] = Request["Vertical"];

            using (DataView dv = DSDocType.Select(new DataSourceSelectArguments()) as DataView)
            {
                if (dv != null && dv.Count != 0)
                {
                    LBTypeName.Text = dv[0]["TypeName"].ToString();
                    ViewState["HQBrnoU"] = dv[0]["HQBrnoU"].ToString();
                    minusMonth = dv[0]["MinusMonth"].ToString();
                    ViewState["HQRole"] = dv[0]["HQRole"].ToString();
                    ViewState["QuerySQL2"] = dv[0]["QuerySQL2"].ToString();
                    ViewState["Period"] = dv[0]["Period"].ToString();
                }
                dv.Dispose();
            }

            // KH 固定使用內建 SQL（覆蓋 DB 設定）
            if (HFDocType.Value == "KH")
                ViewState["QuerySQL2"] = KhDetailSql;

            TBYYMM.Text = GetDefaultPeriodText(minusMonth);
        }

        if (HasQueryPermission())
        {
            DSSourceQuery2.SelectCommand = (string)ViewState["QuerySQL2"];
        }
        else
        {
            msg.Text = "您無查詢權限";
            BTExcel.Visible = false;
            BTQuery.Visible = false;
        }

        ApplyPeriodParameters();
        SetReportTitle();

        if ((string)ViewState["Vertical"] == "y")
        {
            GridView1.DataSourceID = "";
            GridView1.DataSource = EfromBI.DSVertical(DSSourceQuery2);
            GridView1.DataBind();
        }
    }

    private bool HasQueryPermission()
    {
        string brno   = (string)ViewState["Brno"];
        string hqBrnoU = (string)ViewState["HQBrnoU"];
        string userId = (string)ViewState["UserId"];
        string hqRole = (string)ViewState["HQRole"];

        return brno == hqBrnoU
            || brno == "5050"
            || UserInfo.UserRoleContainIntraMaintain(userId)
            || UserInfo.IdentifyUserRoleID(userId, hqRole);
    }

    private void SetReportTitle()
    {
        LBReportTitle.Text = "存放央行及存放銀行同業(新臺幣)往來未達帳調節彙整表";
        string yymm = TBYYMM.Text.Trim();
        if (yymm.Length >= 5)
        {
            int yyy = int.Parse(yymm.Substring(0, 3));
            int mm  = int.Parse(yymm.Substring(3, 2));
            int dd  = DateTime.DaysInMonth(yyy + 1911, mm);
            LBReportDate.Text = string.Format("{0}年{1:D2}月{2:D2}日", yyy, mm, dd);
        }
        else if (yymm.Length == 3)
        {
            LBReportDate.Text = yymm + "年12月31日";
        }
        else
        {
            LBReportDate.Text = string.Empty;
        }
    }

    private void ApplyPeriodParameters()
    {
        if ((string)ViewState["Period"] == "y")
        {
            LBPeriod.Text = "年度";
            LBPeriodPS.Text = "年度YYY";
            DSSourceQuery2.SelectParameters["YYMM"].DefaultValue = TBYYMM.Text + "00";
        }
        else
        {
            LBPeriod.Text = "年月";
            LBPeriodPS.Text = "年月YYYMM";
            DSSourceQuery2.SelectParameters["YYMM"].DefaultValue = TBYYMM.Text;
        }
    }

    private string GetDefaultPeriodText(string minusMonth)
    {
        switch ((string)ViewState["Period"])
        {
            case "y":
                if (minusMonth == "1")
                    return (int.Parse(DateTime.Now.AddYears(-1).ToString("yyyy")) - 1911).ToString();
                return (int.Parse(DateTime.Now.ToString("yyyy")) - 1911).ToString();
            case "s":
                int monthNow = int.Parse(DateTime.Now.ToString("MM"));
                string year = (int.Parse(DateTime.Now.ToString("yyyy")) - 1911).ToString();
                if (monthNow < 4)  return year + "01";
                if (monthNow < 7)  return year + "04";
                if (monthNow < 10) return year + "07";
                return year + "10";
            default:
                if (minusMonth == "1")
                    return DateTimeUtility.getTaiwanYYMM(DateTime.Now.AddMonths(-1));
                return DateTimeUtility.getTaiwanYYMM(DateTime.Now);
        }
    }

    protected void BTQuery_Click(object sender, EventArgs e)
    {
        string yymm = TBYYMM.Text != "" ? TBYYMM.Text : "%";

        if ((string)ViewState["Period"] == "y")
            DSSourceQuery2.SelectParameters["YYMM"].DefaultValue = yymm != "%" ? yymm + "00" : "%";
        else
            DSSourceQuery2.SelectParameters["YYMM"].DefaultValue = yymm;

        SetReportTitle();

        if ((string)ViewState["Vertical"] == "y")
        {
            GridView1.DataSource = EfromBI.DSVertical(DSSourceQuery2);
            GridView1.DataBind();
        }
        else
        {
            GridView1.DataBind();
        }
    }

    protected void BTExcel_Click(object sender, EventArgs e)
    {
        // 明確觸發 DataBind，確保 RowDataBound/DataBound 執行，合計列才會 append
        GridView1.DataBind();

        string style = "<style> .text { mso-number-format:\"\\@\"; } </style> ";
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=KHSummary.xls");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-excel";
        Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>");

        // 表頭標題
        Response.Write(string.Format(
            "<div style='text-align:center'><strong style='font-size:large'>{0}</strong><br />{1}</div><br />",
            LBReportTitle.Text, LBReportDate.Text));

        System.IO.StringWriter sw = new System.IO.StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        GridView1.RenderControl(hw);
        Response.Write(style);
        Response.Write(sw.ToString());
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control) { }

    // ── RowDataBound：格式化金額欄、累積合計 ──────────────────────────────
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.HorizontalAlign = HorizontalAlign.Center;
            return;
        }

        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        // 金額欄：右對齊、N2 格式，同時累積合計（用格式化前的原始值）
        for (int i = ColIdxAmtStart; i <= ColIdxAmtE && i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
            decimal val;
            if (decimal.TryParse(e.Row.Cells[i].Text, out val))
            {
                e.Row.Cells[i].Text = val.ToString("N2");
                switch (i)
                {
                    case ColIdxAmtStart: _sumA += val; break;
                    case ColIdxAmtB:     _sumB += val; break;
                    case ColIdxAmtC:     _sumC += val; break;
                    case ColIdxAmtD:     _sumD += val; break;
                }
            }
        }

        // 分行代號（col 1）、帳號（col 3）靠左
        if (e.Row.Cells.Count > 1) e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
        if (e.Row.Cells.Count > 3) e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;

        // 上傳對帳單（col 9）置中
        if (e.Row.Cells.Count > ColIdxUpload)
            e.Row.Cells[ColIdxUpload].HorizontalAlign = HorizontalAlign.Center;

        if ((string)ViewState["Vertical"] == "y" && e.Row.Cells.Count > 0)
        {
            e.Row.Cells[0].BackColor = Color.DarkRed;
            e.Row.Cells[0].ForeColor = Color.White;
        }
    }

    // ── DataBound：合計有資料才 append 兩列到 GridView1 的 Table ──────────
    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        if (GridView1.Rows.Count == 0)
            return;

        if (GridView1.Controls.Count == 0)
            return;

        Table tbl = (Table)GridView1.Controls[0];

        // ---- 第一列：標題（A/B/C/D/E 標籤）----
        GridViewRow labelRow = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Normal);

        // 「合　計」合併前 4 欄 × 跨 2 列
        labelRow.Cells.Add(MakeCell("<strong>合&nbsp;&nbsp;計</strong>",
            HorizontalAlign.Center, IdentifierColumnCount, 2, isHeader: true));
        labelRow.Cells.Add(MakeCell("A",     HorizontalAlign.Center, 1, 1, isHeader: true));
        labelRow.Cells.Add(MakeCell("B",     HorizontalAlign.Center, 1, 1, isHeader: true));
        labelRow.Cells.Add(MakeCell("C",     HorizontalAlign.Center, 1, 1, isHeader: true));
        labelRow.Cells.Add(MakeCell("D=B-C", HorizontalAlign.Center, 1, 1, isHeader: true));
        labelRow.Cells.Add(MakeCell("E=A+D", HorizontalAlign.Center, 1, 1, isHeader: true));
        labelRow.Cells.Add(MakeCell(string.Empty, HorizontalAlign.Center, 1, 1, isHeader: true));
        tbl.Rows.Add(labelRow);

        // ---- 第二列：數值（E=A+D）----
        // 前 4 欄已被 RowSpan=2 佔用，從第 5 欄開始
        GridViewRow valueRow = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Normal);
        decimal sumE = _sumA + _sumD;   // E = A + D
        valueRow.Cells.Add(MakeCell(_sumA.ToString("N2"), HorizontalAlign.Right));
        valueRow.Cells.Add(MakeCell(_sumB.ToString("N2"), HorizontalAlign.Right));
        valueRow.Cells.Add(MakeCell(_sumC.ToString("N2"), HorizontalAlign.Right));
        valueRow.Cells.Add(MakeCell(_sumD.ToString("N2"), HorizontalAlign.Right));
        valueRow.Cells.Add(MakeCell(sumE.ToString("N2"),  HorizontalAlign.Right));
        valueRow.Cells.Add(MakeCell(string.Empty, HorizontalAlign.Center));
        tbl.Rows.Add(valueRow);
    }

    // ── 輔助：建立儲存格 ──────────────────────────────────────────────────
    private static TableCell MakeCell(string text, HorizontalAlign align,
        int colSpan = 1, int rowSpan = 1, bool isHeader = false)
    {
        TableCell c = new TableCell
        {
            Text = text,
            HorizontalAlign = align,
            ColumnSpan = colSpan,
            RowSpan = rowSpan
        };
        if (isHeader)
        {
            c.BackColor = Color.DarkRed;
            c.ForeColor = Color.White;
            c.Font.Bold  = true;
        }
        else
        {
            c.BackColor = ColorTranslator.FromHtml("#FFFBD6");
        }
        return c;
    }

}
