using System.Windows.Forms;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>BK502 失敗明細查詢 (二扣)</summary>
public partial class BK502 : Form
{
    public BK502()
    {
        InitializeComponent();
        Reload();
    }
    private void Reload()
    {
        var list = BkacRepository.Results.Where(r => r.RESULT != "00").ToList();
        grid.DataSource = list;
        lblTotal.Text = $"失敗 {list.Count} 筆 / 失敗總額 {list.Sum(x => x.EMP_PAY):N0}";
    }
    private BkReportPrinter.ReportConfig MakeCfg() => new()
    {
        Type = BkReportPrinter.ReportType.SecondDebitDetail,
        Title = "二次扣帳 - 失敗明細",
        Results = BkacRepository.Results.ToList(),
        IsSuccess = false
    };
    private void btnPreview_Click(object s, EventArgs e) => BkReportPrinter.ShowPreview(MakeCfg());
    private void btnPrint_Click(object s, EventArgs e)   => BkReportPrinter.Print(MakeCfg());
    private void btnExit_Click(object s, EventArgs e)    => Close();
}
