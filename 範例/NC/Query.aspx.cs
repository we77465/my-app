using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Reporting;
using Microsoft.Reporting.WebForms;
using System.IO;

public partial class Program_NC_Query : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            hidn_EmpNo.Value = UserInfo.getUserId(Request.QueryString["sid"], Request);
            string[] usrinfo = UserInfo.getUserInfo(hidn_EmpNo.Value).Split(',');
            hidn_BRNO.Value = usrinfo[0];
            //Response.Write(hidn_BRNO.Value);
            sdNC.SelectParameters["EmpId"].DefaultValue = hidn_EmpNo.Value;
            sdNC.SelectParameters["Brno"].DefaultValue = "%"+hidn_BRNO.Value+"%";

        }
    }

    protected void Btn_Submit_Click(object sender, EventArgs e)
    {

        if (txtDateS.Text.Trim() == "")
        {
            DateTime date1 = new DateTime(2000, 12, 31);
            sdNC.SelectParameters["DateS"].DefaultValue = date1.ToString();
        }
        else
        {
            sdNC.SelectParameters["DateS"].DefaultValue = txtDateS.Text.Trim();
        }

        if (txtDateE.Text.Trim() == "")
        {

            string date2 = DateTime.Now.ToString("yyyyMMdd");
            sdNC.SelectParameters["DateE"].DefaultValue = date2;
        }
        else
        {

            sdNC.SelectParameters["DateE"].DefaultValue = txtDateE.Text.Trim();
        }

        string drListFlowStatus_text = drListFlowStatus.SelectedValue;
        if (drListFlowStatus_text == "0.查詢全部")
        {
            //Response.Write(drListFlowStatus_text.Substring(2));          
            sdNC.SelectParameters["FlowStatus_text"].DefaultValue = "%";
        }
        else
        {
            //Response.Write(drListFlowStatus_text.Substring(2));
            sdNC.SelectParameters["FlowStatus_text"].DefaultValue = drListFlowStatus_text.Substring(2);
        }

    }
    private void gv已存在的表單Bind()
    {


        gv已存在的表單.DataBind();
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {

        if (txtDateS.Text.Trim() == "")
        {
            DateTime date1 = new DateTime(2000, 12, 31);
            sdNC.SelectParameters["DateS"].DefaultValue = date1.ToString();
        }
        else
        {
            sdNC.SelectParameters["DateS"].DefaultValue = txtDateS.Text.Trim();
        }

        if (txtDateE.Text.Trim() == "")
        {

            string date2 = DateTime.Now.ToString("yyyyMMdd");
            sdNC.SelectParameters["DateE"].DefaultValue = date2;
        }
        else
        {

            sdNC.SelectParameters["DateE"].DefaultValue = txtDateE.Text.Trim();
        }

        string drListFlowStatus_text = drListFlowStatus.SelectedValue;
        if (drListFlowStatus_text == "0.查詢全部")
        {
            //Response.Write(drListFlowStatus_text.Substring(2));          
            sdNC.SelectParameters["FlowStatus_text"].DefaultValue = "%";
        }
        else
        {
            //Response.Write(drListFlowStatus_text.Substring(2));
            sdNC.SelectParameters["FlowStatus_text"].DefaultValue = drListFlowStatus_text.Substring(2);
        }


        gv已存在的表單.AllowPaging = false;
        gv已存在的表單Bind();
        ExportExcel(gv已存在的表單);
        //ExportExcel(HttpUtility.UrlEncode("資料清單", System.Text.Encoding.UTF8) + ".xls", gv_rpt);
        gv已存在的表單.AllowPaging = true;
    }

    protected void ExportExcel(GridView ExportGridView)
    {
        //欄位為文字的 CSS設定
        string style = "<style> .text { mso-number-format:\"\\@\"; } </style> ";
        //以下為 GridView 轉 Excel 的程式
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=filename.xls");
        //Response.Charset = "utf-8"
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.ms-<b style='color:black;background-color:#ffff66'>excel</b>";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>");
        ExportGridView.RenderControl(htmlWrite); //指定GridView，若此行錯誤，請新增EnableEventValidation = "false"於aspx的<page>
        Response.Write(style);
        Response.Write(stringWrite.ToString());
        //Response.Write("<B>* 當月無資料者免列印&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;前台負責人：&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;個金AO：&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;	保存期限：5年 </B>");
        Response.End();
    }


    public override void VerifyRenderingInServerForm(Control control)
    {
        // 加此 sub 就不會出現「run at server」錯誤
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Page")
            return;
        string docNo = e.CommandArgument.ToString();

        if (e.CommandName == "MyEdit") //修改
        {
            runBtn修改(docNo);
        }
        else if (e.CommandName == "Send") //修改
        {
            runBtn送簽(docNo);
        }
        if (e.CommandName == "MyReadOnly")
        {
            Response.Redirect("Detail.aspx?DocNo=" + docNo);
        }
    }
    void runBtn修改(string a_docno)
    {
        foreach (GridViewRow row in gv已存在的表單.Rows)
        {
            string x案件編號 = row.Cells[0].Text;
            if (x案件編號 == a_docno) {

                //流程狀態
                int Flow_Type_num = 0;
                string sFlow_Type="";
                CheckBox cbFlow_Type1 = (CheckBox)row.Cells[5].FindControl("CheckFlow_Type1");//1.未處理
                CheckBox cbFlow_Type2 = (CheckBox)row.Cells[5].FindControl("CheckFlow_Type2");//2.處理中
                CheckBox cbFlow_Type3 = (CheckBox)row.Cells[5].FindControl("CheckFlow_Type3");//3.已結案-欲合作
                CheckBox cbFlow_Type4 = (CheckBox)row.Cells[5].FindControl("CheckFlow_Type4");//4.已結案-未合作
                if (cbFlow_Type1.Checked == true)
                {
                    sFlow_Type = cbFlow_Type1.Text;
                    Flow_Type_num = Flow_Type_num + 1;
                }
                if (cbFlow_Type2.Checked == true)
                {
                    sFlow_Type = cbFlow_Type2.Text;
                    Flow_Type_num = Flow_Type_num + 1;
                }
                if (cbFlow_Type3.Checked == true)
                {
                    sFlow_Type = cbFlow_Type3.Text;
                    Flow_Type_num = Flow_Type_num + 1;
                }
                if (cbFlow_Type4.Checked == true)
                {
                    sFlow_Type = cbFlow_Type4.Text;
                    Flow_Type_num = Flow_Type_num + 1;
                }
                //Response.Write(Flow_Type_num);
                if (Flow_Type_num == 1)
                {

                    TextBox tb員工編號 = (TextBox)row.Cells[2].FindControl("tb員工編號");
                    sdNCModify.UpdateParameters["DocNo"].DefaultValue = a_docno;
                    sdNCModify.UpdateParameters["EmpId"].DefaultValue = tb員工編號.Text;
                    sdNCModify.UpdateParameters["Flow_Type"].DefaultValue = sFlow_Type;
                    sdNCModify.UpdateParameters["FlowStatus"].DefaultValue = "-2";
                    sdNCModify.Update();
                    string alert_text = "<script>alert('"+ a_docno + "修改成功！');</script>";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Save", alert_text);
                    gv已存在的表單Bind();
                }
                else {
                    string alert_text = "<script>alert('" + a_docno + "流程狀態 必須點選一項');</script>";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Save", alert_text);
                }
            }
        }


        
        
        
    }

    void runBtn送簽(string a_docno)
    {
        foreach (GridViewRow row in gv已存在的表單.Rows)
        {
            string x案件編號 = row.Cells[0].Text;
            if (x案件編號 == a_docno)
            {

                //流程狀態
                int Flow_Type_num = 0;
                string sFlow_Type = "";
                CheckBox cbFlow_Type1 = (CheckBox)row.Cells[5].FindControl("CheckFlow_Type1");//1.未處理
                CheckBox cbFlow_Type2 = (CheckBox)row.Cells[5].FindControl("CheckFlow_Type2");//2.處理中
                CheckBox cbFlow_Type3 = (CheckBox)row.Cells[5].FindControl("CheckFlow_Type3");//3.已結案-欲合作
                CheckBox cbFlow_Type4 = (CheckBox)row.Cells[5].FindControl("CheckFlow_Type4");//4.已結案-未合作
                if (cbFlow_Type1.Checked == true)
                {
                    sFlow_Type = cbFlow_Type1.Text;
                    Flow_Type_num = Flow_Type_num + 1;
                }
                if (cbFlow_Type2.Checked == true)
                {
                    sFlow_Type = cbFlow_Type2.Text;
                    Flow_Type_num = Flow_Type_num + 1;
                }
                if (cbFlow_Type3.Checked == true)
                {
                    sFlow_Type = cbFlow_Type3.Text;
                    Flow_Type_num = Flow_Type_num + 1;
                }
                if (cbFlow_Type4.Checked == true)
                {
                    sFlow_Type = cbFlow_Type4.Text;
                    Flow_Type_num = Flow_Type_num + 1;
                }
                //Response.Write(Flow_Type_num);
                if (Flow_Type_num == 1)
                {

                    TextBox tb員工編號 = (TextBox)row.Cells[2].FindControl("tb員工編號");
                    sdNCModify.UpdateParameters["DocNo"].DefaultValue = a_docno;
                    sdNCModify.UpdateParameters["EmpId"].DefaultValue = tb員工編號.Text;
                    sdNCModify.UpdateParameters["Flow_Type"].DefaultValue = sFlow_Type;
                    sdNCModify.UpdateParameters["FlowStatus"].DefaultValue = "1";
                    sdNCModify.Update();
                    string alert_text = "<script>alert('" + a_docno + "送簽成功！');</script>";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Save", alert_text);
                    gv已存在的表單Bind();
                }
                else
                {
                    string alert_text = "<script>alert('" + a_docno + "流程狀態 必須點選一項');</script>";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Save", alert_text);
                }
            }
        }
    }


}