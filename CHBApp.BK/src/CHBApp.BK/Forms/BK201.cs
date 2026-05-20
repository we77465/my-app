using System.Windows.Forms;
using CHBApp.BK.Models;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>
/// BK201 員工資料檔維護查詢 - 對應原 SCT BK201.SCX (25 元件)
/// 13 欄表頭：員工代號 / 員工姓名 / 銀行帳號 / 金額 / 身分證字號 / 存提 /
///           類別 / 轉帳名稱 / ID檢查 / 傳真或MAIL / 傳真號碼 / e-Mail Add / Mail或傳真內容
/// </summary>
public partial class BK201 : Form
{
    public BK201()
    {
        InitializeComponent();
        BuildColumns();
        Reload();
    }

    private void BuildColumns()
    {
        grid.AutoGenerateColumns = false;
        grid.Columns.Clear();

        DataGridViewTextBoxColumn Col(string header, string field, int width)
            => new() { HeaderText = header, DataPropertyName = field, Name = field, Width = width };

        grid.Columns.Add(Col("員工代號",     nameof(Bkac.EMP_NO),    80));
        grid.Columns.Add(Col("員工姓名",     nameof(Bkac.EMP_NAME),  90));
        grid.Columns.Add(Col("銀行帳號",     nameof(Bkac.EMP_ACCNO), 130));
        var colPay = Col("金額",             nameof(Bkac.EMP_PAY),   90);
        colPay.DefaultCellStyle.Format = "N0";
        colPay.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        grid.Columns.Add(colPay);
        grid.Columns.Add(Col("身分證字號",   nameof(Bkac.EMP_PID),   100));
        grid.Columns.Add(Col("存提",         nameof(Bkac.EMP_FLAG),  50));
        grid.Columns.Add(Col("類別",         nameof(Bkac.EMP_KIND),  50));
        grid.Columns.Add(Col("轉帳名稱",     nameof(Bkac.EMP_KNAME), 90));
        grid.Columns.Add(Col("ID檢查",       nameof(Bkac.EMP_FLAG1), 60));

        // 「傳真或MAIL」是衍生欄位 (F=有傳真 / M=有Email / 兩者皆有 = "F+M" / 空)
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "傳真或MAIL",
            Name = "FaxOrMail",
            Width = 90,
            DataPropertyName = ""    // 自填
        });

        grid.Columns.Add(Col("傳真號碼",          nameof(Bkac.FAXNO),   100));
        grid.Columns.Add(Col("e-Mail Add",        nameof(Bkac.MAILADD), 200));
        grid.Columns.Add(Col("Mail或傳真內容",     nameof(Bkac.CONTENT), 200));

        grid.CellFormatting += Grid_CellFormatting;
    }

    private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        var col = grid.Columns[e.ColumnIndex];
        var row = grid.Rows[e.RowIndex];
        if (row.DataBoundItem is not Bkac emp) return;

        if (col.Name == nameof(Bkac.EMP_FLAG))
        {
            e.Value = emp.EMP_FLAG == "1" ? "1 轉提" : "2 轉存";
            e.FormattingApplied = true;
        }
        else if (col.Name == nameof(Bkac.EMP_FLAG1))
        {
            e.Value = emp.EMP_FLAG1 == "Y" ? "Y 檢查" : "N 不檢查";
            e.FormattingApplied = true;
        }
        else if (col.Name == "FaxOrMail")
        {
            string s = "";
            if (!string.IsNullOrWhiteSpace(emp.FAXNO))   s += "F";
            if (!string.IsNullOrWhiteSpace(emp.MAILADD)) s += (s == "" ? "M" : "+M");
            e.Value = s;
            e.FormattingApplied = true;
        }
    }

    private void Reload()
    {
        IEnumerable<Bkac> q = BkacRepository.Employees;
        var k = txtKey.Text.Trim();
        if (rb1.Checked)       q = q.OrderBy(e => e.EMP_NO, StringComparer.Ordinal);
        else if (rb2.Checked)  q = q.OrderBy(e => e.EMP_NAME);
        else if (rb3.Checked)  q = q.OrderBy(e => e.EMP_ACCNO);
        else                   q = q.OrderBy(e => e.EMP_PID);
        if (!string.IsNullOrEmpty(k))
            q = q.Where(e =>
                e.EMP_NO.Contains(k) || e.EMP_NAME.Contains(k) ||
                e.EMP_ACCNO.Contains(k) || e.EMP_PID.Contains(k));
        grid.DataSource = q.ToList();
        lblTotal.Text = $"共 {grid.RowCount} 筆";
    }

    private void btnQuery_Click(object s, EventArgs e) => Reload();
    private BkReportPrinter.ReportConfig MakeCfg() => new()
    {
        Type = BkReportPrinter.ReportType.EmployeeMaster,
        Title = "員工資料表",
        Employees = (grid.DataSource as List<Bkac>) ?? BkacRepository.Employees.ToList()
    };
    private void btnPreview_Click(object s, EventArgs e) => BkReportPrinter.ShowPreview(MakeCfg());
    private void btnPrint_Click(object s, EventArgs e)   => BkReportPrinter.Print(MakeCfg());
    private void btnExit_Click(object s, EventArgs e)    => Close();
}
