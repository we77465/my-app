namespace CHBApp.BK.Forms;
partial class BK202
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.CheckBox chkRange;
    private System.Windows.Forms.Label lblFrom, lblTo;
    private System.Windows.Forms.TextBox txtFrom, txtTo;
    private System.Windows.Forms.Button btnPreview, btnPrint, btnExit;
    private System.Windows.Forms.RichTextBox log;
    protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
    private void InitializeComponent()
    {
        this.chkRange=new System.Windows.Forms.CheckBox{Text="使用員工編號起迄條件 (列印本月薪資二)",Location=new System.Drawing.Point(20,15),AutoSize=true};
        this.lblFrom=new System.Windows.Forms.Label{Text="起始：",Location=new System.Drawing.Point(20,45),AutoSize=true};
        this.txtFrom=new System.Windows.Forms.TextBox{Location=new System.Drawing.Point(70,42),Size=new System.Drawing.Size(120,23)};
        this.lblTo=new System.Windows.Forms.Label{Text="迄止：",Location=new System.Drawing.Point(210,45),AutoSize=true};
        this.txtTo=new System.Windows.Forms.TextBox{Location=new System.Drawing.Point(260,42),Size=new System.Drawing.Size(120,23)};
        this.btnPreview=new System.Windows.Forms.Button{Text="預覽",Location=new System.Drawing.Point(400,40),Size=new System.Drawing.Size(80,28)};
        this.btnPrint=new System.Windows.Forms.Button{Text="列印",Location=new System.Drawing.Point(490,40),Size=new System.Drawing.Size(80,28)};
        this.btnExit=new System.Windows.Forms.Button{Text="離開",Location=new System.Drawing.Point(580,40),Size=new System.Drawing.Size(80,28)};
        this.log=new System.Windows.Forms.RichTextBox{Location=new System.Drawing.Point(20,80),Size=new System.Drawing.Size(820,400),ReadOnly=true,Font=new System.Drawing.Font("Consolas",9F)};
        this.btnPreview.Click+=btnPreview_Click;this.btnPrint.Click+=btnPrint_Click;this.btnExit.Click+=btnExit_Click;
        this.AutoScaleDimensions=new System.Drawing.SizeF(7F,15F);this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize=new System.Drawing.Size(860,500);
        this.Controls.AddRange(new System.Windows.Forms.Control[]{chkRange,lblFrom,txtFrom,lblTo,txtTo,btnPreview,btnPrint,btnExit,log});
        this.Name="BK202";this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text="列印本月薪資 / 列印本月薪資二";
    }
}
