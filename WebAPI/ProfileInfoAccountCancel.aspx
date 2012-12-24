<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
  Inherits="ProfileInfoAccountCancel" Title="Cancel Registration" Codebehind="ProfileInfoAccountCancel.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" Runat="Server">

<p><strong>Are You Sure You Want to Cancel Your Registration?</strong></p>

    <asp:CheckBox ID="CheckBox1" AutoPostBack="true" Text="To Cancel, check this box, and click the 'Cancel Registration' button below." runat="server" />
    <br />
    <br />

    <asp:Button ID="ButtonCancel" runat="server" Text="Cancel Registration" Enabled="false" OnClick="ButtonCancel_Click" /><br />
    <br />
    <asp:Label ID="LabelStatus" runat="server" Text=""></asp:Label>

</asp:Content>

