namespace CHBApp.BK.Forms;
partial class BK407
{
    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
    private void InitializeComponent()
    {
        BuildBaseUi("轉出磁片作業(百年規格--外幣薪資入戶)");
        this.lblAcc = new System.Windows.Forms.Label{Text="轉出帳號 :",Location=new System.Drawing.Point(80,180),Size=new System.Drawing.Size(80,24)};
        this.txtAcc = new System.Windows.Forms.TextBox{Location=new System.Drawing.Point(170,177),Size=new System.Drawing.Size(180,23),MaxLength=14,Text=CHBApp.BK.Services.BkacRepository.Enterprise.CORP_ACCNO};
        this.lblCur = new System.Windows.Forms.Label{Text="幣別 :",Location=new System.Drawing.Point(380,180),Size=new System.Drawing.Size(50,24)};
        this.cbCur = new System.Windows.Forms.ComboBox{Location=new System.Drawing.Point(440,177),Size=new System.Drawing.Size(120,23),DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList};
        this.cbCur.Items.AddRange(new object[]{"01-USD","02-JPY","03-EUR","04-CNY"}); this.cbCur.SelectedIndex = 0;
        this.lblOrder = new System.Windows.Forms.Label{Text="磁片排列順序 :",Location=new System.Drawing.Point(80,210),Size=new System.Drawing.Size(120,24)};
        this.cbOrder = new System.Windows.Forms.ComboBox{Location=new System.Drawing.Point(210,207),Size=new System.Drawing.Size(180,23),DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList};
        this.cbOrder.Items.AddRange(new object[]{"依員工編號","依姓名","依帳號"});
        this.cbOrder.SelectedIndex = 0;
        this.Controls.AddRange(new System.Windows.Forms.Control[]{lblAcc,txtAcc,lblCur,cbCur,lblOrder,cbOrder});
        this.Name = "BK407";
    }
}
