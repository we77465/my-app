using System.Windows.Forms;
using CHBApp.BK.Common;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>BK405 轉出磁片作業(百年規格--全體) - SCT 14 元件 (多 轉出帳號 / 磁片排列方式)</summary>
public partial class BK405 : ExportFormBase
{
    private Label lblAcc = null!, lblOrder = null!;
    private TextBox txtAcc = null!;
    private ComboBox cbOrder = null!;
    public BK405()
    {
        InitializeComponent();
        btnOk.Click += (_, _) =>
            DoExport(BkacRepository.ValidForExport(), PccutExporter.Format.Bnyr130);
    }
}
