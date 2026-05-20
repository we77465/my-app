namespace CHBApp.BK.Forms;
partial class BK110
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.GroupBox grp;
    private System.Windows.Forms.RadioButton rbAdd, rbSub, rbMul, rbSet;
    private System.Windows.Forms.Label lblV;
    private System.Windows.Forms.NumericUpDown numV;
    private System.Windows.Forms.Button btnRun, btnExit;
    protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
    private void InitializeComponent()
    {
        this.grp = new System.Windows.Forms.GroupBox{Text="變更方式",Location=new System.Drawing.Point(20,20),Size=new System.Drawing.Size(380,55)};
        this.rbAdd=new System.Windows.Forms.RadioButton{Text="加",Checked=true,Location=new System.Drawing.Point(15,22),AutoSize=true};
        this.rbSub=new System.Windows.Forms.RadioButton{Text="減",Location=new System.Drawing.Point(80,22),AutoSize=true};
        this.rbMul=new System.Windows.Forms.RadioButton{Text="乘以(倍)",Location=new System.Drawing.Point(150,22),AutoSize=true};
        this.rbSet=new System.Windows.Forms.RadioButton{Text="設為",Location=new System.Drawing.Point(250,22),AutoSize=true};
        this.grp.Controls.AddRange(new System.Windows.Forms.Control[]{rbAdd,rbSub,rbMul,rbSet});
        this.lblV = new System.Windows.Forms.Label{Text="金額/倍數：",Location=new System.Drawing.Point(20,90),AutoSize=true};
        this.numV = new System.Windows.Forms.NumericUpDown{Location=new System.Drawing.Point(120,87),Size=new System.Drawing.Size(150,23),Maximum=9999999,DecimalPlaces=2,ThousandsSeparator=true};
        this.btnRun = new System.Windows.Forms.Button{Text="整批變更",Location=new System.Drawing.Point(120,130),Size=new System.Drawing.Size(120,35)};
        this.btnExit= new System.Windows.Forms.Button{Text="離開",Location=new System.Drawing.Point(260,130),Size=new System.Drawing.Size(100,35)};
        this.btnRun.Click+=btnRun_Click;this.btnExit.Click+=btnExit_Click;
        this.AutoScaleDimensions=new System.Drawing.SizeF(7F,15F);this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize=new System.Drawing.Size(440,200);
        this.Controls.AddRange(new System.Windows.Forms.Control[]{grp,lblV,numV,btnRun,btnExit});
        this.Name="BK110";this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text="整批變更薪資資料";
    }
}
