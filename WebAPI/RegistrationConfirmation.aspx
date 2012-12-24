<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="RegistrationConfirmation"
    Title="Registration Confirmation" Codebehind="RegistrationConfirmation.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" runat="Server">
    <div class="mainHeading">Confirmation Screen</div>
        <h3>Congratulations!  You are Registered.</h3>
    <p>&nbsp;</p>
        <div class="pad">
        <div id="DivVolunteerInfo" runat="server">
        <h2>Help us Make CodeCamp Better!</h2><br />
        <p>
            The CodeCamp is a volunteer based event.&nbsp; We do not charge attendess
            or pay our speakers.&nbsp; Would you like to help make this event better
            by volunteering some of your time?&nbsp; If so, please make sure you have 
            checked &quot;I&#39;d like to volunteer!&quot; on the 
            <asp:HyperLink ID="HyperLink1" NavigateUrl="~/Register.aspx" runat="server">registration page</asp:HyperLink> (or go back and do it 
            now).&nbsp; If you have any questions send an email to <a href="mailto:Volunteer2011@SiliconValley-CodeCamp.com">
                Volunteer2011@SiliconValley-CodeCamp.com</a> .&nbsp; Typical jobs are listed 
            below.</p>
        <p>
            Once you check the box that you want to volunteer, and update your registration, 
            you will see on the left column of this site a Volunteer Jobs link (assuming we 
            have set up the jobs which we will not until the week before the event).&nbsp; Click on the link and you can pick the jobs and 
            times that work for you.</p>
        <p>
        </p>
        <ul>
            <li>Identify and/or solicit speakers</li>
            <li>Registration (in advance and/or day of the event)</li>
            <li>Room Monitoring</li>
            <li>Parking </li>
            <li>Web Site Help</li>
            <li>Event Support (helping with setup, putting up signs, etc. just before the event)</li>
            <li>Food preparation</li>
            <li>Getting raffle items</li>
        </ul>
    </div>
    <div id="DivGenerateReferral" runat="server">
        <h2>
            Help us Spread the Word!
        </h2>
        <p>
            You can help spread the word about Silicon Valley Code Camp.&nbsp; Below is a custom
            URL that you can paste on your web site to let everyone know about CodeCamp!&nbsp;
        </p>
        <p>
            <asp:HyperLink ID="HyperLinkReferral" runat="server"></asp:HyperLink><br /><br />
            <asp:TextBox ID="TextBoxReferralLink" Width="600" runat="server"></asp:TextBox>
        </p>
        <ul>
            <li>Cut and Paste the URL above and email it to someone</li>
            <li>Click on the link above and send that page to someone</li>
            <li>Go To Our
                <asp:HyperLink ID="HyperLinkSpreadTheWord" NavigateUrl="~/SpreadTheWord.aspx" runat="server">Help Spread The Word</asp:HyperLink>
                Page and get more options</li>
        </ul>
        <p>
            You can cut and paste the URL from the textbox above and email it to someone asking
            them to register
        </p>
    </div>
    <h2>
        <a href="SessionInterestChart.aspx">Pick the Sessions You Are Interested in Seeing</a>
    </h2>
    <div id="DivSpeaker" runat="server">
        <h2>
            Speaker Session</h2>
        <p>
            You have indicated you would like to present at CodeCamp. You can add your presentation
            at anytime by going to the PROGRAM page and press the link below the Program 
            line named &quot;Submit a Session&quot;.&nbsp; That is, in the submenu of Program.&nbsp; You must be logged in to submit a session 
            and sessions submissions must be open.&nbsp; Typically, session submissions are 
            open a few months before code camp and up until about 30 days before.
            You can also click on the URL below and go directly to adding a session
        </p>
        <p>
            <a href="SubmitSession.aspx">Submit A Presentation</a></p>
       
    </div>
    </div>

</asp:Content>
