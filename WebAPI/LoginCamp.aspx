<%@ Page Title="" Language="C#" MasterPageFile="~/RightNote.master" AutoEventWireup="true" Inherits="LoginCamp" Codebehind="LoginCamp.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <br />
<br />

<div align="center" 
    style="top: 100px; right: 100px; bottom: 100px; left: 100px">

    <asp:Login ID="Login1" runat="server"  RememberMeSet="true" 
        OnLoggedIn="Login1_LoggedIn"  >
    </asp:Login>


</div>

</asp:Content>

