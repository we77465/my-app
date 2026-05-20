using CHBApp.BK.Common;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>BK4011 轉換磁片作業(百年規格--個別) - SCT 14 元件
/// 對應舊版 CHBApp.NET BK4011Form: BK4011 轉出磁片－百年規格 個別 (130 Bytes)</summary>
public partial class BK4011 : ExportFormBase
{
    public BK4011()
    {
        InitializeComponent();
        btnOk.Click += (_, _) =>
            DoExport(BkacRepository.ValidForExport().Where(e => !string.IsNullOrEmpty(e.EMP_KIND)),
                     PccutExporter.Format.Bnyr130);
    }
}
