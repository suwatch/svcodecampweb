<%@ Page Language="C#" AutoEventWireup="true" Inherits="LoginForm" Codebehind="LoginForm.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Login ID="Login1" runat="server">
        </asp:Login>
        <hr />
        Logged In As:
        <asp:LoginName ID="LoginName1" runat="server" />

        <br />
        <br />

        <asp:Button ID="ButtonSignOff" runat="server" Text="Sign Off" 
            onclick="ButtonSignOff_Click" />
    
        <br />
        <br />
        <br />
    
    </div>
    </form>
    <p>
        <a href="Default.aspx">Home Page</a></p>
</body>
</html>
