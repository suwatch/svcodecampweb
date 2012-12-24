<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="InterestCheck" Title="Untitled Page" Codebehind="InterestCheck.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" Runat="Server">
    <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
    <br />
    <br />

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="SELECT count(*),dbo.Sessions.title&#13;&#10;FROM dbo.SessionAttendee&#13;&#10;     INNER JOIN dbo.Attendees ON (dbo.SessionAttendee.attendees_username =&#13;&#10;     dbo.Attendees.PKID)&#13;&#10;     INNER JOIN dbo.Sessions ON (dbo.SessionAttendee.sessions_id =&#13;&#10;     dbo.Sessions.id)&#13;&#10;&#13;&#10;GROUP BY dbo.Sessions.title&#13;&#10;WHERE dbo.Attendees.CreateDate > @CreateDate">
        <SelectParameters>
            <asp:ControlParameter ControlID="Calendar1" Name="CreateDate" PropertyName="SelectedDate" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" DataSourceID="SqlDataSource1">
    </asp:GridView>
    <br />


</asp:Content>

