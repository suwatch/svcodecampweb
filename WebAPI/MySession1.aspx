<%@ Page Language="C#" AutoEventWireup="true" Inherits="MySession1" Codebehind="MySession1.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
     <script src="assets/js/ext-2.2/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="assets/js/ext-2.2/ext-all-debug.js" type="text/javascript"></script>
    <link href="assets/js/ext-2.2/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    
    
</head>
<body>
    <form id="form1" runat="server">
    <div id='ContentId' style="height: 500px;">
    
        <script type="text/javascript" src="MySessions.aspx.js">
        </script>

        <div id="loading-mask" style="">
        </div>
        <div id="loading">
            <div class="loading-indicator">
                <img src="docs/extanim32.gif" width="32" height="32" style="margin-right: 8px;" align="absmiddle" />Loading...</div>
        </div>
        
        <textarea id="detailtpl" style="display: none;height:300px;width:500px;">
        <div>
        <p>{UserBio}</p>
        </div>
        </textarea>
        
        
        <%--<textarea id="detailtpl" style="display: none;">
        <div class="pad">
            <table cellpadding="0" cellspacing="0" width="100%"><tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0" width="100%"><tr>
                    <td><div class="subHeading">{FirstName} {LastName}</div></td>
                    <td valign="middle"><div class="slashBullet">
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="Sessions.aspx">Sessions</asp:HyperLink></div></td>
                </tr></table>
                <div class="speakerDescription"  >{UserBio}</div>
                <div class="slashBullet"><asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="http://developer.marklogic.com" Target="_blank">Visit web site</asp:HyperLink></div>
            </td>
            <td valign="top">
                <asp:Image ID="Image2" ImageUrl="~/Images/Speakers/RonHitchens.jpg" ImageAlign="middle" runat="server" BorderStyle="None" CssClass="speakerImage" />
            </td>
            </tr></table>
        </div>
        </textarea>--%>
        <div id='myDiv'>
        </div>
    </div>
    </div>
    </form>
</body>
</html>
