namespace CHBApp.BK.Forms;

partial class BK503
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.DataGridView grid;
    private System.Windows.Forms.ToolStrip toolBar;
    private System.Windows.Forms.ToolStripButton btnPreview, btnPrint, btnExit;
    private System.Windows.Forms.StatusStrip status;
    private System.Windows.Forms.ToolStripStatusLabel lblTotal;

    protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }

    private void InitializeComponent()
    {
        this.toolBar    = new System.Windows.Forms.ToolStrip();
        this.btnPreview = new System.Windows.Forms.ToolStripButton("預覽") { DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text };
        this.btnPrint   = new System.Windows.Forms.ToolStripButton("列印") { DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text };
        this.btnExit    = new System.Windows.Forms.ToolStripButton("離開") { DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text };
        this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
        {
            btnPreview, btnPrint,
            new System.Windows.Forms.ToolStripSeparator(),
            btnExit
        });
        this.toolBar.Dock = System.Windows.Forms.DockStyle.Top;
        this.btnPreview.Click += btnPreview_Click;
        this.btnPrint.Click   += btnPrint_Click;
        this.btnExit.Click    += btnExit_Click;

        this.grid = new System.Windows.Forms.DataGridView
        {
            Dock = System.Windows.Forms.DockStyle.Fill,
            AutoGenerateColumns = true,
            ReadOnly = true
        };
        this.status = new System.Windows.Forms.StatusStrip();
        this.lblTotal = new System.Windows.Forms.ToolStripStatusLabel("");
        this.status.Items.Add(this.lblTotal);

        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(900, 500);
        this.Controls.Add(this.grid);
        this.Controls.Add(this.toolBar);
        this.Controls.Add(this.status);
        this.Name = "BK503";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "成功明細查詢";
    }
}
