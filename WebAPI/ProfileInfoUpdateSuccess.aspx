<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
   Inherits="ProfileInfoUpdateSuccess" 
   Title="Profile Information Update Success" Codebehind="ProfileInfoUpdateSuccess.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" Runat="Server">

<hr />
    <span style="font-size: 1.4em">
<strong>Profile Information Updated Successfully</strong>
<br />
    </span>

    <asp:HyperLink ID="HyperLink1" NavigateUrl="~/ProfileInfoAccount.aspx" runat="server">Return to Profile Information</asp:HyperLink>

<hr />

</asp:Content>

