<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Volunteer" Title="Volunteer CodeCamp SFBA" Codebehind="Volunteer.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" runat="Server">
    <br />
    <strong>We Need Your Help!!!</strong>
    <br />
    <br />
    <p>
        We need volunteers to make this event successful!
    </p>
    <br />
    
    <p>
        If you want to volunteer first indicate it on your profile. Go to the Register page
        and just check the box at the bottom letting us know that you would like to volunteer.
        Make sure you put your phone contact information so we can contact you directly
        during the event. A cell phone would be preferable.
    </p>
    <div runat="server" id="VolunteerForJobId"> 
    <br />
    <p>
        Next go the
        <asp:HyperLink ID="HyperLinkVolunteerForJob" NavigateUrl="VolunteerForJob.aspx" runat="server">Volunteer Jobs link</asp:HyperLink>
        (in the left pane just about Recent Job Posts) and pick what you want to do.
    </p>
    </div>
</asp:Content>
