using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Program_KL_Query : System.Web.UI.Page
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
            }

            if (hfBrNo.Value != "0501" && hfBrNo.Value != "0201")
            {
                drpBrno.SelectedValue = hfBrNo.Value;
                drpBrno.Enabled = false;
            }
            else
            {
                drpBrno.SelectedValue = "%";
                drpBrno.Enabled = true;
            }

            gvDataBind();
        }
    }
    private void gvDataBind()
    {
        sdsList.SelectParameters["Brno"].DefaultValue = drpBrno.SelectedValue;
        sdsList.SelectParameters["ID"].DefaultValue = txtID.Text.Trim().Length > 0 ? txtID.Text.Trim() : "%";
        gvData.DataBind();
    }
    protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView gv = (GridView)sender;

        if (e.CommandName == "Select")
        {
            LinkButton btn = (LinkButton)e.CommandSource;
            GridViewRow myRow = (GridViewRow)btn.NamingContainer;

            string sNo = gv.DataKeys[myRow.RowIndex][0].ToString();
            Response.Redirect("../KL/QueryDetail.aspx?SID=" + Request.QueryString["SID"] + "&No=" + sNo);
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        gvDataBind();
    }
}