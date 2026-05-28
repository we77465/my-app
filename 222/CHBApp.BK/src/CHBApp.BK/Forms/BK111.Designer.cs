namespace CHBApp.BK.Forms;
partial class BK111
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblPath;
    private System.Windows.Forms.TextBox txtPath;
    private System.Windows.Forms.Button btnPick, btnRun, btnExit;
    private System.Windows.Forms.GroupBox grpFmt;
    private System.Windows.Forms.RadioButton rbOld, rbBnyr;
    private System.Windows.Forms.RichTextBox log;
    protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
    private void InitializeComponent()
    {
        this.lblPath=new System.Windows.Forms.Label{Text="請選擇資料檔路徑：",Location=new System.Drawing.Point(20,20),AutoSize=true};
        this.txtPath=new System.Windows.Forms.TextBox{Location=new System.Drawing.Point(150,17),Size=new System.Drawing.Size(380,23)};
        this.btnPick=new System.Windows.Forms.Button{Text="選擇...",Location=new System.Drawing.Point(540,16),Size=new System.Drawing.Size(70,25)};
        this.grpFmt=new System.Windows.Forms.GroupBox{Text="磁片格式",Location=new System.Drawing.Point(20,55),Size=new System.Drawing.Size(280,55)};
        this.rbOld=new System.Windows.Forms.RadioButton{Text="舊規格",Checked=true,Location=new System.Drawing.Point(15,22),AutoSize=true};
        this.rbBnyr=new System.Windows.Forms.RadioButton{Text="百年規格",Location=new System.Drawing.Point(120,22),AutoSize=true};
        this.grpFmt.Controls.AddRange(new System.Windows.Forms.Control[]{rbOld,rbBnyr});
        this.btnRun=new System.Windows.Forms.Button{Text="處理",Location=new System.Drawing.Point(320,67),Size=new System.Drawing.Size(80,32)};
        this.btnExit=new System.Windows.Forms.Button{Text="離開",Location=new System.Drawing.Point(420,67),Size=new System.Drawing.Size(80,32)};
        this.log=new System.Windows.Forms.RichTextBox{Location=new System.Drawing.Point(20,130),Size=new System.Drawing.Size(620,200),ReadOnly=true,Font=new System.Drawing.Font("Consolas",9F)};
        this.btnPick.Click+=btnPick_Click;this.btnRun.Click+=btnRun_Click;this.btnExit.Click+=btnExit_Click;
        this.AutoScaleDimensions=new System.Drawing.SizeF(7F,15F);this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize=new System.Drawing.Size(660,350);
        this.Controls.AddRange(new System.Windows.Forms.Control[]{lblPath,txtPath,btnPick,grpFmt,btnRun,btnExit,log});
        this.Name="BK111";this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text="薪資磁片重新輸入";
    }
}
