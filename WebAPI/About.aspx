<%@ Page Title="" Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="WebAPI.About" %>


<asp:Content ID="SublinksSessions" ContentPlaceHolderID="Sublinks" runat="server">
    <asp:Menu ID="subMenu" runat="server" DataSourceID="SiteMapAbout" SkinID="subMenu">
    </asp:Menu>
</asp:Content>



<asp:Content ID="SessionsContent" ContentPlaceHolderID="MainContent" runat="server">



<div class="mainHeading">About</div>



<div class="pad">
    
    <p>Code Camp is a new type of community event where developers learn from fellow developers. All are welcome to attend and speak. Code Camps have been wildly successful, and we’re going to bring that success to Northern California.</p>
   
</div>

<div class="pad" >
<h3>The Code Camp Web Site</h3>
<p>The Code Camp web site has been used for this code camp as well as all the past 
    ones starting in 2005. It is developed using the Microsoft .Net Framework, 
    ASP.NET 4.0 and SqlServer 2008 and is constantly being updated.<p>&nbsp;<p>The site was built
By  
    <asp:HyperLink ID="HyperLink1" NavigateUrl="http://PeterKellner.net"  runat="server">Peter Kellner</asp:HyperLink>, 
        one of the code camp organizers.  The new site design 
        was built and donated by <a href="http://www.udanium.com">Uday Gajendar</a>.<p>&nbsp;<p><a href="http://PeterKellner.net">Peter Kellner</a> has written a series of
                                <a href="http://peterkellner.net/2008/05/19/codecampwebsiteseries2/">Blog Posts 
                                on Building this Code Camp Web Site Here</a><p>&nbsp;</div>




<script type="text/javascript"><!--
    google_ad_client = "pub-3690548624166179";
    /* 728x90, created 3/22/09 */
    google_ad_slot = "0153537864";
    google_ad_width = 728;
    google_ad_height = 90;
    //-->
</script>
<script type="text/javascript"
src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
</script>



</asp:Content>







