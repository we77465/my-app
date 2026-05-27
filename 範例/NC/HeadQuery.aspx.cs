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

public partial class Program_NC_HeadQuery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hidn_EmpNo.Value = UserInfo.getUserId(Request.QueryString["sid"], Request);
            string[] usrinfo = UserInfo.getUserInfo(hidn_EmpNo.Value).Split(',');
            hidn_BRNO.Value = usrinfo[0];

        }

    }

    protected void Btn_Submit_Click(object sender, EventArgs e)
    {
        //Response.Write(drpBrno.Text=="5710");
        if (drpBrno.Text == "-001")
        {
            sdNC.SelectParameters["Brno"].DefaultValue = "%";
        }
        else
        {
            sdNC.SelectParameters["Brno"].DefaultValue = "%"+drpBrno.Text.ToString()+ "%";
        }

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
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "Page")
            return;
        string docNo = e.CommandArgument.ToString();
        if (e.CommandName == "MyReadOnly")
        {
            Response.Redirect("Detail.aspx?DocNo=" + docNo);
            //Response.Redirect("~/../DotNet6/OACHB/OACHB01-12/JJ/OAFRM_Manager/sign/" + docNo + "?sid=" + Request.QueryString["sid"].ToString());
        }




    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {


        //Response.Write(drpBrno.Text=="5710");
        if (drpBrno.Text == "-001")
        {
            sdNC.SelectParameters["Brno"].DefaultValue = "%";
        }
        else
        {
            sdNC.SelectParameters["Brno"].DefaultValue = "%" + drpBrno.Text.ToString() + "%";
        }

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


}