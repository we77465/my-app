using System.Windows.Forms;
using CHBApp.BK.Models;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>FRMLOGIN - 對應原 CHBK.EXE 登入畫面</summary>
public partial class FrmLogin : Form
{
    public AppUser? LoggedUser { get; private set; }

    public FrmLogin()
    {
        InitializeComponent();
        txtUserId.Text = "1111";
        txtPassword.Text = "1111";
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        var u = BkacRepository.Users.FirstOrDefault(x =>
            x.UserId.Equals(txtUserId.Text.Trim(), StringComparison.OrdinalIgnoreCase) &&
            x.Password == txtPassword.Text);
        if (u == null)
        {
            MessageBox.Show("使用者代碼或密碼錯誤！", "錯誤",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPassword.SelectAll();
            txtPassword.Focus();
            return;
        }
        LoggedUser = u;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void lnkForgotPassword_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
    {
        MessageBox.Show(
            "若您忘記密碼，請聯絡系統管理員協助重設。\n\n" +
            "管理員帳號：1111\n" +
            "請備妥您的使用者代碼以供身份驗證。\n\n" +
            "管理員可至「密碼作業」選單為您重設密碼。",
            "忘記密碼",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}
