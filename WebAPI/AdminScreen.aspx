<%@ Page Language="C#" AutoEventWireup="true" Inherits="AdminScreen" Codebehind="AdminScreen.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Admin Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="ButtonRemoveData" runat="server" Text="Delete Attendees,Presentations" OnClick="ButtonRemoveData_Click" />
        <br />
          <br />
       
        
        <asp:Button ID="ButtonAssignpkellneradmin" runat="server" Text="Assign pkellner Admin Role" OnClick="ButtonAssignpkellneradmin_Click" />
        <br />
          <br />
       
        <asp:Button ID="ButtonSignOff" runat="server" Text="SignOff" OnClick="ButtonSignOff_Click" />
        <br />
          <br />
        
         <asp:Button ID="ButtonRererralUrlData" runat="server" 
            Text="Truncate ReferralUrl and ReferralUrlGroup" 
            onclick="ButtonRererralUrlData_Click"  />
        <br />
        <br />
        
          <asp:Button ID="ButtonTruncateTwitterData" runat="server" 
            Text="Truncate Twitter Data" onclick="ButtonTruncateTwitterData_Click"   />
        <br />
        <br />
        
          <asp:Button ID="ButtonTruncateLog4NetAll" runat="server" 
            Text="Truncate Log4NetAll" onclick="ButtonTruncateLog4NetAll_Click"    />
        <br />
        <br />

        <br />
        <br />
        <br />
        <br />
        

        

        <hr />
          <br />
        <asp:Label ID="LabelStatus" runat="server" Text="Label"></asp:Label>
    </div>
    </form>
</body>
</html>
