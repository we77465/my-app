namespace CHBApp.BK.Forms;
partial class BK404
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblSrc, lblDst;
    private System.Windows.Forms.TextBox txtSrc, txtDst;
    private System.Windows.Forms.Button btnSrc, btnDst, btnRun, btnExit;
    private System.Windows.Forms.RichTextBox log;
    protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
    private void InitializeComponent()
    {
        this.lblSrc = new System.Windows.Forms.Label{Text="來源路徑名稱：",Location=new System.Drawing.Point(20,20),AutoSize=true};
        this.txtSrc = new System.Windows.Forms.TextBox{Location=new System.Drawing.Point(150,17),Size=new System.Drawing.Size(380,23)};
        this.btnSrc = new System.Windows.Forms.Button{Text="選擇...",Location=new System.Drawing.Point(540,16),Size=new System.Drawing.Size(70,25)};
        this.lblDst = new System.Windows.Forms.Label{Text="產生路徑名稱：",Location=new System.Drawing.Point(20,55),AutoSize=true};
        this.txtDst = new System.Windows.Forms.TextBox{Location=new System.Drawing.Point(150,52),Size=new System.Drawing.Size(380,23)};
        this.btnDst = new System.Windows.Forms.Button{Text="選擇...",Location=new System.Drawing.Point(540,51),Size=new System.Drawing.Size(70,25)};
        this.btnRun = new System.Windows.Forms.Button{Text="新格式明細產生",Location=new System.Drawing.Point(150,90),Size=new System.Drawing.Size(160,32)};
        this.btnExit= new System.Windows.Forms.Button{Text="離開",Location=new System.Drawing.Point(330,90),Size=new System.Drawing.Size(80,32)};
        this.log    = new System.Windows.Forms.RichTextBox{Location=new System.Drawing.Point(20,135),Size=new System.Drawing.Size(620,200),ReadOnly=true,Font=new System.Drawing.Font("Consolas",9F)};
        this.btnSrc.Click+=btnSrc_Click;this.btnDst.Click+=btnDst_Click;this.btnRun.Click+=btnRun_Click;this.btnExit.Click+=btnExit_Click;
        this.AutoScaleDimensions=new System.Drawing.SizeF(7F,15F);this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize=new System.Drawing.Size(660,360);
        this.Controls.AddRange(new System.Windows.Forms.Control[]{lblSrc,txtSrc,btnSrc,lblDst,txtDst,btnDst,btnRun,btnExit,log});
        this.Name="BK404";this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text="中連匯款資料檔 -> 本行代理撥帳格式";
    }
}
