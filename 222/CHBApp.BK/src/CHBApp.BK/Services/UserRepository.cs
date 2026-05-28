using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CHBApp.BK.Models;

namespace CHBApp.BK.Services;

/// <summary>
/// 使用者帳號存取層：讀寫 USER_INFO.DBF，密碼使用 TRANSPWD.PRG 相同的加解密演算法。
/// DBF 結構：USER_ID(C,10)  USER_NAME(C,40)  PASSWD(C,20)  GROUP(C,2)  MEMO(M,4)  _NullFlags(0,1)
/// RecordSize = 78，HeaderSize = 488
/// </summary>
public static class UserRepository
{
    // USER_INFO.DBF 相對於 CHBK.EXE 的路徑（C:\CHBAPP374\DATA\USER_INFO.DBF）
    private static readonly string DbfPath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\..\..\..\..\DATA\USER_INFO.DBF");

    private static readonly Encoding Cp950 = Encoding.GetEncoding("big5");

    // DBF layout 常數（由 python 分析確認）
    private const int HeaderSize   = 488;
    private const int RecordSize   = 78;
    private const int UserIdOffset = 1;   // 欄位偏移（含刪除旗標 byte）
    private const int UserIdLen    = 10;
    private const int PasswdOffset = 51;
    private const int PasswdLen    = 20;

    // ──────────────────────────────────────────────────
    // 公開 API
    // ──────────────────────────────────────────────────

