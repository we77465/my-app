using System.Windows.Forms;
using CHBApp.BK.Common;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>BK403 外幣薪資入戶-轉換磁片作業(舊規格) - SCT 15 元件 (多 幣別)</summary>
public partial class BK403 : ExportFormBase
{
    private Label lblCur = null!;
    private ComboBox cbCur = null!;
    public BK403()
    {
        InitializeComponent();
        btnOk.Click += (_, _) =>
            DoExport(BkacRepository.ValidForExport().Where(e => e.EMP_KIND == "04"),
                     PccutExporter.Format.Old);
    }
}
