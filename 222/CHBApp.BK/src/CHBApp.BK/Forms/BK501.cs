using System.IO;
using System.Windows.Forms;
using CHBApp.BK.Models;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>BK501 二扣作業 - 結果磁片匯入</summary>
public partial class BK501 : Form
{
    public BK501() => InitializeComponent();
    private void btnPick_Click(object s, EventArgs e)
    { using var d = new OpenFileDialog{Filter="二扣結果檔 (*.txt)|*.txt"}; if (d.ShowDialog(this)==DialogResult.OK) txtPath.Text=d.FileName; }
    private void btnRun_Click(object s, EventArgs e)
    {
        log.Clear();
        if (!File.Exists(txtPath.Text))
        {
            // 模擬：把員工檔前 30 筆設成失敗、其餘成功
            BkacRepository.Results.Clear();
            int idx = 0;
            foreach (var emp in BkacRepository.Employees)
            {
                idx++;
                BkacRepository.Results.Add(new BkacResult
                {
                    EMP_NO = emp.EMP_NO, EMP_NAME = emp.EMP_NAME, EMP_ACCNO = emp.EMP_ACCNO,
                    EMP_PAY = emp.EMP_PAY, EMP_PID = emp.EMP_PID,
                    RESULT = idx % 8 == 0 ? "01" : "00",
                    FAIL_DESC = idx % 8 == 0 ? "餘額不足" : "成功"
                });
            }
            log.AppendText($"模擬匯入 {BkacRepository.Results.Count} 筆 (失敗 {BkacRepository.Results.Count(x => x.RESULT != "00")})\r\n");
            return;
        }
        BkacRepository.Results.Clear();
        foreach (var ln in File.ReadAllLines(txtPath.Text))
        {
            if (ln.Length < 30) continue;
            BkacRepository.Results.Add(new BkacResult
            {
                EMP_ACCNO = ln.Substring(0, 14).Trim(),
                EMP_PAY = decimal.TryParse(ln.Substring(14, 13), out var c) ? c / 100m : 0,
                RESULT = ln.Length >= 30 ? ln.Substring(28, 2) : "00"
            });
        }
        log.AppendText($"完成 {BkacRepository.Results.Count} 筆\r\n");
    }
    private void btnExit_Click(object s, EventArgs e) => Close();
}
