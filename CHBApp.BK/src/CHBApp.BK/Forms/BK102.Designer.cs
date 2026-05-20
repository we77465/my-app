namespace CHBApp.BK.Forms;

partial class BK102
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.ToolStrip toolBar;
    private System.Windows.Forms.ToolStripButton btnSave, btnCancel, btnPreview, btnPrint, btnExit;
    private System.Windows.Forms.Label lblTitle, lblPay, lblFlag, lblKind, lblNote, lblCheck, lblSep, lblMail, lblContent, lblCustomKind;
    private System.Windows.Forms.NumericUpDown numPay;
    private System.Windows.Forms.ComboBox cbFlag, cbKind;
    private System.Windows.Forms.RadioButton rbCheckYes, rbCheckNo;
    private System.Windows.Forms.TextBox txtMail, txtContent, txtCustomKind;

    protected override void Dispose(bool disposing)
    { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }

    private static System.Windows.Forms.ToolStripButton TBtn(string text)
        => new System.Windows.Forms.ToolStripButton(text)
        {
            DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text,
            AutoSize = true,
            Padding = new System.Windows.Forms.Padding(8, 0, 8, 0)
        };

    private void InitializeComponent()
    {
        // 工具列
        this.toolBar = new System.Windows.Forms.ToolStrip();
        this.btnSave    = TBtn("儲存");
        this.btnCancel  = TBtn("取消");
        this.btnPreview = TBtn("預覽");
        this.btnPrint   = TBtn("列印");
        this.btnExit    = TBtn("離開");
        this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[]{
            btnSave, btnCancel,
            new System.Windows.Forms.ToolStripSeparator(),
            btnPreview, btnPrint,
            new System.Windows.Forms.ToolStripSeparator(),
            btnExit });
        this.toolBar.Dock = System.Windows.Forms.DockStyle.Top;
        this.btnSave.Click+=btnSave_Click; this.btnCancel.Click+=btnCancel_Click;
        this.btnPreview.Click+=btnPreview_Click; this.btnPrint.Click+=btnPrint_Click;
        this.btnExit.Click+=btnExit_Click;

        this.lblTitle = new System.Windows.Forms.Label{ Text = "薪資輸入－全公司",
            Location = new System.Drawing.Point(180, 75), Size = new System.Drawing.Size(170, 24),
            Font = new System.Drawing.Font("標楷體", 13F, System.Drawing.FontStyle.Bold),
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter };

        this.lblPay = new System.Windows.Forms.Label{ Text = "輸入本月薪資",
            Location = new System.Drawing.Point(120, 115), Size = new System.Drawing.Size(120, 21) };
        this.numPay = new System.Windows.Forms.NumericUpDown{
            Location = new System.Drawing.Point(250, 112), Size = new System.Drawing.Size(150, 23),
            Maximum = 9999999, ThousandsSeparator = true };

        this.lblFlag = new System.Windows.Forms.Label{ Text = "存提區分",
            Location = new System.Drawing.Point(120, 150), Size = new System.Drawing.Size(120, 21) };
        this.cbFlag = new System.Windows.Forms.ComboBox{
            Location = new System.Drawing.Point(250, 147), Size = new System.Drawing.Size(150, 23),
            DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList };

        this.lblKind = new System.Windows.Forms.Label{ Text = "轉帳類別",
            Location = new System.Drawing.Point(120, 185), Size = new System.Drawing.Size(120, 21) };
        this.cbKind = new System.Windows.Forms.ComboBox{
            Location = new System.Drawing.Point(250, 182), Size = new System.Drawing.Size(200, 23),
            DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList };

        this.lblCustomKind = new System.Windows.Forms.Label{ Text = "自訂類別名稱",
            Location = new System.Drawing.Point(460, 185), Size = new System.Drawing.Size(90, 21),
            Visible = false, ForeColor = System.Drawing.Color.DarkRed };
        this.txtCustomKind = new System.Windows.Forms.TextBox{
            Location = new System.Drawing.Point(555, 182), Size = new System.Drawing.Size(120, 23),
            MaxLength = 10, Visible = false, PlaceholderText = "如：年終獎金" };

        this.lblNote = new System.Windows.Forms.Label{ Text = "註：選 '97' 時請於右方輸入自訂名稱",
            Location = new System.Drawing.Point(250, 210), Size = new System.Drawing.Size(280, 18),
            Font = new System.Drawing.Font("新細明體", 9F), ForeColor = System.Drawing.Color.DarkRed };

        this.lblCheck = new System.Windows.Forms.Label{ Text = "薪資入帳是否檢查身份證號碼",
            Location = new System.Drawing.Point(120, 240), Size = new System.Drawing.Size(220, 21) };
        this.rbCheckYes = new System.Windows.Forms.RadioButton{ Text = ".是",
            Location = new System.Drawing.Point(360, 238), Size = new System.Drawing.Size(50, 23) };
        this.rbCheckNo  = new System.Windows.Forms.RadioButton{ Text = ".否",
            Location = new System.Drawing.Point(420, 238), Size = new System.Drawing.Size(50, 23) };

        this.lblSep = new System.Windows.Forms.Label{ Text = "---------------以下為非必要欄位---------------",
            Location = new System.Drawing.Point(80, 272), Size = new System.Drawing.Size(420, 18),
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter };

        this.lblMail = new System.Windows.Forms.Label{ Text = "傳真或Email",
            Location = new System.Drawing.Point(120, 300), Size = new System.Drawing.Size(120, 21) };
        this.txtMail = new System.Windows.Forms.TextBox{
            Location = new System.Drawing.Point(250, 297), Size = new System.Drawing.Size(280, 23), MaxLength = 60, PlaceholderText = "範例：employee@company.com" };

        this.lblContent = new System.Windows.Forms.Label{ Text = "傳真或Email內容",
            Location = new System.Drawing.Point(120, 330), Size = new System.Drawing.Size(120, 21) };
        this.txtContent = new System.Windows.Forms.TextBox{
            Location = new System.Drawing.Point(250, 327), Size = new System.Drawing.Size(280, 60),
            Multiline = true, MaxLength = 75 };

        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(700, 420);
        this.Controls.AddRange(new System.Windows.Forms.Control[]{
            toolBar, lblTitle,
            lblPay, numPay, lblFlag, cbFlag,
            lblKind, cbKind, lblCustomKind, txtCustomKind, lblNote,
            lblCheck, rbCheckYes, rbCheckNo,
            lblSep, lblMail, txtMail, lblContent, txtContent });
        this.Name = "BK102";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "薪資輸入－全公司";
    }
}
