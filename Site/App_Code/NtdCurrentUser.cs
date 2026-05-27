using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

/// <summary>
/// NTD 專案登入使用者資訊。
///
/// 【公司正式環境】
///   URL 帶 sid (由 SSO 注入)，呼叫 UserInfo.getUserId(sid, Request) 取得員編，
///   再呼叫 UserInfo.getUserInfo(empId) 取得人員資料。
///   getUserInfo 回傳格式（逗號分隔）：
///     [0] OU_ID    → BranchCode（分行代碼）
///     [1] USR_NAME → EmpName（員工姓名）
///     [2] OU_NAME  → BranchName（分行名稱）
///
///   getUserBrNoAndLevel 回傳格式（逗號分隔）：
///     [0] BRNO           → BranchCode
///     [1] NAME           → EmpName
///     [2] MANAGER_LEVEL  → 10=經辦, 09=負責人(襄理/科長), 00=主管(經理)
///
/// 【TitleLevel 對應】
///   分行：10→1(經辦)  09→2(襄理)  00→3(經理)
///   總行：10→4(總行經辦)  09→5(總行科長)
///
/// 【本機開發環境】
///   URL ?sid=員編 (例：Main.aspx?sid=A0001)，getUserId 例外時 fallback 直接用 sid。
///
/// 注意：此 class 名稱為 NtdCurrentUser，避免與公司共用的
///       靜態 UserInfo (AD/SSO) 類別衝突 (CS0722)。
/// </summary>
public class NtdCurrentUser
{
    public string EmpID      { get; private set; }
    public string EmpName    { get; private set; }
    public string BranchCode { get; private set; }
    public string BranchName { get; private set; }
    public string Title      { get; private set; }
    public int    TitleLevel { get; private set; }
    public int    BranchType { get; private set; }  // 1=分行, 2=總行

    /// <summary>是否為總行單位 (BranchType = 2)。</summary>
    public bool IsHeadOffice { get { return BranchType == 2; } }

    /// <summary>是否可新增申請 (僅「經辦」TitleLevel=1)。</summary>
    public bool CanCreateApply { get { return TitleLevel == 1; } }

    private NtdCurrentUser() { }

    // =====================================================================
    // Current 屬性：取得本次請求的登入使用者；同 Session 內快取。
    // =====================================================================
    public static NtdCurrentUser Current
    {
        get
        {
            var context = HttpContext.Current;
            var cached  = context.Session != null
                        ? context.Session["NTD_NtdCurrentUser"] as NtdCurrentUser
                        : null;

            string sid = (context.Request.QueryString["sid"]
                       ?? context.Request.QueryString["SID"]
                       ?? "").Trim();

            if (string.IsNullOrEmpty(sid) && cached != null) return cached;

            string empId = "";
            try   { empId = UserInfo.getUserId(sid, context.Request); }
            catch { empId = sid; }

            if (string.IsNullOrEmpty(empId))
                empId = string.IsNullOrEmpty(sid) ? "A0001" : sid;

            var user = LoadByUserInfo(empId) ?? DefaultUser(empId);

            if (context.Session != null)
                context.Session["NTD_NtdCurrentUser"] = user;

            return user;
        }
    }

    // =====================================================================
    // 導覽輔助：在相對 URL 後面附加 sid 參數，讓 SSO 不中斷。
    // =====================================================================
    public string AppendSid(string url)
    {
        if (string.IsNullOrEmpty(url)) return url;
        string sid = HttpContext.Current.Request.QueryString["sid"]
                  ?? HttpContext.Current.Request.QueryString["SID"]
                  ?? "";
        if (string.IsNullOrEmpty(sid)) return url;
        return url + (url.Contains("?") ? "&" : "?") + "sid=" + sid;
    }

