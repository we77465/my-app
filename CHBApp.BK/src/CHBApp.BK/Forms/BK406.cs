using System.Windows.Forms;
using CHBApp.BK.Common;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>BK406 轉出磁片作業(百年規格--個別) - SCT 14 元件</summary>
public partial class BK406 : ExportFormBase
{
    private Label lblAcc = null!, lblOrder = null!;
    private TextBox txtAcc = null!;
    private ComboBox cbOrder = null!;
    public BK406()
    {
        InitializeComponent();
        btnOk.Click += (_, _) =>
            DoExport(BkacRepository.ValidForExport().Where(e => !string.IsNullOrEmpty(e.EMP_KIND)),
                     PccutExporter.Format.Bnyr130);
    }
}
