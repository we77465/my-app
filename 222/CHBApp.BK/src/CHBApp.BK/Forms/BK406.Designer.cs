namespace CHBApp.BK.Forms;
partial class BK406
{
    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
    private void InitializeComponent()
    {
        BuildBaseUi("轉出磁片作業(百年規格--個別)");
        this.lblAcc = new System.Windows.Forms.Label{Text="轉出帳號 :",Location=new System.Drawing.Point(80,180),Size=new System.Drawing.Size(80,24)};
        this.txtAcc = new System.Windows.Forms.TextBox{Location=new System.Drawing.Point(170,177),Size=new System.Drawing.Size(180,23),MaxLength=14,Text=CHBApp.BK.Services.BkacRepository.Enterprise.CORP_ACCNO};
        this.lblOrder = new System.Windows.Forms.Label{Text="磁片排列方式 :",Location=new System.Drawing.Point(80,210),Size=new System.Drawing.Size(120,24)};
        this.cbOrder = new System.Windows.Forms.ComboBox{Location=new System.Drawing.Point(210,207),Size=new System.Drawing.Size(180,23),DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList};
        this.cbOrder.Items.AddRange(new object[]{"依員工編號","依姓名","依帳號"});
        this.cbOrder.SelectedIndex = 0;
        this.Controls.AddRange(new System.Windows.Forms.Control[]{lblAcc,txtAcc,lblOrder,cbOrder});
        this.Name = "BK406";
    }
}
