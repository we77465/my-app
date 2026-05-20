using CHBApp.BK.Common;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>BK4012 轉換磁片作業(百年規格--外幣薪資入戶) - SCT 12 元件
/// 對應舊版 CHBApp.NET BK4012Form: BK4012 轉出磁片－百年規格 外幣薪資入戶 (130 Bytes)</summary>
public partial class BK4012 : ExportFormBase
{
    public BK4012()
    {
        InitializeComponent();
        btnOk.Click += (_, _) =>
            DoExport(BkacRepository.ValidForExport(), PccutExporter.Format.Bnyr130);
    }
}
