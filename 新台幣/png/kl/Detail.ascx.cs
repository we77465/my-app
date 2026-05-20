using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Program_KL_Detail : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //取得員編
            string sUserID = UserInfo.getUserId(Request.QueryString["SID"], Request);
            hfUserID.Value = sUserID;
            //取得登入單位代號
            string sInfo = UserInfo.getUserInfo(sUserID);
            Char delimiter = ',';
            String[] arrInfo = sInfo.Split(delimiter);
            if (arrInfo.Length == 3)
            {
                hfBrNo.Value = arrInfo[0];
                hfUserName.Value = arrInfo[1];
                hfBrName.Value = arrInfo[2];
            }

            if (hfNo.Value.Length > 0)
            {
                DataView dv = (DataView)sdsList.Select(DataSourceSelectArguments.Empty);
                if (dv.Count > 0)
                {
                    SetAll(dv.ToTable());
                }
            }

            if (hfAction.Value == "QTY")
            {
                //隱藏
                btnSave.Visible = btnSign.Visible = false;
            }
        }
    }
    protected void SetAll(DataTable dt)
    {
        txtCRC.Text = dt.Rows[0]["CRC"].ToString(); txtID.Text = dt.Rows[0]["ID"].ToString(); txtName.Text = dt.Rows[0]["Name"].ToString();
        txtMangerBank.Text = dt.Rows[0]["MangerBank"].ToString(); txtFirstDay.Text = dt.Rows[0]["FirstDay"].ToString(); txtSetDate.Text = dt.Rows[0]["SetDate"].ToString(); txtAgent.Text = dt.Rows[0]["Agent"].ToString(); txtIndustry.Text = dt.Rows[0]["Industry"].ToString();
        txtMainItem.Text = dt.Rows[0]["MainItem"].ToString(); txtCapital.Text = dt.Rows[0]["Capital"].ToString();
        txtProduct.Text = dt.Rows[0]["Product"].ToString(); txtCHBRating.Text = dt.Rows[0]["CHBRating"].ToString(); txtProportion1.Text = dt.Rows[0]["Proportion1"].ToString(); txtProportion2.Text = dt.Rows[0]["Proportion2"].ToString(); txtExterRating.Text = dt.Rows[0]["ExterRating"].ToString(); txtStockNo.Text = dt.Rows[0]["StockNo"].ToString();
        SetCheckBox(chkStock, dt.Rows[0]["Stock"].ToString()); SetCheckBox(chkEnterprise, dt.Rows[0]["Enterprise"].ToString()); SetCheckBox(chkSalaryTransfer, dt.Rows[0]["SalaryTransfer"].ToString()); SetCheckBox(chkType, dt.Rows[0]["Type"].ToString()); SetCheckBox(chk5P, dt.Rows[0]["5P"].ToString());
        txtAverage1_1.Text = dt.Rows[0]["Average1_1"].ToString(); txtAverage1_2.Text = dt.Rows[0]["Average1_2"].ToString(); txtAverage1_3.Text = dt.Rows[0]["Average1_3"].ToString(); txtAverage1_4.Text = dt.Rows[0]["Average1_4"].ToString();
        txtAverage2_1.Text = dt.Rows[0]["Average2_1"].ToString(); txtAverage2_2.Text = dt.Rows[0]["Average2_2"].ToString(); txtAverage2_3.Text = dt.Rows[0]["Average2_3"].ToString(); txtAverage2_4.Text = dt.Rows[0]["Average2_4"].ToString();
        txtExchange1_1.Text = dt.Rows[0]["Exchange1_1"].ToString(); txtExchange1_2.Text = dt.Rows[0]["Exchange1_2"].ToString(); txtExchange1_3.Text = dt.Rows[0]["Exchange1_3"].ToString(); txtExchange1_4.Text = dt.Rows[0]["Exchange1_4"].ToString();
        txtExchange2_1.Text = dt.Rows[0]["Exchange2_1"].ToString(); txtExchange2_2.Text = dt.Rows[0]["Exchange2_2"].ToString(); txtExchange2_3.Text = dt.Rows[0]["Exchange2_3"].ToString(); txtExchange2_4.Text = dt.Rows[0]["Exchange2_4"].ToString();
        txtCreditExchange.Text = dt.Rows[0]["CreditExchange"].ToString(); txtTranDate.Text = dt.Rows[0]["TranDate"].ToString(); txtTotalFina.Text = dt.Rows[0]["TotalFina"].ToString(); txtBankShort.Text = dt.Rows[0]["BankShort"].ToString(); txtTotalAmount.Text = dt.Rows[0]["TotalAmount"].ToString(); txtCHBProp1.Text = dt.Rows[0]["CHBProp1"].ToString(); txtCurrentTotal.Text = dt.Rows[0]["CurrentTotal"].ToString(); txtCHBProp2.Text = dt.Rows[0]["CHBProp2"].ToString();
        txtAdd1.Text = dt.Rows[0]["Add1"].ToString(); txtAdd2.Text = dt.Rows[0]["Add2"].ToString(); txtAdd3.Text = dt.Rows[0]["Add3"].ToString(); txtAdd4.Text = dt.Rows[0]["Add4"].ToString(); txtAdd5.Text = dt.Rows[0]["Add5"].ToString();
        txtRecentYear1.Text = dt.Rows[0]["RecentYear1"].ToString(); txtRecentYear2.Text = dt.Rows[0]["RecentYear2"].ToString(); txtRecentYear3.Text = dt.Rows[0]["RecentYear3"].ToString();
        txtQuota.Text = dt.Rows[0]["Quota"].ToString(); txtMainUse.Text = dt.Rows[0]["MainUse"].ToString(); txtNotificationDay.Text = dt.Rows[0]["NotificationDay"].ToString();
        txtEloan.Text = dt.Rows[0]["Eloan"].ToString(); txtProcessDay.Text = dt.Rows[0]["ProcessDay"].ToString(); txtApprovalDate.Text = dt.Rows[0]["ApprovalDate"].ToString(); txtAuditDays.Text = dt.Rows[0]["AuditDays"].ToString(); txtDescript.Text = dt.Rows[0]["Descript"].ToString();
        txtVisitDate1.Text = dt.Rows[0]["VisitDate1"].ToString(); txtVisitDate2.Text = dt.Rows[0]["VisitDate2"].ToString(); txtVisitDate3.Text = dt.Rows[0]["VisitDate3"].ToString(); txtVisitDate4.Text = dt.Rows[0]["VisitDate4"].ToString();
        txtVisitObject1.Text = dt.Rows[0]["VisitObject1"].ToString(); txtVisitObject2.Text = dt.Rows[0]["VisitObject2"].ToString(); txtVisitObject3.Text = dt.Rows[0]["VisitObject3"].ToString(); txtVisitObject4.Text = dt.Rows[0]["VisitObject4"].ToString();
        txtVisitors1.Text = dt.Rows[0]["Visitors1"].ToString(); txtVisitors2.Text = dt.Rows[0]["Visitors2"].ToString(); txtVisitors3.Text = dt.Rows[0]["Visitors3"].ToString(); txtVisitors4.Text = dt.Rows[0]["Visitors4"].ToString();
        txtNotificationDay2.Text = dt.Rows[0]["NotificationDay2"].ToString(); txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
        txtCRC.Text = dt.Rows[0]["CRC"].ToString(); txtManger2.Text = dt.Rows[0]["Manger2"].ToString(); txtPrincipal2.Text = dt.Rows[0]["Principal2"].ToString(); txtAO2.Text = dt.Rows[0]["AO2"].ToString();
        txtEvaluationDay.Text = dt.Rows[0]["EvaluationDay"].ToString();
    }
    private void SetCheckBox(CheckBoxList ckList, string Value)
    {
        for (int i = 0; i < ckList.Items.Count; i++)
        {
            ckList.Items[i].Selected = false;
            for (int a = 0; a < ckList.Items.Count; a++)
            {
                if (ckList.Items[i].Value == Value)
                {
                    ckList.Items[i].Selected = true;
                }
            }

        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        hfNo.Value = hfNo.Value.Trim().Length > 0 ? hfNo.Value.Trim() : "KL" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + hfBrNo.Value;
        string CRC = txtCRC.Text.ToString().Trim(), ID = txtID.Text.ToString().Trim(), Name = txtName.Text.ToString().Trim(), MangerBank = txtMangerBank.Text.ToString().Trim(), FirstDay = txtFirstDay.Text.ToString().Trim(), SetDate = txtSetDate.Text.ToString().Trim(), Agent = txtAgent.Text.ToString().Trim(), Industry = txtIndustry.Text.ToString().Trim(), Capital = txtCapital.Text.ToString().Trim(), MainItem = txtMainItem.Text.ToString().Trim(),
Product = txtProduct.Text.ToString().Trim(), CHBRating = txtCHBRating.Text.ToString().Trim(), Proportion1 = txtProportion1.Text.ToString().Trim(), Proportion2 = txtProportion2.Text.ToString().Trim(), ExterRating = txtExterRating.Text.ToString().Trim(), StockNo = txtStockNo.Text.ToString().Trim(),
Average1_1 = txtAverage1_1.Text.ToString().Trim(), Average1_2 = txtAverage1_2.Text.ToString().Trim(), Average1_3 = txtAverage1_3.Text.ToString().Trim(), Average1_4 = txtAverage1_4.Text.ToString().Trim(),
Average2_1 = txtAverage2_1.Text.ToString().Trim(), Average2_2 = txtAverage2_2.Text.ToString().Trim(), Average2_3 = txtAverage2_3.Text.ToString().Trim(), Average2_4 = txtAverage2_4.Text.ToString().Trim(),
Exchange1_1 = txtExchange1_1.Text.ToString().Trim(), Exchange1_2 = txtExchange1_2.Text.ToString().Trim(), Exchange1_3 = txtExchange1_3.Text.ToString().Trim(), Exchange1_4 = txtExchange1_4.Text.ToString().Trim(),
Exchange2_1 = txtExchange2_1.Text.ToString().Trim(), Exchange2_2 = txtExchange2_2.Text.ToString().Trim(), Exchange2_3 = txtExchange2_3.Text.ToString().Trim(), Exchange2_4 = txtExchange2_4.Text.ToString().Trim(),
CreditExchange = txtCreditExchange.Text.ToString().Trim(), TranDate = txtTranDate.Text.ToString().Trim(), TotalFina = txtTotalFina.Text.ToString().Trim(), BankShort = txtBankShort.Text.ToString().Trim(), TotalAmount = txtTotalAmount.Text.ToString().Trim(), CHBProp1 = txtCHBProp1.Text.ToString().Trim(),
CurrentTotal = txtCurrentTotal.Text.ToString().Trim(), CHBProp2 = txtCHBProp2.Text.ToString().Trim(),
Add1 = txtAdd1.Text.ToString().Trim(), Add2 = txtAdd2.Text.ToString().Trim(), Add3 = txtAdd3.Text.ToString().Trim(), Add4 = txtAdd4.Text.ToString().Trim(), Add5 = txtAdd5.Text.ToString().Trim(),
RecentYear1 = txtRecentYear1.Text.ToString().Trim(), RecentYear2 = txtRecentYear2.Text.ToString().Trim(), RecentYear3 = txtRecentYear3.Text.ToString().Trim(),
Quota = txtQuota.Text.ToString().Trim(), MainUse = txtMainUse.Text.ToString().Trim(), NotificationDay = txtNotificationDay.Text.ToString().Trim(),
Eloan = txtEloan.Text.ToString().Trim(), ProcessDay = txtProcessDay.Text.ToString().Trim(), ApprovalDate = txtApprovalDate.Text.ToString().Trim(), AuditDays = txtAuditDays.Text.ToString().Trim(), Descript = txtDescript.Text.ToString().Trim(),
VisitDate1 = txtVisitDate1.Text.ToString().Trim(), VisitDate2 = txtVisitDate2.Text.ToString().Trim(), VisitDate3 = txtVisitDate3.Text.ToString().Trim(), VisitDate4 = txtVisitDate4.Text.ToString().Trim(),
VisitObject1 = txtVisitObject1.Text.ToString().Trim(), VisitObject2 = txtVisitObject2.Text.ToString().Trim(), VisitObject3 = txtVisitObject3.Text.ToString().Trim(), VisitObject4 = txtVisitObject4.Text.ToString().Trim(),
Visitors1 = txtVisitors1.Text.ToString().Trim(), Visitors2 = txtVisitors2.Text.ToString().Trim(), Visitors3 = txtVisitors3.Text.ToString().Trim(), Visitors4 = txtVisitors4.Text.ToString().Trim(),
NotificationDay2 = txtNotificationDay2.Text.ToString().Trim(), Remarks = txtRemarks.Text.ToString().Trim(),
Stock = chkStock.SelectedValue, Enterprise = chkEnterprise.SelectedValue, SalaryTransfer = chkSalaryTransfer.SelectedValue, Type = chkType.SelectedValue, _5P = chk5P.SelectedValue
        , EvaluationDay = txtEvaluationDay.Text.ToString().Trim();

        if (hfAction.Value == "ADD")
        {
            sdsList.InsertParameters["No"].DefaultValue = hfNo.Value; sdsList.InsertParameters["BrName"].DefaultValue = hfBrName.Value;
            sdsList.InsertParameters["MangerBank"].DefaultValue = MangerBank;
            sdsList.InsertParameters["FirstDay"].DefaultValue = FirstDay;
            sdsList.InsertParameters["Name"].DefaultValue = Name;
            sdsList.InsertParameters["SetDate"].DefaultValue = SetDate;
            sdsList.InsertParameters["Agent"].DefaultValue = Agent;
            sdsList.InsertParameters["ID"].DefaultValue = ID;
            sdsList.InsertParameters["Industry"].DefaultValue = Industry;
            sdsList.InsertParameters["MainItem"].DefaultValue = MainItem;
            sdsList.InsertParameters["Capital"].DefaultValue = Capital;
            sdsList.InsertParameters["Product"].DefaultValue = Product;
            sdsList.InsertParameters["CHBRating"].DefaultValue = CHBRating;
            sdsList.InsertParameters["Proportion1"].DefaultValue = Proportion1;
            sdsList.InsertParameters["Proportion2"].DefaultValue = Proportion2;
            sdsList.InsertParameters["ExterRating"].DefaultValue = ExterRating;
            sdsList.InsertParameters["Stock"].DefaultValue = Stock;
            sdsList.InsertParameters["StockNo"].DefaultValue = StockNo;
            sdsList.InsertParameters["Enterprise"].DefaultValue = Enterprise;
            sdsList.InsertParameters["SalaryTransfer"].DefaultValue = SalaryTransfer;
            sdsList.InsertParameters["Average1_1"].DefaultValue = Average1_1;
            sdsList.InsertParameters["Average1_2"].DefaultValue = Average1_2;
            sdsList.InsertParameters["Average1_3"].DefaultValue = Average1_3;
            sdsList.InsertParameters["Average1_4"].DefaultValue = Average1_4;
            sdsList.InsertParameters["Average2_1"].DefaultValue = Average2_1;
            sdsList.InsertParameters["Average2_2"].DefaultValue = Average2_2;
            sdsList.InsertParameters["Average2_3"].DefaultValue = Average2_3;
            sdsList.InsertParameters["Average2_4"].DefaultValue = Average2_4;
            sdsList.InsertParameters["Exchange1_1"].DefaultValue = Exchange1_1;
            sdsList.InsertParameters["Exchange1_2"].DefaultValue = Exchange1_2;
            sdsList.InsertParameters["Exchange1_3"].DefaultValue = Exchange1_3;
            sdsList.InsertParameters["Exchange1_4"].DefaultValue = Exchange1_4;
            sdsList.InsertParameters["Exchange2_1"].DefaultValue = Exchange2_1;
            sdsList.InsertParameters["Exchange2_2"].DefaultValue = Exchange2_2;
            sdsList.InsertParameters["Exchange2_3"].DefaultValue = Exchange2_3;
            sdsList.InsertParameters["Exchange2_4"].DefaultValue = Exchange2_4;
            sdsList.InsertParameters["CreditExchange"].DefaultValue = CreditExchange;
            sdsList.InsertParameters["TranDate"].DefaultValue = TranDate;
            sdsList.InsertParameters["TotalFina"].DefaultValue = TotalFina;
            sdsList.InsertParameters["BankShort"].DefaultValue = BankShort;
            sdsList.InsertParameters["TotalAmount"].DefaultValue = TotalAmount;
            sdsList.InsertParameters["CHBProp1"].DefaultValue = CHBProp1;
            sdsList.InsertParameters["CurrentTotal"].DefaultValue = CurrentTotal;
            sdsList.InsertParameters["CHBProp2"].DefaultValue = CHBProp2;
            sdsList.InsertParameters["Add1"].DefaultValue = Add1;
            sdsList.InsertParameters["Add2"].DefaultValue = Add2;
            sdsList.InsertParameters["Add3"].DefaultValue = Add3;
            sdsList.InsertParameters["Add4"].DefaultValue = Add4;
            sdsList.InsertParameters["Add5"].DefaultValue = Add5;
            sdsList.InsertParameters["RecentYear1"].DefaultValue = RecentYear1;
            sdsList.InsertParameters["RecentYear2"].DefaultValue = RecentYear2;
            sdsList.InsertParameters["RecentYear3"].DefaultValue = RecentYear3;
            sdsList.InsertParameters["Quota"].DefaultValue = Quota;
            sdsList.InsertParameters["MainUse"].DefaultValue = MainUse;
            sdsList.InsertParameters["NotificationDay"].DefaultValue = NotificationDay;
            sdsList.InsertParameters["Eloan"].DefaultValue = Eloan;
            sdsList.InsertParameters["ProcessDay"].DefaultValue = ProcessDay;
            sdsList.InsertParameters["ApprovalDate"].DefaultValue = ApprovalDate;
            sdsList.InsertParameters["AuditDays"].DefaultValue = AuditDays;
            sdsList.InsertParameters["Type"].DefaultValue = Type;
            sdsList.InsertParameters["Descript"].DefaultValue = Descript;
            sdsList.InsertParameters["VisitDate1"].DefaultValue = VisitDate1;
            sdsList.InsertParameters["VisitDate2"].DefaultValue = VisitDate2;
            sdsList.InsertParameters["VisitDate3"].DefaultValue = VisitDate3;
            sdsList.InsertParameters["VisitDate4"].DefaultValue = VisitDate4;
            sdsList.InsertParameters["VisitObject1"].DefaultValue = VisitObject1;
            sdsList.InsertParameters["VisitObject2"].DefaultValue = VisitObject2;
            sdsList.InsertParameters["VisitObject3"].DefaultValue = VisitObject3;
            sdsList.InsertParameters["VisitObject4"].DefaultValue = VisitObject4;
            sdsList.InsertParameters["Visitors1"].DefaultValue = Visitors1;
            sdsList.InsertParameters["Visitors2"].DefaultValue = Visitors2;
            sdsList.InsertParameters["Visitors3"].DefaultValue = Visitors3;
            sdsList.InsertParameters["Visitors4"].DefaultValue = Visitors4;
            sdsList.InsertParameters["NotificationDay2"].DefaultValue = NotificationDay2;
            sdsList.InsertParameters["Manger"].DefaultValue = "";
            sdsList.InsertParameters["Principal"].DefaultValue = "";
            sdsList.InsertParameters["AO"].DefaultValue = "";
            sdsList.InsertParameters["5P"].DefaultValue = _5P;
            sdsList.InsertParameters["EvaluationDay"].DefaultValue = EvaluationDay;
            sdsList.InsertParameters["Remarks"].DefaultValue = Remarks;
            sdsList.InsertParameters["CRC"].DefaultValue = CRC;
            sdsList.InsertParameters["Manger2"].DefaultValue = "";
            sdsList.InsertParameters["Principal2"].DefaultValue = "";
            sdsList.InsertParameters["AO2"].DefaultValue = "";
            sdsList.InsertParameters["CrtID"].DefaultValue = hfUserID.Value; sdsList.InsertParameters["CrtName"].DefaultValue = hfUserName.Value; sdsList.InsertParameters["UpdID"].DefaultValue = hfUserID.Value; sdsList.InsertParameters["UpdName"].DefaultValue = hfUserName.Value;
            sdsList.Insert();
        }
        else if (hfAction.Value == "UPD")
        {
            sdsList.UpdateParameters["No"].DefaultValue = hfNo.Value;
            sdsList.UpdateParameters["MangerBank"].DefaultValue = MangerBank;
            sdsList.UpdateParameters["FirstDay"].DefaultValue = FirstDay;
            sdsList.UpdateParameters["Name"].DefaultValue = Name;
            sdsList.UpdateParameters["SetDate"].DefaultValue = SetDate;
            sdsList.UpdateParameters["Agent"].DefaultValue = Agent;
            sdsList.UpdateParameters["Industry"].DefaultValue = Industry;
            sdsList.UpdateParameters["MainItem"].DefaultValue = MainItem;
            sdsList.UpdateParameters["Capital"].DefaultValue = Capital;
            sdsList.UpdateParameters["Product"].DefaultValue = Product;
            sdsList.UpdateParameters["CHBRating"].DefaultValue = CHBRating;
            sdsList.UpdateParameters["Proportion1"].DefaultValue = Proportion1;
            sdsList.UpdateParameters["Proportion2"].DefaultValue = Proportion2;
            sdsList.UpdateParameters["ExterRating"].DefaultValue = ExterRating;
            sdsList.UpdateParameters["Stock"].DefaultValue = Stock;
            sdsList.UpdateParameters["StockNo"].DefaultValue = StockNo;
            sdsList.UpdateParameters["Enterprise"].DefaultValue = Enterprise;
            sdsList.UpdateParameters["SalaryTransfer"].DefaultValue = SalaryTransfer;
            sdsList.UpdateParameters["Average1_1"].DefaultValue = Average1_1;
            sdsList.UpdateParameters["Average1_2"].DefaultValue = Average1_2;
            sdsList.UpdateParameters["Average1_3"].DefaultValue = Average1_3;
            sdsList.UpdateParameters["Average1_4"].DefaultValue = Average1_4;
            sdsList.UpdateParameters["Average2_1"].DefaultValue = Average2_1;
            sdsList.UpdateParameters["Average2_2"].DefaultValue = Average2_2;
            sdsList.UpdateParameters["Average2_3"].DefaultValue = Average2_3;
            sdsList.UpdateParameters["Average2_4"].DefaultValue = Average2_4;
            sdsList.UpdateParameters["Exchange1_1"].DefaultValue = Exchange1_1;
            sdsList.UpdateParameters["Exchange1_2"].DefaultValue = Exchange1_2;
            sdsList.UpdateParameters["Exchange1_3"].DefaultValue = Exchange1_3;
            sdsList.UpdateParameters["Exchange1_4"].DefaultValue = Exchange1_4;
            sdsList.UpdateParameters["Exchange2_1"].DefaultValue = Exchange2_1;
            sdsList.UpdateParameters["Exchange2_2"].DefaultValue = Exchange2_2;
            sdsList.UpdateParameters["Exchange2_3"].DefaultValue = Exchange2_3;
            sdsList.UpdateParameters["Exchange2_4"].DefaultValue = Exchange2_4;
            sdsList.UpdateParameters["CreditExchange"].DefaultValue = CreditExchange;
            sdsList.UpdateParameters["TranDate"].DefaultValue = TranDate;
            sdsList.UpdateParameters["TotalFina"].DefaultValue = TotalFina;
            sdsList.UpdateParameters["BankShort"].DefaultValue = BankShort;
            sdsList.UpdateParameters["TotalAmount"].DefaultValue = TotalAmount;
            sdsList.UpdateParameters["CHBProp1"].DefaultValue = CHBProp1;
            sdsList.UpdateParameters["CurrentTotal"].DefaultValue = CurrentTotal;
            sdsList.UpdateParameters["CHBProp2"].DefaultValue = CHBProp2;
            sdsList.UpdateParameters["Add1"].DefaultValue = Add1;
            sdsList.UpdateParameters["Add2"].DefaultValue = Add2;
            sdsList.UpdateParameters["Add3"].DefaultValue = Add3;
            sdsList.UpdateParameters["Add4"].DefaultValue = Add4;
            sdsList.UpdateParameters["Add5"].DefaultValue = Add5;
            sdsList.UpdateParameters["RecentYear1"].DefaultValue = RecentYear1;
            sdsList.UpdateParameters["RecentYear2"].DefaultValue = RecentYear2;
            sdsList.UpdateParameters["RecentYear3"].DefaultValue = RecentYear3;
            sdsList.UpdateParameters["Quota"].DefaultValue = Quota;
            sdsList.UpdateParameters["MainUse"].DefaultValue = MainUse;
            sdsList.UpdateParameters["NotificationDay"].DefaultValue = NotificationDay;
            sdsList.UpdateParameters["Eloan"].DefaultValue = Eloan;
            sdsList.UpdateParameters["ProcessDay"].DefaultValue = ProcessDay;
            sdsList.UpdateParameters["ApprovalDate"].DefaultValue = ApprovalDate;
            sdsList.UpdateParameters["AuditDays"].DefaultValue = AuditDays;
            sdsList.UpdateParameters["Type"].DefaultValue = Type;
            sdsList.UpdateParameters["Descript"].DefaultValue = Descript;
            sdsList.UpdateParameters["VisitDate1"].DefaultValue = VisitDate1;
            sdsList.UpdateParameters["VisitDate2"].DefaultValue = VisitDate2;
            sdsList.UpdateParameters["VisitDate3"].DefaultValue = VisitDate3;
            sdsList.UpdateParameters["VisitDate4"].DefaultValue = VisitDate4;
            sdsList.UpdateParameters["VisitObject1"].DefaultValue = VisitObject1;
            sdsList.UpdateParameters["VisitObject2"].DefaultValue = VisitObject2;
            sdsList.UpdateParameters["VisitObject3"].DefaultValue = VisitObject3;
            sdsList.UpdateParameters["VisitObject4"].DefaultValue = VisitObject4;
            sdsList.UpdateParameters["Visitors1"].DefaultValue = Visitors1;
            sdsList.UpdateParameters["Visitors2"].DefaultValue = Visitors2;
            sdsList.UpdateParameters["Visitors3"].DefaultValue = Visitors3;
            sdsList.UpdateParameters["Visitors4"].DefaultValue = Visitors4;
            sdsList.UpdateParameters["NotificationDay2"].DefaultValue = NotificationDay2;
            sdsList.UpdateParameters["5P"].DefaultValue = _5P;
            sdsList.UpdateParameters["EvaluationDay"].DefaultValue = EvaluationDay;
            sdsList.UpdateParameters["Remarks"].DefaultValue = Remarks;
            sdsList.UpdateParameters["UpdID"].DefaultValue = hfUserID.Value; sdsList.UpdateParameters["UpdName"].DefaultValue = hfUserName.Value;
            sdsList.Update();
        }

        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Save", "<script>alert('存檔成功!');</script>");
    }

    protected void btnSign_Click(object sender, EventArgs e)
    {
        hfURL.Value = "Main.aspx?sid=" + Request.QueryString["SID"].ToString();
        //少儲存
        sdsSummit.UpdateParameters["No"].DefaultValue = hfNo.Value;
        sdsSummit.UpdateParameters["UpdID"].DefaultValue = hfUserID.Value;
        sdsSummit.UpdateParameters["UpdName"].DefaultValue = hfUserName.Value;
        sdsSummit.Update();
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "add", "<script>alert('送主管核可成功！');location.href='" + hfURL.Value + "';</script>");
    }
}