using System.Windows.Forms;
using CHBApp.BK.Common;
using CHBApp.BK.Models;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>
/// BK101 員工基本資料建檔 - 對應原 CHBK.EXE FORMS\BK101.SCX
/// 開啟時為空白模式，可直接輸入新員工。
/// 員工編號 Enter 可載入既有員工資料修改。
/// </summary>
public partial class BK101 : CrudFormBase
{
    private Bkac _current = new();   // 預設空白
    private bool _isNew = true;      // true = 新增、false = 修改既有

    public BK101()
    {
        InitializeComponent();
        BtnSave = btnSave; BtnCancel = btnCancel; BtnPreview = btnPreview;
        BtnPrint = btnPrint; BtnExit = btnExit;

        ClearForm();   // 開啟時為空白，等待輸入

        // 員工編號 Enter 自動帶出資料 (修改既有員工)
        txtEmpNo.KeyDown += (s, e) =>
        {
            if (e.KeyCode != Keys.Enter) return;
            var no = txtEmpNo.Text.Trim();
            if (string.IsNullOrEmpty(no)) return;
            var found = BkacRepository.Employees.FirstOrDefault(x => x.EMP_NO == no);
            if (found != null)
            {
                _current = found;
                _isNew = false;
                Bind(found);
                lblPos.Text = $"修改既有員工 [{found.EMP_NAME}]";
                lblPos.ForeColor = System.Drawing.Color.DarkBlue;
            }
            else
            {
                lblPos.Text = $"新增員工 [{no}]";
                lblPos.ForeColor = System.Drawing.Color.DarkGreen;
                _isNew = true;
                txtEmpName.Focus();
            }
        };
    }

    /// <summary>清空所有輸入欄，重置為新增模式</summary>
    private void ClearForm()
    {
        _current = new Bkac();
        _isNew = true;
        rbLocal.Checked = true;
        txtEmpNo.Clear();
        txtEmpName.Clear();
        txtPid.Clear();
        txtAccNo.Clear();
        txtFax.Clear();
        txtMail.Clear();
        lblPos.Text = "新增員工 (請輸入員工編號)";
        lblPos.ForeColor = System.Drawing.Color.DarkGreen;
        txtEmpNo.Focus();
    }

    private void Bind(Bkac e)
    {
        _current = e;
        rbLocal.Checked   = e.MORF == "1";
        rbForeign.Checked = e.MORF == "2";
        rbCorp.Checked    = e.MORF == "3";
        txtEmpNo.Text   = e.EMP_NO;
        txtEmpName.Text = e.EMP_NAME;
        txtPid.Text     = e.EMP_PID;
        txtAccNo.Text   = e.EMP_ACCNO;
        txtFax.Text     = e.FAXNO;
        txtMail.Text    = e.MAILADD;
    }

    private void btnSave_Click(object s, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtEmpNo.Text))
        { MessageBox.Show("員工編號不可空白！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtEmpNo.Focus(); return; }
        if (string.IsNullOrWhiteSpace(txtEmpName.Text))
        { MessageBox.Show("員工姓名不可空白！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtEmpName.Focus(); return; }
        if (string.IsNullOrWhiteSpace(txtAccNo.Text))
        { MessageBox.Show("帳號不可空白！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtAccNo.Focus(); return; }

        var no = txtEmpNo.Text.Trim();
        // 新增模式時檢查重覆
        if (_isNew && BkacRepository.Employees.Any(x => x.EMP_NO == no))
        {
            MessageBox.Show("員工編號資料重複！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtEmpNo.Focus();
            return;
        }

        _current.EMP_NO    = no;
        _current.EMP_NAME  = txtEmpName.Text.Trim();
        _current.EMP_PID   = txtPid.Text.Trim();
        _current.EMP_ACCNO = txtAccNo.Text.Trim().PadRight(14, '0').Substring(0, 14);
        _current.MORF      = rbForeign.Checked ? "2" : (rbCorp.Checked ? "3" : "1");
        _current.FAXNO     = txtFax.Text.Trim();
        _current.MAILADD   = txtMail.Text.Trim();

        if (_isNew)
        {
            // 設預設值
            if (string.IsNullOrEmpty(_current.EMP_FLAG))   _current.EMP_FLAG  = "2";
            if (string.IsNullOrEmpty(_current.EMP_FLAG1))  _current.EMP_FLAG1 = "Y";
            if (string.IsNullOrEmpty(_current.EMP_KIND))   _current.EMP_KIND  = "51";
            if (string.IsNullOrEmpty(_current.EMP_KNAME))  _current.EMP_KNAME = "薪資";
            BkacRepository.Employees.Add(_current);
            MessageBox.Show($"員工 [{_current.EMP_NAME}] 新增完成。", "訊息");
        }
        else
        {
            MessageBox.Show($"員工 [{_current.EMP_NAME}] 資料已更新。", "訊息");
        }

        // 儲存後清空繼續輸入
        ClearForm();
    }

    private void btnClear_Click(object s, EventArgs e)
    {
        if (MessageBox.Show("是否清除員工檔（全部）？", "注意",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        BkacRepository.Employees.Clear();
        MessageBox.Show("員工檔全部刪除完畢。", "訊息");
        ClearForm();
    }

    private BkReportPrinter.ReportConfig MakeCfg() => new()
    {
        Type = BkReportPrinter.ReportType.EmployeeMaster,
        Title = "員工基本資料表",
        Employees = BkacRepository.Employees.ToList()
    };

    private void btnPreview_Click(object s, EventArgs e) => BkReportPrinter.ShowPreview(MakeCfg());
    private void btnPrint_Click(object s, EventArgs e)   => BkReportPrinter.Print(MakeCfg());
    private void btnCancel_Click(object s, EventArgs e)  => ClearForm();
    private void btnExit_Click(object s, EventArgs e)    => Close();

    private void rbMORF_CheckedChanged(object s, EventArgs e)
    {
        lblPid.Text = rbCorp.Checked ? "統     編" : "身份字號/統編";
    }
}
