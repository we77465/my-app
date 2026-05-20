using System.Windows.Forms;
using CHBApp.BK.Models;

namespace CHBApp.BK.Forms;

/// <summary>FRMSUB_M5 - 員工薪資撥帳系統 主視窗 (含 8 大頂層選單)</summary>
public partial class FrmSubM5 : Form
{
    private readonly AppUser _currentUser;

    public FrmSubM5(AppUser currentUser)
    {
        _currentUser = currentUser;
        InitializeComponent();
        this.Text = $"員工薪資撥帳管理系統  [{currentUser.UserId} {currentUser.UserName}]";
    }

    private void Open(Form f)
    {
        f.MdiParent = this;
        f.Show();
        f.Activate();
    }

    // === 員工資料作業 ===
    private void miEmpReg_Click(object s, EventArgs e)    => Open(new BK101());
    private void miEmpQuery_Click(object s, EventArgs e)  => Open(new BK201());
    private void miEmpImport_Click(object s, EventArgs e) => Open(new BK_EmpImport());
    private void miOtherImportXunLian_Click(object s, EventArgs e) =>
        MessageBox.Show("(模擬) 磁片輸入(EXCEL)--訊連CO專用", "其他格式讀入");

    // === 薪資輸入 ===
    private void miSalAll_Click(object s, EventArgs e)        => Open(new BK102());
    private void miSalIndividual_Click(object s, EventArgs e) => Open(new BK103());
    private void miSalClear_Click(object s, EventArgs e)      => Open(new BK104());
    private void miSalByCode_Click(object s, EventArgs e)     => Open(new BK105());
    private void miSalByName_Click(object s, EventArgs e)     => Open(new BK106());
    private void miSalByAccount_Click(object s, EventArgs e)  => Open(new BK109());
    private void miSalUnreg_Click(object s, EventArgs e)      => Open(new BK108());
    private void miSalBatch_Click(object s, EventArgs e)      => Open(new BK110());
    private void miSalImport_Click(object s, EventArgs e)     => Open(new BK107());
    private void miSalReimport_Click(object s, EventArgs e)   => Open(new BK111());

    // === 列印作業 ===
    private void miPrint1_Click(object s, EventArgs e)   => Open(new BK301());
    private void miPrint2_Click(object s, EventArgs e)   => Open(new BK302());
    private void miPrintHub_Click(object s, EventArgs e) => Open(new BK202());

    // === 轉換磁片作業 ===
    private void miExportAll_Click(object s, EventArgs e)        => Open(new BK401());
    private void miExportIndividual_Click(object s, EventArgs e) => Open(new BK4011());
    private void miExportGroup_Click(object s, EventArgs e)      => Open(new BK402());
    private void miExportForeignI_Click(object s, EventArgs e)   => Open(new BK403());
    private void miMidLink_Click(object s, EventArgs e)          => Open(new BK404());
    private void miExportAllNew_Click(object s, EventArgs e)     => Open(new BK405());
    private void miExportIndNew_Click(object s, EventArgs e)     => Open(new BK406());
    private void miExportForeignNew_Click(object s, EventArgs e) => Open(new BK407());
    private void miExportGroupNew_Click(object s, EventArgs e)   => Open(new BK408());

    // === 二次轉扣帳作業 (順序對齊舊版 SUB_M5.MPR BAR 1-5) ===
    // BAR1=BK501 BAR2=BK502 BAR3=BK503 BAR4=BK505(百年) BAR5=BK504(舊)
    private void miSecImport_Click(object s, EventArgs e)  => Open(new BK501());
    private void miSecFail_Click(object s, EventArgs e)    => Open(new BK502());
    private void miSecOk_Click(object s, EventArgs e)      => Open(new BK503());
    private void miSecExpNew_Click(object s, EventArgs e)  => Open(new BK505());  // BAR4：百年規格（舊版順序）
    private void miSecExpOld_Click(object s, EventArgs e)  => Open(new BK504());  // BAR5：舊規格（舊版順序）

    // === 密碼作業 - 對應 frmpswd ===
    private void miPassword_Click(object s, EventArgs e)
    {
        using var frm = new FrmPasswordChange(_currentUser);
        frm.ShowDialog(this);
    }

    // === 系統維護 - 對應 @s popup (SYS_BANK/SYS_TR11/SYS_ETPS/SYS_CUST/SYS_CUS1/SYS_MAT/FRMDBF_INFO/DBF_IN) ===
    private void miSysBank_Click(object s, EventArgs e) =>
        MessageBox.Show("(模擬) 銀行基本資料設定 (SYS_BANK)", "系統維護");
    private void miSysTr11_Click(object s, EventArgs e) =>
        MessageBox.Show("(模擬) 轉帳類別設定 (SYS_TR11)", "系統維護");
    private void miSysEtps_Click(object s, EventArgs e) =>
        MessageBox.Show("(模擬) 企業基本資料設定 (SYS_ETPS)", "系統維護");
    private void miSysCust_Click(object s, EventArgs e) =>
        MessageBox.Show("(模擬) 客戶資料設定 (SYS_CUST)", "系統維護");
    private void miSysCus1_Click(object s, EventArgs e) =>
        MessageBox.Show("(模擬) 客戶資料設定2 (SYS_CUS1)", "系統維護");
    private void miSysMat_Click(object s, EventArgs e) =>
        MessageBox.Show("(模擬) 對應資料設定 (SYS_MAT)", "系統維護");
    private void miDbfInfo_Click(object s, EventArgs e) =>
        MessageBox.Show("(模擬) DBF 檔案資訊 (FRMDBF_INFO)", "系統維護");
    private void miDbfIn_Click(object s, EventArgs e) =>
        MessageBox.Show("(模擬) DBF 資料匯入 (DBF_IN)", "系統維護");

    // === 說明 / 離開 ===
    private void miHelp_Click(object s, EventArgs e) =>
        MessageBox.Show(
            "員工薪資撥帳系統\n版本 1.0  © 2026\n\n" +
            "F2 新增  F3 修改  F4 刪除  F5 儲存\n" +
            "F6 查詢  F7 執行   ESC 取消\n",
            "輔助說明");
    private void miExit_Click(object s, EventArgs e) => Close();
}
