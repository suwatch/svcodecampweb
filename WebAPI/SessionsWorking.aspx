<%@ Page Language="C#" MasterPageFile="~/RightNote.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" Inherits="SessionsWorking" Codebehind="SessionsWorking.aspx.cs" %>

<%@ Register Assembly="App_Code" Namespace="CodeCampSV" TagPrefix="RBUTIL" %>

<asp:Content ID="SublinksSessions" ContentPlaceHolderID="Sublinks" runat="server">
 
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

        var timeId = Ext.getDom(sessionRadio).parentNode.className,input;

        Ext.select('span.' + timeId).each(function(el, c, i) {
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

            success: function(r, o) {
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
            failure: function(r, o) {
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

    <div class="mainHeading">
        <asp:Literal ID="LiteralTitle" Text="Sessions" runat="server"></asp:Literal>
    </div>
    
    <div class="sessionsList">
    <div runat="server" id="SessionsDIV" visible="false">
        <asp:Image ID="IDNov8" SkinID="SessionNov8On" runat="server" />
        <asp:Image ID="IDNov9" SkinID="SessionNov9Off" runat="server" />
    </div>
<%--    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
        
        
        
            <div id="SessionsUsingExtJSFromDOMId" visible="false" runat="server" enableviewstate="false" >
              <asp:Literal ID="HtmlForSessionsId" runat="server" EnableViewState="false" ></asp:Literal>
            </div>
        
        
            <!-- Sessions heading -->
            <div class="mainHeading" runat="server" id="MainHeadingDIV" visible="false">
               CodeCamp Sessions</div>
           
           
           
            <!-- Sort sessions -->
            <div class="pad" runat="server" id="IdTrackDescription" visible="true" >
                <div class="grayBackground">
                <p>
                     <asp:Literal ID="LiteralTrackDescription"  Text="Sessions" runat="server"></asp:Literal>
                </p>
                </div>
            </div>
            
              
            <!-- Sort sessions -->
            <div class="pad"  runat="server" id="IdSortBy" visible="false" >
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
                    <asp:CheckBox ID="CheckBoxShowOnlyAssigned" AutoPostBack="true" runat="server" Text="Show Only Assigned"
                OnCheckedChanged="CheckBoxShowOnlyAssigned_CheckedChanged" />
                </div>
            </div>
            
             <!-- Begin container for session listings -->
            <div class="sessionsContainer" id="SessionsContainerId" runat="server" >
                <div runat="server" id="DivSessionList">
                    <asp:Repeater ID="Repeater1" DataSourceID="ObjectDataSourceSessions" OnItemDataBound="Repeater1_ItemDataBound"
                        runat="server" OnItemCommand="Repeater1_ItemCommand" EnableViewState="true">
                        <HeaderTemplate>
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
                                                            <asp:Label ID="Label1" Visible="true" EnableViewState="false" runat="server" Text='<%# Eval("title").ToString().Trim()  %>'></asp:Label></div>
                                                    </td>
                                                    <td valign="middle" align="right">
                                                        <asp:HyperLink EnableViewState="false" runat="server" ID="WetPaintWikiID" Visible='<%# ShowWetPaintWiki((int) Eval("Id")) %>'
                                                            NavigateUrl='<%# "SessionWiki.aspx?SessionId=" + String.Format("{0}", Eval("id")) %>'
                                                            Text="WetPaint Wiki"></asp:HyperLink>
                                                        <asp:LinkButton Visible='<%# ShowPBWiki((int) Eval("Id")) %>' ID="LinkButton1" runat="server"
                                                            OnClick="LinkButtonWiki_Click" EnableViewState="true" Text="Wiki Here" CommandArgument='<%# "PostWiki^" + Eval("id") %>'></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td valign="top">
                                                        <div class="sessionName">
                                                            <span class="itemTitle">Speaker:</span> 
                                                            <asp:Literal runat="server" text='<%# GetAllSpeakersHtml((int) Eval("Id")) %>' ID="SessionSpeakersID" ></asp:Literal>
                                                            
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
                                                            <asp:Label ID="Label2" runat="server" EnableViewState="false" Text='<%# SessionLevelsDictionary[(int) Eval("SessionLevel_id")] %>'></asp:Label> &nbsp; | &nbsp;
                                                            <asp:Label ID="Label5" EnableViewState="false" runat="server" Text='<%# GetRoomNumberFromRoomId((int) Eval("LectureRoomsId")) %>'></asp:Label></span> &nbsp; | &nbsp; 
                                                            <span class="sessionTime"><asp:Label ID="Label3" EnableViewState="false" runat="server" Text='<%# GetAgendaDescriptionFromAgendaId((int) Eval("SessionTimesId")) %>'></asp:Label></span>
                                                            <span class="sessionRoom"><asp:Label ID="LabelTrackSpacer" Visible='<%# GetHideTrackInfo() %>' EnableViewState="false" runat="server" Text='<%# GetTrackSpacerSessionId((int) Eval("Id")) %>' ></asp:Label>
                                                            <asp:HyperLink ID="HyperLinkTrack" NavigateUrl='<%# "~/Sessions.aspx?track=" + GetTrackIdFromSessionId((int) Eval("Id"))  %>'
                                                                    Visible='<%# GetHideTrackInfo() %>' Text='<%# GetTrackNameFromSessionId((int) Eval("Id")) %>'
                                                                    EnableViewState="true"  runat="server">
                                                            </asp:HyperLink>
                                                            </span>
                                                        </div>
                                                    </td>
                                                    <td valign="top" align="right">
                                                        
                                                    </td>
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
                                                <asp:DropDownList ID="DropDownListTracks" runat="server" AutoPostBack="true"    ToolTip='<%# Eval("id") %>'
                                                    Visible='<%# GetAdminVisible() %>'  OnSelectedIndexChanged="DropDownListTracks_SelectedIndexChanged"    >
                                                </asp:DropDownList>
                                                            
                                                <asp:ImageButton ID="ImageButton1" EnableViewState="true" SkinID="ButtonEditSession" Visible='<%# GetDeleteOrEditButtonVisible((string) Eval("Username")) %>'
                                                    runat="server" CommandArgument='<%# "Edit^" + Eval("id") %>' />
                                                <asp:ImageButton ID="ButtonEmailSpeaker" EnableViewState="true" SkinID="ButtonEmailSpeaker"
                                                    Visible='<%# GetEmailSpeakerVisible((string) Eval("Username")) %>' runat="server"
                                                    CommandArgument='<%# "EmailSpeaker^" + Eval("id") %>' />
                                                <asp:ImageButton ID="Button3" EnableViewState="true" SkinID="ButtonDeleteSession"
                                                    OnClientClick="return confirm('OK to Delete Session? (Session Will Appear For Several Minutes Until Database Cache Clears)');"
                                                    Visible='<%# GetDeleteOrEditButtonVisible((string) Eval("Username")) %>' runat="server"
                                                    CommandArgument='<%# "Delete^" + Eval("id") %>' />
                                                <asp:ImageButton ID="Button4" EnableViewState="true" SkinID="ButtonReviewSession"
                                                        Visible='<%# GetReviewEvaluationsButtonVisible((string) Eval("Username")) %>'
                                                    runat="server" CommandArgument='<%# "Evaluations^" + Eval("id") %>' />
                                                 <asp:ImageButton ID="ImageButton2" EnableViewState="true" SkinID="ButtonEmailPlannedAndInterested"
                                                        Visible='<%# GetEmailPlannedAndInterestedVisible((string) Eval("Username")) %>'
                                                    runat="server" CommandArgument='<%# "Evaluations^" + Eval("id") %>' />
                                                <%--<asp:Label ID="Label7" EnableViewState="false" runat="server" Width="90" Text='<%# SessionLevelsDictionary[(int) Eval("SessionLevel_id")] %>'>
                                                </asp:Label>--%>
                                            </div>
                                            <div class="sessionEvaluation">
                                                <asp:HyperLink ID="HyperLink3" NavigateUrl='<%# "~/SessionEval.aspx?id=" + Eval("id") %>'
                                                    EnableViewState="true" Text='<%# GetEvalTextForHyperlink( (int) Eval("id") ) %>'
                                                    Visible='<%# ShowCourseEvaluation() %>' runat="server">
                                                </asp:HyperLink>
                                            </div>
                                            <div class="attendBottom" >
                                                    <asp:RadioButton ID="rbClick1"  Checked='<%# IsCheckedNotInterested((int) Eval("Id")) %>' Enabled="<%# IsNotInterestedEnabled()             %>" Text=" Not Interested" runat="server" GroupName='<%# Eval("Id") %>' />
                                                    <asp:RadioButton ID="rbClick2" ToolTip='<%# (string) GetCountOfSpeakerToAttendee("Interested",(int) Eval("Id")) %>'
                                                         Checked='<%# IsCheckedInterested((int) Eval("Id")) %>'    Enabled="<%# IsInterestedEnabled()                %>"  Text='<%# (string) GetInterestedText((int) Eval("Id")) %>' runat="server" GroupName='<%# Eval("Id") %>'  />
                                                    <asp:RadioButton ID="rbClick3" ToolTip='<%# (string) GetCountOfSpeakerToAttendee("PlanToAttend",(int) Eval("Id")) %>'
                                                         Checked='<%# IsCheckedPlanToAttend((int) Eval("Id")) %>'  Enabled="<%# IsPlanToAttendInterestedEnabled()    %>" Text='<%# (string) GetWillAttendText((int) Eval("Id")) %>' runat="server" GroupName='<%# Eval("Id") %>'  />
                                               <%-- <RBUTIL:RadioButtonListWithArg AutoPostBack="true" RepeatDirection="horizontal" CommandArg='<%# DataBinder.Eval(Container.DataItem, "id") %>'
                                                    ID="RadioButtonListWithArg1" runat="server" OnSelectedIndexChanged="RadioButtonListInterest_SelectedIndexChanged"
                                                    EnableViewState="true" Visible="true">
                                                </RBUTIL:RadioButtonListWithArg>--%>
                                            </div>
                                        </td>
                                        <td class="sessionTags" valign="top">
                                            <div id="Div2" class="tags" runat="server" visible='<%# GetHideSessionDescription() %>'>
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
                                        </td>
                                    </tr>
                                </table>
                            </div>
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
        TypeName="CodeCampSV.SessionsODS"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjectDataSourceSessionsOnlyAssigned" runat="server" SelectMethod="GetAllByStartTimeOnlyAssigned"
        TypeName="CodeCampSV.SessionsODS"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjectDataSourceBySessionPresenter" runat="server" SelectMethod="GetByPresenterLastName"
        TypeName="CodeCampSV.SessionsODS"></asp:ObjectDataSource>
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
        TypeName="CodeCampSV.SessionsODS"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjectDataSourceSessionSubmitted" runat="server" SelectMethod="GetBySessionSubmittedDate"
        TypeName="CodeCampSV.SessionsODS"></asp:ObjectDataSource>
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
        EnableCaching="True" SelectMethod="GetData" TypeName="DataSetLevelsTableAdapters.SessionLevelsTableAdapter">
    </asp:ObjectDataSource>
    
  </div>
        
</asp:Content>
