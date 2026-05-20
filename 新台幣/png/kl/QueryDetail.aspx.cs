using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Program_KL_QueryDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserControl u1 = (UserControl)this.FindControl("Detail");
            HiddenField hfNo = (HiddenField)u1.FindControl("hfNo");
            HiddenField hfAction = (HiddenField)u1.FindControl("hfAction");
            hfNo.Value = Request.QueryString["No"]; hfAction.Value = "QTY";
        }
    }
}