<%@ Page Title="" Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true" Inherits="SessionWiki" Codebehind="SessionWiki.aspx.cs" %>

<%--<%@ Register Assembly="WetPaintControl" Namespace="Webhun.WetPaintControl" TagPrefix="cc1" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" runat="Server">
    <asp:HyperLink ID="HyperLinkReturnToSession" runat="server" Text="Back To Session"
        NavigateUrl="~/Sessions.aspx"></asp:HyperLink>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="mainHeading">
        Wiki Sessions
    </div>
    <div id="sessionsHolder">
        <div class="pad">
            <div class="subHeading">
                <asp:Label ID="IDPresenter" runat="server" Text="Presenter"></asp:Label>
                <br />
                <%--<asp:Image runat="server" ID="IDPresenterImage" />
                <br />--%>
                <asp:Label ID="IDTitle" runat="server" Text="Title"></asp:Label>
                
            </div>
            <div class="sessionDetails">
                <asp:Label ID="IDSessionTime" runat="server" CssClass="sessionTime" Text=""></asp:Label>
                <asp:Label ID="IDSessionRoom" runat="server" CssClass="sessionRoom" Text=""></asp:Label>
            </div>
            <div class="sessionWPContentLeft">
                <div class="sessionDescription">
                    <asp:Label ID="IDSessionDescription" runat="server" Text="Developer practices for traditional and agile Java development are well understood and documented. But dynamic languages—Groovy, Ruby, and others—change the ground rules. Many of the common practices, refactoring techniques, and design patterns we have been taught either no longer apply or should be applied differently and some new techniques also come into play. In this talk, techniques for agile development with dynamic languages are discussed. How should we apply design patterns? How should we better apply refactoring techniques? What new aspects do we need to think about?"></asp:Label>
                </div>
                <%--<div class="">
                    <asp:Label ID="Label1" runat="server" Text="Developer practices for traditional and agile Java development are well understood and documented. But dynamic languages—Groovy, Ruby, and others—change the ground rules. Many of the common practices, refactoring techniques, and design patterns we have been taught either no longer apply or should be applied differently and some new techniques also come into play. In this talk, techniques for agile development with dynamic languages are discussed. How should we apply design patterns? How should we better apply refactoring techniques? What new aspects do we need to think about?"></asp:Label>
                </div>--%>
                <div class="sessionWetPaint">
                    <%--<cc1:WetPaintWebControl ID="WetPaintWebControl1" runat="server" />--%>
                </div>
            </div>
           <%-- <div class="sessionWPContentRight">
                <div class="sessionTags">
                    <ul>
                        <li class="tagBullet">Asp.Net</li>
                        <li class="tagBullet">Asp.Net</li>
                        <li class="tagBullet">Asp.Net</li>
                        <li class="tagBullet">Asp.Net</li>
                    </ul>
                </div>
            </div>--%>
        </div>
    </div>
</asp:Content>
