namespace CHBApp.BK.Forms;

partial class BK_EmpImport
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblTitle, lblTip, lblPathLbl;
    private System.Windows.Forms.TextBox txtPath;
    private System.Windows.Forms.Button btnPick, btnRun, btnFormat, btnExit;
    private System.Windows.Forms.GroupBox grpMode;
    private System.Windows.Forms.RadioButton rbReplace, rbSkip;
    private System.Windows.Forms.Label lblStatus, lblTotal, lblOk, lblReplace, lblAdd, lblErr;
    private System.Windows.Forms.RichTextBox log;

    protected override void Dispose(bool d)
    { if (d && components != null) components.Dispose(); base.Dispose(d); }

    private void InitializeComponent()
    {
        // 主標題
        this.lblTitle = new System.Windows.Forms.Label
        {
            Text = "員工資料文字檔轉入",
            Location = new System.Drawing.Point(180, 15),
            Size = new System.Drawing.Size(280, 28),
            Font = new System.Drawing.Font("標楷體", 14F, System.Drawing.FontStyle.Bold),
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        };

        // 提示
        this.lblTip = new System.Windows.Forms.Label
        {
            Text = "資料要轉入前請先選擇資料檔路徑 & 名稱：",
            Location = new System.Drawing.Point(20, 55),
            Size = new System.Drawing.Size(360, 21),
            Font = new System.Drawing.Font("新細明體", 10F)
        };

        // 路徑欄位
        this.lblPathLbl = new System.Windows.Forms.Label
        {
            Text = "檔案路徑：",
            Location = new System.Drawing.Point(20, 85),
            Size = new System.Drawing.Size(80, 23)
        };
        this.txtPath = new System.Windows.Forms.TextBox
        {
            Location = new System.Drawing.Point(105, 82),
            Size = new System.Drawing.Size(420, 23),
            Text = @"C:\CHBAPP374\BKAC_TEST.TXT", PlaceholderText = "請選擇 BKAC.TXT 檔案路徑"
        };
        this.btnPick = new System.Windows.Forms.Button
        {
            Text = "選擇...",
            Location = new System.Drawing.Point(535, 81),
            Size = new System.Drawing.Size(70, 25)
        };

        // 轉入方式
        this.grpMode = new System.Windows.Forms.GroupBox
        {
            Text = "轉入方式",
            Location = new System.Drawing.Point(20, 120),
            Size = new System.Drawing.Size(380, 55)
        };
        this.rbReplace = new System.Windows.Forms.RadioButton
        {
            Text = "依編號置換 (覆寫)",
            Checked = true,
            Location = new System.Drawing.Point(15, 22),
            AutoSize = true
        };
        this.rbSkip = new System.Windows.Forms.RadioButton
        {
            Text = "已存在則略過",
            Location = new System.Drawing.Point(180, 22),
            AutoSize = true
        };
        this.grpMode.Controls.AddRange(new System.Windows.Forms.Control[] { rbReplace, rbSkip });

        // 操作鈕
        this.btnFormat = new System.Windows.Forms.Button
        {
            Text = "格式列印",
            Location = new System.Drawing.Point(415, 130),
            Size = new System.Drawing.Size(80, 35)
        };
        this.btnRun = new System.Windows.Forms.Button
        {
            Text = "確定 / 處理",
            Location = new System.Drawing.Point(505, 130),
            Size = new System.Drawing.Size(100, 35)
        };
        this.btnExit = new System.Windows.Forms.Button
        {
            Text = "離開",
            Location = new System.Drawing.Point(615, 130),
            Size = new System.Drawing.Size(70, 35)
        };

        // 處理狀態統計區
        this.lblStatus = new System.Windows.Forms.Label
        {
            Text = "處理狀態",
            Location = new System.Drawing.Point(20, 195),
            Size = new System.Drawing.Size(80, 21),
            Font = new System.Drawing.Font("新細明體", 10F, System.Drawing.FontStyle.Bold)
        };
        this.lblTotal   = new System.Windows.Forms.Label { Text = "資料總筆數：0", Location = new System.Drawing.Point(20, 220), Size = new System.Drawing.Size(160, 21) };
        this.lblOk      = new System.Windows.Forms.Label { Text = "已處理筆數：0", Location = new System.Drawing.Point(180, 220), Size = new System.Drawing.Size(160, 21) };
        this.lblReplace = new System.Windows.Forms.Label { Text = "覆寫筆數　：0", Location = new System.Drawing.Point(340, 220), Size = new System.Drawing.Size(160, 21) };
        this.lblAdd     = new System.Windows.Forms.Label { Text = "新增筆數　：0", Location = new System.Drawing.Point(500, 220), Size = new System.Drawing.Size(160, 21) };
        this.lblErr     = new System.Windows.Forms.Label { Text = "錯誤筆數　：0", Location = new System.Drawing.Point(20, 245), Size = new System.Drawing.Size(160, 21), ForeColor = System.Drawing.Color.DarkRed };

        // 日誌
        this.log = new System.Windows.Forms.RichTextBox
        {
            Location = new System.Drawing.Point(20, 275),
            Size = new System.Drawing.Size(665, 200),
            ReadOnly = true,
            Font = new System.Drawing.Font("Consolas", 9F)
        };

        this.btnPick.Click   += btnPick_Click;
        this.btnRun.Click    += btnRun_Click;
        this.btnFormat.Click += btnFormat_Click;
        this.btnExit.Click   += btnExit_Click;

        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(710, 495);
        this.Controls.AddRange(new System.Windows.Forms.Control[] {
            lblTitle, lblTip, lblPathLbl, txtPath, btnPick,
            grpMode, btnFormat, btnRun, btnExit,
            lblStatus, lblTotal, lblOk, lblReplace, lblAdd, lblErr,
            log
        });
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.Name = "BK_EmpImport";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "員工資料文字檔轉入";
    }
}
