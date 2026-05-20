using System.Windows.Forms;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>BK301 列印本月薪資</summary>
public partial class BK301 : Form
{
    public BK301() => InitializeComponent();

    private BkReportPrinter.ReportConfig MakeCfg() => new()
    {
        Type = BkReportPrinter.ReportType.PayrollList,
        Title = "本月薪資清冊",
        Employees = BkacRepository.ValidForExport().ToList(),
        SortBy = cbMethod.SelectedIndex switch
        {
            0 => "員工編號",
            1 => "員工編號",
            2 => "員工姓名",
            _ => "員工編號"
        },
        Rate = numRate.Value
    };

    private void btnPreview_Click(object s, EventArgs e) => BkReportPrinter.ShowPreview(MakeCfg());
    private void btnPrint_Click(object s, EventArgs e)   => BkReportPrinter.Print(MakeCfg());
    private void btnHelp_Click(object s, EventArgs e)    =>
        MessageBox.Show("列印條件：員工檔內帳號 ≠ '00000000000000' 且薪資 ≠ 0\n" +
                        "排序方式：依下拉選擇\n匯率：外幣薪資 × 匯率轉台幣顯示", "說明");
    private void btnExit_Click(object s, EventArgs e)    => Close();
}
