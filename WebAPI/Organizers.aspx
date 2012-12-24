<%@ Page Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true"
    Inherits="Organizers" Title="Organizers" Codebehind="Organizers.aspx.cs" %>
    
    
    
<asp:Content ID="SublinksOrganizers" ContentPlaceHolderID="Sublinks" runat="server">
    <asp:Menu ID="organizersSubMenu" runat="server" DataSourceID="SiteMapAbout" SkinID="subMenu">
    </asp:Menu>
</asp:Content>



<asp:Content ID="SessionsContent" ContentPlaceHolderID="MainContent" runat="server">



<div class="mainHeading">Organizers</div>



<div class="pad">
<p>Silicon Valley Code Camp is put on by a dedicated group of volunteers whose mission is to both provide the highest quality content
built around the topic of computer code, as well as create an environment where shared knowledge is paramount.  The volunteers not only include the organizers, but all the speakers as well.</p>
    <p>If you are interested in helping, please go to the 
        <a href="http://www.siliconvalley-codecamp.com/Volunteer.aspx"> page</a>, or send email to Volunteers@siliconvalley-codecamp.com.</p>
</div>



</asp:Content>



