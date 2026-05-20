using System.IO;
using System.Text;
using System.Windows.Forms;
using CHBApp.BK.Models;
using CHBApp.BK.Services;

namespace CHBApp.BK.Common;

/// <summary>
/// BK401~BK408/BK504/BK505 轉換磁片作業共通版面，依原 SCT 萃取結果：
///   主標題 Label5  T=76~112 L=228
///   "預定撥帳日期 :"  Label4  T=141~183
///   "是否列印遞送單 :" Label3 + .是 .否 RadioButton
///   "請選擇資料檔路徑：" Label6 + 選擇 + 路徑欄
///   按鈕： 預覽 / 列印 / 說明 / 確定 (CmdOk) / 離開 (CmdCancel)
///   "處理狀態" Label2 + 訊息列
/// 各子類於 InitializeComponent 中呼叫 BuildBaseUi 後，可加入子類專屬控件
/// </summary>
public class ExportFormBase : Form
{
    protected Label lblTitle = null!;
    protected Label lblPay = null!;
    protected DateTimePicker dtPay = null!;
    protected Label lblPrintNotice = null!;
    protected RadioButton rbPrintYes = null!, rbPrintNo = null!;
    protected Label lblPath = null!;
    protected TextBox txtPath = null!;
    protected Button btnPick = null!;
    protected Button btnPreview = null!, btnPrint = null!, btnHelp = null!;
    protected Button btnOk = null!, btnExit = null!;
    protected Label lblStatusTitle = null!;
    protected Label lblStatus = null!;
    /// <summary>子類可加入額外控件的「擴充區」起始 Y 座標</summary>
    protected int ExtraTop = 175;

    /// <summary>建立共通基底版面</summary>
    protected void BuildBaseUi(string title)
    {
        Text = title;
        ClientSize = new System.Drawing.Size(580, 380);
        StartPosition = FormStartPosition.CenterParent;

        // 主標題 - 對應 Label5
        lblTitle = new Label { Text = title, Location = new System.Drawing.Point(150, 30),
            Size = new System.Drawing.Size(280, 24), Font = new System.Drawing.Font("標楷體", 13F, System.Drawing.FontStyle.Bold),
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter };

        // 撥帳日期 - 對應 Label4
        lblPay = new Label { Text = "預定撥帳日期 :", Location = new System.Drawing.Point(80, 75),
            Size = new System.Drawing.Size(120, 24) };
        dtPay = new DateTimePicker { Location = new System.Drawing.Point(210, 72),
            Size = new System.Drawing.Size(150, 23), Format = DateTimePickerFormat.Short };

        // 是否列印遞送單 - 對應 Label3 + .是 .否
        lblPrintNotice = new Label { Text = "是否列印遞送單 :", Location = new System.Drawing.Point(80, 110),
            Size = new System.Drawing.Size(120, 24) };
        rbPrintYes = new RadioButton { Text = ".是", Location = new System.Drawing.Point(210, 108),
            AutoSize = true, Checked = true };
        rbPrintNo  = new RadioButton { Text = ".否", Location = new System.Drawing.Point(280, 108), AutoSize = true };

        // 路徑 - 對應 Label6
        lblPath = new Label { Text = "請選擇資料檔路徑：", Location = new System.Drawing.Point(80, 145),
            Size = new System.Drawing.Size(140, 24) };
        txtPath = new TextBox { Location = new System.Drawing.Point(220, 142),
            Size = new System.Drawing.Size(280, 23), Text = @"A:\PCCUT.TXT" };
        btnPick = new Button { Text = "選擇...", Location = new System.Drawing.Point(505, 141),
            Size = new System.Drawing.Size(60, 25) };
        btnPick.Click += (_, _) =>
        {
            using var d = new SaveFileDialog { Filter = "PCCUT (*.TXT)|*.TXT", FileName = "PCCUT.TXT" };
            if (d.ShowDialog(this) == DialogResult.OK) txtPath.Text = d.FileName;
        };

        // 處理狀態 (固定在底部)
        lblStatusTitle = new Label { Text = "處理狀態", Location = new System.Drawing.Point(80, 290),
            Size = new System.Drawing.Size(80, 20), Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold) };
        lblStatus = new Label { Text = "", Location = new System.Drawing.Point(170, 290),
            Size = new System.Drawing.Size(380, 20) };

        // 按鈕列 (底部)
        btnPreview = new Button { Text = "預覽", Location = new System.Drawing.Point(40, 330), Size = new System.Drawing.Size(70, 35) };
        btnPrint   = new Button { Text = "列印", Location = new System.Drawing.Point(120, 330), Size = new System.Drawing.Size(70, 35) };
        btnHelp    = new Button { Text = "說明", Location = new System.Drawing.Point(200, 330), Size = new System.Drawing.Size(70, 35) };
        btnOk      = new Button { Text = "確定", Location = new System.Drawing.Point(360, 330), Size = new System.Drawing.Size(90, 35) };
        btnExit    = new Button { Text = "離開", Location = new System.Drawing.Point(460, 330), Size = new System.Drawing.Size(90, 35) };
        btnExit.Click   += (_, _) => Close();
        btnPreview.Click+= (_, _) =>
            BkReportPrinter.ShowPreview(new BkReportPrinter.ReportConfig
            {
                Type = BkReportPrinter.ReportType.DispatchSlip,
                Title = "彰化商業銀行  受託代理撥帳資料媒體遞送單",
                SubTitle = Text,
                Employees = BkacRepository.ValidForExport().ToList(),
                PayDate = dtPay.Value
            });
        btnPrint.Click  += (_, _) =>
            BkReportPrinter.Print(new BkReportPrinter.ReportConfig
            {
                Type = BkReportPrinter.ReportType.DispatchSlip,
                Title = "彰化商業銀行  受託代理撥帳資料媒體遞送單",
                SubTitle = Text,
                Employees = BkacRepository.ValidForExport().ToList(),
                PayDate = dtPay.Value
            });
        btnHelp.Click   += (_, _) => MessageBox.Show(
            "產生彰銀薪資磁片撥帳格式 (PCCUT.TXT)\n" +
            "「預覽」/「列印」會輸出媒體遞送單", "說明");

        Controls.AddRange(new Control[] {
            lblTitle, lblPay, dtPay, lblPrintNotice, rbPrintYes, rbPrintNo,
            lblPath, txtPath, btnPick,
            lblStatusTitle, lblStatus,
            btnPreview, btnPrint, btnHelp, btnOk, btnExit
        });
    }

    protected void DoExport(IEnumerable<Bkac> source, PccutExporter.Format format)
    {
        var list = source.ToList();
        var content = PccutExporter.Export(list, BkacRepository.Enterprise, dtPay.Value, format);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(txtPath.Text) ?? ".");
            File.WriteAllText(txtPath.Text, content, Encoding.UTF8);
            lblStatus.Text = $"總筆數 {list.Count} ／ 總金額 {list.Sum(x => x.EMP_PAY):N0}";
            if (rbPrintYes.Checked) lblStatus.Text += "  (已列印遞送單)";
        }
        catch (Exception ex)
        {
            MessageBox.Show("檔案輸出失敗，請檢查磁碟機：" + ex.Message, "錯誤");
        }
    }
}
