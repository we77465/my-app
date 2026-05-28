using System.Windows.Forms;

namespace CHBApp.BK.Common;

/// <summary>
/// 對應原 CHBK.EXE 中所有資料維護類 SCX 共用的工具列基底。
/// 工具列：新增 F2, 修改 F3, 刪除 F4, 儲存 F5, 查詢 F6, 執行查詢 F7, 取消 ESC,
///         首筆/上筆/下筆/末筆, 預覽, 列印, 說明, 離開
/// 模式：Browse(瀏覽) / Add(新增) / Edit(修改) / Query(查詢)
/// </summary>
public class CrudFormBase : Form
{
    public enum FormMode { Browse, Add, Edit, Query }

    public FormMode Mode { get; protected set; } = FormMode.Browse;

    // 子類別需於 Designer 產生這些按鈕
    protected ToolStripButton? BtnAdd;
    protected ToolStripButton? BtnEdit;
    protected ToolStripButton? BtnDelete;
    protected ToolStripButton? BtnSave;
    protected ToolStripButton? BtnQuery;
    protected ToolStripButton? BtnExecQuery;
    protected ToolStripButton? BtnCancel;
    protected ToolStripButton? BtnFirst;
    protected ToolStripButton? BtnPrev;
    protected ToolStripButton? BtnNext;
    protected ToolStripButton? BtnLast;
    protected ToolStripButton? BtnPreview;
    protected ToolStripButton? BtnPrint;
    protected ToolStripButton? BtnHelp;
    protected ToolStripButton? BtnExit;

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.F2: BtnAdd?.PerformClick();      return true;
            case Keys.F3: BtnEdit?.PerformClick();     return true;
            case Keys.F4: BtnDelete?.PerformClick();   return true;
            case Keys.F5: BtnSave?.PerformClick();     return true;
            case Keys.F6: BtnQuery?.PerformClick();    return true;
            case Keys.F7: BtnExecQuery?.PerformClick();return true;
            case Keys.Escape: BtnCancel?.PerformClick(); return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <summary>進入指定模式並更新各 F-key 按鈕的「亮起 / 變字」狀態</summary>
    protected void SetMode(FormMode m)
    {
        Mode = m;
        if (BtnAdd  != null) BtnAdd.Text  = m == FormMode.Add   ? "取消新增" : "新增";
        if (BtnEdit != null) BtnEdit.Text = m == FormMode.Edit  ? "取消修改" : "修改";
        if (BtnQuery!= null) BtnQuery.Text= m == FormMode.Query ? "取消查詢" : "查詢";
    }
}
