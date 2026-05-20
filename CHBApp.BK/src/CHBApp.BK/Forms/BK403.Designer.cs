namespace CHBApp.BK.Forms;
partial class BK403
{
    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
    private void InitializeComponent()
    {
        BuildBaseUi("轉換磁片作業(舊規格--外幣薪資入戶)");
        this.lblCur = new System.Windows.Forms.Label{Text="幣別 :",Location=new System.Drawing.Point(380,75),Size=new System.Drawing.Size(50,24)};
        this.cbCur = new System.Windows.Forms.ComboBox{Location=new System.Drawing.Point(440,72),Size=new System.Drawing.Size(120,23),DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList};
        this.cbCur.Items.AddRange(new object[]{"01-USD 美金","02-JPY 日圓","03-EUR 歐元","04-CNY 人民幣"});
        this.cbCur.SelectedIndex = 0;
        this.Controls.AddRange(new System.Windows.Forms.Control[]{lblCur,cbCur});
        this.Name = "BK403";
    }
}
