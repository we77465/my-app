using System.Windows.Forms;
using CHBApp.BK.Common;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>BK407 轉出磁片作業(百年規格--外幣薪資入戶) - SCT 15 元件 (轉出帳號+磁片排列順序+幣別)</summary>
public partial class BK407 : ExportFormBase
{
    private Label lblAcc = null!, lblOrder = null!, lblCur = null!;
    private TextBox txtAcc = null!;
    private ComboBox cbOrder = null!, cbCur = null!;
    public BK407()
    {
        InitializeComponent();
        btnOk.Click += (_, _) =>
            DoExport(BkacRepository.ValidForExport().Where(e => e.EMP_KIND == "04"),
                     PccutExporter.Format.Bnyr130);
    }
}
