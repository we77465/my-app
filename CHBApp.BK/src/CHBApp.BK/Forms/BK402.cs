using System.IO;
using System.Text;
using System.Windows.Forms;
using CHBApp.BK.Common;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>
/// BK402 分組轉換磁片作業(舊規格) - SCT 14 元件
/// 多了「員工編組起號 / 編組迄號」
/// </summary>
public partial class BK402 : ExportFormBase
{
    private TextBox txtFrom = null!, txtTo = null!;
    private Label lblFromLbl = null!, lblToLbl = null!;
    public BK402()
    {
        InitializeComponent();
        btnOk.Click += (_, _) =>
        {
            int total = 0;
            string f = txtFrom.Text.Trim(), t = txtTo.Text.Trim();
            var src = BkacRepository.ValidForExport()
                .Where(e => (string.IsNullOrEmpty(f) || string.CompareOrdinal(e.EMP_NO, f) >= 0) &&
                            (string.IsNullOrEmpty(t) || string.CompareOrdinal(e.EMP_NO, t) <= 0));
            foreach (var g in src.GroupBy(x => x.EMP_KIND))
            {
                var fname = txtPath.Text.Replace(".TXT", $"_{g.Key}.TXT", StringComparison.OrdinalIgnoreCase);
                var content = PccutExporter.Export(g, BkacRepository.Enterprise, dtPay.Value, PccutExporter.Format.Old);
                File.WriteAllText(fname, content, Encoding.UTF8);
                total += g.Count();
            }
            lblStatus.Text = $"分組轉檔完成 / 總筆數 {total}";
        };
    }
}
