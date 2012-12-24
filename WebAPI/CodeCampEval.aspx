<%@ Page Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" Inherits="CodeCampEval"
    Title="Code Camp Evaluation" Codebehind="CodeCampEval.aspx.cs" %>
<asp:Content ID="SublinksSessions" ContentPlaceHolderID="blankSublinks" runat="server">
    <asp:Menu ID="subMenu" runat="server" DataSourceID="SiteMapProgram" SkinID="subMenu">
    </asp:Menu>
    <%--The next line should be here, but seems to work without because it is in master page--%>
    <asp:SiteMapDataSource ID="SiteMapProgram" runat="server" ShowStartingNode="False"
        StartingNodeUrl="~/Program.aspx" />
</asp:Content>




<asp:Content ID="Content1" ContentPlaceHolderID="blankContent" runat="Server">
    <asp:CheckBox ID="CheckBoxAnswersMode" Visible='<%# (bool) IsRoleSurveyViewerOrAdmin() %>'
     AutoPostBack="true" runat="server" 
     Text="Toggle to Answers Mode (Survey Viewer Role Enabled)"  />

    <p>
        Please answer the following questions as honestly as you can. 
        None of the answers are required so feel free to answer just what you want.</p>
    <br />
    <asp:Button ID="ButtonUpdate1" runat="server" Text="Submit Evaluation" Enabled="False"
        OnClick="ButtonUpdate1_Click" />
    <asp:Label ID="Label1" runat="server"></asp:Label><br />
    <br />
    <table border="0" bgcolor="#ccffff" cellpadding="3" width="500">
        <tr runat="server" visible="false">
            <td style="float: right">
                I Attended Code Camp Only
            </td>
            <td>
                <asp:CheckBox ID="CheckBoxCodeCampOnly" runat="server" Visible="<%# (bool) IsModeSurvey() %>" />
                <asp:Literal runat="server" ID="CheckBoxCodeCampOnlyLiteral"
                Text='<%# (string) GetCheckBoxResults("AttendedCCOnly") %>' Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td style="float: right">
                I Attended Code Camp and Vista Fair
            </td>
            <td>
                <asp:CheckBox ID="CheckBoxCodeCampAndVistaFair" runat="server" Visible="<%# (bool) IsModeSurvey() %>" />
                <asp:Literal runat="server" ID="Literal1"
                  Text='<%# (string) GetCheckBoxResults("AttendedVistaFairAndCC") %>' Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>
                
            </td>
        </tr>
        <tr runat="server" visible="false">
            <td style="float: right">
                I Attended Vista Fair Only
            </td>
            <td>
                <asp:CheckBox ID="CheckBoxVistaFairOnly" runat="server" Visible="<%# (bool) IsModeSurvey() %>"  />
                <asp:Literal runat="server" ID="Literal2"
                Text='<%# (string) GetCheckBoxResults("AttendedVistaFairOnly") %>' Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>
            </td>
        </tr>
        <tr>
            <td style="float: right;" colspan="2">
                <br />
                
                <asp:RadioButtonList   ID="RadioButtonListSponsorship" Visible="<%# (bool) IsModeSurvey() %>" runat="server" OnSelectedIndexChanged="RadioButtonListSponsorship_SelectedIndexChanged">
                   <%-- <asp:ListItem Value="1"></asp:ListItem>--%>
                    <asp:ListItem Value="1">I Likely Will Not Ever Take a Class At Foothill College
                    </asp:ListItem>
                    <asp:ListItem Value="2">I Have Taken Classes Before Code Camp and Will Continue Taking Them
                    </asp:ListItem>
                    <asp:ListItem Value="3">I Plan On Taking Classes At Foothill College Based on My Experience At Code Camp</asp:ListItem>
					<asp:ListItem Value="4">N/A</asp:ListItem>
                    <asp:ListItem Value="0">N/A</asp:ListItem>
                </asp:RadioButtonList>
                <asp:Literal runat="server" ID="Literal8"
                Text="Sponsorship Question: " 
                Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>
                <asp:Literal runat="server" ID="Literal5"
                Text='<%# (string) GetRBLResults("RatherNoSponsorAndNoFreeFood") %>' 
                Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>
                <br />
            </td>
                
        </tr>
        
        <tr>
        <td colspan="2">
        This Code Camp Met My Expectations
        <br />
        <span class="CodeCampEval"><asp:RadioButtonList ID="RBLMetExpectations" Visible="<%# (bool) IsModeSurvey() %>" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
                <asp:Literal runat="server" ID="Literal3"
                Text='<%# (string) GetRBLResults("MetExpectations") %>' 
                Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal></span>
        </td>
        </tr>
        
        <tr>
            <td  colspan="2">
                I will likely attend Code Camp Again
                <br />
                <span class="CodeCampEval"><asp:RadioButtonList ID="RBLWillAttendAgain" Visible="<%# (bool) IsModeSurvey() %>" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
                <asp:Literal runat="server" ID="Literal4"
                Text='<%# (string) GetRBLResults("PlanToAttendAgain") %>' 
                Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal></span>
            </td>
        </tr>
        <tr>
            <td  colspan="2">
                I enjoyed the free food at Code Camp<br />
                <span class="CodeCampEval"><asp:RadioButtonList ID="RBLEnjoyedFreeFood" Visible="<%# (bool) IsModeSurvey() %>" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
                <asp:Literal runat="server" ID="Literal6"
                Text='<%# (string) GetRBLResults("EnjoyedFreeFood") %>' 
                Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal></span>
            </td>
        </tr>
        <tr>
           
            <td  colspan="2">
                The Sessions were varied enough<br />
                <span class="CodeCampEval"><asp:RadioButtonList ID="RBLSessionsVariedEnough" Visible="<%# (bool) IsModeSurvey() %>" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
                <asp:Literal runat="server" ID="Literal7"
                  Text='<%# (string) GetRBLResults("SessionsVariedEnough") %>'  Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>
                </span>
            </td>
        </tr>
        <tr>
            
            <td  colspan="2">
                There Were Enough Sessions At My Level<br />
                <span class="CodeCampEval">
                <asp:RadioButtonList ID="RBLEnoughSessionsMyLevel" Visible="<%# (bool) IsModeSurvey() %>" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
                <asp:Literal runat="server" ID="Literal9"
                  Text='<%# (string) GetRBLResults("EnoughSessionsAtMyLevel") %>'  Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>
                </span>
            </td>
        </tr>
        <tr>
            
            <td  colspan="2">
            Foothill was a good location for Code Camp<br />
                <span class="CodeCampEval"><asp:RadioButtonList ID="RBLFoothillGoodVenue" Visible="<%# (bool) IsModeSurvey() %>" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
                <asp:Literal runat="server" ID="Literal10"
                  Text='<%# (string) GetRBLResults("FoothillGoodVenue") %>'  Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>
                </span>
            </td>
        </tr>
        <tr>
           
            <td  colspan="2">
                I wish I had told more friends about Code Camp<br />
                <span class="CodeCampEval"><asp:RadioButtonList ID="RBLWishToldMoreFriends" Visible="<%# (bool) IsModeSurvey() %>" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
                <asp:Literal runat="server" ID="Literal11"
                  Text='<%# (string) GetRBLResults("WishToldMoreFriends") %>'  Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>
                </span>
            </td>
        </tr>
        <tr>
           
            <td  colspan="2">
                The Event was well planned<br />
                <span class="CodeCampEval"><asp:RadioButtonList ID="RBLEventWellPlanned" Visible="<%# (bool) IsModeSurvey() %>" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
                <asp:Literal runat="server" ID="Literal12"
                  Text='<%# (string) GetRBLResults("EventWellPlanned") %>'  Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>
                </span>
            </td>
        </tr>
        <tr>
            
            <td  colspan="2">
             Wireless Internet Access Was Very Important to me<br />
                <span class="CodeCampEval"><asp:RadioButtonList ID="RBLWirelessInternetImportant" Visible="<%# (bool) IsModeSurvey() %>" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
                <asp:Literal runat="server" ID="Literal13"
                  Text='<%# (string) GetRBLResults("WirelessAccessImportant") %>'  Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>
                </span>
            </td>
        </tr>
        <tr>
           
            <td  colspan="2">
            I Liked Having Special Tracks Setup<br />
                <span class="CodeCampEval"><asp:RadioButtonList ID="RBLWiredInternetImportant" Visible="<%# (bool) IsModeSurvey() %>" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
                <asp:Literal runat="server" ID="Literal14"
                  Text='<%# (string) GetRBLResults("WiredAccessImportant") %>'  Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>
                </span>
            </td>
        </tr>
        <tr>
          
            <td  colspan="2">
           I <b>Have</b> Taken Classes At Foothill College Based On Learning About the College Through Code Camp<br />
                <span class="CodeCampEval"><asp:RadioButtonList ID="RBLLikedEmailUpdates" Visible="<%# (bool) IsModeSurvey() %>" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
                <asp:Literal runat="server" ID="Literal15"
                  Text='<%# (string) GetRBLResults("LikeReceivingUpdateByEmail") %>'  Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>
                </span>
            </td>
        </tr>
        <tr>
           
            <td  colspan="2">
            I <i>Plan On</i> Taking Classes At Foothill College Based Learning About the College Through Code Camp<br />
                <span class="CodeCampEval"><asp:RadioButtonList ID="RBLLikedRSSUpdates" Visible="<%# (bool) IsModeSurvey() %>" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
                <asp:Literal runat="server" ID="Literal16"
                  Text='<%# (string) GetRBLResults("LikeReceivingUpdateByByRSSFeed") %>'  Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>
                </span>
            </td>
        </tr>
    </table>
    <table width="500" border="0" bgcolor="#ccffff" cellpadding="3">
        <tr>
            <td colspan="2" style="float: right">
                <br />
                <p>
                    What did you enjoy about the event?</p>
            </td>
        </tr>
        <tr>
            <td style="float: right;" colspan="2">
                <asp:TextBox ID="TextBoxWhatEnjoyMost" TextMode="MultiLine" Width="450px" Height="80"
                    runat="server"></asp:TextBox>
                    
            </td>
        </tr>
        <tr>
            <td colspan="2" style="float: right">
                <br />
                <p>
                    What changes do you think could be made to make the event better?</p>
            </td>
        </tr>
        <tr>
            <td style="float: right;" colspan="2">
                <asp:TextBox ID="TextBoxWhatChanges" TextMode="MultiLine" Width="450px" Height="80"
                    runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="float: right">
                <br />
                <p>
                    Are there any courses you would like to see offered at Foothill College that currently
                    do not exist?</p>
            </td>
        </tr>
        <tr>
            <td style="float: right;" colspan="2">
                <asp:TextBox ID="TextBoxWhatFoothillCoursesAdded" TextMode="MultiLine" Width="450px"
                    Height="80" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="float: right">
                <br />
                <p>
                    If you were not fully satisfied by any of the above items, can you please explain
                    why?</p>
            </td>
        </tr>
        <tr>
            <td style="float: right;" colspan="2">
                <asp:TextBox ID="TextBoxIfNotSatisfiedWhy" TextMode="MultiLine" Width="450px" Height="80"
                    runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="float: right">
                <br />
                <p>
                    And finally - This is an all volunteer event. Would you be interested in helping
                    out in the future? Please check all areas in which you would be interested in helping
                    (You can always change later, this is just to give us an idea of what you might
                    be interested in helping with):</p>
            </td>
        </tr>
        <tr>
            <td style="float: right;" colspan="2">
                <ul>
                    <li>
                        <asp:CheckBox ID="CheckBoxLongTermPlanning"  Text="Long Term Planning" runat="server" />
                        <asp:Literal runat="server" ID="Literal17"
                          Text='<%# (string) GetCheckBoxResults("InteresteInLongTermPlanning") %>' Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>

                    </li>
                    <li>
                        <asp:CheckBox ID="CheckBoxWebSiteBackEnd"  Text="Web Site Back End" runat="server" />
                    <asp:Literal runat="server" ID="Literal18"
                          Text='<%# (string) GetCheckBoxResults("InteresteInWebBackEnd") %>' Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>

                    </li>
                    <li>
                        <asp:CheckBox ID="CheckBoxWebSiteCss"  Text="Web Site Style (UI,CSS, etc.)" runat="server" />
                    <asp:Literal runat="server" ID="Literal19"
                          Text='<%# (string) GetCheckBoxResults("InterestedInWebFrontEnd") %>' Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>

                    </li>
                    <li>
                        <asp:CheckBox ID="CheckBoxSessionReview"  Text="Session Review Panel" runat="server" />
                    <asp:Literal runat="server" ID="Literal20"
                          Text='<%# (string) GetCheckBoxResults("InteresteInLongSessionReviewPanel") %>' Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>

                    </li>
                    <li>
                        <asp:CheckBox ID="CheckBoxContributorSolicitation"  Text="Contributor Solicitation"
                            runat="server" />
                    <asp:Literal runat="server" ID="Literal21"
                          Text='<%# (string) GetCheckBoxResults("InteresteInContributorSolicitation") %>' Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>

                    </li>
                    <li>
                        <asp:CheckBox ID="CheckBoxActAsContributor"  Text="Be a Contributor" runat="server" />
                    <asp:Literal runat="server" ID="Literal22"
                          Text='<%# (string) GetCheckBoxResults("InteresteInBeingContributor") %>' Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>

                    </li>
                    <li>
                        <asp:CheckBox ID="CheckBoxPreEventVolunteer"  Text="Volunteer Before Event (Badges, food, signs, etc.)"
                            runat="server" />
                            <asp:Literal runat="server" ID="Literal23"
                          Text='<%# (string) GetCheckBoxResults("InteresteInBeforeEvent") %>' Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>

                    </li>
                    <li>
                        <asp:CheckBox ID="CheckBoxDayOfEventVolunteer"  Text="Day of Event (Registration, etc.)"
                            runat="server" />
                            <asp:Literal runat="server" ID="Literal24"
                          Text='<%# (string) GetCheckBoxResults("InteresteInDayOfEvent") %>' Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>

                    </li>
                    <li>
                        <asp:CheckBox ID="CheckBoxEventTearDown"  Text="Help With Event Tear Down" runat="server" />
                    <asp:Literal runat="server" ID="Literal25"
                          Text='<%# (string) GetCheckBoxResults("InteresteInEventTearDown") %>' Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>

                    </li>
                    <li>
                        <asp:CheckBox ID="CheckBoxAfterEventVolunteer"  Text="After Event (Surveys, etc.)"
                            runat="server" />
                            <asp:Literal runat="server" ID="Literal26"
                          Text='<%# (string) GetCheckBoxResults("InteresteInAfterEvent") %>' Visible="<%# (bool) !IsModeSurvey() %>" ></asp:Literal>

                    </li>
                </ul>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="float: right">
                <br />
                <p>
                    If you would like to volunteer, what would be the best way to contact you:</p>
            </td>
        </tr>
    </table>
    <table width="500" border="0" bgcolor="#ccffff" cellpadding="3">
        <tr>
        <tr>
            <td>
                &nbsp; &nbsp;&nbsp; Contact Email:
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="TextBoxContactEmail" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp; &nbsp;&nbsp; Contact Phone Number:
            </td>
            <td>
                <asp:TextBox ID="TextBoxPhoneNumber" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <asp:Button ID="ButtonUpdate2" runat="server" Text="Submit Evaluation" Enabled="False"
        OnClick="ButtonUpdate2_Click" />
    <asp:Label ID="Label2" runat="server"></asp:Label><br />
</asp:Content>
