namespace CHBApp.BK.Forms;
partial class BK109
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.ToolStrip toolBar;
    private System.Windows.Forms.ToolStripButton btnSave, btnCancel, btnFirst, btnPrev, btnNext, btnLast, btnPreview, btnPrint, btnExit;
    private System.Windows.Forms.ToolStripStatusLabel lblPos;
    private System.Windows.Forms.Label lblTitle, lblKeyLbl, lblEmpNoLbl, lblEmpNoVal, lblNameLbl, lblName, lblPayLbl, lblFlag, lblKind, lblNote, lblCheck, lblSep, lblMail, lblContent, lblCustomKind;
    private System.Windows.Forms.TextBox txtKey, txtMail, txtContent, txtCustomKind;
    private System.Windows.Forms.NumericUpDown numPay;
    private System.Windows.Forms.ComboBox cbFlag, cbKind;
    private System.Windows.Forms.RadioButton rbCheckYes, rbCheckNo;
    private System.Windows.Forms.StatusStrip statusBar;
    protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }
    private static System.Windows.Forms.ToolStripButton TBtn(string t) => new System.Windows.Forms.ToolStripButton(t)
    { DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text, AutoSize = true, Padding = new System.Windows.Forms.Padding(8, 0, 8, 0) };

    private void InitializeComponent()
    {
        toolBar = new System.Windows.Forms.ToolStrip();
        btnSave = TBtn("儲存"); btnCancel = TBtn("取消"); btnFirst = TBtn("首筆"); btnPrev = TBtn("上筆");
        btnNext = TBtn("下筆"); btnLast = TBtn("末筆"); btnPreview = TBtn("預覽"); btnPrint = TBtn("列印"); btnExit = TBtn("離開");
        toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            btnSave, btnCancel, new System.Windows.Forms.ToolStripSeparator(),
            btnFirst, btnPrev, btnNext, btnLast, new System.Windows.Forms.ToolStripSeparator(),
            btnPreview, btnPrint, new System.Windows.Forms.ToolStripSeparator(), btnExit });
        toolBar.Dock = System.Windows.Forms.DockStyle.Top;
        btnSave.Click += btnSave_Click; btnCancel.Click += btnCancel_Click;
        btnFirst.Click += btnFirst_Click; btnPrev.Click += btnPrev_Click;
        btnNext.Click += btnNext_Click; btnLast.Click += btnLast_Click;
        btnPreview.Click += btnPreview_Click; btnPrint.Click += btnPrint_Click; btnExit.Click += btnExit_Click;

        lblTitle = new System.Windows.Forms.Label { Text = "薪資輸入－帳號", Location = new System.Drawing.Point(180, 75), Size = new System.Drawing.Size(170, 24), Font = new System.Drawing.Font("標楷體", 13F, System.Drawing.FontStyle.Bold), TextAlign = System.Drawing.ContentAlignment.MiddleCenter };
        lblKeyLbl = new System.Windows.Forms.Label { Text = "存款帳號", Location = new System.Drawing.Point(120, 115), Size = new System.Drawing.Size(80, 21) };
        txtKey = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(210, 112), Size = new System.Drawing.Size(180, 23), MaxLength = 14, PlaceholderText = "輸入帳號 (Enter 帶出)" };
        txtKey.KeyDown += txtKey_KeyDown;
        lblEmpNoLbl = new System.Windows.Forms.Label { Text = "員工編號", Location = new System.Drawing.Point(120, 145), Size = new System.Drawing.Size(80, 21) };
        lblEmpNoVal = new System.Windows.Forms.Label { Text = "", Location = new System.Drawing.Point(210, 145), Size = new System.Drawing.Size(180, 21), Font = new System.Drawing.Font("新細明體", 10F, System.Drawing.FontStyle.Bold) };
        lblNameLbl = new System.Windows.Forms.Label { Text = "姓     名", Location = new System.Drawing.Point(120, 175), Size = new System.Drawing.Size(80, 21) };
        lblName = new System.Windows.Forms.Label { Text = "", Location = new System.Drawing.Point(210, 175), Size = new System.Drawing.Size(180, 21), Font = new System.Drawing.Font("新細明體", 10F, System.Drawing.FontStyle.Bold) };
        lblPayLbl = new System.Windows.Forms.Label { Text = "本月薪資", Location = new System.Drawing.Point(120, 205), Size = new System.Drawing.Size(80, 21) };
        numPay = new System.Windows.Forms.NumericUpDown { Location = new System.Drawing.Point(210, 202), Size = new System.Drawing.Size(150, 23), Maximum = 9999999, ThousandsSeparator = true };
        lblFlag = new System.Windows.Forms.Label { Text = "存提區分", Location = new System.Drawing.Point(120, 235), Size = new System.Drawing.Size(80, 21) };
        cbFlag = new System.Windows.Forms.ComboBox { Location = new System.Drawing.Point(210, 232), Size = new System.Drawing.Size(150, 23), DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList };
        lblKind = new System.Windows.Forms.Label { Text = "轉帳類別", Location = new System.Drawing.Point(120, 265), Size = new System.Drawing.Size(80, 21) };
        cbKind = new System.Windows.Forms.ComboBox { Location = new System.Drawing.Point(210, 262), Size = new System.Drawing.Size(200, 23), DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList };
        cbKind.SelectedIndexChanged += cbKind_SelectedIndexChanged;
        lblCustomKind = new System.Windows.Forms.Label { Text = "自訂類別名稱", Location = new System.Drawing.Point(420, 265), Size = new System.Drawing.Size(90, 21), Visible = false, ForeColor = System.Drawing.Color.DarkRed };
        txtCustomKind = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(515, 262), Size = new System.Drawing.Size(140, 23), MaxLength = 10, Visible = false, PlaceholderText = "如：技術獎金" };
        lblNote = new System.Windows.Forms.Label { Text = "註：選 '97' 時請於右方輸入自訂名稱", Location = new System.Drawing.Point(210, 290), Size = new System.Drawing.Size(280, 18), Font = new System.Drawing.Font("新細明體", 9F), ForeColor = System.Drawing.Color.DarkRed };
        lblCheck = new System.Windows.Forms.Label { Text = "薪資入帳是否檢查身份證號碼", Location = new System.Drawing.Point(120, 320), Size = new System.Drawing.Size(220, 21) };
        rbCheckYes = new System.Windows.Forms.RadioButton { Text = ".是", Location = new System.Drawing.Point(360, 318), Size = new System.Drawing.Size(50, 23), Checked = true };
        rbCheckNo = new System.Windows.Forms.RadioButton { Text = ".否", Location = new System.Drawing.Point(420, 318), Size = new System.Drawing.Size(50, 23) };
        lblSep = new System.Windows.Forms.Label { Text = "---------------以下為非必要欄位---------------", Location = new System.Drawing.Point(80, 355), Size = new System.Drawing.Size(420, 18), TextAlign = System.Drawing.ContentAlignment.MiddleCenter };
        lblMail = new System.Windows.Forms.Label { Text = "傳真或Email", Location = new System.Drawing.Point(120, 385), Size = new System.Drawing.Size(120, 21) };
        txtMail = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(250, 382), Size = new System.Drawing.Size(280, 23), MaxLength = 60, PlaceholderText = "範例：employee@company.com" };
        lblContent = new System.Windows.Forms.Label { Text = "傳真或Email內容", Location = new System.Drawing.Point(120, 415), Size = new System.Drawing.Size(120, 21) };
        txtContent = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(250, 412), Size = new System.Drawing.Size(280, 50), Multiline = true, MaxLength = 75 };
        statusBar = new System.Windows.Forms.StatusStrip();
        lblPos = new System.Windows.Forms.ToolStripStatusLabel("");
        statusBar.Items.Add(lblPos);
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F); AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(700, 520);
        Controls.AddRange(new System.Windows.Forms.Control[] { toolBar, lblTitle, lblKeyLbl, txtKey, lblEmpNoLbl, lblEmpNoVal, lblNameLbl, lblName, lblPayLbl, numPay, lblFlag, cbFlag, lblKind, cbKind, lblCustomKind, txtCustomKind, lblNote, lblCheck, rbCheckYes, rbCheckNo, lblSep, lblMail, txtMail, lblContent, txtContent, statusBar });
        Name = "BK109"; StartPosition = System.Windows.Forms.FormStartPosition.CenterParent; Text = "薪資輸入－帳號";
    }
}
