namespace CHBApp.BK.Forms;

partial class BK103
{
    private System.ComponentModel.IContainer components = null;

    // 工具列
    private System.Windows.Forms.ToolStrip toolBar;
    private System.Windows.Forms.ToolStripButton btnSave, btnCancel;
    private System.Windows.Forms.ToolStripButton btnFirst, btnPrev, btnNext, btnLast;
    private System.Windows.Forms.ToolStripButton btnPreview, btnPrint, btnExit;
    private System.Windows.Forms.ToolStripStatusLabel lblPos;

    // 標題與欄位
    private System.Windows.Forms.Label lblTitle, lblEmpNo, lblNameLbl, lblName, lblAcc, lblPay, lblPayValue,
                                       lblFlag, lblKind, lblNote, lblCheck, lblSep, lblMail, lblContent,
                                       lblCustomKind;
    private System.Windows.Forms.TextBox txtEmpNo, txtAccNo, txtMail, txtContent, txtCustomKind;
    private System.Windows.Forms.NumericUpDown numPay;
    private System.Windows.Forms.ComboBox cbFlag, cbKind;
    private System.Windows.Forms.RadioButton rbCheckYes, rbCheckNo;
    private System.Windows.Forms.StatusStrip statusBar;

    protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }

    private static System.Windows.Forms.ToolStripButton TBtn(string text)
        => new System.Windows.Forms.ToolStripButton(text)
        {
            DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text,
            AutoSize = true,
            Padding = new System.Windows.Forms.Padding(8, 0, 8, 0)
        };

    private void InitializeComponent()
    {
        // ===== 工具列 (含 4 顆導覽鈕) =====
        this.toolBar    = new System.Windows.Forms.ToolStrip();
        this.btnSave    = TBtn("儲存");
        this.btnCancel  = TBtn("取消");
        this.btnFirst   = TBtn("首筆");
        this.btnPrev    = TBtn("上筆");
        this.btnNext    = TBtn("下筆");
        this.btnLast    = TBtn("末筆");
        this.btnPreview = TBtn("預覽");
        this.btnPrint   = TBtn("列印");
        this.btnExit    = TBtn("離開");

        this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
        {
            btnSave, btnCancel,
            new System.Windows.Forms.ToolStripSeparator(),
            btnFirst, btnPrev, btnNext, btnLast,
            new System.Windows.Forms.ToolStripSeparator(),
            btnPreview, btnPrint,
            new System.Windows.Forms.ToolStripSeparator(),
            btnExit
        });
        this.toolBar.Dock = System.Windows.Forms.DockStyle.Top;

        this.btnSave.Click    += btnSave_Click;
        this.btnCancel.Click  += btnCancel_Click;
        this.btnFirst.Click   += btnFirst_Click;
        this.btnPrev.Click    += btnPrev_Click;
        this.btnNext.Click    += btnNext_Click;
        this.btnLast.Click    += btnLast_Click;
        this.btnPreview.Click += btnPreview_Click;
        this.btnPrint.Click   += btnPrint_Click;
        this.btnExit.Click    += btnExit_Click;

        // ===== 主標題 =====
        this.lblTitle = new System.Windows.Forms.Label
        {
            Text = "薪資輸入－個人",
            Location = new System.Drawing.Point(180, 75),
            Size = new System.Drawing.Size(170, 24),
            Font = new System.Drawing.Font("標楷體", 13F, System.Drawing.FontStyle.Bold),
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        };

        // ===== 員工編號 =====
        this.lblEmpNo  = new System.Windows.Forms.Label  { Text = "員工編號", Location = new System.Drawing.Point(120, 115), Size = new System.Drawing.Size(80, 21) };
        this.txtEmpNo  = new System.Windows.Forms.TextBox{ Location = new System.Drawing.Point(210, 112), Size = new System.Drawing.Size(150, 23), MaxLength = 10, PlaceholderText = "輸入員工編號 (Enter 帶出資料)" };
        this.txtEmpNo.KeyDown += txtEmpNo_KeyDown;

        this.lblNameLbl= new System.Windows.Forms.Label  { Text = "姓     名", Location = new System.Drawing.Point(120, 145), Size = new System.Drawing.Size(80, 21) };
        this.lblName   = new System.Windows.Forms.Label  { Text = "", Location = new System.Drawing.Point(210, 145), Size = new System.Drawing.Size(180, 21), Font = new System.Drawing.Font("新細明體", 10F, System.Drawing.FontStyle.Bold) };

        this.lblAcc    = new System.Windows.Forms.Label  { Text = "帳     號", Location = new System.Drawing.Point(120, 175), Size = new System.Drawing.Size(80, 21) };
        this.txtAccNo  = new System.Windows.Forms.TextBox{ Location = new System.Drawing.Point(210, 172), Size = new System.Drawing.Size(180, 23), ReadOnly = true };

        this.lblPay        = new System.Windows.Forms.Label{ Text = "薪     資", Location = new System.Drawing.Point(120, 205), Size = new System.Drawing.Size(80, 21) };
        this.lblPayValue   = new System.Windows.Forms.Label{ Text = "本月薪資", Location = new System.Drawing.Point(210, 205), Size = new System.Drawing.Size(80, 21) };
        this.numPay        = new System.Windows.Forms.NumericUpDown{ Location = new System.Drawing.Point(290, 202), Size = new System.Drawing.Size(150, 23), Maximum = 9999999, ThousandsSeparator = true };

        this.lblFlag  = new System.Windows.Forms.Label   { Text = "存提區分", Location = new System.Drawing.Point(120, 235), Size = new System.Drawing.Size(80, 21) };
        this.cbFlag   = new System.Windows.Forms.ComboBox{ Location = new System.Drawing.Point(210, 232), Size = new System.Drawing.Size(150, 23), DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList };

        // ===== 轉帳類別（DataSource = PayKinds 18 項，DisplayMember = KIND_NAME）=====
        this.lblKind  = new System.Windows.Forms.Label   { Text = "轉帳類別", Location = new System.Drawing.Point(120, 265), Size = new System.Drawing.Size(80, 21) };
        this.cbKind   = new System.Windows.Forms.ComboBox{ Location = new System.Drawing.Point(210, 262), Size = new System.Drawing.Size(200, 23), DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList };
        this.cbKind.SelectedIndexChanged += cbKind_SelectedIndexChanged;

        // ===== 自訂類別名稱 (97 時才顯示) =====
        this.lblCustomKind = new System.Windows.Forms.Label   { Text = "自訂類別名稱", Location = new System.Drawing.Point(420, 265), Size = new System.Drawing.Size(90, 21), Visible = false, ForeColor = System.Drawing.Color.DarkRed };
        this.txtCustomKind = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(515, 262), Size = new System.Drawing.Size(140, 23), MaxLength = 10, Visible = false, PlaceholderText = "如：技術獎金" };

        this.lblNote = new System.Windows.Forms.Label{ Text = "註：選 '97' 時請於右方輸入自訂名稱", Location = new System.Drawing.Point(210, 290), Size = new System.Drawing.Size(280, 18), Font = new System.Drawing.Font("新細明體", 9F), ForeColor = System.Drawing.Color.DarkRed };

        this.lblCheck    = new System.Windows.Forms.Label  { Text = "薪資入帳是否檢查身份證號碼", Location = new System.Drawing.Point(120, 320), Size = new System.Drawing.Size(220, 21) };
        this.rbCheckYes  = new System.Windows.Forms.RadioButton{ Text = ".是", Location = new System.Drawing.Point(360, 318), Size = new System.Drawing.Size(50, 23), Checked = true };
        this.rbCheckNo   = new System.Windows.Forms.RadioButton{ Text = ".否", Location = new System.Drawing.Point(420, 318), Size = new System.Drawing.Size(50, 23) };

        this.lblSep    = new System.Windows.Forms.Label{ Text = "---------------以下為非必要欄位---------------",
            Location = new System.Drawing.Point(80, 355), Size = new System.Drawing.Size(420, 18),
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter };

        this.lblMail    = new System.Windows.Forms.Label   { Text = "傳真或Email", Location = new System.Drawing.Point(120, 385), Size = new System.Drawing.Size(120, 21) };
        this.txtMail    = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(250, 382), Size = new System.Drawing.Size(280, 23), MaxLength = 60, PlaceholderText = "範例：employee@company.com" };

        this.lblContent = new System.Windows.Forms.Label   { Text = "傳真或Email內容", Location = new System.Drawing.Point(120, 415), Size = new System.Drawing.Size(120, 21) };
        this.txtContent = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(250, 412), Size = new System.Drawing.Size(280, 50), Multiline = true, MaxLength = 75 };

        // ===== 底部狀態列 (顯示第 N / M 筆) =====
        this.statusBar = new System.Windows.Forms.StatusStrip();
        this.lblPos    = new System.Windows.Forms.ToolStripStatusLabel("");
        this.statusBar.Items.Add(this.lblPos);

        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(700, 520);

        this.Controls.AddRange(new System.Windows.Forms.Control[] {
            toolBar,
            lblTitle,
            lblEmpNo, txtEmpNo, lblNameLbl, lblName,
            lblAcc, txtAccNo,
            lblPay, lblPayValue, numPay,
            lblFlag, cbFlag,
            lblKind, cbKind,
            lblCustomKind, txtCustomKind,
            lblNote,
            lblCheck, rbCheckYes, rbCheckNo,
            lblSep,
            lblMail, txtMail,
            lblContent, txtContent,
            statusBar
        });
        this.Name = "BK103";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "薪資輸入－個人";
    }
}
