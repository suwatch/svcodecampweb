using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class LoginForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            ButtonSignOff.Visible = true;
            LoginName1.Visible = true;
        }
        else
        {
            ButtonSignOff.Visible = false;
            LoginName1.Visible = false;
        }
    }
    protected void ButtonSignOff_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
        Response.Redirect("~/LoginForm.aspx", true);
    }
}