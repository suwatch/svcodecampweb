using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class PasswordUpdateImp : BaseContentPage
{



    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("ResetPassword"))
        {
            string userName = (string) e.CommandArgument;
            MembershipUser mu =
                Membership.GetUser(userName, false);

            if (!string.IsNullOrEmpty("pass@word"))
            {
                string newPassword = mu.ResetPassword();
                mu.ChangePassword(newPassword, "pass@word");
            }
        }
        else
        {
            FormsAuthentication.SignOut();
            string userName = (string) e.CommandArgument;
            FormsAuthentication.SetAuthCookie(userName, true);
            //Response.Redirect("~/PasswordUpdateImp.aspx");
        }
    }

    protected void ButtonSearch_Click(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }
    protected void ButtonResetPassword_Click(object sender, EventArgs e)
    {
       
    }
    protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("ResetPassword"))
        {
            string userName = (string)e.CommandArgument;
            MembershipUser mu =
                Membership.GetUser(userName, false);

            if (!string.IsNullOrEmpty("pass@word"))
            {
                string newPassword = mu.ResetPassword();
                mu.ChangePassword(newPassword, "pass@word");
            }
        }
        else if (e.CommandName.Equals("MakeCurrentUser"))
        {
            FormsAuthentication.SignOut();
            string userName = (string)e.CommandArgument;
            FormsAuthentication.SetAuthCookie(userName, true);
            //Response.Redirect("~/PasswordUpdateImp.aspx");
        }
    }
}

//string userName = eee.Row.
//FormsAuthentication.SetAuthCookie(userName, true);
//Response.Redirect("~/PasswordUpdateImp.aspx");