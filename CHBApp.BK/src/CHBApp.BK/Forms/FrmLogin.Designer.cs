namespace CHBApp.BK.Forms;

partial class FrmLogin
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblUserId;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.TextBox txtUserId;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.LinkLabel lnkForgotPassword;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.lblTitle           = new System.Windows.Forms.Label();
        this.lblUserId          = new System.Windows.Forms.Label();
        this.lblPassword        = new System.Windows.Forms.Label();
        this.txtUserId          = new System.Windows.Forms.TextBox();
        this.txtPassword        = new System.Windows.Forms.TextBox();
        this.btnOk              = new System.Windows.Forms.Button();
        this.btnCancel          = new System.Windows.Forms.Button();
        this.lnkForgotPassword  = new System.Windows.Forms.LinkLabel();
        this.SuspendLayout();
        // lblTitle
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("標楷體", 16F, System.Drawing.FontStyle.Bold);
        this.lblTitle.Location = new System.Drawing.Point(80, 20);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Text = "員工薪資撥帳系統";
        // lblUserId
        this.lblUserId.AutoSize = true;
        this.lblUserId.Location = new System.Drawing.Point(50, 90);
        this.lblUserId.Name = "lblUserId";
        this.lblUserId.Text = "使用者代碼:";
        // lblPassword
        this.lblPassword.AutoSize = true;
        this.lblPassword.Location = new System.Drawing.Point(50, 130);
        this.lblPassword.Name = "lblPassword";
        this.lblPassword.Text = "密     碼:";
        // txtUserId
        this.txtUserId.Location = new System.Drawing.Point(160, 87);
        this.txtUserId.MaxLength = 20;
        this.txtUserId.Name = "txtUserId";
        this.txtUserId.Size = new System.Drawing.Size(180, 23);
        // txtPassword
        this.txtPassword.Location = new System.Drawing.Point(160, 127);
        this.txtPassword.MaxLength = 20;
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.Size = new System.Drawing.Size(180, 23);
        this.txtPassword.UseSystemPasswordChar = true;
        // btnOk
        this.btnOk.Location = new System.Drawing.Point(80, 180);
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new System.Drawing.Size(100, 35);
        this.btnOk.Text = "確定 (&O)";
        this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
        // btnCancel
        this.btnCancel.Location = new System.Drawing.Point(220, 180);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(100, 35);
        this.btnCancel.Text = "離開 (&X)";
        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
        // lnkForgotPassword
        this.lnkForgotPassword.AutoSize = true;
        this.lnkForgotPassword.Location = new System.Drawing.Point(160, 155);
        this.lnkForgotPassword.Name = "lnkForgotPassword";
        this.lnkForgotPassword.Text = "忘記密碼？";
        this.lnkForgotPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkForgotPassword_LinkClicked);
        // FrmLogin
        this.AcceptButton = this.btnOk;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new System.Drawing.Size(420, 250);
        this.Controls.Add(this.lblTitle);
        this.Controls.Add(this.lblUserId);
        this.Controls.Add(this.lblPassword);
        this.Controls.Add(this.txtUserId);
        this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.lnkForgotPassword);
        this.Controls.Add(this.btnOk);
        this.Controls.Add(this.btnCancel);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "FrmLogin";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "彰化銀行 - 系統登入";
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
