<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="ConfirmRegistration" Title="Untitled Page" Codebehind="ConfirmRegistration.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <asp:Label ID="LabelConfirm" runat="server" Font-Size="Larger" Text=""></asp:Label>
    <br />
    <asp:Button ID="ButtonProfile" Visible="false" runat="server" Text="Update Profile" OnClick="ButtonProfile_Click" />
    <br />
</asp:Content>

