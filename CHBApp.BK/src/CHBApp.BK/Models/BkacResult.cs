namespace CHBApp.BK.Models;

/// <summary>二次扣帳結果 (對應 BKAC_RS.DBF)</summary>
public class BkacResult
{
    public string EMP_NO    { get; set; } = "";
    public string EMP_NAME  { get; set; } = "";
    public string EMP_ACCNO { get; set; } = "";
    public decimal EMP_PAY  { get; set; }
    public string EMP_PID   { get; set; } = "";
    public string RESULT    { get; set; } = "";  // 00=成功 / 其他=失敗代碼
    public string FAIL_DESC { get; set; } = "";
    public DateTime PAY_DATE { get; set; } = DateTime.Today;
}
