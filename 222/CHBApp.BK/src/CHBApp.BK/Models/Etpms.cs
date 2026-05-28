namespace CHBApp.BK.Models;

/// <summary>企業基本資料檔 (ETPMS) - 對應原 SYS_ETPS.SCX</summary>
public class Etpms
{
    public string CORP_NO     { get; set; } = "12345678";        // 統編
    public string CORP_NAME   { get; set; } = "百年示範股份有限公司";
    public string CORP_BRANCH { get; set; } = "5185";            // 分行代號
    public string CORP_ACCNO  { get; set; } = "00951850123456";  // 公司帳號 14 碼
    public string CORP_OWNER  { get; set; } = "陳大文";
    public string CORP_TEL    { get; set; } = "02-2345-6789";
    public string CORP_ADDR   { get; set; } = "台北市信義區信義路五段 7 號";
}
