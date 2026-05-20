namespace CHBApp.BK.Forms;

partial class BK101
{
    private System.ComponentModel.IContainer components = null;

    // 工具列
    private System.Windows.Forms.ToolStrip toolBar;
    private System.Windows.Forms.ToolStripButton btnClear;   // 清檔
    private System.Windows.Forms.ToolStripButton btnSave;    // 儲存
    private System.Windows.Forms.ToolStripButton btnCancel;  // 取消
    private System.Windows.Forms.ToolStripButton btnPreview; // 預覽
    private System.Windows.Forms.ToolStripButton btnPrint;   // 列印
    private System.Windows.Forms.ToolStripButton btnExit;    // 離開

    // 標題
    private System.Windows.Forms.Label lblTitle;

    // 身份別
    private System.Windows.Forms.Label lblMORF;
    private System.Windows.Forms.RadioButton rbLocal, rbForeign, rbCorp;

    // 主要欄位
    private System.Windows.Forms.Label lblEmpNo, lblEmpName, lblPid, lblAccNo;
    private System.Windows.Forms.TextBox txtEmpNo, txtEmpName, txtPid, txtAccNo;

    // 分隔線 + 非必要欄位
    private System.Windows.Forms.Label lblSep;
    private System.Windows.Forms.Label lblFax, lblMail;
    private System.Windows.Forms.TextBox txtFax, txtMail;

    // 底部狀態列 (顯示新增/修改模式)
    private System.Windows.Forms.StatusStrip statusBar;
    private System.Windows.Forms.ToolStripStatusLabel lblPos;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private static System.Windows.Forms.ToolStripButton TBtn(string text)
        => new System.Windows.Forms.ToolStripButton(text)
        {
            DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text,
            AutoSize = true,
            Padding = new System.Windows.Forms.Padding(8, 0, 8, 0)
        };

    private void InitializeComponent()
    {
        // ===== 工具列 =====
        this.toolBar    = new System.Windows.Forms.ToolStrip();
        this.btnClear   = TBtn("清檔");
        this.btnSave    = TBtn("儲存");
        this.btnCancel  = TBtn("取消");
        this.btnPreview = TBtn("預覽");
        this.btnPrint   = TBtn("列印");
        this.btnExit    = TBtn("離開");
        this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
        {
            btnClear,
            new System.Windows.Forms.ToolStripSeparator(),
            btnSave, btnCancel,
            new System.Windows.Forms.ToolStripSeparator(),
            btnPreview, btnPrint,
            new System.Windows.Forms.ToolStripSeparator(),
            btnExit
        });
        this.toolBar.Dock = System.Windows.Forms.DockStyle.Top;
        this.btnClear.Click   += btnClear_Click;
        this.btnSave.Click    += btnSave_Click;
        this.btnCancel.Click  += btnCancel_Click;
        this.btnPreview.Click += btnPreview_Click;
        this.btnPrint.Click   += btnPrint_Click;
        this.btnExit.Click    += btnExit_Click;

        // ===== 主標題 =====
        this.lblTitle = new System.Windows.Forms.Label
        {
            Text = "員工基本資料建檔",
            Location = new System.Drawing.Point(202, 75),
            Size = new System.Drawing.Size(168, 24),
            Font = new System.Drawing.Font("標楷體", 14F, System.Drawing.FontStyle.Bold),
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        };

        // ===== 身份別 =====
        this.lblMORF = new System.Windows.Forms.Label
        {
            Text = "身份別",
            Location = new System.Drawing.Point(120, 115),
            Size = new System.Drawing.Size(80, 21),
            Font = new System.Drawing.Font("新細明體", 10F)
        };
        this.rbLocal   = new System.Windows.Forms.RadioButton { Text = "本國人", Location = new System.Drawing.Point(210, 113), AutoSize = true, Checked = true };
        this.rbForeign = new System.Windows.Forms.RadioButton { Text = "外國人", Location = new System.Drawing.Point(290, 113), AutoSize = true };
        this.rbCorp    = new System.Windows.Forms.RadioButton { Text = "公司",   Location = new System.Drawing.Point(370, 113), AutoSize = true };
        this.rbLocal.CheckedChanged   += rbMORF_CheckedChanged;
        this.rbForeign.CheckedChanged += rbMORF_CheckedChanged;
        this.rbCorp.CheckedChanged    += rbMORF_CheckedChanged;

