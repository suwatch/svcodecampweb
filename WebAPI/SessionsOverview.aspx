<%@ Page Language="C#" MasterPageFile="~/RightNote.master" AutoEventWireup="true" Inherits="SessionsOverview" MaintainScrollPositionOnPostback="true"
    Title="Sessions Overview" Codebehind="SessionsOverview.aspx.cs" %>


<asp:Content ID="SublinksSessionOverview" ContentPlaceHolderID="Sublinks" runat="server">
    <asp:Menu ID="subMenu" runat="server" DataSourceID="SiteMapProgram" SkinID="subMenu">
    </asp:Menu>
    <%--The next line should be here, but seems to work without because it is in master page--%>
    <asp:SiteMapDataSource ID="SiteMapProgram" runat="server" ShowStartingNode="False"
                    StartingNodeUrl="~/Program.aspx" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <h1 class="mainHeading">
        <asp:Literal ID="LiteralTitle" Text="Sessions Overview" runat="server"></asp:Literal>
    </h1>
    <div class="contentWrap">
    <div id="MainHeadingDivSatOrSunday" >
        <asp:RadioButtonList ID="RadioButtonIPTAList" runat="server" AutoPostBack="true" 
                        RepeatDirection="Horizontal" Font-Bold="True" 
            Font-Size="X-Large" 
            onselectedindexchanged="RadioButtonIPTAList_SelectedIndexChanged"  >
                        <asp:ListItem Selected="True">All</asp:ListItem>
                        <asp:ListItem>Interested</asp:ListItem>
                        <asp:ListItem>Plan To Attend</asp:ListItem>
                        <asp:ListItem>I & P2A</asp:ListItem>
        </asp:RadioButtonList>
    </div>

    <asp:SqlDataSource ID="SqlDataSourceTimes" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        SelectCommand="SELECT [id], [Description], [StartTimeFriendly],[TitleBeforeOnAgenda],[TitleAfterOnAgenda] FROM [SessionTimes] WHERE ([id] <> @id AND [CodeCampYearId] = @CodeCampYearId) ORDER BY [StartTime]"
        CacheDuration="1" EnableCaching="True">
        <%-- Cache Duration set in codebehind--%>
        <SelectParameters>
            <asp:Parameter DefaultValue="10" Name="id" Type="Int32" />
            <asp:ControlParameter ControlID="LabelCodeCampYearId" DefaultValue="" 
                Name="CodeCampYearId" PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:Repeater ID="RepeaterTimes" runat="server" 
        DataSourceID="SqlDataSourceTimes" EnableViewState="false" 
        onitemcommand="RepeaterTimes_ItemCommand" 
        onitemdatabound="RepeaterTimes_ItemDataBound" >
        <HeaderTemplate>
            <div style="float: right; margin-right: 30px;" >
                
               <%-- <asp:HyperLink ID="HyperLink1" 
                NavigateUrl="http://code.google.com/p/vanbox/wiki/howToInstallAddon" 
                runat="server">Great Firefox Plugin By Mike "Van" Riper  </asp:HyperLink>
                <br />
                
                <asp:Image ID="ImageSessionsOverview" ImageUrl="~\Images\SessionsOverViewPluginSmall.jpg" runat="server" />--%>
                
                
            </div>
            <div>
            <p><strong>Event Listing For Code 
                <asp:Label ID="LabelCodeCampYear1" runat="server" Text="Label"></asp:Label>
            
            Event Listing For Silicon Valley Code Camp 
                <asp:Label ID="LabelCodeCampYear2" runat="server" Text="Label"></asp:Label>.</strong><br />
            Please follow our twitter feed for any last minute camp updates:
            <a href='http://twitter.com/sv_code_camp' alt='Twitter Feed For CodeCamp' >http://twitter.com/sv_code_camp</a>

            
            </p>
             <h1>Saturday</h1>
            </div>
           
            
           
            <%--<p>8:00AM to 9:00AM Registration.  Coffee provided courtesy of our Sponsor <a href="http://www.wetpaint.com/" alt="Coffee Provided By our sponsor WetPaint" >WetPaint</a> <br /><br />
            9:00AM to 9:30AM Announcements on the Grassy Nole</p>--%>
            <table class="sessionOverview">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Label ID="LabelStartTime" runat="server" Text='<%# Eval ("StartTimeFriendly") %>'></asp:Label>
                    <asp:Label ID="LabelSessionsPerTimeSlot" runat="server" Text='<%# (string) GetHTMLForTimeSlot((int) Eval ("id"),(object) Eval("TitleBeforeOnAgenda"),(object) Eval("TitleAfterOnAgenda")) %>'></asp:Label>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <br />
    <asp:Label ID="LabelCodeCampYearId" Visible="false" runat="server" Text="Label"></asp:Label>
    <asp:Label ID="LabelRadioButtonValue" Visible="false" runat="server" Text="Label"></asp:Label>
    <asp:Label ID="LabelTiming" Visible="true" runat="server" Text=""></asp:Label>
        
        </div>

</asp:Content>
