using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class LoginCamp : System.Web.UI.Page
{

    protected void Login1_LoggedIn(object sender, EventArgs e)
    {
        if (Request.IsSecureConnection)
        {
            Response.Redirect("http://siliconvalley-codecamp.com");
        }
    }
}