        // ===== 主要欄位 =====
        this.lblEmpNo  = new System.Windows.Forms.Label   { Text = "員工編號",      Location = new System.Drawing.Point(120, 150), Size = new System.Drawing.Size(80, 21), Font = new System.Drawing.Font("新細明體", 10F) };
        this.txtEmpNo  = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(210, 147), Size = new System.Drawing.Size(150, 23), MaxLength = 10, PlaceholderText = "範例：E0001" };

        this.lblEmpName= new System.Windows.Forms.Label   { Text = "姓     名",     Location = new System.Drawing.Point(120, 180), Size = new System.Drawing.Size(80, 21), Font = new System.Drawing.Font("新細明體", 10F) };
        this.txtEmpName= new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(210, 177), Size = new System.Drawing.Size(150, 23), MaxLength = 10, PlaceholderText = "範例：王大明" };

        this.lblPid    = new System.Windows.Forms.Label   { Text = "身份字號/統編", Location = new System.Drawing.Point(120, 210), Size = new System.Drawing.Size(105, 21), Font = new System.Drawing.Font("新細明體", 10F) };
        this.txtPid    = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(230, 207), Size = new System.Drawing.Size(150, 23), MaxLength = 10, PlaceholderText = "範例：A123456789" };

        this.lblAccNo  = new System.Windows.Forms.Label   { Text = "帳     號",     Location = new System.Drawing.Point(120, 240), Size = new System.Drawing.Size(80, 21), Font = new System.Drawing.Font("新細明體", 10F) };
        this.txtAccNo  = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(210, 237), Size = new System.Drawing.Size(180, 23), MaxLength = 14, PlaceholderText = "範例：00951850123456" };

        // ===== 分隔線 =====
        this.lblSep = new System.Windows.Forms.Label
        {
            Text = "---------------以下為非必要欄位---------------",
            Location = new System.Drawing.Point(80, 273),
            Size = new System.Drawing.Size(420, 18),
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
            Font = new System.Drawing.Font("新細明體", 9F)
        };

        // ===== 傳真 / Email =====
        this.lblFax  = new System.Windows.Forms.Label  { Text = "傳真號碼", Location = new System.Drawing.Point(120, 300), Size = new System.Drawing.Size(80, 21), Font = new System.Drawing.Font("新細明體", 10F) };
        this.txtFax  = new System.Windows.Forms.TextBox{ Location = new System.Drawing.Point(210, 297), Size = new System.Drawing.Size(150, 23), MaxLength = 10, PlaceholderText = "範例：02-23456789" };

        this.lblMail = new System.Windows.Forms.Label  { Text = "Email",   Location = new System.Drawing.Point(120, 330), Size = new System.Drawing.Size(80, 21), Font = new System.Drawing.Font("新細明體", 10F) };
        this.txtMail = new System.Windows.Forms.TextBox{ Location = new System.Drawing.Point(210, 327), Size = new System.Drawing.Size(280, 23), MaxLength = 60, PlaceholderText = "範例：employee@company.com" };

        // ===== 狀態列 =====
        this.statusBar = new System.Windows.Forms.StatusStrip();
        this.lblPos    = new System.Windows.Forms.ToolStripStatusLabel("新增員工 (請輸入員工編號)")
        {
            ForeColor = System.Drawing.Color.DarkGreen,
            Font = new System.Drawing.Font("新細明體", 10F, System.Drawing.FontStyle.Bold)
        };
        this.statusBar.Items.Add(this.lblPos);

        // ===== 表單 =====
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(580, 410);
        this.Controls.Add(this.toolBar);
        this.Controls.Add(this.lblTitle);
        this.Controls.Add(this.lblMORF);
        this.Controls.Add(this.rbLocal);
        this.Controls.Add(this.rbForeign);
        this.Controls.Add(this.rbCorp);
        this.Controls.Add(this.lblEmpNo);   this.Controls.Add(this.txtEmpNo);
        this.Controls.Add(this.lblEmpName); this.Controls.Add(this.txtEmpName);
        this.Controls.Add(this.lblPid);     this.Controls.Add(this.txtPid);
        this.Controls.Add(this.lblAccNo);   this.Controls.Add(this.txtAccNo);
        this.Controls.Add(this.lblSep);
        this.Controls.Add(this.lblFax);     this.Controls.Add(this.txtFax);
        this.Controls.Add(this.lblMail);    this.Controls.Add(this.txtMail);
        this.Controls.Add(this.statusBar);
        this.Name = "BK101";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "員工基本資料建檔";
    }
}
