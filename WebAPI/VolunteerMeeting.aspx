<%@ Page Title="" Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true" Inherits="VolunteerMeeting" Codebehind="VolunteerMeeting.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="mainHeading">
        <h2>Volunteer Meeting Tuesday August 16th, 2011</h2>
        <br/>
        (Foothill 
        College Room 5015, Starting at 6PM)
        <br />
        (5000 Building in this
        <a href="http://codecamp.pbworks.com/w/page/16049554/Campus%20Map">MAP</a>, 
        Bring $2 for Parking)<br />
        (Pizza and Soda On Patio From 5:45 to 6:15, No food in meeting rooms allowed)</div>
    <h2>
       Come Here About The Volunteer Jobs and Benefits of Volunteering At Code Camp in October.  Ask all your questions!  Provide us feedback.
            <asp:Label ID="LabelUsername" Visible="false" runat="server" Text="Label"></asp:Label></b> Volunteer
       </h2>
    <br />

   <%-- 1:person visited this page   2:NotInterested   3:Interested   4:WillAttend --%>

    <asp:RadioButtonList ID="EventInterestId" runat="server" Width="400px" AutoPostBack="true"
        OnSelectedIndexChanged="EventInterestId_SelectedIndexChanged">
       <%-- <asp:ListItem Value="3">Interested in Attending Our Pre-Camp Meeting</asp:ListItem>
        <asp:ListItem runat="server" Value="4" Id="RadioButtonWillAttend" >I Will Attend</asp:ListItem>
        <asp:ListItem Value="2">Not Interested Or Not Able to Attend Our Pre-Camp Meeting</asp:ListItem>--%>
    </asp:RadioButtonList>
    <asp:Label ID="LabelNeedToBeLoggedIn" ForeColor="Red" Visible="false" runat="server"
        Text="Label">(You Need To Be Logged In To Select Interest)</asp:Label>
    <br />
    <h3>
        Benefits</h3>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Satisfaction of being part of this great all volunteer
    event<br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Meet All Kinds of Interesting People<br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Interesting Assigments<br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Invite to Speakers and Volunteer Appreciation Dinner
    Saturday Night, October 8,2011<br />
    <br />
    <h3>
        Meeting Agenda</h3>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Pizza and Soda<br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Short Presentation of the Jobs Available<br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; New Jobs for 2011!<br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Q&A (anything goes)<br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Adjourn (plan to wrap no later than 8PM)<br />
    <hr />
    <br />
    <h3>
        Status</h3>
    <h4>
        (We Will Contact You By Email and Post Here Also When our Location is Confirmed
        for the meeting)</h4>
    <br />
    <i>(If you do not make the
        meeting, we still would love you to volunteer. Just go to the registration page
        and update your profile to let us know you want to volunteer and we will contact
        you in the weeks leading up to the event) </i>


        <br/>
        <br />
<br />
<br />
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="select volunteerMeetingstatus as 'Status 1-NotInterested;2-Interested;3-attending',
       count(*) as 'Count'
FROM attendees
where volunteerMeetingstatus is not null
group by volunteerMeetingstatus
"></asp:SqlDataSource>

<br/>
    <asp:Label ID="LabelAdmin" runat="server" Text="Label"></asp:Label>
    <br/>
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSource1">
    <Columns>
        <asp:BoundField DataField="Status 1-NotInterested;2-Interested;3-attending" 
            HeaderText="Status 1-NotInterested;2-Interested;3-attending" 
            SortExpression="Status 1-NotInterested;2-Interested;3-attending" />
        <asp:BoundField DataField="Count" HeaderText="Count" ReadOnly="True" 
            SortExpression="Count" />
    </Columns>
</asp:GridView>
<br />
</asp:Content>
