using System.Windows.Forms;
using CHBApp.BK.Services;

namespace CHBApp.BK.Forms;

/// <summary>BK110 整批變更薪資資料</summary>
public partial class BK110 : Form
{
    public BK110() => InitializeComponent();
    private void btnRun_Click(object s, EventArgs e)
    {
        var v = numV.Value; int n = 0;
        foreach (var emp in BkacRepository.Employees)
        {
            if (rbAdd.Checked) emp.BASE_SAL += v;
            else if (rbSub.Checked) emp.BASE_SAL = Math.Max(0, emp.EMP_PAY - v);
            else if (rbMul.Checked) { emp.BASE_SAL = Math.Round(emp.EMP_PAY * v, 0); emp.ALLOWANCE = 0; emp.OVERTIME = 0; emp.DEDUCTION = 0; }
            else { emp.BASE_SAL = v; emp.ALLOWANCE = 0; emp.OVERTIME = 0; emp.DEDUCTION = 0; }   // Set
            n++;
        }
        MessageBox.Show($"整批變更資料成功，共 {n} 筆", "訊息");
    }
    private void btnExit_Click(object s, EventArgs e) => Close();
}
