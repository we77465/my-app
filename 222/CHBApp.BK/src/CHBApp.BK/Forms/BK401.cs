using CHBApp.BK.Common;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>BK401 轉換磁片作業(百年規格--全體) - SCT 14 元件
/// 對應舊版 CHBApp.NET BK401Form: BK401 轉出磁片－百年規格 全體 (130 Bytes)</summary>
public partial class BK401 : ExportFormBase
{
    public BK401()
    {
        InitializeComponent();
        btnOk.Click += (_, _) =>
            DoExport(BkacRepository.ValidForExport(), PccutExporter.Format.Bnyr130);
    }
}
