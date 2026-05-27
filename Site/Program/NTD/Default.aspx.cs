using System;
using System.Web.UI;

/// <summary>
/// 新臺幣定期存款專案利率申請 - 系統進入點。
/// 將 ?SID 帶到 Main.aspx 後立即轉址。
/// </summary>
public partial class NtdDefault : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string url = NtdCurrentUser.Current.AppendSid("Main.aspx");
        HlnkMain.HRef = url;
        Response.Redirect(url, true);
    }
}
