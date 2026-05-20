using System.Windows.Forms;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>BK503 成功明細查詢 (二扣)</summary>
public partial class BK503 : Form
{
    public BK503()
    {
        InitializeComponent();
        var list = BkacRepository.Results.Where(r => r.RESULT == "00").ToList();
        grid.DataSource = list;
        lblTotal.Text = $"成功 {list.Count} 筆 / 成功總額 {list.Sum(x => x.EMP_PAY):N0}";
    }
    private BkReportPrinter.ReportConfig MakeCfg() => new()
    {
        Type = BkReportPrinter.ReportType.SecondDebitDetail,
        Title = "二次扣帳 - 成功明細",
        Results = BkacRepository.Results.ToList(),
        IsSuccess = true
    };
    private void btnPreview_Click(object s, EventArgs e) => BkReportPrinter.ShowPreview(MakeCfg());
    private void btnPrint_Click(object s, EventArgs e)   => BkReportPrinter.Print(MakeCfg());
    private void btnExit_Click(object s, EventArgs e)    => Close();
}
