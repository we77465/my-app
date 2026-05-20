namespace CHBApp.BK.Forms;

partial class BK402
{
    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }
    private void InitializeComponent()
    {
        BuildBaseUi("分組轉換磁片作業(舊規格)");
        this.lblFromLbl = new System.Windows.Forms.Label
        {
            Text = "員工編組起號 :",
            Location = new System.Drawing.Point(80, 180),
            Size = new System.Drawing.Size(120, 24)
        };
        this.txtFrom = new System.Windows.Forms.TextBox
        {
            Location = new System.Drawing.Point(210, 177),
            Size = new System.Drawing.Size(120, 23),
            MaxLength = 10
        };
        this.lblToLbl = new System.Windows.Forms.Label
        {
            Text = "員工編組迄號 :",
            Location = new System.Drawing.Point(80, 210),
            Size = new System.Drawing.Size(120, 24)
        };
        this.txtTo = new System.Windows.Forms.TextBox
        {
            Location = new System.Drawing.Point(210, 207),
            Size = new System.Drawing.Size(120, 23),
            MaxLength = 10
        };
        this.Controls.AddRange(new System.Windows.Forms.Control[] {
            lblFromLbl, txtFrom, lblToLbl, txtTo
        });
        this.Name = "BK402";
        this.ClientSize = new System.Drawing.Size(580, 400);
    }
}
