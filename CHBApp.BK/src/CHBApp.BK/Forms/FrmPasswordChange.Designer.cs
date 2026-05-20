namespace CHBApp.BK.Forms;

partial class FrmPasswordChange
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblUserInfo;
    private System.Windows.Forms.Label lblOldPassword;
    private System.Windows.Forms.Label lblNewPassword;
    private System.Windows.Forms.Label lblConfirmPassword;
    private System.Windows.Forms.TextBox txtOldPassword;
    private System.Windows.Forms.TextBox txtNewPassword;
    private System.Windows.Forms.TextBox txtConfirmPassword;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnCancel;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.lblTitle           = new System.Windows.Forms.Label();
        this.lblUserInfo        = new System.Windows.Forms.Label();
        this.lblOldPassword     = new System.Windows.Forms.Label();
        this.lblNewPassword     = new System.Windows.Forms.Label();
        this.lblConfirmPassword = new System.Windows.Forms.Label();
        this.txtOldPassword     = new System.Windows.Forms.TextBox();
        this.txtNewPassword     = new System.Windows.Forms.TextBox();
        this.txtConfirmPassword = new System.Windows.Forms.TextBox();
        this.btnOk              = new System.Windows.Forms.Button();
        this.btnCancel          = new System.Windows.Forms.Button();
        this.SuspendLayout();

        // lblTitle
        this.lblTitle.AutoSize  = true;
        this.lblTitle.Font      = new System.Drawing.Font("標楷體", 14F, System.Drawing.FontStyle.Bold);
        this.lblTitle.Location  = new System.Drawing.Point(100, 18);
        this.lblTitle.Text      = "密碼作業－變更密碼";

        // lblUserInfo
        this.lblUserInfo.AutoSize = true;
        this.lblUserInfo.Location = new System.Drawing.Point(50, 65);
        this.lblUserInfo.ForeColor = System.Drawing.Color.DarkBlue;

        // lblOldPassword
        this.lblOldPassword.AutoSize = true;
        this.lblOldPassword.Location = new System.Drawing.Point(50, 105);
        this.lblOldPassword.Text     = "舊   密   碼：";

        // txtOldPassword
        this.txtOldPassword.Location              = new System.Drawing.Point(170, 102);
        this.txtOldPassword.MaxLength             = 20;
        this.txtOldPassword.Name                  = "txtOldPassword";
        this.txtOldPassword.Size                  = new System.Drawing.Size(200, 23);
        this.txtOldPassword.UseSystemPasswordChar = true;

        // lblNewPassword
        this.lblNewPassword.AutoSize = true;
        this.lblNewPassword.Location = new System.Drawing.Point(50, 145);
        this.lblNewPassword.Text     = "新   密   碼：";

        // txtNewPassword
        this.txtNewPassword.Location              = new System.Drawing.Point(170, 142);
        this.txtNewPassword.MaxLength             = 20;
        this.txtNewPassword.Name                  = "txtNewPassword";
        this.txtNewPassword.Size                  = new System.Drawing.Size(200, 23);
        this.txtNewPassword.UseSystemPasswordChar = true;

        // lblConfirmPassword
        this.lblConfirmPassword.AutoSize = true;
        this.lblConfirmPassword.Location = new System.Drawing.Point(50, 185);
        this.lblConfirmPassword.Text     = "確認新密碼：";

        // txtConfirmPassword
        this.txtConfirmPassword.Location              = new System.Drawing.Point(170, 182);
        this.txtConfirmPassword.MaxLength             = 20;
        this.txtConfirmPassword.Name                  = "txtConfirmPassword";
        this.txtConfirmPassword.Size                  = new System.Drawing.Size(200, 23);
        this.txtConfirmPassword.UseSystemPasswordChar = true;

        // btnOk
        this.btnOk.Location = new System.Drawing.Point(90, 230);
        this.btnOk.Name     = "btnOk";
        this.btnOk.Size     = new System.Drawing.Size(100, 35);
        this.btnOk.Text     = "確定 (&O)";
        this.btnOk.Click   += new System.EventHandler(this.btnOk_Click);

        // btnCancel
        this.btnCancel.Location = new System.Drawing.Point(230, 230);
        this.btnCancel.Name     = "btnCancel";
        this.btnCancel.Size     = new System.Drawing.Size(100, 35);
        this.btnCancel.Text     = "取消 (&C)";
        this.btnCancel.Click   += new System.EventHandler(this.btnCancel_Click);

        // FrmPasswordChange
        this.AcceptButton    = this.btnOk;
        this.CancelButton    = this.btnCancel;
        this.ClientSize      = new System.Drawing.Size(440, 290);
        this.Controls.Add(this.lblTitle);
        this.Controls.Add(this.lblUserInfo);
        this.Controls.Add(this.lblOldPassword);
        this.Controls.Add(this.txtOldPassword);
        this.Controls.Add(this.lblNewPassword);
        this.Controls.Add(this.txtNewPassword);
        this.Controls.Add(this.lblConfirmPassword);
        this.Controls.Add(this.txtConfirmPassword);
        this.Controls.Add(this.btnOk);
        this.Controls.Add(this.btnCancel);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox     = false;
        this.MinimizeBox     = false;
        this.Name            = "FrmPasswordChange";
        this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text            = "密碼作業";
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
