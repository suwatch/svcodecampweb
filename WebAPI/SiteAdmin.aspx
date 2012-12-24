<%@ Page Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true" Inherits="SiteAdmin" Title="Site Admin CodeCamp SFBA" Codebehind="SiteAdmin.aspx.cs" %>

<asp:Content ID="SublinksSessions" ContentPlaceHolderID="Sublinks" runat="server">
    <asp:Menu ID="subMenu" runat="server" DataSourceID="SiteMapAbout" SkinID="subMenu">
    </asp:Menu>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <br />
    <br />
    <a href="SpeakerShirtSize.aspx">Speaker Shirt Size</a>
    <br />
    <br />
    <a href="SessionLevels.aspx">SessionLevels (should not change)</a>
    <br />
    <br />
    <a href="SessionTags.aspx">Session Tags (only for debug)</a>
    <br />
    <br />
    <a href="Membership.aspx">Membership.aspx (do not edit, just change approval status)</a>
    <br />
    <br />
    <a href="MembershipExt.aspx">MembershipExt.aspx (Just View and Search)</a>
    <br />
    <br />
    <a href="PasswordUpdateImp.aspx">Update Passwords and Take User Identity (USE THIS) </a>
    <br />
    <br />
    <a href="PasswordUpdate.aspx">Update Passwords By Username </a>
    <br />
    <br />
    <a href="EmailerAdmin.aspx">Bulk Emailer</a>
    <br />
    <br />
    <a href="AllAttendees.aspx">All Attendess Editor (careful)</a>
    <br />
    <br />
    <a href="PostEdit.aspx">Post Editor for Home Page Posts</a>
    <br />
    <br />
    <a href="ClassRoomsForPictures.aspx">Class Room Editor (pictures)</a>
    <br />
     <br />
    <a href="ClassRooms.aspx">Class Room Editor (GRID For Edit)</a>
    <br />
    <br />
    <a href="SessionRooms.aspx">Sessions By Room</a>
    <br />
    <br />
    <a href="AgendaEdit.aspx">Edit Agenda File</a>
    <br />
    <br />
    <a href="AgendaUpdate.aspx">Assign Sessions to Rooms and Time (hyperlink on top left for assigning sessions with no room yet)</a>
    <br />
    <br />
    <a href="AgendaUpdateByTime.aspx">Assign Sessions to Time (For Right After Big Schedule)</a>
     <br />
    <br />
    <a href="SponsorListEmail.aspx">Email Platinum Sponsors List</a>
   
    <br />
    <a href="MiscPages/UpdateSponsorship.aspx">Sponsor GridView Includes Amount/Table/BagItem/Comment
        SponsorListCodeCampYear</a>
   
    <br />
     <a href="SponsorManager.aspx">Sponsor Manager</a>
    <br/>
    <a href="MiscPages/SponsorImagesToSql.aspx">SponsorImagesToSql plus view logs</a>
    <br/>
      <a href="SponsorList.aspx">Sponsor List including Short Message For Mailing</a>
    <br />
    
    <a href="SponsorListImage.aspx">Sponsor List Image (Upload Image to DB)</a>
    <br />
    
   
    <br/>
    <br/>
    <a href="TrackEditor.aspx">Used for Adding first track of year</a>
    <br />
    <br />
    <a href="BadgeList.aspx">BadgeList.aspx</a>
    <br />
    <br />
    <a href="BadgeListForQR.aspx">BadgeListForQR.aspx</a>
    <br />
    <br />
  
    

    <br />
    <a href="DefaultDeleteTags.aspx">Delete Unassigned Tags</a>
    <br />
    <br />
    <a href="TrackViewer.aspx">TrackViewer (Used For Printing Tracks Mostly)</a>
    <br />

    <a href=" TrackEditorImage.aspx">TrackEditorImage (Used For updating track images)</a>
    <br />
    
   

    <%--  <br />
     <a href="SiteAdminUpdateSessionSpeaker.aspx">Update SessionSpeaker Table From Old Session Table</a>
     <br />--%>
    <br />
    <br />
    <a href="VolunteerJobEdit.aspx">Edit Volunteer Jobs and See Who is assigned</a>
    <br />
    <br />
    <a href="VolunteerJobSetup.aspx">Edit Volunteer Jobs (better editor, but does not show
        who has job)</a>
    <br />
    <br />
    <a href="VolunteersShowAll.aspx">Show All Volunteers For Current Year With Phone</a>
    <br />

     <br />
    <a href="VolunteersShowAllWithTime.aspx">Show All Volunteers For Current Year With Phone And Time</a>
    <br />

     <br />
    <a href="SessionChangeOwner.aspx">Change Owner of Session (must no ID of speaker and Session)</a>
    <br />
    
      <br />
    <a href="AdminScreen.aspx">Truncate Tables (new code camp year)</a>
    <br />
    
       <br />
    <a href="RegistrationCount.aspx">RegistrationCount</a>
    <br />
    
        <br />
    <a href="SessionHashTagsGrid.aspx">Add Hash Tags in Grid (twitter)</a>
    <br />
    

    


    
    <%--<a href=" ReferralTracker.aspx"> ReferralTracker.aspx</a>--%>
    <br />
    <%--  <asp:Button ID="ButtonReadOptOutConvertToAttendee" runat="server" 
        Text="Read Opt Out Convert To Attendee" 
        onclick="ButtonReadOptOutConvertToAttendee_Click" />--%>
    <a href="ListNanny.aspx">ListNanny.aspx</a>
    <br />
    <asp:Label ID="LabelStatus" runat="server" Text="Label"></asp:Label>
</asp:Content>
