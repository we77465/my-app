using System.IO;
using System.Text;
using System.Windows.Forms;
using CHBApp.BK.Common;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>BK408 分組轉換磁片作業(百年規格) - SCT 16 元件 (員工編組起迄+轉出帳號+磁片排列順序)</summary>
public partial class BK408 : ExportFormBase
{
    private Label lblFromLbl = null!, lblToLbl = null!, lblAcc = null!, lblOrder = null!;
    private TextBox txtFrom = null!, txtTo = null!, txtAcc = null!;
    private ComboBox cbOrder = null!;
    public BK408()
    {
        InitializeComponent();
        btnOk.Click += (_, _) =>
        {
            string f = txtFrom.Text.Trim(), t = txtTo.Text.Trim();
            int total = 0;
            var src = BkacRepository.ValidForExport()
                .Where(e => (string.IsNullOrEmpty(f) || string.CompareOrdinal(e.EMP_NO, f) >= 0) &&
                            (string.IsNullOrEmpty(t) || string.CompareOrdinal(e.EMP_NO, t) <= 0));
            foreach (var g in src.GroupBy(x => x.EMP_KIND))
            {
                var fname = txtPath.Text.Replace(".TXT", $"_{g.Key}_BNYR.TXT", StringComparison.OrdinalIgnoreCase);
                var content = PccutExporter.Export(g, BkacRepository.Enterprise, dtPay.Value, PccutExporter.Format.Bnyr130);
                File.WriteAllText(fname, content, Encoding.UTF8);
                total += g.Count();
            }
            lblStatus.Text = $"分組轉檔完成 / 總筆數 {total}";
        };
    }
}
