using System.Windows.Forms;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>BK302 列印本月資料</summary>
public partial class BK302 : Form
{
    public BK302() => InitializeComponent();

    private BkReportPrinter.ReportConfig MakeCfg() => new()
    {
        Type = BkReportPrinter.ReportType.PayrollByRange,
        Title = "本月薪資清冊 (含起迄)",
        SubTitle = $"{BkacRepository.Enterprise.CORP_NAME}    " +
                   $"員工編號 {txtFrom.Text} ~ {txtTo.Text}",
        Employees = BkacRepository.ValidForExport().ToList(),
        FromEmpNo = txtFrom.Text.Trim(),
        ToEmpNo = txtTo.Text.Trim()
    };

    private void btnPreview_Click(object s, EventArgs e) => BkReportPrinter.ShowPreview(MakeCfg());
    private void btnPrint_Click(object s, EventArgs e)   => BkReportPrinter.Print(MakeCfg());
    private void btnHelp_Click(object s, EventArgs e)    =>
        MessageBox.Show("列印條件：員工編號起迄 + 帳號 ≠ '0' × 14 + 薪資 ≠ 0", "說明");
    private void btnExit_Click(object s, EventArgs e)    => Close();
}
