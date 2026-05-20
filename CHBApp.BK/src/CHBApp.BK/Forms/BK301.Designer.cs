namespace CHBApp.BK.Forms;
partial class BK301
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblTitle, lblMethod, lblRate, lblTip;
    private System.Windows.Forms.ComboBox cbMethod;
    private System.Windows.Forms.NumericUpDown numRate;
    private System.Windows.Forms.Button btnPreview, btnPrint, btnHelp, btnExit;
    protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
    private void InitializeComponent()
    {
        this.lblTitle = new System.Windows.Forms.Label{Text="列印本月薪資",Location=new System.Drawing.Point(180,30),Size=new System.Drawing.Size(160,28),Font=new System.Drawing.Font("標楷體",14F,System.Drawing.FontStyle.Bold),TextAlign=System.Drawing.ContentAlignment.MiddleCenter};

        this.lblMethod=new System.Windows.Forms.Label{Text="列印方式 :",Location=new System.Drawing.Point(80,80),Size=new System.Drawing.Size(80,24)};
        this.cbMethod = new System.Windows.Forms.ComboBox{Location=new System.Drawing.Point(170,77),Size=new System.Drawing.Size(160,23),DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList};
        this.cbMethod.Items.AddRange(new object[]{"1.員工編號(文字)","2.員工編號(數字)","3.員工姓名"});
        this.cbMethod.SelectedIndex=0;

        this.lblRate=new System.Windows.Forms.Label{Text="匯率 :",Location=new System.Drawing.Point(360,80),Size=new System.Drawing.Size(50,24)};
        this.numRate=new System.Windows.Forms.NumericUpDown{Location=new System.Drawing.Point(420,77),Size=new System.Drawing.Size(80,23),DecimalPlaces=4,Increment=0.0001m,Maximum=999,Value=1};

        this.lblTip=new System.Windows.Forms.Label{Text="    請設定好印表機, 並開啟印表機....",Location=new System.Drawing.Point(80,180),Size=new System.Drawing.Size(380,20),Font=new System.Drawing.Font("新細明體",10F),ForeColor=System.Drawing.Color.DarkRed};

        this.btnPreview=new System.Windows.Forms.Button{Text="預覽",Location=new System.Drawing.Point(80,130),Size=new System.Drawing.Size(80,40)};
        this.btnPrint  =new System.Windows.Forms.Button{Text="列印",Location=new System.Drawing.Point(180,130),Size=new System.Drawing.Size(80,40)};
        this.btnHelp   =new System.Windows.Forms.Button{Text="說明",Location=new System.Drawing.Point(280,130),Size=new System.Drawing.Size(80,40)};
        this.btnExit   =new System.Windows.Forms.Button{Text="離開",Location=new System.Drawing.Point(380,130),Size=new System.Drawing.Size(80,40)};
        this.btnPreview.Click+=btnPreview_Click;this.btnPrint.Click+=btnPrint_Click;
        this.btnHelp.Click+=btnHelp_Click;this.btnExit.Click+=btnExit_Click;

        this.AutoScaleDimensions=new System.Drawing.SizeF(7F,15F);this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize=new System.Drawing.Size(550,230);
        this.Controls.AddRange(new System.Windows.Forms.Control[]{lblTitle,lblMethod,cbMethod,lblRate,numRate,lblTip,btnPreview,btnPrint,btnHelp,btnExit});
        this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox=false;
        this.Name="BK301";this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text="列印本月薪資";
    }
}
