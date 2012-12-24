<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Travel" Title="Travel CodeCamp SFBA" Codebehind="Travel.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <br />
    <strong></strong>
    <p>
        The Wiki entry has all kinds of information about travel to and from Foothill College
        where we will be having CodeCamp.  Please follow the link below to the Wiki Travel Page.
    </p>
    <p>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="http://codecamp.pbwiki.com/CodeCampSiliconValleyTravel">Wiki Link For Travel Information</asp:HyperLink></p>
    <p>
        <i>(Wiki is generously provided free of charge by the folks at <a href="http://pbwiki.com/">
            PBWiki</a> )</i>
    </p>

</asp:Content>

