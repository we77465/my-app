using System;
using System.Windows.Forms;
using CHBApp.BK.Models;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>密碼作業 - 對應原 FRMPSWD.SCX (frmpswd)。
/// 驗證舊密碼後，將新密碼同時更新至記憶體與 USER_INFO.DBF。</summary>
public partial class FrmPasswordChange : Form
{
    private readonly AppUser _user;

    public FrmPasswordChange(AppUser user)
    {
        _user = user;
        InitializeComponent();
        lblUserInfo.Text = $"使用者：{user.UserId}  ({user.UserName})";
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        var oldPwd     = txtOldPassword.Text;
        var newPwd     = txtNewPassword.Text;
        var confirmPwd = txtConfirmPassword.Text;

        // ── 輸入驗證 ──────────────────────────────────
        if (string.IsNullOrWhiteSpace(oldPwd))
        {
            ShowError("請輸入舊密碼。");
            txtOldPassword.Focus();
            return;
        }
        if (oldPwd != _user.Password)
        {
            ShowError("舊密碼不正確！");
            txtOldPassword.SelectAll();
            txtOldPassword.Focus();
            return;
        }
        if (string.IsNullOrWhiteSpace(newPwd))
        {
            ShowError("請輸入新密碼。");
            txtNewPassword.Focus();
            return;
        }
        if (newPwd.Length < 4)
        {
            ShowError("新密碼長度至少需要 4 個字元。");
            txtNewPassword.SelectAll();
            txtNewPassword.Focus();
            return;
        }
        if (newPwd != confirmPwd)
        {
            ShowError("新密碼與確認密碼不一致！");
            txtConfirmPassword.SelectAll();
            txtConfirmPassword.Focus();
            return;
        }
        if (newPwd == oldPwd)
        {
            ShowError("新密碼不得與舊密碼相同。");
            txtNewPassword.SelectAll();
            txtNewPassword.Focus();
            return;
        }

        // ── 更新密碼 ─────────────────────────────────
        // 1. 先寫入 USER_INFO.DBF（加密後存入 PASSWD 欄位）
        bool savedToDbf = UserRepository.SavePassword(_user, newPwd);

        // 2. 更新記憶體物件（本次 session 立即生效）
        _user.Password = newPwd;

        string msg = savedToDbf
            ? "密碼已成功變更並寫入資料庫！\n下次登入請使用新密碼。"
            : "密碼已在本次工作階段變更。\n（注意：USER_INFO.DBF 無法存取，密碼將不會持久保存）";

        MessageBox.Show(msg, "密碼作業", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e) => Close();

    private void ShowError(string msg) =>
        MessageBox.Show(msg, "密碼作業", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
