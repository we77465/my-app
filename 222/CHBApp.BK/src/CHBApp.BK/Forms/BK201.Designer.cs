namespace CHBApp.BK.Forms;

partial class BK201
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.GroupBox grp;
    private System.Windows.Forms.RadioButton rb1, rb2, rb3, rb4;
    private System.Windows.Forms.Label lblKey;
    private System.Windows.Forms.TextBox txtKey;
    private System.Windows.Forms.Button btnQuery, btnPreview, btnPrint, btnExit;
    private System.Windows.Forms.DataGridView grid;
    private System.Windows.Forms.StatusStrip statusBar;
    private System.Windows.Forms.ToolStripStatusLabel lblTotal;

    protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }

    private void InitializeComponent()
    {
        // ===== 頂部查詢列 =====
        this.grp = new System.Windows.Forms.GroupBox
        {
            Text = "查詢方式",
            Dock = System.Windows.Forms.DockStyle.Top,
            Height = 56
        };
        this.rb1 = new System.Windows.Forms.RadioButton { Text = "依員工代號", Checked = true, Location = new System.Drawing.Point(15, 22), AutoSize = true };
        this.rb2 = new System.Windows.Forms.RadioButton { Text = "依姓名", Location = new System.Drawing.Point(120, 22), AutoSize = true };
        this.rb3 = new System.Windows.Forms.RadioButton { Text = "依帳號", Location = new System.Drawing.Point(220, 22), AutoSize = true };
        this.rb4 = new System.Windows.Forms.RadioButton { Text = "依統編", Location = new System.Drawing.Point(320, 22), AutoSize = true };
        this.lblKey = new System.Windows.Forms.Label { Text = "查詢值：", Location = new System.Drawing.Point(420, 24), AutoSize = true };
        this.txtKey = new System.Windows.Forms.TextBox { Location = new System.Drawing.Point(480, 21), Size = new System.Drawing.Size(150, 23), PlaceholderText = "輸入關鍵字 (空白=全部)" };
        this.btnQuery   = new System.Windows.Forms.Button { Text = "執行查詢", Location = new System.Drawing.Point(640, 20), Size = new System.Drawing.Size(80, 25) };
        this.btnPreview = new System.Windows.Forms.Button { Text = "預覽",     Location = new System.Drawing.Point(730, 20), Size = new System.Drawing.Size(70, 25) };
        this.btnPrint   = new System.Windows.Forms.Button { Text = "列印",     Location = new System.Drawing.Point(810, 20), Size = new System.Drawing.Size(70, 25) };
        this.btnExit    = new System.Windows.Forms.Button { Text = "離開",     Location = new System.Drawing.Point(890, 20), Size = new System.Drawing.Size(70, 25) };
        this.grp.Controls.AddRange(new System.Windows.Forms.Control[] {
            rb1, rb2, rb3, rb4, lblKey, txtKey, btnQuery, btnPreview, btnPrint, btnExit
        });
        this.btnQuery.Click   += btnQuery_Click;
        this.btnPreview.Click += btnPreview_Click;
        this.btnPrint.Click   += btnPrint_Click;
        this.btnExit.Click    += btnExit_Click;

        // ===== 主 DataGridView (顯示 13 欄中文表頭) =====
        this.grid = new System.Windows.Forms.DataGridView
        {
            Dock = System.Windows.Forms.DockStyle.Fill,
            ReadOnly = true,
            AutoGenerateColumns = false,    // 由 BK201.cs BuildColumns 自定欄位
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ColumnHeadersDefaultCellStyle = { Font = new System.Drawing.Font("新細明體", 10F, System.Drawing.FontStyle.Bold) },
            RowHeadersVisible = false,
            SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        };

        // ===== 狀態列 =====
        this.statusBar = new System.Windows.Forms.StatusStrip();
        this.lblTotal  = new System.Windows.Forms.ToolStripStatusLabel("");
        this.statusBar.Items.Add(this.lblTotal);

        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1180, 600);
        this.Controls.Add(this.grid);
        this.Controls.Add(this.statusBar);
        this.Controls.Add(this.grp);
        this.Name = "BK201";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "員工資料檔維護查詢";
    }
}
