using System.IO;
using System.Windows.Forms;

namespace CHBApp.BK.Forms;

/// <summary>BK404 中連匯款資料檔 -> 本行代理撥帳格式</summary>
public partial class BK404 : Form
{
    public BK404() => InitializeComponent();
    private void btnSrc_Click(object s, EventArgs e)
    { using var d = new OpenFileDialog { Filter = "中連匯款 (*.txt)|*.txt" }; if (d.ShowDialog(this) == DialogResult.OK) txtSrc.Text = d.FileName; }
    private void btnDst_Click(object s, EventArgs e)
    { using var d = new SaveFileDialog { Filter = "本行格式 (*.TXT)|*.TXT", FileName = "PCCUT.TXT" }; if (d.ShowDialog(this) == DialogResult.OK) txtDst.Text = d.FileName; }
    private void btnRun_Click(object s, EventArgs e)
    {
        if (txtSrc.Text == txtDst.Text) { MessageBox.Show("來源與產生路徑不可相同！", "錯誤"); return; }
        if (!File.Exists(txtSrc.Text)) { log.AppendText("(模擬) 來源不存在，產生 5 筆示範\r\n"); return; }
        try
        {
            var lines = File.ReadAllLines(txtSrc.Text);
            // 將中連格式包裝為本行格式 (簡化)
            var output = lines.Select(l => "009" + l).ToArray();
            File.WriteAllLines(txtDst.Text, output);
            log.AppendText($"轉檔完成 → {txtDst.Text}（{output.Length} 筆）\r\n");
        }
        catch (Exception ex) { log.AppendText("錯誤：" + ex.Message + "\r\n"); }
    }
    private void btnExit_Click(object s, EventArgs e) => Close();
}
