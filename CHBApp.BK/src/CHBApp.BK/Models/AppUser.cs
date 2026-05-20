namespace CHBApp.BK.Models;

/// <summary>使用者 (對應 USER_INFO.DBF)</summary>
public class AppUser
{
    public string UserId   { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
    /// <summary>群組代碼 (GROUP 欄位，A=管理員 / C=一般使用者)</summary>
    public string Group    { get; set; } = "C";
}
