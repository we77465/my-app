namespace CHBApp.BK.Forms;
partial class BK104
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Button btnClear, btnHelp, btnExit;
    protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
    private void InitializeComponent()
    {
        this.lblTitle = new System.Windows.Forms.Label{Text="清除本月薪資",Location=new System.Drawing.Point(170,40),Size=new System.Drawing.Size(160,30),Font=new System.Drawing.Font("標楷體",14F,System.Drawing.FontStyle.Bold),TextAlign=System.Drawing.ContentAlignment.MiddleCenter};
        this.btnClear = new System.Windows.Forms.Button{Text="清除薪資",Location=new System.Drawing.Point(50,110),Size=new System.Drawing.Size(120,45)};
        this.btnHelp  = new System.Windows.Forms.Button{Text="說明",   Location=new System.Drawing.Point(190,110),Size=new System.Drawing.Size(120,45)};
        this.btnExit  = new System.Windows.Forms.Button{Text="離開",   Location=new System.Drawing.Point(330,110),Size=new System.Drawing.Size(120,45)};
        this.btnClear.Click+=btnClear_Click;this.btnHelp.Click+=btnHelp_Click;this.btnExit.Click+=btnExit_Click;
        this.AutoScaleDimensions=new System.Drawing.SizeF(7F,15F);this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize=new System.Drawing.Size(500,200);
        this.Controls.AddRange(new System.Windows.Forms.Control[]{lblTitle,btnClear,btnHelp,btnExit});
        this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox=false;this.MinimizeBox=false;
        this.Name="BK104";this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text="清除本月薪資";
    }
}
