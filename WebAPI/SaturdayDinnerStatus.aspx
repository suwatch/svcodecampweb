<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="SaturdayDinnerStatus" Codebehind="SaturdayDinnerStatus.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="defaultSublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="parentContent" Runat="Server">
 <br />
    <br />
    <asp:Literal ID="LabelStatus" runat="server" ></asp:Literal>
    
    <br />
    <br />
    <asp:HyperLink ID="HyperLinkProfile"  NavigateUrl="~/ProfileInfoAccount.aspx" runat="server">Update Your Profile</asp:HyperLink>
    <br />
    <br />
    <asp:HyperLink ID="HyperLinkSessions" NavigateUrl="~/Sessions.aspx" runat="server">Update Sessions You Plan to Attend</asp:HyperLink>
    <br />
    <br />
    <asp:HyperLink ID="HyperLinkSessionsOverview" NavigateUrl="~/SessionsOverview.aspx" runat="server">Sessions Overview</asp:HyperLink>
    <br />
    <br />
    <asp:HyperLink ID="HyperLinkSponsors" NavigateUrl="~/Sponsors.aspx" runat="server">See Who is Sponsoring Code Camp</asp:HyperLink>
    <br />
    <br />
    <asp:HyperLink ID="HyperLinkPayPal" NavigateUrl="~/SponsorPayPal.aspx" runat="server">Make a PayPal contribution to help pay for all this free stuff</asp:HyperLink>




</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarRight" Runat="Server">
</asp:Content>