    // =====================================================================
    // 私有：員編 → 完整使用者物件
    // =====================================================================
    private static NtdCurrentUser LoadByUserInfo(string empId)
    {
        try
        {
            string raw = UserInfo.getUserInfo(empId);
            if (string.IsNullOrEmpty(raw)) return null;

            string[] info = raw.Split(',');
            if (info.Length < 3) return null;

            string branchCode = info[0].Trim();
            string empName    = info[1].Trim();
            string branchName = info[2].Trim();

            bool isHead    = IsHeadOfficeBranch(branchCode);
            int branchType = isHead ? 2 : 1;

            int titleLevel = 1;
            string title   = "經辦";
            try
            {
                string rawLevel = UserInfo.getUserBrNoAndLevel(empId);
                if (!string.IsNullOrEmpty(rawLevel))
                {
                    string[] lv = rawLevel.Split(',');
                    string managerLevel = lv.Length > 2 ? lv[2].Trim() : "10";
                    titleLevel = MapManagerLevel(managerLevel, isHead);
                    title      = GetTitleName(titleLevel);
                }
            }
            catch { }

            return new NtdCurrentUser
            {
                EmpID      = empId,
                EmpName    = empName,
                BranchCode = branchCode,
                BranchName = branchName,
                Title      = title,
                TitleLevel = titleLevel,
                BranchType = branchType
            };
        }
        catch { return null; }
    }

    // 分行：10→1(經辦)  09→2(襄理)  00→3(經理)
    // 總行：10→4(總行經辦)  09→5(總行科長)
    private static int MapManagerLevel(string managerLevel, bool isHeadOffice)
    {
        if (!isHeadOffice)
        {
            if (managerLevel == "00") return 3;
            if (managerLevel == "09") return 2;
            return 1;
        }
        else
        {
            if (managerLevel == "09") return 5;
            return 4;
        }
    }

    private static string GetTitleName(int titleLevel)
    {
        switch (titleLevel)
        {
            case 2: return "襄理";
            case 3: return "經理";
            case 4: return "總行經辦";
            case 5: return "總行科長";
            default: return "經辦";
        }
    }

    // =====================================================================
    // 私有：查 chb_pub.dbo.HEADQUARTERS 判斷分行代碼是否為總行單位。
    // =====================================================================
    private static bool IsHeadOfficeBranch(string branchCode)
    {
        if (string.IsNullOrEmpty(branchCode)) return false;
        try
        {
            string connStr = ConfigurationManager
                .ConnectionStrings["chb_iom"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd  = new SqlCommand(
                "SELECT COUNT(1) FROM chb_pub.dbo.HEADQUARTERS " +
                "WHERE BRNOINTRA = @BranchCode AND ALIVE = 'Y'", conn))
            {
                cmd.Parameters.AddWithValue("@BranchCode", branchCode);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
        catch { return false; }
    }

    // =====================================================================
    // 公用靜態 helper：透過 getUserInfo 查詢任意員編的姓名。
    // =====================================================================
    public static string GetEmpName(string empId)
    {
        if (string.IsNullOrEmpty(empId)) return empId;
        try
        {
            string raw = UserInfo.getUserInfo(empId);
            if (string.IsNullOrEmpty(raw)) return empId;
            string[] info = raw.Split(',');
            string name = info.Length > 1 ? info[1].Trim() : "";
            return string.IsNullOrEmpty(name) ? empId : name;
        }
        catch { return empId; }
    }

    public static void FillEmpNames(System.Data.DataTable table, params string[] columns)
    {
        foreach (System.Data.DataRow row in table.Rows)
            foreach (string col in columns)
                if (table.Columns.Contains(col))
                    row[col] = GetEmpName(Convert.ToString(row[col]));
    }

    private static NtdCurrentUser DefaultUser(string empId)
    {
        return new NtdCurrentUser
        {
            EmpID      = string.IsNullOrEmpty(empId) ? "UNKNOWN" : empId,
            EmpName    = string.IsNullOrEmpty(empId) ? "未知使用者" : empId,
            BranchCode = "000",
            BranchName = "未知分行",
            Title      = "經辦",
            TitleLevel = 1,
            BranchType = 1
        };
    }
}
