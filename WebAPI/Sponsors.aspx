<%@ Page Title="" Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true" CodeBehind="Sponsors.aspx.cs" Inherits="WebAPI.Sponsors" %>



<%@ Register src="SponsorsList.ascx" tagname="SponsorsList" tagprefix="uc2" %>

<asp:Content ID="SublinksSponsors" ContentPlaceHolderID="Sublinks" runat="server">
    <asp:Menu ID="sponsorsSubMenu" runat="server" DataSourceID="SiteMapAbout" SkinID="subMenu">
    </asp:Menu>
</asp:Content>
<asp:Content ID="SessionsContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="mainHeading">
        Sponsors</div>
    <div class="pad">
        <p>
            Silicon Valley Code Camp is proud to offer sponsorships at four levels, 
            <strong>Platinum,
            Gold, Silver and Bronze</strong>. All levels will receive 
            <span style="text-decoration: underline">recognition and thanks</span> from the community
            for their participation. Platinum and Gold Sponsors may be listed in our schedule
            along with the specific events their contributions help pay for. For Example, Along
            side Lunch on the schedule will be the name of the sponsor(s) who helped make that
            happen.&nbsp;&nbsp; Contact <a href="mailto:info@siliconvalley-codecamp.com?subject=Information On Sponsorsing SV Code Camp">info@siliconvalley-codecamp.com</a> if you are 
            interested in being a sponsor or you would like to make a raffle prize donation</p>
            
        <p><span style="text-decoration: underline">Platinum sponsors will receive additional benefits</span> 
            which include being able to set up a table at our Registration area where these sponsors can give out material about their company as well
        as <i>discuss employment opportunities</i> with Code Camp attendees</p>
      
            
     
                            <br />
        
       <%-- You can go directly to our 
        <asp:HyperLink ID="HyperLinkPayPal" NavigateUrl="~/SponsorPayPal.aspx" runat="server">PayPal page</asp:HyperLink> &nbsp;and contribute directly from there.&nbsp; 
        Any amount is appreciated.<br />
                            <br />--%>
                            If you are interested in being a Community Sponsor, we ask 
        for three things:&nbsp; 1) Do a mailing now to your members letting them know about 
        Code Camp; 2) Display the Code Camp Advertisement on your home page; 3) Do a 
        final mailing 1 to 2 weeks before the Code Camp event (Here are some
        <a href="http://siliconvalley-codecamp.com/docs/SampleCodeCampAnnouncements.pdf">
        sample annoucements</a> from previous years).<br />
        <br />
        We are having a raffle Sunday at lunch. There are still 
        opportunities to make donations of services or products to be in the raffle. You 
        can see the raffle prizes donated so far here:<br />
        <br />
        
        
        <asp:HyperLink ID="HyperLinkRaffle" runat="server">FILL IN FROM WEB.CONFIG SponsorPageRaffleURL</asp:HyperLink>
        <%--<a href="http://codecamp.pbwiki.com/Raffle+Prizes+2009" >
        http://codecamp.pbwiki.com/Raffle+Prizes+2009</a>--%>
        <br />
        <br />
                            Contact <a href="mailto:info@siliconvalley-codecamp.com">
                            info@siliconvalley-codecamp.com</a> if you are interested in being a sponsor.<br />
        
        
        <uc2:SponsorsList ID="SponsorsList1" ShowPictures="true"
                                    runat="server" />
        
       </div>
    
</asp:Content>
