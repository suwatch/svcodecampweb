using System;
using System.Text;
using System.Web.UI;

public partial class Login : BaseContentPage
{
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if (Context.User.Identity.IsAuthenticated)
    //    {
    //        RegisterStartupScriptBlock();
    //    }
    //}

    

    //private void RegisterStartupScriptBlock()
    //{
    //    // Define the name and type of the client scripts on the page.
    //    String csname1 = "returnValue";
    //    String csname2 = "ButtonClickScript";
    //    Type cstype = GetType();

    //    // Get a ClientScriptManager reference from the Page class.
    //    ClientScriptManager cs = Page.ClientScript;

    //    // Check to see if the client script is already registered.
    //    if (!cs.IsClientScriptBlockRegistered(cstype, csname2))
    //    {
    //        StringBuilder cstext2 = new StringBuilder();
    //        cstext2.Append("<script type=\"text/javascript\"> function returnValue() {");
    //        cstext2.AppendLine("var oWindow = null;");
    //        cstext2.AppendLine("if (window.radWindow) oWindow = window.radWindow;");
    //        cstext2.AppendLine("else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;");
    //        cstext2.AppendLine("oWindow.close();");
    //        cstext2.Append("} </");
    //        cstext2.Append("script>");
    //        cs.RegisterClientScriptBlock(cstype, csname2, cstext2.ToString(), false);


    //        if (!cs.IsStartupScriptRegistered(cstype, csname1))
    //        {
    //            String cstext1 = "returnValue();";
    //            cs.RegisterStartupScript(cstype, csname1, cstext1, true);
    //        }

    //    }
    //}
    //protected void Login1_LoggedIn(object sender, EventArgs e)
    //{
        
    //}
}
