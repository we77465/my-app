namespace CHBApp.BK.Forms;
partial class BK501
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblPath;
    private System.Windows.Forms.TextBox txtPath;
    private System.Windows.Forms.Button btnPick, btnRun, btnExit;
    private System.Windows.Forms.RichTextBox log;
    protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
    private void InitializeComponent()
    {
        this.lblPath=new System.Windows.Forms.Label{Text="路徑名稱：",Location=new System.Drawing.Point(20,25),AutoSize=true};
        this.txtPath=new System.Windows.Forms.TextBox{Location=new System.Drawing.Point(120,22),Size=new System.Drawing.Size(380,23)};
        this.btnPick=new System.Windows.Forms.Button{Text="選擇...",Location=new System.Drawing.Point(510,21),Size=new System.Drawing.Size(70,25)};
        this.btnRun =new System.Windows.Forms.Button{Text="匯入",Location=new System.Drawing.Point(120,60),Size=new System.Drawing.Size(80,32)};
        this.btnExit=new System.Windows.Forms.Button{Text="離開",Location=new System.Drawing.Point(220,60),Size=new System.Drawing.Size(80,32)};
        this.log=new System.Windows.Forms.RichTextBox{Location=new System.Drawing.Point(20,110),Size=new System.Drawing.Size(620,200),ReadOnly=true,Font=new System.Drawing.Font("Consolas",9F)};
        this.btnPick.Click+=btnPick_Click;this.btnRun.Click+=btnRun_Click;this.btnExit.Click+=btnExit_Click;
        this.AutoScaleDimensions=new System.Drawing.SizeF(7F,15F);this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize=new System.Drawing.Size(660,330);
        this.Controls.AddRange(new System.Windows.Forms.Control[]{lblPath,txtPath,btnPick,btnRun,btnExit,log});
        this.Name="BK501";this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text="薪資磁片輸入 (二扣作業)";
    }
}
