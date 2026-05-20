namespace CHBApp.BK.Models;

/// <summary>轉帳類別 (對應 BACK_TYPE.DBF)</summary>
public class PayKind
{
    public string KIND_CODE { get; set; } = "";   // 2 碼
    public string KIND_NAME { get; set; } = "";

    public override string ToString() => $"{KIND_CODE} {KIND_NAME}";
}
