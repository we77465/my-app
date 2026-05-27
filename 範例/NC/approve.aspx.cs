using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Program_NC_approve : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hidn_EmpNo.Value = UserInfo.getUserId(Request.QueryString["sid"], Request);
            string[] usrinfo = UserInfo.getUserInfo(hidn_EmpNo.Value).Split(',');
            hidn_BRNO.Value = usrinfo[0];
            sdNC.SelectParameters["DocNo"].DefaultValue = Request.QueryString["DocNo"];
            sdsNC.SelectParameters["DocNo"].DefaultValue = Request.QueryString["DocNo"];
            DataView dvList = sdsNC.Select(new DataSourceSelectArguments()) as DataView;


            DataTable dtList = dvList.ToTable();
            //Response.Write(dtList.Rows.Count > 0);
            if (dtList.Rows.Count > 0)
            {
                //Response.Write(FlowStatus1);
                string FlowStatus = dtList.Rows[0]["FlowStatus"].ToString();
                //Response.Write(FlowStatus);
                //待主管放行
                if (FlowStatus == "2")
                {
                    if (Request.QueryString["hOvrd"] == "1")
                    {
                        btnSign.Visible = btnBack.Visible = false;
                    }
                    else
                    {
                        btnSign.Visible = btnBack.Visible = true;
                    }
                    //Response.Write("222222222");
                }
                //待負責人放行
                if (FlowStatus == "1")
                {
                    if (Request.QueryString["hOvrd"] == "1")
                    {
                        btnSign.Visible = btnBack.Visible = true;
                    }
                    else
                    {
                        btnSign.Visible = btnBack.Visible = false;
                    }
                    //Response.Write("111111111");
                }
                if (FlowStatus == "3" || Convert.ToInt32(FlowStatus) <= 0)
                {
                    btnSign.Visible = btnBack.Visible = false;
                    //Response.Write("+++++++++++");
                }
            }

        }

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

    protected void btnSign_Click(object sender, EventArgs e)
    {
        //Label2.Text = "USER1:" + hfUserID.Value + ",DocNo:" + Request.QueryString["DocNo"] + ",hOvrd:" + Request.QueryString["hOvrd"] + ",len:" + Request.QueryString["hOvrd"].Length.ToString();
        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "UPD", "<script>alert('"+ Label2.Text + "');</script>");
        //hOvrd：1 - 負責人;2-主管
        if (Request.QueryString["hOvrd"] == "1")
        {
            sdsNC.UpdateParameters["Sign1"].DefaultValue = hfUserID.Value;
            //sdsNC.UpdateParameters["Sign1"].DefaultValue = "123456";
            sdsNC.UpdateParameters["Action1"].DefaultValue = "D";
            //sdsNC.UpdateParameters["FlowStatus"].DefaultValue = "2";
            sdsNC.UpdateParameters["FlowStatus"].DefaultValue = "3";
            sdsNC.UpdateParameters["DocNo"].DefaultValue = Request.QueryString["DocNo"];
            //sdsNC.UpdateParameters["DocNo"].DefaultValue = "KQ20230302220019";
            sdsNC.Update();
            //Label2.Text = "666," + Label2.Text;
        }
        //else
        //{
        //    sdsUpd2.UpdateParameters["Sign2"].DefaultValue = hfUserID.Value;
        //    sdsUpd2.UpdateParameters["Action2"].DefaultValue = "D";
        //    sdsUpd2.UpdateParameters["FlowStatus"].DefaultValue = "3";
        //    sdsUpd2.UpdateParameters["DocNo"].DefaultValue = Request.QueryString["DocNo"];
        //    sdsUpd2.Update();
        //    Label2.Text = "777," + Label2.Text;
        //    btnSign.Visible = btnBack.Visible = false;
        //    Response.Write("+++++++++++");
        //}
        string url = string.Empty;
        if (Request.QueryString["hOvrd"] == "1")
        {
            url = System.Web.Configuration.WebConfigurationManager.AppSettings["FormManage1_Url"]; //Web.Config的放行列表頁面
        }
        //else
        //{
        //    url = System.Web.Configuration.WebConfigurationManager.AppSettings["FormManage2_Url"]; //Web.Config的放行列表頁面
        //}
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "UPD", "<script>alert('放行成功!');location.href='" + url + "';</script>");



    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        //Insert_Hidden();
        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Out", "<script>alert('放行成功！');</script>");
        //btnSave.Visible = false;
        //btnOut.Visible = false;

        //Label2.Text = "USER22:" + hfUserID.Value;
        //hOvrd：1 - 負責人;2-主管
        if (Request.QueryString["hOvrd"] == "1")
        {
            sdsUpd3.UpdateParameters["Sign1"].DefaultValue = hfUserID.Value;
            sdsUpd3.UpdateParameters["DocNo"].DefaultValue = Request.QueryString["DocNo"];
            sdsUpd3.Update();
        }
        //else
        //{
        //    sdsUpd4.UpdateParameters["Sign2"].DefaultValue = hfUserID.Value;
        //    sdsUpd4.UpdateParameters["DocNo"].DefaultValue = Request.QueryString["DocNo"];
        //    sdsUpd4.Update();
        //}

        string url = string.Empty;
        if (Request.QueryString["hOvrd"] == "1")
        {
            url = System.Web.Configuration.WebConfigurationManager.AppSettings["FormManage1_Url"]; //Web.Config的放行列表頁面
        }
        //else
        //{
        //    url = System.Web.Configuration.WebConfigurationManager.AppSettings["FormManage2_Url"]; //Web.Config的放行列表頁面
        //}
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "UPD", "<script>alert('退回成功!');location.href='" + url + "';</script>");

    }
}