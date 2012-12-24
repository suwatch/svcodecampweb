<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true" Inherits="SiteAdminUpdateSessionSpeaker" Codebehind="SiteAdminUpdateSessionSpeaker.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="blankSublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" Runat="Server">


    <br />


    <asp:Button ID="Button1" runat="server" Text="Update SessionSpeakers From original data" onclick="Button1_Click" />
    
    <br />
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
</asp:Content>

