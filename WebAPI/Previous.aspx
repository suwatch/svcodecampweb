<%@ Page Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true"
    Inherits="Previous" Title="Previous" Codebehind="Previous.aspx.cs" %>
    
    
    
<asp:Content ID="SublinksPrevious" ContentPlaceHolderID="Sublinks" runat="server">
    <asp:Menu ID="previousSubMenu" runat="server" DataSourceID="SiteMapAbout" SkinID="subMenu">
    </asp:Menu>
</asp:Content>



<asp:Content ID="SessionsContent" ContentPlaceHolderID="MainContent" runat="server">



<div class="mainHeading">Past CodeCamps</div>



<div class="pad">


<p>Previously, two successful Code Camp's have been run at the same location, Foothill College in Lost Altos. To access these old sites with
their sessions and presenters, click on the links below. (Data was not presered for the first two years)</p>

<asp:Repeater ID="RepeaterId" runat="server">
    <HeaderTemplate><ul></HeaderTemplate>
    
    <ItemTemplate>
        
        <li><asp:HyperLink   Enabled='<%# GetCodeCampYearEnabled((int) Eval("YearValue")) %>'
            Text='<%# GetCodeCampYearText((int) Eval("YearValue")) %>'
            NavigateUrl='<%#    GetCodeCampYearUrl((int) Eval("YearValue"))     %>'
            runat="server"></asp:HyperLink></li>

    </ItemTemplate>
    
    <FooterTemplate></ul></FooterTemplate>
</asp:Repeater>

<%--
<ul>
    <li><asp:HyperLink ID="HyperLink1"  Enabled="false"  runat="server">2006 CodeCamp</asp:HyperLink></li>
    <li><asp:HyperLink ID="HyperLink2"  Enabled="false"  runat="server">2007 CodeCamp</asp:HyperLink></li>
    <li><asp:HyperLink ID="HyperLink3" NavigateUrl="~/Default.aspx?Year=2008"  runat="server">2008 CodeCamp</asp:HyperLink></li>
    <li><asp:HyperLink ID="HyperLink4" NavigateUrl="~/Default.aspx?Year=2009"  runat="server">2009 CodeCamp</asp:HyperLink></li>
    <li><asp:HyperLink ID="HyperLink5" NavigateUrl="~/Default.aspx?Year=2010"  runat="server">2010 CodeCamp</asp:HyperLink></li>
   
</ul>--%>

<br />
This Year:  <asp:HyperLink ID="HyperLinkThisYear" NavigateUrl="~/Default.aspx"  runat="server">Current CodeCamp</asp:HyperLink>





</div>



</asp:Content>

