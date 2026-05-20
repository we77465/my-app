using System.Collections.Generic;
/// <summary>
/// F0104 的摘要描述
/// </summary>
public class F0104 : eai
{
    // 分行別
    public string Q_IBRNO = "IBRNO";
    // 查詢方式(1:統一編號 2:帳號)
    public string Q_INQCD = "INQCD";
    // 統一編號
    public string Q_CIFKEY = "CIFKEY";
    // 查詢帳務種類
    public string Q_IQKIND = "IQKIND";
    // 資料查詢範圍
    public string Q_SFG = "SFG";

    // 統一編號
    public string S_CIFKEY = "CIFKEY";
    // 中文名字
    public string S_CNAME = "CNAME";
    public string S_Detail = "Detail";

    // 帳號
    public string D_ACTNO = "ACTNO";
    // 帳戶性質一
    public string D_CHARCD = "CHARCD";
    // 帳戶性質二
    public string D_CHARCD2 = "CHARCD2";

    public F0104(string sEmpID, string sTaskName) : base(sEmpID, sTaskName)
    {
        TxnId = this.GetType().Name;
        SetPmtID(sEmpID, sTaskName);
    }

    public void GetRS()
    {
        SetRQ();

        if (isSuccess)
        {
            CIFKEY = GetValue(docsvcRs, S_CIFKEY);
            CNAME = GetValue(docsvcRs, S_CNAME); 
            List<string> list = new List<string>();

            list.Add(D_ACTNO);
            list.Add(D_CHARCD);
            list.Add(D_CHARCD2);

            Detail = GetDetailValue(docsvcRs, list);
        }
        else
        {
            if (docsvcHeader == null)
                Desc = "查詢電文失敗，請稍後再試。";
        }
    }

    public string CIFKEY { get; set; }
    public string CNAME { get; set; }
    public Dictionary<int, Dictionary<string, string>> Detail { get; set; }
}