    /// <summary>從 DBF 讀入所有使用者（密碼已解密為明文）。若 DBF 不可存取則回傳空清單。</summary>
    public static List<AppUser> LoadAll()
    {
        var result = new List<AppUser>();
        var path = ResolvePath();
        if (path == null) return result;

        try
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var br = new BinaryReader(fs);

            fs.Seek(0, SeekOrigin.Begin);
            var hdr = br.ReadBytes(32);
            int numRec = BitConverter.ToInt32(hdr, 4);
            int hdrSz  = BitConverter.ToUInt16(hdr, 8);
            int recSz  = BitConverter.ToUInt16(hdr, 10);

            // 跳過欄位描述，直接用已知偏移
            for (int i = 0; i < numRec; i++)
            {
                fs.Seek(hdrSz + (long)i * recSz, SeekOrigin.Begin);
                var rec = br.ReadBytes(recSz);
                if (rec[0] == 0x2A) continue; // 已刪除

                var userId   = ReadField(rec, UserIdOffset, UserIdLen);
                var userName = ReadField(rec, 11, 40);
                var passwdEnc = ReadField(rec, PasswdOffset, PasswdLen);
                var group    = ReadField(rec, 71, 2);

                if (string.IsNullOrWhiteSpace(userId)) continue;

                result.Add(new AppUser
                {
                    UserId   = userId,
                    UserName = userName,
                    Password = DecryptPassword(passwdEnc),
                    Group    = group
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[UserRepository.LoadAll] {ex.Message}");
        }

        return result;
    }

    /// <summary>將使用者的新密碼（明文）加密後回寫 USER_INFO.DBF。</summary>
    /// <returns>true = 成功寫入 DBF；false = DBF 不可存取（記憶體更新仍有效）。</returns>
    public static bool SavePassword(AppUser user, string newPassword)
    {
        var path = ResolvePath();
        if (path == null) return false;

        try
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            using var br = new BinaryReader(fs);
            using var bw = new BinaryWriter(fs);

            var hdr   = br.ReadBytes(32);
            int numRec = BitConverter.ToInt32(hdr, 4);
            int hdrSz  = BitConverter.ToUInt16(hdr, 8);
            int recSz  = BitConverter.ToUInt16(hdr, 10);

            string encPwd = EncryptPassword(newPassword);
            byte[] encBytes = PadRight(encPwd, PasswdLen);

            for (int i = 0; i < numRec; i++)
            {
                long recPos = hdrSz + (long)i * recSz;
                fs.Seek(recPos, SeekOrigin.Begin);
                var rec = br.ReadBytes(recSz);
                if (rec[0] == 0x2A) continue;

                var uid = ReadField(rec, UserIdOffset, UserIdLen);
                if (!string.Equals(uid, user.UserId, StringComparison.OrdinalIgnoreCase)) continue;

                // 找到了 → 寫入加密密碼
                fs.Seek(recPos + PasswdOffset, SeekOrigin.Begin);
                bw.Write(encBytes);
                bw.Flush();
                return true;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[UserRepository.SavePassword] {ex.Message}");
        }

        return false;
    }

    // ──────────────────────────────────────────────────
    // TRANSPWD.PRG 演算法（C# 對應實作）
    // ──────────────────────────────────────────────────

    /// <summary>明文 → 加密（對應 TRANSPWD CN="N"）</summary>
    public static string EncryptPassword(string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return "";

        // 每個字元：ASC(char) - index(1-based)，補足 3 位
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < plainText.Length; i++)
        {
            int val = (int)plainText[i] - (i + 1);
            sb.Append(val.ToString("D3"));
        }
        string tmp = sb.ToString();

        // 交換位置 1 和位置 3（1-indexed）
        if (tmp.Length >= 3)
        {
            char[] arr = tmp.ToCharArray();
            (arr[0], arr[2]) = (arr[2], arr[0]);
            tmp = new string(arr);
        }
        return tmp;
    }

    /// <summary>加密 → 明文（對應 TRANSPWD CN="C"）</summary>
    public static string DecryptPassword(string encrypted)
    {
        if (string.IsNullOrEmpty(encrypted)) return "";

        // 先交換回來
        string tmp = encrypted;
        if (tmp.Length >= 3)
        {
            char[] arr = tmp.ToCharArray();
            (arr[0], arr[2]) = (arr[2], arr[0]);
            tmp = new string(arr);
        }

        // 每 3 個數字 → chr(val + j)
        var sb = new System.Text.StringBuilder();
        int j = 1;
        for (int i = 0; i + 2 < tmp.Length; i += 3, j++)
        {
            if (int.TryParse(tmp.Substring(i, 3), out int val))
                sb.Append((char)(val + j));
        }
        return sb.ToString();
    }

    // ──────────────────────────────────────────────────
    // 內部輔助
    // ──────────────────────────────────────────────────

    private static string ReadField(byte[] rec, int offset, int length)
    {
        try
        {
            var raw = new byte[length];
            Array.Copy(rec, offset, raw, 0, Math.Min(length, rec.Length - offset));
            return Cp950.GetString(raw).Trim('\0', ' ');
        }
        catch { return ""; }
    }

    private static byte[] PadRight(string s, int totalLen)
    {
        var bytes = new byte[totalLen];
        var src   = Cp950.GetBytes(s);
        Array.Copy(src, bytes, Math.Min(src.Length, totalLen));
        // 剩餘填空白
        for (int i = src.Length; i < totalLen; i++) bytes[i] = 0x20;
        return bytes;
    }

    /// <summary>嘗試多個候選路徑，回傳第一個存在的，否則 null。</summary>
    private static string? ResolvePath()
    {
        // 根據部署位置嘗試多個相對路徑
        var base1 = AppDomain.CurrentDomain.BaseDirectory;
        var candidates = new[]
        {
            Path.GetFullPath(Path.Combine(base1, @"..\..\..\..\..\..\DATA\USER_INFO.DBF")),
            Path.GetFullPath(Path.Combine(base1, @"..\..\..\..\..\DATA\USER_INFO.DBF")),
            @"C:\CHBAPP374\DATA\USER_INFO.DBF",
        };
        foreach (var c in candidates)
            if (File.Exists(c)) return c;

        System.Diagnostics.Debug.WriteLine($"[UserRepository] USER_INFO.DBF not found. Tried: {string.Join(", ", candidates)}");
        return null;
    }
}
