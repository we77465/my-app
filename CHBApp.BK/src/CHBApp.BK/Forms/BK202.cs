using System.Windows.Forms;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>BK202 員工薪資資料檔維護</summary>
public partial class BK202 : Form
{
    public BK202() => InitializeComponent();
    private System.Collections.Generic.List<Models.Bkac> Filter()
    {
        var q = BkacRepository.ValidForExport();
        if (chkRange.Checked)
        {
            var f = txtFrom.Text.Trim(); var t = txtTo.Text.Trim();
            if (!string.IsNullOrEmpty(f)) q = q.Where(e => string.CompareOrdinal(e.EMP_NO, f) >= 0);
            if (!string.IsNullOrEmpty(t)) q = q.Where(e => string.CompareOrdinal(e.EMP_NO, t) <= 0);
        }
        return q.ToList();
    }
    private BkReportPrinter.ReportConfig MakeCfg() => new()
    {
        Type = chkRange.Checked ? BkReportPrinter.ReportType.PayrollByRange : BkReportPrinter.ReportType.PayrollList,
        Title = chkRange.Checked ? "本月薪資清冊 (含起迄)" : "本月薪資清冊",
        Employees = Filter(),
        FromEmpNo = chkRange.Checked ? txtFrom.Text.Trim() : "",
        ToEmpNo   = chkRange.Checked ? txtTo.Text.Trim()   : ""
    };
    private void btnPreview_Click(object s, EventArgs e)
    {
        var list = Filter();
        log.Clear();
        log.AppendText($"=== 本月薪資清冊（共 {list.Count} 筆，總額 {list.Sum(x=>x.EMP_PAY):N0}）===\r\n");
        BkReportPrinter.ShowPreview(MakeCfg());
    }
    private void btnPrint_Click(object s, EventArgs e) => BkReportPrinter.Print(MakeCfg());
    private void btnExit_Click(object s, EventArgs e)  => Close();
}
