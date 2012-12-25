<%@ Page Language="C#" MasterPageFile="~/RightNote.master" AutoEventWireup="true" Inherits="Speakers" Title="Speakers CodeCamp" Codebehind="Speakers.aspx.cs" %>

<asp:Content ID="SublinksSpeakers" ContentPlaceHolderID="Sublinks" runat="server">
    <asp:Menu ID="subMenu" runat="server" DataSourceID="SiteMapProgram" SkinID="subMenu">
    </asp:Menu>
    <%--The next line should be here, but seems to work without because it is in master page--%>
    <asp:SiteMapDataSource ID="SiteMapProgram" runat="server" ShowStartingNode="False"
        StartingNodeUrl="~/Program.aspx" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script src="JavaScript/jquery-1.3.2.min.js" type="text/javascript" charset="utf-8"></script>
    <link rel="stylesheet" href="JavaScript/prettyPhoto/css/prettyPhoto.css" type="text/css"
        media="screen" charset="utf-8" />
    <script src="JavaScript/prettyPhoto/js/jquery.prettyPhoto.js" type="text/javascript"
        charset="utf-8"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("a[rel^='prettyPhoto']").prettyPhoto({
                default_width: 500,
                default_height: 344,
                theme: 'light_square'
            });
        });
    </script>
    <div class="mainHeading">
        Speakers
    </div>
    <div class="sessionsPanel" runat="server" id="SessionsPanelID" visible="false">
        <asp:Image ID="Nov8" ImageUrl="~/App_Themes/Gray/Images/sessionsNov8CodeCamp.gif"
            Alt="Nov 8: Annual CodeCamp" Width="140" Height="116" runat="server" CssClass="sessionsButton" />
        <asp:Image ID="Nov9" ImageUrl="~/App_Themes/Gray/Images/sessionsNov9CodeCamp.gif"
            Alt="Nov 9: Annual CodeCamp" Width="140" Height="116" runat="server" CssClass="sessionsButton" />
    </div>
    <!-- Speakers heading -->
    <div class="mainHeading" runat="server" id="MainHeadingDIV" visible="false">
    </div>
    <!-- Sort speakers -->
    <div class="pad">
        <div runat="server" id="DivSpeakerSort" visible="false" class="grayBackground">
            Sort by: &nbsp;&nbsp;
            <asp:DropDownList ID="SortSpeakers" runat="server">
                <asp:ListItem Value="Title">Last Name</asp:ListItem>
                <asp:ListItem Value="Time">First Name</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <!-- Begin container for speaker listings -->
    <div class="speakersContainer">
        <asp:Repeater ID="Repeater1" runat="server" EnableViewState="true" DataSourceID="ObjectDataSourceAllPresenters">
            <HeaderTemplate>
                <div id="three-column-containerw">
                    <table class="speakersList" cellpadding="0" cellspacing="0">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td class="speakerPic">
                        <asp:Image EnableViewState="false" runat="server" ID="Image1" ImageUrl='<%# "~/DisplayImage.ashx?sizex=100&PKID=" + Eval("PKID")  %>'
                            ImageAlign="middle" BorderStyle="None" CssClass="speakerImage" />
                    </td>
                    <td>
                        <h4 class="name">
                            <asp:Literal ID="Label2" runat="server" EnableViewState="false" Text='<%# Eval("UserFirstName") + " " + Eval("UserLastName") %>'></asp:Literal>
                            <asp:HyperLink ID="HyperLink3" NavigateUrl='<%# GetUserWebSite((object) Eval("UserWebSite")) %>'
                                EnableViewState="false" Text='<%# GetUserWebSite((object) Eval("UserWebSite")) %>'
                                runat="server" CssClass="speakerUrl"></asp:HyperLink>
                        </h4>
                        <div class="contentWrap">
                            <asp:HyperLink ID="HyperLinkSessions" NavigateUrl='<%# "~/Sessions.aspx?ForceSortBySessionTime=true&AttendeeId=" + Eval("Id") %>'
                                Text="Sessions" CssClass="sessions" runat="server"></asp:HyperLink>
                            <p class="Description">
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("UserBio") %>' EnableViewState="false"></asp:Label>
                            </p>
                            <div runat="server" id="VideoID" visible="false" class="noWrap">
                                <a href="http://www.youtube.com/watch?v=6uev1CJUwq8" title="Beth Massi of Microsoft Talks About Visual Studio LightSwitch"
                                    rel="prettyPhoto" class="video">
                                    <img src="Images/Video/WhatMakesYouATrueGeek.jpg" alt="Beth Massi of Microsoft Talks About Visual Studio LightSwitch" />
                                    <br />
                                    Beth Massi of Microsoft Talks About Visual Studio LightSwitch</a>
                            </div>
                        </div>
                    </td>
                    <td></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table> </div>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <asp:ObjectDataSource ID="ObjectDataSourceAllPresenters" runat="server" SelectMethod="GetData"
        TypeName="WebAPI.Code.DataSetPresentersTableAdapters.AttendeesTableAdapter">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelCodeCampYearId" DefaultValue="" Name="CodeCampYearId"
                PropertyName="Text" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjectDataSourceBySession" runat="server" SelectMethod="GetBySessionId"
        TypeName="CodeCampSV.AttendeesODS">
        <SelectParameters>
            <asp:QueryStringParameter Name="sessionId" QueryStringField="id" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjectDataSourceBySessionX" runat="server" SelectMethod="GetAttendeeBySessionId"
        TypeName="WebAPI.Code.DataSetPresentersTableAdapters.AttendeesTableAdapter" CacheDuration="120"
        EnableCaching="True">
        <SelectParameters>
            <asp:QueryStringParameter Name="id" QueryStringField="id" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjectDataSourceByAttendeeId" runat="server" SelectMethod="GetAttendeeByAttendeeId"
        TypeName="WebAPI.Code.DataSetPresentersTableAdapters.AttendeesTableAdapter" CacheDuration="120"
        EnableCaching="True">
        <SelectParameters>
            <asp:QueryStringParameter Name="AttendeeId" QueryStringField="Attendeeid" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    
    <div runat="server" id="AdminsOnlyId" Visible="False">
        <hr/>
        <asp:Label ID="LabelCodeCampYearId" runat="server" Text="" Visible="false"></asp:Label>
        <br/><br/>
         <p><b>(For Admins Only, Need to refresh after pressing button to see updated roles)</b></p>
        </div>
         <asp:Label Visible="false"  runat="server" ID="SpeakerShowRolesLabel" ></asp:Label>
    <br/>
   
    <asp:Button Visible="False" runat="server" 
        Text="Set Speaker Role To Submit 2 sessions" ID="ButtonAddSubmit2SessionsRole" 
        OnClick="ButtonAddSubmit2SessionsRole_Click" />
    <br />
    <asp:Button Visible="False" runat="server" 
        Text="Set Speaker Role To Submit 3 or more sessions" 
        ID="ButtonAddSubmit3orMoreSessionsRole" 
        OnClick="ButtonAddSubmit3orMoreSessionsRole_Click" />
    
     <br />
    <asp:Button Visible="False" runat="server" 
        Text="Set Speaker Role To Submit 3 or more sessions" 
        ID="Button1" 
        OnClick="ButtonAddSubmit3orMoreSessionsRole_Click" />
    
     <br />
    <asp:Button Visible="False" runat="server" 
        Text="Set Speaker Role To Submit 3  sessions" 
        ID="ButtonAddSubmit3Sessions" OnClick="ButtonAddSubmit3Sessions_Click" 
        />
    
     <br />
    <asp:Button Visible="False" runat="server" 
        Text="Set Speaker Role To Submit 4  sessions" 
        ID="ButtonAddSubmit4Sessions" OnClick="ButtonAddSubmit4Sessions_Click" 
         />
    
     <br />
    <asp:Button Visible="False" runat="server" 
        Text="Set Submit Sessions" 
        ID="ButtonAddSubmitSession" OnClick="ButtonAddSubmitSession_Click" 
        />

    
    
    
    

</asp:Content>
