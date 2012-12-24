<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    protected void ButtonSignOff_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Force Signoff</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="ButtonSignOff" runat="server" Text="SignOff" 
            onclick="ButtonSignOff_Click" style="height: 26px" />
        
        <asp:HyperLink ID="HyperLink1" NavigateUrl="~/Default.aspx" runat="server">Home</asp:HyperLink>
    
    </div>
    </form>
</body>
</html>
