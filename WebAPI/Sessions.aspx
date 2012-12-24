<%@ Page Language="C#" MasterPageFile="~/RightNote.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" Inherits="Sessions" Codebehind="Sessions.aspx.cs" %>




<%@ Register Assembly="App_Code" Namespace="CodeCampSV" TagPrefix="RBUTIL" %>

<asp:Content ID="SublinksSessions" ContentPlaceHolderID="Sublinks" runat="server">

    <script type="text/javascript" src="JSProd/resources/extjs/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="JSProd/resources/extjs/ext-all.js"></script>
    <script type="text/javascript" src="JSProd/resources/ux/extensions/FileUploadField.js"></script>

    <%-- <link href="JSProd/resources/extjs/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
 <script src="JSProd/resources/extjs/adapter/ext/ext-base.js" type="text/javascript"></script>
 <script src="JSProd/resources/extjs/ext-all.js" type="text/javascript"> </script>--%>

    <asp:Menu ID="subMenu" runat="server" DataSourceID="SiteMapProgram" SkinID="subMenu">
    </asp:Menu>
    <%--The next line should be here, but seems to work without because it is in master page--%>
    <asp:SiteMapDataSource ID="SiteMapProgram" runat="server" ShowStartingNode="False"
        StartingNodeUrl="~/Program.aspx" />

</asp:Content>

