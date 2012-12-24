<%@ Page Language="C#" AutoEventWireup="true" Inherits="SessionChangeOwner" Title="Session Change Owner" Codebehind="SessionChangeOwner.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <br />
        SessionId:&nbsp;&nbsp;  
        <asp:TextBox ID="TextBoxSessionId" runat="server"></asp:TextBox>
          <br />
        NewPresenterUsername:&nbsp; wardwar 
        <asp:TextBox ID="TextBoxNewPresenterUsername" runat="server"></asp:TextBox>

        <br />

        <asp:CheckBox Text="For Real Update" ID="CheckBoxForReal" runat="server" />
        <br />
        <asp:Button ID="ButtonChangePresenter" runat="server" 
            Text="Change SessionOwner" onclick="ButtonChangePresenter_Click" />





    
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:TextBox ID="TextBoxStatus" runat="server" Height="465px" Width="929px"></asp:TextBox>





    
    </div>
    </form>
</body>
</html>
