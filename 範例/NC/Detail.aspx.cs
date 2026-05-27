using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Collections;
using System.Net;

public partial class Program_NC_Detail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            //hidn_EmpNo.Value = (Request.Cookies["CHB_IW_"]["User_ID"] != null) ? Request.Cookies["CHB_IW_"]["User_ID"].ToString() : (Request.QueryString["sid"] != null) ? UserInfo.getUserId(Request.QueryString["sid"].ToString(), Request) : Request.QueryString["EMPID"].ToString();
            //string[] usrinfo = UserInfo.getUserInfo(hidn_EmpNo.Value).Split(',');
            //hidn_BRNO.Value = usrinfo[0];
            //Response.Write("++++++++");

            Label_NO.Text = Request.QueryString["DocNo"];
            sdsNC.SelectParameters["DocNo"].DefaultValue = Request.QueryString["DocNo"];

            var dv = sdsNC.Select(DataSourceSelectArguments.Empty) as DataView;
            if (dv != null && dv.Table.Rows.Count > 0)
            {
                Label_Apply_Identity.Text = dv.Table.Rows[0]["Apply_Identity"].ToString();
                Label_Apply_Item.Text = dv.Table.Rows[0]["Apply_Item"].ToString();
                Label_Party_Name.Text = dv.Table.Rows[0]["Party_Name"].ToString();
                Label_Party_Id.Text = dv.Table.Rows[0]["Party_Id"].ToString();
                Label_Out_Party_Name.Text = dv.Table.Rows[0]["Out_Party_Name"].ToString();
                Label_Addr.Text = dv.Table.Rows[0]["Zip_Code"].ToString() + dv.Table.Rows[0]["City"].ToString() + dv.Table.Rows[0]["Area"].ToString() + dv.Table.Rows[0]["Addr"].ToString();
                Label_Contact_Person.Text = dv.Table.Rows[0]["Contact_Person"].ToString();
                Label_TEL.Text = dv.Table.Rows[0]["TEL"].ToString();
                Label_Contact_Time.Text = dv.Table.Rows[0]["Contact_Time"].ToString();
                Label_MAIL.Text = dv.Table.Rows[0]["MAIL"].ToString();
                Label_Business_Content.Text = dv.Table.Rows[0]["Business_Content"].ToString();
                Label_Remark.Text = dv.Table.Rows[0]["Remark"].ToString();
            }
        }
    }
}