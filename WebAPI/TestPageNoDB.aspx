﻿<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        string str = ConfigurationManager.ConnectionStrings["CodeCampSV06"].ToString();
        int pos = str.ToLower().IndexOf("password");
        Label1.Text = str.Substring(0, pos + 8) + "...";

        Label2.Text = DateTime.Now.ToLongTimeString();

        Label3.Text = Context.Request["SERVER_NAME"];// Request.ServerVariables("SERVER_NAME")

        Label4.Text = Context.Request["REMOTE_ADDR"];
        
        
        if (Context.Request.QueryString["sleep"] != null)
        {
            int sleepMs = Convert.ToInt32(Context.Request.QueryString["sleep"]);
            System.Threading.Thread.Sleep(sleepMs);

        }
        
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Test Page, NO DB - Code Camp </title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Test 
    <br />
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <br />
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        <br />
         <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
         <br />
         <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>
        
    
    </div>
    </form>
</body>
</html>
