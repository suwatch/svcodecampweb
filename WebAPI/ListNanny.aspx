<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListNanny.aspx.cs" Inherits="WebAPI.ListNanny" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

    
 
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

        <asp:HyperLink ID="HyperLinkHome" NavigateUrl="~/Default.aspx" runat="server">Code Camp Home</asp:HyperLink>
        <hr />

     <asp:Button ID="ButtonRun" runat="server" Text="Process" 
        onclick="ButtonRun_Click" />
   
    <asp:CheckBox ID="CheckBoxDeleteAfterProcessing" Text="Delete After Processing" AutoPostBack = "true"  runat="server" />
   
    Max To Process:  
    <asp:TextBox ID="TextBoxMaxCnt" Text="100" runat="server"></asp:TextBox>
   
    <hr />
     <asp:CheckBox ID="CheckBoxUpdateAttendees" Text="Update Attendees" AutoPostBack = "true"  runat="server" />
    <hr />

       <asp:GridView ID="GridViewTotals" runat="server">
    </asp:GridView>

    <hr />

    <asp:GridView ID="GridViewResults" runat="server">
    </asp:GridView>
    
        <br />
        <br />
        <br />
    
    </div>


    <div>
    
    vacation and onhold will not change email status
    <br/>
    -1 will not update records, 1 will and 2 will both update the records
    
    </div>
 
    </form>
</body>
</html>