<asp:Content ID="SessionsContent" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

        Ext.BLANK_IMAGE_URL = 'JSProd/resources/extjs/resources/images/default/s.gif';
        function MakeExclusiveCheck(rbcntrl, sessionId, username, buttonName, choiceNumber) {

            //alert('sessionId: ' + sessionId + ' username: ' + username + ' buttonname: ' + buttonName);

            // below done with Help of Kevin 
            var sessionRadio = rbcntrl;  //'ctl00_ctl00_ctl00_blankContent_parentContent_MainContent_Repeater1_ctl139_rbClick3';

            var timeId = Ext.getDom(sessionRadio).parentNode.className, input;

            Ext.select('span.' + timeId).each(function (el, c, i) {
                if (i % 3 === 2) {
                    input = el.dom.childNodes[0];
                    input.checked = input.id === sessionRadio;
                }
            });

            var reProcess = new Ext.data.Connection({
                listeners: {
                }
            });

            reProcess.request({
                url: "SessionInterest.ashx",
                method: "POST",

                success: function (r, o) {
                    var responseVal = Ext.decode(r.responseText, true);
                    if (responseVal.success == true) {
                        //Ext.Msg.alert("Success ! " + responseVal.msg);
                    }
                    else {
                        Ext.Msg.show({
                            title: "Exception",
                            icon: Ext.MessageBox.ERROR,
                            msg: responseVal.msg,
                            buttons: Ext.Msg.OK
                        });
                    }
                },
                failure: function (r, o) {
                    Ext.Msg.show({
                        title: "Exception",
                        icon: Ext.MessageBox.ERROR,
                        msg: "An Error has occurred. :(",
                        buttons: Ext.Msg.OK
                    });
                },
                params: {
                    SessionId: sessionId,
                    ButtonName: buttonName,
                    UserName: username,
                    ChoiceNumber: choiceNumber
                }
            });
        }

    </script>
    <script src="JavaScript/jquery-1.3.2.min.js" type="text/javascript" charset="utf-8"></script>
    <link rel="stylesheet" href="JavaScript/prettyPhoto/css/prettyPhoto.css" type="text/css" media="screen" charset="utf-8" />
    <script src="JavaScript/prettyPhoto/js/jquery.prettyPhoto.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("a[rel^='prettyPhoto']").prettyPhoto({
                default_width: 500,
                default_height: 344,
                theme: 'light_square'
            });
        });
    </script>
    <div class="searchSessions" runat="server" id="MainHeadingDIV" visible="true">

        <%--per this thread by Josh Stodola.  hopefully causes enter to hit text search textobx --%>
        <asp:TextBox ID="TextBox2" runat="server" Style="display: none; visibility: hidden;"></asp:TextBox>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="ButtonSearch" runat="server" OnClick="ButtonSearch_Click" />
    </div>
    <h1 class="mainHeading">
        <asp:Literal ID="LiteralTitle" Text="Sessions" runat="server"></asp:Literal>
    </h1>

    <div class="sessionsList">
        <div runat="server" id="SessionsDIV" visible="false">
            <asp:Image ID="IDNov8" SkinID="SessionNov8On" runat="server" />
            <asp:Image ID="IDNov9" SkinID="SessionNov9Off" runat="server" />
        </div>




        <div id="SessionsUsingExtJSFromDOMId" visible="false" runat="server" enableviewstate="false">
            <asp:Literal ID="HtmlForSessionsId" runat="server" EnableViewState="false"></asp:Literal>
        </div>


        <!-- Sessions heading -->
        <div id="MainHeadingDivSatOrSunday">

            <asp:RadioButtonList ID="RadioButtonSatOrSundayList" runat="server" AutoPostBack="true"
                RepeatDirection="Horizontal" Font-Bold="True" Font-Size="X-Large"
                OnSelectedIndexChanged="RadioButtonSatOrSundayList_SelectedIndexChanged">
                <asp:ListItem Selected="True">Saturday</asp:ListItem>
                <asp:ListItem>Sunday</asp:ListItem>
                <asp:ListItem>All</asp:ListItem>
            </asp:RadioButtonList>

            <asp:RadioButtonList ID="RadioButtonListTimes" runat="server" AutoPostBack="true"
                RepeatDirection="Horizontal" Font-Bold="True" Font-Size="X-Large" OnSelectedIndexChanged="RadioButtonListTimes_SelectedIndexChanged">
            </asp:RadioButtonList>
        </div>

        <!-- Sort sessions -->
        <div class="pad" runat="server" id="IdTrackDescription" visible="true">
            <div class="grayBackground">
                <p>
                    <asp:Literal ID="LiteralTrackDescription" Text="Sessions" runat="server"></asp:Literal>
                </p>
            </div>
        </div>


        <!-- Sort sessions -->
        <div class="pad" runat="server" id="IdSortBy" visible="false">
            <div class="grayBackground">
                Sort by: &nbsp;&nbsp;
                    <asp:DropDownList ID="DropDownListSessionSortBy" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="DropDownListSessionSortBy_SelectedIndexChanged">
                    </asp:DropDownList>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
                    <asp:CheckBox ID="CheckBoxHideDescriptions" AutoPostBack="true" runat="server" Text="Hide Descriptions"
                        OnCheckedChanged="CheckBoxHideDescriptions_CheckedChanged" />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
                    <asp:CheckBox ID="CheckBoxHideCloud" AutoPostBack="true" runat="server" Text="Hide Cloud Tags"
                        Visible="false" OnCheckedChanged="CheckBoxHideCloud_CheckedChanged" />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
                     <asp:CheckBox ID="CheckBoxJustSessionsWithVideo" AutoPostBack="true" runat="server" Text="Sessions With Video"
                         Visible="false" OnCheckedChanged="CheckBoxSessionsWithVideo_CheckedChanged" />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
                    <asp:CheckBox ID="CheckBoxShowOnlyAssigned" AutoPostBack="true" runat="server" Text="Show Only Assigned"
                        OnCheckedChanged="CheckBoxShowOnlyAssigned_CheckedChanged" />


            </div>
        </div>

        <!-- Begin container for session listings -->
        <div class="sessionsContainer" id="SessionsContainerId" runat="server">
            <div runat="server" id="DivSessionList">
                <asp:Repeater ID="Repeater1" DataSourceID="ObjectDataSourceSessions" OnItemDataBound="Repeater1_ItemDataBound"
                    runat="server" OnItemCommand="Repeater1_ItemCommand"
                    EnableViewState="true" OnItemCreated="Repeater1_ItemCreated">
                    <HeaderTemplate>
                        <div class="page-break"></div>
                        <div>
                    </HeaderTemplate>
                    <FooterTemplate>
                        </div>
                    </FooterTemplate>
                    <ItemTemplate>
                        <div class="pad">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="sessionContent">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td>
                                                    <div class="title">
                                                        <h2>
                                                            <asp:Literal ID="Label1" Visible="true" EnableViewState="false" runat="server" Text='<%# Eval("title").ToString().Trim()  %>'></asp:Literal></h2>
                                                    </div>
                                                </td>
                                                <td valign="middle" align="right">

                                                  <%--  <asp:HyperLink EnableViewState="false" runat="server" ID="WetPaintWikiID" Visible='<%# ShowWetPaintWiki((int) Eval("Id")) %>'
                                                        NavigateUrl='<%# "SessionWiki.aspx?SessionId=" + String.Format("{0}", Eval("id")) %>' CssClass="hidePrint"
                                                        Text="WetPaint Wiki"></asp:HyperLink>
                                                    <asp:LinkButton Visible='<%# ShowPBWiki((int) Eval("Id")) %>' ID="LinkButton1" runat="server" CssClass="hidePrint"
                                                        EnableViewState="true" Text="Wiki Here" CommandArgument='<%# "PostWiki^" + Eval("id") %>'></asp:LinkButton>--%>
                                                    
                                                    

                                                    <asp:LinkButton Enabled='<%# ShowMaterialsUrlLink((int) Eval("Id")) %>' ID="LinkButton2" runat="server" CssClass="hidePrint"
                                                        Text ='<%# GetTextForSlidesAndCode((int) Eval("Id")) %>' 
                                                        EnableViewState="true"  CommandArgument='<%# "MaterailsUrl^" + Eval("id") %>'></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td valign="top">
                                                    <div class="sessionName">
                                                        <span class="itemTitle">Speaker:</span>
                                                        <asp:Literal runat="server" Text='<%# GetAllSpeakersHtml((int) Eval("Id")) %>' ID="SessionSpeakersID"></asp:Literal>

                                                        <%-- <asp:HyperLink ID="HyperLink1" NavigateUrl='<%# string.Format("~/Speakers.aspx?id={0}", 
                                                                GetAttendeeAnySessionIdByUsername((string) Eval("Username"))) %>'
                                                                Text='<%# GetAttendeeNameByUsername(Eval("Username")) %>' 
                                                                runat="server" EnableViewState="true"> 
                                                            </asp:HyperLink>--%>
                                                    </div>
                                                    <div class="sessionLevel">
                                                    </div>
                                                    <div class="sessionDetails">
                                                        <span class="itemTitle">Level:</span>
                                                        <asp:Label ID="Label2" runat="server" EnableViewState="false" Text='<%# SessionLevelsDictionary[(int) Eval("SessionLevel_id")] %>'></asp:Label>
                                                        &nbsp; | &nbsp;
                                                            <asp:Label ID="Label5" EnableViewState="false" runat="server" Text='<%# GetRoomNumberFromRoomId((int) Eval("LectureRoomsId")) %>'></asp:Label></span> &nbsp; | &nbsp; 
                                                            <span class="sessionTime">
                                                                <asp:Label ID="Label3" EnableViewState="false" runat="server" Text='<%# GetAgendaDescriptionFromAgendaId((int) Eval("SessionTimesId")) %>'></asp:Label></span>
                                                        <span class="sessionRoom">
                                                            <asp:Label ID="LabelTrackSpacer" Visible='<%# GetHideTrackInfo() %>' EnableViewState="false" runat="server" Text='<%# GetTrackSpacerSessionId((int) Eval("Id")) %>'></asp:Label>
                                                            <asp:HyperLink ID="HyperLinkTrack" NavigateUrl='<%# "~/Sessions.aspx?track=" + GetTrackIdFromSessionId((int) Eval("Id"))  %>'
                                                                Visible='<%# GetHideTrackInfo() %>' Text='<%# GetTrackNameFromSessionId((int) Eval("Id")) %>'
                                                                EnableViewState="true" runat="server">
                                                            </asp:HyperLink>
                                                        </span>
                                                    </div>
                                                </td>
                                                <td valign="top" align="right"></td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr>
                                                <td valign="top">
                                                    <div class="sessionDescription">
                                                        <div id="Div3" runat="server" visible='<%# GetHideSessionDescription() %>'>
                                                            <asp:Label ID="Label6" Visible='<%# GetHideSessionDescription() %>' runat="server"
                                                                EnableViewState="false" Text='<%# CheckForValidHTML((string) Eval("description")) %>'>
                                                            </asp:Label>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td valign="top">
                                                    <div id="Div4" style="float: right;" runat="server" visible='<%# GetHideSessionDescription() %>'>
                                                        <asp:Image runat="server" ID="Image3" CssClass="sessionRoomPhoto" ImageUrl='<%# "~/DisplayImage.ashx?roomid=" + Eval("LectureRoomsId")  %>'
                                                            Visible='<%# IsRoomImageVisible() %>' EnableViewState="false" />
                                                        <asp:HyperLink ID="HyperLink2" NavigateUrl='<%# "~/SessionAssignPictures.aspx?sessionid=" + Eval("id")  %>'
                                                            Visible='<%# IsPictureHyperlinkForSessionVisible() %>' Text="Pictures From Class"
                                                            EnableViewState="true" CssClass="sessionRoomPhoto" runat="server">
                                                        </asp:HyperLink>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="sessionButtons">
                                            <asp:DropDownList ID="DropDownListTracks" runat="server" AutoPostBack="true" ToolTip='<%# Eval("id") %>'
                                                Visible='<%# GetAdminVisible() %>' OnSelectedIndexChanged="DropDownListTracks_SelectedIndexChanged">
                                            </asp:DropDownList>

                                            <asp:ImageButton ID="ImageButton1" EnableViewState="true" SkinID="ButtonEditSession" Visible='<%# GetEditButtonVisible((string) Eval("Username")) %>'
                                                runat="server" CommandArgument='<%# "Edit^" + Eval("id") %>' CssClass="hidePrint" />
                                            <asp:ImageButton ID="ButtonEmailSpeaker" EnableViewState="true" SkinID="ButtonEmailSpeaker"
                                                Visible='<%# GetEmailSpeakerVisible((string) Eval("Username"))   %>' CssClass="hidePrint" runat="server"
                                                CommandArgument='<%# "EmailSpeaker^" + Eval("id") %>' />
                                            <asp:ImageButton ID="Button3" EnableViewState="true" SkinID="ButtonDeleteSession"
                                                OnClientClick="return confirm('OK to Delete Session? (Session Will Appear For Several Minutes Until Database Cache Clears)');"
                                                Visible='<%# GetDeleteButtonVisible((string) Eval("Username")) %>' CssClass="hidePrint" runat="server"
                                                CommandArgument='<%# "Delete^" + Eval("id") %>' />
                                            <asp:ImageButton ID="Button4" EnableViewState="true" SkinID="ButtonReviewSession"
                                                Visible='<%# GetReviewEvaluationsButtonVisible((string) Eval("Username")) %>'
                                                runat="server" CommandArgument='<%# "Evaluations^" + Eval("id") %>' CssClass="hidePrint" />
                                            <asp:ImageButton ID="ImageButton2" EnableViewState="true" SkinID="ButtonEmailPlannedAndInterested"
                                                Visible='<%# GetEmailPlannedAndInterestedVisible((string) Eval("Username")) %>'
                                                runat="server" CommandArgument='<%# "Evaluations^" + Eval("id") %>' CssClass="hidePrint" />
                                            <asp:ImageButton ID="ImageButton3" EnableViewState="true" SkinID="ButtonAssignVideo"
                                                Visible='<%# GetAssignVideoButtonVisible((string) Eval("Username")) %>'
                                                runat="server" CommandArgument='<%# "AssignVideo^" + Eval("id") %>' CssClass="hidePrint" />
                                            <asp:Label ID="SessionNumberForAdminID" Text='<%# " SessionId:" + Eval("id")  %>'
                                                Visible='<%# GetIsAdmin() %>' runat="server"></asp:Label>
                                        </div>
                                        <div class="sessionEvaluation">
                                            <asp:HyperLink ID="HyperLink3" NavigateUrl='<%# "~/SessionEval.aspx?id=" + Eval("id") %>'
                                                EnableViewState="true" Text='<%# GetEvalTextForHyperlink( (int) Eval("id") ) %>'
                                                Visible='<%# ShowCourseEvaluation() %>' runat="server">
                                            </asp:HyperLink>
                                        </div>
                                        <div class="attendBottom">
                                            <asp:RadioButton ID="rbClick1" Checked='<%# IsCheckedNotInterested((int) Eval("Id")) %>' Enabled="<%# IsNotInterestedEnabled()             %>" Text=" Not Interested" runat="server" GroupName='<%# Eval("Id") %>' />
                                            <asp:RadioButton ID="rbClick2" ToolTip='<%# (string) GetCountOfSpeakerToAttendee("Interested",(int) Eval("Id")) %>'
                                                Checked='<%# IsCheckedInterested((int) Eval("Id")) %>' Enabled="<%# IsInterestedEnabled()                %>" Text='<%# (string) GetInterestedText((int) Eval("Id")) %>' runat="server" GroupName='<%# Eval("Id") %>' />
                                            <asp:RadioButton ID="rbClick3" ToolTip='<%# (string) GetCountOfSpeakerToAttendee("PlanToAttend",(int) Eval("Id")) %>'
                                                Checked='<%# IsCheckedPlanToAttend((int) Eval("Id")) %>' Enabled="<%# IsPlanToAttendInterestedEnabled()    %>" Text='<%# (string) GetWillAttendText((int) Eval("Id")) %>' runat="server" GroupName='<%# Eval("Id") %>' />

                                            <asp:HyperLink ID="HyperLinkMustRegister"
                                                Visible='<%# IsAuthenticatedByNotRegisteredForCurrentYear() %>'
                                                BackColor="Orange" ForeColor="Black"
                                                NavigateUrl="~/Register.aspx" runat="server">(MUST REGISTER TO SELECT INTEREST)</asp:HyperLink>


                                            <%-- <RBUTIL:RadioButtonListWithArg AutoPostBack="true" RepeatDirection="horizontal" CommandArg='<%# DataBinder.Eval(Container.DataItem, "id") %>'
                                                    ID="RadioButtonListWithArg1" runat="server" OnSelectedIndexChanged="RadioButtonListInterest_SelectedIndexChanged"
                                                    EnableViewState="true" Visible="true">
                                                </RBUTIL:RadioButtonListWithArg>--%>
                                        </div>
                                    </td>
                                    <td class="sessionTags hideOnScreenLessthan800px" valign="top">
                                        <div id="Div2" class="tags hideOnScreenLessthan800px" runat="server" visible='<%# GetHideSessionDescription() %>'>
                                            <asp:Repeater ID="RepeaterTagNamesBySession" EnableViewState="false" runat="server">
                                                <ItemTemplate>
                                                    <div class="tagBullet">
                                                        <asp:HyperLink ID="HyperLinkCategory" EnableViewState="true" NavigateUrl='<%# "~/Sessions.aspx?sortby=title&by=category&tag=" + Eval("tagid") %>'
                                                            Text='<%# Eval("tagName") %>' runat="server">
                                                        </asp:HyperLink>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <div>



                                            <asp:ImageButton ID="ImageButtonJobs" CssClass="JobImageOnSession"
                                                PostBackUrl='<%# GetJobsPostbackURL((int) Eval("Id")) %>'
                                                ToolTip='<%# GetJobsButtonToolTip((int) Eval("Id")) %>'
                                                AlternateText="Jobs" Visible='<%# GetJobsButtonVisible((int) Eval("Id")) %>'
                                                ImageUrl="~/Images/jobs-button.jpg" runat="server" />
                                            <br />
                                            <asp:Label ID="LabelJobs" runat="server" Visible='<%# GetJobsLabelVisible((int) Eval("Id")) %>'
                                                Text='<%# GetJobsLabelText((int) Eval("Id")) %>'></asp:Label>
                                            <br />
                                            <asp:Literal ID="VideoID" runat="server" Visible='<%# GetVideoLiteralVisible((int) Eval("Id")) %>'
                                                Text='<%# GetVideoLiteralText((int) Eval("Id")) %>'></asp:Literal>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div runat="server" id="SessionPageBreakId" class="page-break"></div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
        <%-- <asp:UpdatePanel ID="panel1" runat="server">
        <ContentTemplate>--%>
        <asp:Label ID="LabelStatus" runat="server"></asp:Label><br />
        <asp:DropDownList ID="DropDownListLevels" runat="server" DataSourceID="ObjectDataSourceLevels"
            DataTextField="description" DataValueField="id" Visible="False">
        </asp:DropDownList>
        <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
        <asp:ObjectDataSource ID="ObjectDataSourceSessions" runat="server" SelectMethod="GetAllByStartTime"
            TypeName="CodeCampSV.SessionsODS"
            OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:ControlParameter ControlID="LabelSessionListFinalToShow" Name="sessionListInts"
                    PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSourceSessionsOnlyAssigned" runat="server" SelectMethod="GetAllByStartTimeOnlyAssigned"
            TypeName="CodeCampSV.SessionsODS"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSourceBySessionPresenter" runat="server" SelectMethod="GetByPresenterLastName"
            TypeName="CodeCampSV.SessionsODS"
            OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:ControlParameter ControlID="LabelSessionListToShow" Name="sessionListInts"
                    PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSourceByPresenterGUID" runat="server" SelectMethod="GetByPKID"
            TypeName="CodeCampSV.SessionsODS">
            <SelectParameters>
                <asp:QueryStringParameter Name="userGuid" QueryStringField="id" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSourceBySessionId" runat="server" SelectMethod="GetByPrimaryKeySessions"
            TypeName="CodeCampSV.SessionsODS">
            <SelectParameters>
                <asp:QueryStringParameter Name="searchid" QueryStringField="id" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSourceBySessionIdOnly" runat="server" SelectMethod="GetByPrimaryKeySessions"
            TypeName="CodeCampSV.SessionsODS">
            <SelectParameters>
                <asp:QueryStringParameter Name="searchid" QueryStringField="sessionid" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSourceGetSessionsBySession" runat="server" SelectMethod="GetAllSessionsBySessionId"
            TypeName="CodeCampSV.SessionsODS">
            <SelectParameters>
                <asp:QueryStringParameter Name="sessionId" QueryStringField="id" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>

        <asp:ObjectDataSource ID="ObjectDataSourceGetSessionsByAttendeeId" runat="server" SelectMethod="GetAllSessionsByAttendeeId"
            TypeName="CodeCampSV.SessionsODS">
            <SelectParameters>
                <asp:QueryStringParameter Name="attendeeId" QueryStringField="AttendeeId" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>

        <asp:ObjectDataSource ID="ObjectDataSourceBySessionTitle" runat="server" SelectMethod="GetBySessionTitle"
            TypeName="CodeCampSV.SessionsODS"
            OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:ControlParameter ControlID="LabelSessionListToShow" Name="sessionListInts"
                    PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSourceSessionSubmitted" runat="server" SelectMethod="GetBySessionSubmittedDate"
            TypeName="CodeCampSV.SessionsODS"
            OnDataBinding="ObjectDataSourceSessionSubmitted_DataBinding"
            OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:ControlParameter ControlID="LabelSessionListToShow" Name="sessionListInts"
                    PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSourceByTag" runat="server" SelectMethod="GetByTag"
            TypeName="CodeCampSV.SessionsODS">
            <SelectParameters>
                <asp:QueryStringParameter Name="tagid" QueryStringField="tag" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>

        <asp:ObjectDataSource ID="ObjectDataSourceByTrack" runat="server" SelectMethod="GetByTrack"
            TypeName="CodeCampSV.SessionsODS">
            <SelectParameters>
                <asp:QueryStringParameter Name="trackid" QueryStringField="track" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>

        <asp:ObjectDataSource ID="ObjectDataSourceLevels" runat="server" CacheDuration="3600"
            EnableCaching="True" SelectMethod="GetData" TypeName="DataSetLevelsTableAdapters.SessionLevelsTableAdapter"></asp:ObjectDataSource>

    </div>

    <asp:Label ID="LabelSessionListToShow" runat="server" Text=""></asp:Label>
    <asp:Label ID="LabelSessionListFromSearch" runat="server" Text=""></asp:Label>
    <asp:Label ID="LabelSessionListFinalToShow" runat="server" Text=""></asp:Label>



</asp:Content>
