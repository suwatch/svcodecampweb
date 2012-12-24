<%@ Control Language="C#" AutoEventWireup="true" Inherits="TwitterFeed" Codebehind="TwitterFeed.ascx.cs" %>

<%@ Register src="RegistrationCount.ascx" tagname="RegistrationCount" tagprefix="uc1" %>

<div runat="server" id="TopAreaAboveTweetsID" visible="true" >
<%--<div style="float:right;margin: 0px 50px 0px 0px;">
     <h3>Registration Counts</h3>
     <h4>Days Before Camp Verses Last Year</h4>
    <uc1:RegistrationCount ID="RegistrationCount1" runat="server" ChartHeight="200" ChartWidth="200" />
</div>--%>
<div>
    <div class="pad" >
    <asp:CheckBoxList ID="CheckBoxList1" runat="server" AutoPostBack="true" RepeatDirection="Vertical">
        <asp:ListItem Value="1" Selected="True">SV Code Camp Tweets (Account sv_code_camp)</asp:ListItem>
        <asp:ListItem Value="2" Selected="True">Related to SVCC Tweets (#svcc or @sv_code_camp)</asp:ListItem>
        <asp:ListItem Value="3" Selected="True">Show Tweets From All Speakers</asp:ListItem>
        <asp:ListItem Value="4">Show Tweets From Speakers I Plan To Attend Plus Interested</asp:ListItem>
        <asp:ListItem Value="5">Show Tweets From Speakers I Plan To Attend</asp:ListItem>
    </asp:CheckBoxList>     
    </div>
</div>
</div>

<asp:Repeater ID="RepeaterTweet" runat="server" 
    DataSourceID="ObjectDataSourceTwitter" 
    onitemcommand="RepeaterTweet_ItemCommand">
    <HeaderTemplate>
        <div style="float: right; width: 24px; height: 16px; margin: 3px 10px; background: #fff url('/App_Themes/Gray2011/Images/twitter-logo-small.png') no-repeat;"></div>
        <div id="tweet" style="margin: 20px 0px 0 10px">        
    </HeaderTemplate>
    <FooterTemplate>
        </div></FooterTemplate>
    <ItemTemplate>
        <div class="post">
            <a  href='<%# GetPictureLink((string)Eval("CodeCampSpeakerUrl"),(string)Eval("AuthorUrl")) %>' target="_blank"><img width="48" height="48" alt="Twitter" src='<%# Eval("AuthorImageUrl") %>' class="profile_icon"></a>
            <div class="content">
                <p>
                        <a  href='<%# GetPictureLink((string)Eval("CodeCampSpeakerUrl"),(string)Eval("AuthorUrl")) %>' target="_blank" class="author"><%# Eval("AuthorName") %></a>: <br />
                        <%# Eval("ContentTweet") %><%# GetCodeCampSessionsString((string)Eval("CodeCampSessionsUrl")) %>
                </p>
                <div class="web_intent">
                    <span class="time"><%# Eval("PrettyDateDifferenceFromNow")%></span>
                </div>
            </div>
            <asp:LinkButton ID="LinkButtonRemoveFromStream" Visible='<%# IsAdmin() %>' CommandArgument='<%# Eval("Id")  %>' runat="server">Remove Above</asp:LinkButton>
        </div>
        
    </ItemTemplate>
</asp:Repeater>
<asp:ObjectDataSource ID="ObjectDataSourceTwitter" runat="server" DataObjectTypeName="CodeCampSV.TwitterUpdateResult" 
    DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetByParams" 
    TypeName="CodeCampSV.TwitterUpdateManager" 
    CacheDuration="100" CacheKeyDependency="TwitterFeedCache"
    EnableCaching="True" OldValuesParameterFormatString="original_{0}">
    <DeleteParameters>
        <asp:Parameter Name="id" Type="Int32" />
    </DeleteParameters>
    <SelectParameters>
        <asp:ControlParameter ControlID="LabelSvCodeCamp" Name="svCodeCamp" 
            PropertyName="Text" Type="String" />
        <asp:ControlParameter ControlID="LabelPoundSVCC" Name="svcc" 
            PropertyName="Text" Type="String" />
        <asp:ControlParameter ControlID="LabelAllSpeakers" Name="allSpeakers" 
            PropertyName="Text" Type="String" />
        <asp:ControlParameter ControlID="LabelPlanPlusInterest" Name="planPlusInterest" 
            PropertyName="Text" Type="String" />
        <asp:ControlParameter ControlID="LabelPlanToAttend" Name="plan" 
            PropertyName="Text" Type="String" />
        <asp:ControlParameter ControlID="LabelUsername" Name="loggedInUsername" 
            PropertyName="Text" Type="String" />
        <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="codeCampYearId" 
            PropertyName="Text" Type="Int32" />
        <asp:ControlParameter ControlID="LabelMaxTweetsToShow" Name="maxTweetsToShow" 
            PropertyName="Text" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>





<asp:Label ID="LabelSvCodeCamp" runat="server" Visible="false" Text=""></asp:Label>
<asp:Label ID="LabelPoundSVCC" runat="server" Visible="false" Text=""></asp:Label>
<asp:Label ID="LabelAllSpeakers" runat="server" Visible="false" Text=""></asp:Label>
<asp:Label ID="LabelPlanPlusInterest" runat="server" Visible="false" Text=""></asp:Label>
<asp:Label ID="LabelPlanToAttend" runat="server" Visible="false" Text=""></asp:Label>
<asp:Label ID="LabelUsername" runat="server" Visible="false" Text=""></asp:Label>
<asp:Label ID="LabelCodeCampYearId" runat="server" Visible="false" Text=""></asp:Label>
<asp:Label ID="LabelMaxTweetsToShow" runat="server" Visible="false" Text=""></asp:Label>

