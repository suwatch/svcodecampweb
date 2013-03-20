<%@ Page Language="C#" MasterPageFile="~/RightNote.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" Inherits="Default"
    Title="Silicon Valley Code Camp Home Page" Codebehind="Default.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<%@ Register Src="RegistrationCount.ascx" TagName="RegistrationCount" TagPrefix="uc1" %>--%>
<%@ Register Src="TwitterFeed.ascx" TagName="TwitterFeed" TagPrefix="uc2" %>
<%@ Register Src="~/SessionTweetList.ascx" TagPrefix="uc1" TagName="SessionTweetList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <div id="SteveJobsMemorialId" runat="server">
        <div id="Div1" class="mainHeading" runat="server">
            In Memory Of Steve Jobs&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
        </div>
        <div style="text-align: center; margin-left: 50px; margin-right: 50px; margin-top: 25px">
            <img src="Images/stevejobs.jpg" alt="In Memory of Steve Jobs" style="border: none;" />
            <p align="center" style="font-size: large; clip: rect(25px, 50px, auto, 50px);">
                Sunday Morning, at 9:15 Lino Tadros Will Honor Steve Jobs Memory with the talk
                    "In Memory of Steve Jobs, the Apple Story".
            </p>
            <p style="text-align: center">
                <asp:HyperLink ID="HyperLinkTheAppleStory" NavigateUrl="~/2011/TheAppleStory" runat="server">(Location: Appreciation Hall)</asp:HyperLink>
            </p>
            <p style="text-align: center">
                &nbsp;
            </p>
        </div>
    </div>
    <div id="NormalPageId" runat="server">
        <div class="contentDivider" id="jobsMemorialLogout" runat="server" style="float: right; margin-left: 15px; padding: 0 0 8px 15px; background: #FFFFFF url('/App_Themes/Gray2011/Images/content-divider.png') no-repeat; width: 255px; height: 290px;">
            <a href="/2011/TheAppleStory">
                <img src="Images/stevejobs-tribute.jpg" style="border: none;" /></a>
            <br />
            <br />
            <strong>In Memory of Steve Jobs</strong><br />
            <p>
                Sunday Morning, at 9:15 Lino Tadros Will Honor Steve Jobs Memory with the talk "In Memory of Steve Jobs, the Apple Story".
                <a href="/2011/TheAppleStory">(Location: Appreciation Hall)</a>
            </p>
        </div>
        <div class="mainHeading" runat="server">
            Welcome!&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="LabelStatus" runat="server" Text=""></asp:Label>
        </div>
        
        
            <div runat="server" visible="false" id="ShowTwitterSessionLinksId" class="pad ShowTwitterSessionLinksId">

                <div class="title">
                    <h1>Retweet Sessions You Plan On Attending or Have Interest In*</h1><div class="twitter"></div>
                    <span>&nbsp;(Click on the links below from your personal Schedule)</span>
                </div>

                 <div class="tweetPanel">
                    <div><h2>Plan To Attend Sessions</h2></div>
                     <uc1:SessionTweetList runat="server" ID="SessionTweetListPlanToAttend" ShowInterested="false" ShowPlanToAttend="true" />
                 </div>

                 <div class="tweetPanel">
                     <div><h2>Sessions Interested In</h2></div>
                     <uc1:SessionTweetList runat="server" ID="SessionTweetListShowInterest" ShowInterested="true" ShowPlanToAttend="false" />
                 </div>

                <p class="note">* As long as you are logged into your twitter account, the tweets will come from you.  No tweets will be sent until you confirm the details of the tweet in your own browser. We are not tweeting on your
                    behalf and have no access to your twitter account.
                </p>
                <br />
            </div>

        
        

        <div runat="server" visible="true" id="CodeCampAnnounceID" class="pad">

            <p>
                Code Camp is a new type of community event where developers learn from fellow developers.
                We also have developer related topics that include software branding, legal issues
                around software as well as other topics developers are interested in hearing about.
                All are welcome to attend and speak.
            </p>
            <p>
                The Code Camp consists of these points:
            </p>
            <ul>
                <li>By and for the developer community</li>
                <li>Always free</li>
                <li>Never occur during working hours</li>
            </ul>
            <p>
                Sessions will range from informal chalk talks to presentations. There will be
                a mix of presenters, some experienced folks, for some it may be their first opportunity
                to speak in public. And we are expecting to see people from throughout the Northern
                California region and beyond.
            </p>
        </div>
        <div style="clear: both"></div>




        <div class="horizontalDivider" runat="server" id="horizontalDivider" style="background: #FFFFFF url('/App_Themes/Gray2011/Images/hr-background.png') repeat-x; height: 3px; margin: 8px; clear: both;"></div>
        <%--  <h2>Previous Code Camp Attendance</h2>--%>

      
       <div>
            <div id="jobsMemorialLogin" runat="server" style="width: 260px; margin-left: 0; padding: 0 0 8px 15px; background: #FFFFFF url('/App_Themes/Gray2011/Images/content-divider.png') no-repeat;">
                <a href="/2011/TheAppleStory">
                    <img src="Images/stevejobs-tribute.jpg" style="border: none;" /></a>
                <br />
                <br />
                <strong>In Memory of Steve Jobs</strong><br />
                <p>
                    Sunday Morning, at 9:15 Lino Tadros Will Honor Steve Jobs Memory with the talk "In Memory of Steve Jobs, the Apple Story".
                    <a href="/2011/TheAppleStory">(Location: Appreciation Hall)</a>
                </p>
                <div class="clear: both"></div>
            </div>
           
           
             <%--<div style="width: 50%; float: right;">--%>
                 <div class="pad" visible="False" runat="server" id="ShowChartWithAttendeesCountsID">
                    <asp:HyperLink ID="HyperLink2" NavigateUrl="~/RegistrationCount.aspx" Font-Bold="true"
                        runat="server">Current Registration Count to Date Versus Last Year</asp:HyperLink>
                  <%--  <uc1:RegistrationCount ID="RegistrationCount2" runat="server" />--%>
                    <br />
                    <i id="I1" runat="server">(Yellow: Last Year Blue: This Year)</i>
                    <br />
                    <i id="I2" runat="server">(X-Axis: Days Before Camp)</i>
                </div>
           <%-- </div>--%>

            <div class="pad">
                <b>Previous Code Camp Attendance</b>
                <table class="tablePreviousAttendance">
                    <thead>
                        <tr>
                            <td>Past Year
                            </td>
                            <td>Sessions
                            </td>
                            <td>Registered
                            </td>
                            <td>Attended
                            </td>
                        </tr>
                    </thead>
                    
                     <tr>
                        <td class="year">2012
                        </td>
                        <td>213
                        </td>
                        <td>3825
                        </td>
                        <td>2448
                        </td>
                    </tr>

                    <tr>
                        <td class="year">2011
                        </td>
                        <td>209
                        </td>
                        <td>3416
                        </td>
                        <td>2247
                        </td>
                    </tr>
                    <tr>
                        <td class="year">2010
                        </td>
                        <td>193
                        </td>
                        <td>3070
                        </td>
                        <td>1876
                        </td>
                    </tr>
                    <tr>
                        <td class="year">2009
                        </td>
                        <td>149
                        </td>
                        <td>1887
                        </td>
                        <td>1026
                        </td>
                    </tr>
                    <tr>
                        <td class="year">2008
                        </td>
                        <td>113
                        </td>
                        <td>1406
                        </td>
                        <td>785
                        </td>
                    </tr>
                    <tr>
                        <td class="year">2007
                        </td>
                        <td>75
                        </td>
                        <td>1070
                        </td>
                        <td>575
                        </td>
                    </tr>
                    <tr>
                        <td class="year">2006
                        </td>
                        <td>55
                        </td>
                        <td>866
                        </td>
                        <td>377
                        </td>
                    </tr>
                </table>
            </div>

            <div class="mainHeading">Code Camp Venue</div>
            <div class="pad">
                <p>
                    <strong>Foothill College<br />
                        12345 El Monte Road<br />
                        Los Altos Hills, CA 94022<br />
                    </strong>
                    <br />
                    <a href="http://maps.google.com/maps?f=q&source=s_q&hl=en&geocode=&q=Foothill+College+12345+El+Monte+Road+Los+Altos+Hills,+CA+94022&sll=37.0625,-95.677068&sspn=41.903538,107.138672&ie=UTF8&hq=Foothill+College&hnear=12345+El+Monte+Rd,+Los+Altos+Hills,+Santa+Clara,+California+94022&z=16"
                        target="_blank">Check the Map here</a>
                    <p>
                        For More Information Contact <a href="mailto:&#105;&#110;&#102;&#111;&#064;&#115;&#105;&#108;&#105;&#099;&#111;&#110;&#118;&#097;&#108;&#108;&#101;&#121;&#045;&#099;&#111;&#100;&#101;&#099;&#097;&#109;&#112;&#046;&#099;&#111;&#109;"
                            target="_blank">&#105;&#110;&#102;&#111;&#064;&#115;&#105;&#108;&#105;&#099;&#111;&#110;&#118;&#097;&#108;&#108;&#101;&#121;&#045;&#099;&#111;&#100;&#101;&#099;&#097;&#109;&#112;&#046;&#099;&#111;&#109;</a>
                    </p>
                </p>
                <br />
            </div>

          

            <div runat="server" id="IDMAPSTUFF" visible="false">
                <p>
                    Below is a map showing everyone registered who put in their zip codes. The speakers
                        are in Orange and the attendees are in blue (hover over the speakers to see their
                        pictures). If multiple people are from the same zip code, they show up as one circle.
                        Later, using PushPin Clustering which was just released in Virtual Earth 6.2, we
                        will improve the functionality.
                </p>
                <asp:CheckBox ID="CheckBoxShowAttendees" onclick="clicked_RedisplayAttendees();"
                    runat="server" AutoPostBack="false" Text="Show Attendees" Checked="false" />
                <asp:CheckBox ID="CheckBoxShowSpeakers" onclick="clicked_RedisplaySpeakers();" runat="server"
                    AutoPostBack="false" Text="Show Speakers" Checked="true" />
                <br />
                <div id='myMap' style="position: relative; width: 400px; height: 400px;">
                </div>
            </div>
        </div>


        <div style="width: 50%; float: left;">
            <uc2:TwitterFeed ID="TwitterFeed2" MaxTweetsToShow="15" ShowTopArea="false" runat="server" />
            <uc2:TwitterFeed ID="TwitterFeed1" MaxTweetsToShow="25" ShowTopArea="true" runat="server" />
        </div>


    </div>
</asp:Content>
