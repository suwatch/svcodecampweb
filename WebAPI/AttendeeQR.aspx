<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true" Inherits="AttendeeQR" Codebehind="AttendeeQR.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="blankSublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" Runat="Server">


    <asp:Button ID="Button1" runat="server" Text="Button" 
        onclick="Button1_Click1" />


        DATA:
    <asp:TextBox ID="TextBox1" TextMode="MultiLine" Height="600" Width="800" runat="server"></asp:TextBox>
    DONE:


</asp:Content>

