namespace CHBApp.BK.Forms;
partial class BK108
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.ToolStrip toolBar;
    private System.Windows.Forms.ToolStripButton btnNew, btnSave, btnCancel, btnPreview, btnPrint, btnExit;
    private System.Windows.Forms.ToolStripStatusLabel lblPos;
    private System.Windows.Forms.Label lblTitle, lblEmpNoLbl, lblNameLbl, lblPidLbl, lblAccLbl, lblPayLbl,
                                       lblFlag, lblKind, lblNote, lblCheck, lblSep, lblMail, lblContent, lblCustomKind, lblMorf;
    private System.Windows.Forms.TextBox txtEmpNo, txtEmpName, txtPid, txtAccNo, txtMail, txtContent, txtCustomKind;
    private System.Windows.Forms.NumericUpDown numPay;
    private System.Windows.Forms.ComboBox cbFlag, cbKind, cbMorf;
    private System.Windows.Forms.RadioButton rbCheckYes, rbCheckNo;
    private System.Windows.Forms.StatusStrip statusBar;
    protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }
    private static System.Windows.Forms.ToolStripButton TBtn(string t) => new System.Windows.Forms.ToolStripButton(t)
    { DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text, AutoSize = true, Padding = new System.Windows.Forms.Padding(8, 0, 8, 0) };

    private void InitializeComponent()
    {
        toolBar = new System.Windows.Forms.ToolStrip();
        btnNew = TBtn("新增"); btnSave = TBtn("儲存"); btnCancel = TBtn("取消");
        btnPreview = TBtn("預覽"); btnPrint = TBtn("列印"); btnExit = TBtn("離開");
        toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            btnNew, btnSave, btnCancel, new System.Windows.Forms.ToolStripSeparator(),
            btnPreview, btnPrint, new System.Windows.Forms.ToolStripSeparator(), btnExit });
        toolBar.Dock = System.Windows.Forms.DockStyle.Top;
        btnNew.Click += btnNew_Click; btnSave.Click += btnSave_Click; btnCancel.Click += btnCancel_Click;
        btnPreview.Click += btnPreview_Click; btnPrint.Click += btnPrint_Click; btnExit.Click += btnExit_Click;

        lblTitle = new System.Windows.Forms.Label { Text = "未建員工資料薪資輸入", Location = new System.Drawing.Point(180, 70), Size = new System.Drawing.Size(220, 24), Font = new System.Drawing.Font("標楷體", 13F, System.Drawing.FontStyle.Bold), TextAlign = System.Drawing.ContentAlignment.MiddleCenter };

        lblEmpNoLbl = new System.Windows.Forms.Label { Text = "員工編號", Location = new System.Drawing.Point(80, 110), Size = new System.Drawing.Size(80, 21) };
        txtEmpNo = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(170, 107), Size = new System.Drawing.Size(150, 23), MaxLength = 10, PlaceholderText = "範例：E0001" };

        lblNameLbl = new System.Windows.Forms.Label { Text = "員工姓名", Location = new System.Drawing.Point(80, 140), Size = new System.Drawing.Size(80, 21) };
        txtEmpName = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(170, 137), Size = new System.Drawing.Size(180, 23), MaxLength = 10, PlaceholderText = "輸入員工姓名" };

        lblPidLbl = new System.Windows.Forms.Label { Text = "身份字號", Location = new System.Drawing.Point(80, 170), Size = new System.Drawing.Size(80, 21) };
        txtPid = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(170, 167), Size = new System.Drawing.Size(150, 23), MaxLength = 10, PlaceholderText = "範例：A123456789" };

        lblMorf = new System.Windows.Forms.Label { Text = "身分別", Location = new System.Drawing.Point(340, 170), Size = new System.Drawing.Size(60, 21) };
        cbMorf = new System.Windows.Forms.ComboBox { Location = new System.Drawing.Point(405, 167), Size = new System.Drawing.Size(120, 23), DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList };

        lblAccLbl = new System.Windows.Forms.Label { Text = "存款帳號", Location = new System.Drawing.Point(80, 200), Size = new System.Drawing.Size(80, 21) };
        txtAccNo = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(170, 197), Size = new System.Drawing.Size(180, 23), MaxLength = 14, PlaceholderText = "範例：12345678901234" };

        lblPayLbl = new System.Windows.Forms.Label { Text = "本月薪資", Location = new System.Drawing.Point(80, 230), Size = new System.Drawing.Size(80, 21) };
        numPay = new System.Windows.Forms.NumericUpDown { Location = new System.Drawing.Point(170, 227), Size = new System.Drawing.Size(150, 23), Maximum = 9999999, ThousandsSeparator = true };

        lblFlag = new System.Windows.Forms.Label { Text = "存提區分", Location = new System.Drawing.Point(80, 260), Size = new System.Drawing.Size(80, 21) };
        cbFlag = new System.Windows.Forms.ComboBox { Location = new System.Drawing.Point(170, 257), Size = new System.Drawing.Size(150, 23), DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList };

        lblKind = new System.Windows.Forms.Label { Text = "轉帳類別", Location = new System.Drawing.Point(80, 290), Size = new System.Drawing.Size(80, 21) };
        cbKind = new System.Windows.Forms.ComboBox { Location = new System.Drawing.Point(170, 287), Size = new System.Drawing.Size(200, 23), DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList };
        cbKind.SelectedIndexChanged += cbKind_SelectedIndexChanged;

        lblCustomKind = new System.Windows.Forms.Label { Text = "自訂類別名稱", Location = new System.Drawing.Point(380, 290), Size = new System.Drawing.Size(90, 21), Visible = false, ForeColor = System.Drawing.Color.DarkRed };
        txtCustomKind = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(475, 287), Size = new System.Drawing.Size(140, 23), MaxLength = 10, Visible = false, PlaceholderText = "如：技術獎金" };

        lblNote = new System.Windows.Forms.Label { Text = "註：選 '97' 時請於右方輸入自訂名稱", Location = new System.Drawing.Point(170, 313), Size = new System.Drawing.Size(280, 18), Font = new System.Drawing.Font("新細明體", 9F), ForeColor = System.Drawing.Color.DarkRed };

        lblCheck = new System.Windows.Forms.Label { Text = "薪資入帳是否檢查身份證號碼", Location = new System.Drawing.Point(80, 340), Size = new System.Drawing.Size(220, 21) };
        rbCheckYes = new System.Windows.Forms.RadioButton { Text = ".是", Location = new System.Drawing.Point(310, 338), Size = new System.Drawing.Size(50, 23), Checked = true };
        rbCheckNo = new System.Windows.Forms.RadioButton { Text = ".否", Location = new System.Drawing.Point(370, 338), Size = new System.Drawing.Size(50, 23) };

        lblSep = new System.Windows.Forms.Label { Text = "---------------以下為非必要欄位---------------", Location = new System.Drawing.Point(50, 370), Size = new System.Drawing.Size(420, 18), TextAlign = System.Drawing.ContentAlignment.MiddleCenter };

        lblMail = new System.Windows.Forms.Label { Text = "傳真或Email", Location = new System.Drawing.Point(80, 400), Size = new System.Drawing.Size(120, 21) };
        txtMail = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(210, 397), Size = new System.Drawing.Size(280, 23), MaxLength = 60, PlaceholderText = "範例：employee@company.com" };

        lblContent = new System.Windows.Forms.Label { Text = "傳真或Email內容", Location = new System.Drawing.Point(80, 430), Size = new System.Drawing.Size(120, 21) };
        txtContent = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(210, 427), Size = new System.Drawing.Size(280, 50), Multiline = true, MaxLength = 75 };

        statusBar = new System.Windows.Forms.StatusStrip();
        lblPos = new System.Windows.Forms.ToolStripStatusLabel("");
        statusBar.Items.Add(lblPos);

        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(700, 540);
        Controls.AddRange(new System.Windows.Forms.Control[] {
            toolBar, lblTitle,
            lblEmpNoLbl, txtEmpNo, lblNameLbl, txtEmpName, lblPidLbl, txtPid, lblMorf, cbMorf,
            lblAccLbl, txtAccNo, lblPayLbl, numPay, lblFlag, cbFlag, lblKind, cbKind,
            lblCustomKind, txtCustomKind, lblNote, lblCheck, rbCheckYes, rbCheckNo,
            lblSep, lblMail, txtMail, lblContent, txtContent, statusBar });
        Name = "BK108";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Text = "未建員工資料薪資輸入";
    }
}
