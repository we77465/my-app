namespace CHBApp.BK.Forms;
partial class BK505
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblPay;
    private System.Windows.Forms.DateTimePicker dt;
    private System.Windows.Forms.Button btnRun, btnExit;
    private System.Windows.Forms.Label lblTotal;
    protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
    private void InitializeComponent()
    {
        this.lblPay = new System.Windows.Forms.Label{Text="撥帳日期：",Location=new System.Drawing.Point(20,25),AutoSize=true};
        this.dt     = new System.Windows.Forms.DateTimePicker{Location=new System.Drawing.Point(120,22),Size=new System.Drawing.Size(150,23),Format=System.Windows.Forms.DateTimePickerFormat.Short};
        this.btnRun = new System.Windows.Forms.Button{Text="產生二次處理磁片 (百年規格)",Location=new System.Drawing.Point(20,65),Size=new System.Drawing.Size(240,35)};
        this.btnExit= new System.Windows.Forms.Button{Text="離開",Location=new System.Drawing.Point(280,65),Size=new System.Drawing.Size(100,35)};
        this.lblTotal = new System.Windows.Forms.Label{Text="",Location=new System.Drawing.Point(20,120),AutoSize=true};
        this.btnRun.Click+=btnRun_Click;this.btnExit.Click+=btnExit_Click;
        this.AutoScaleDimensions=new System.Drawing.SizeF(7F,15F);this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize=new System.Drawing.Size(460,180);
        this.Controls.AddRange(new System.Windows.Forms.Control[]{lblPay,dt,btnRun,btnExit,lblTotal});
        this.Name="BK505";this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text="產生二次處理磁片 (百年規格)";
    }
}
