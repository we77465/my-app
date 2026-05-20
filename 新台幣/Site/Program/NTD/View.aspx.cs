using System;
using System.Web.UI;

/// <summary>
/// 新臺幣定期存款專案利率申請 - 唯讀檢視頁。
/// 透過 Detail.ascx 顯示申請主檔、明細與簽核紀錄。
/// </summary>
public partial class NtdView : Page
{
    protected UserInfo Me { get { return UserInfo.Current; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        LblUser.Text = string.Format("{0} {1} ({2} {3}) [職等 {4}]",
            Me.EmpID, Me.EmpName, Me.BranchCode, Me.BranchName, Me.TitleLevel);

        string applyNo = Request.QueryString["no"];
        if (string.IsNullOrEmpty(applyNo))
        {
            ShowMessage("缺少申請單號。", "err");
            return;
        }
        UcApplyDetail.LoadApply(applyNo);
    }

    protected string NavUrl(string relativeUrl) { return Me.AppendSid(relativeUrl); }

    protected void BtnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(Me.AppendSid("Main.aspx"), true);
    }

    private void ShowMessage(string text, string level)
    {
        LblMessage.Visible  = true;
        LblMessage.CssClass = "alert alert-" + level;
        LblMessage.Text     = Server.HtmlEncode(text);
    }
}
