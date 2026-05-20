namespace CHBApp.BK.Forms;
partial class BK302
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblTitle, lblFrom, lblTo, lblTip;
    private System.Windows.Forms.TextBox txtFrom, txtTo;
    private System.Windows.Forms.Button btnPreview, btnPrint, btnHelp, btnExit;
    protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
    private void InitializeComponent()
    {
        this.lblTitle = new System.Windows.Forms.Label{Text="列印本月資料",Location=new System.Drawing.Point(180,30),Size=new System.Drawing.Size(160,28),Font=new System.Drawing.Font("標楷體",14F,System.Drawing.FontStyle.Bold),TextAlign=System.Drawing.ContentAlignment.MiddleCenter};

        this.lblFrom = new System.Windows.Forms.Label{Text="編號起號",Location=new System.Drawing.Point(80,80),Size=new System.Drawing.Size(80,24)};
        this.txtFrom = new System.Windows.Forms.TextBox{Location=new System.Drawing.Point(170,77),Size=new System.Drawing.Size(120,23),MaxLength=10,PlaceholderText="範例：E0001"};
        this.lblTo   = new System.Windows.Forms.Label{Text="編號迄號",Location=new System.Drawing.Point(80,110),Size=new System.Drawing.Size(80,24)};
        this.txtTo   = new System.Windows.Forms.TextBox{Location=new System.Drawing.Point(170,107),Size=new System.Drawing.Size(120,23),MaxLength=10,PlaceholderText="範例：E9999"};

        this.lblTip=new System.Windows.Forms.Label{Text="    請設定好印表機, 並開啟印表機....",Location=new System.Drawing.Point(80,210),Size=new System.Drawing.Size(380,20),ForeColor=System.Drawing.Color.DarkRed};

        this.btnPreview=new System.Windows.Forms.Button{Text="預覽",Location=new System.Drawing.Point(80,160),Size=new System.Drawing.Size(80,40)};
        this.btnPrint  =new System.Windows.Forms.Button{Text="列印",Location=new System.Drawing.Point(180,160),Size=new System.Drawing.Size(80,40)};
        this.btnHelp   =new System.Windows.Forms.Button{Text="說明",Location=new System.Drawing.Point(280,160),Size=new System.Drawing.Size(80,40)};
        this.btnExit   =new System.Windows.Forms.Button{Text="離開",Location=new System.Drawing.Point(380,160),Size=new System.Drawing.Size(80,40)};
        this.btnPreview.Click+=btnPreview_Click;this.btnPrint.Click+=btnPrint_Click;
        this.btnHelp.Click+=btnHelp_Click;this.btnExit.Click+=btnExit_Click;

        this.AutoScaleDimensions=new System.Drawing.SizeF(7F,15F);this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize=new System.Drawing.Size(520,250);
        this.Controls.AddRange(new System.Windows.Forms.Control[]{lblTitle,lblFrom,txtFrom,lblTo,txtTo,lblTip,btnPreview,btnPrint,btnHelp,btnExit});
        this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox=false;
        this.Name="BK302";this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text="列印本月資料";
    }
}
