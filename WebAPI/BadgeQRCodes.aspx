<%@ Page Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true" Inherits="BadgeQRCodes" Title="Badge QR Codes" Codebehind="BadgeQRCodes.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h2>Silicon Valley Code Camp Badges with QR Codes</h2>
    <br />
    <p>After you meet someone, you ask them for a business card or their contact information.</p>
    <p>Or, you ask them if you can take a picture of their Silicon Valley Code Camp Badge with your Smart Phone (Android, IPhone, Win7 Phone, etc.),
    then, you take the picture and the app asks you if you want to store their information in your contacts.  No internet connectivity necessary, Only the
    the information the person has chosen to share will be included.</p>
    <p>A picture is worth a thousand words!</p>
    <p>&nbsp;</p>

    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/QRDemo1.jpg" />


</asp:Content>
