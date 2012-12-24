<%@ Page Language="C#" AutoEventWireup="true" Inherits="DisplayAdDriver" Codebehind="DisplayAdDriver.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:Image ID="Image1" ImageUrl="~/DisplayImage.ashx?roomid=6" runat="server" />
       <%-- <asp:Image ID="Image1" ImageUrl="~/DisplayAd.ashx?ImageType=4" runat="server" />--%>
    </div>
    </form>
</body>
</html>
