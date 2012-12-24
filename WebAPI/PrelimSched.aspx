<%@ Page Language="C#" AutoEventWireup="true" Inherits="PrelimSched" Codebehind="PrelimSched.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Prelim Schedule</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="SELECT SessionTimes.StartTime,&#13;&#10;       Sessions.title,&#13;&#10;       Attendees.UserLastName,&#13;&#10;       LectureRooms.Number,&#13;&#10;       LectureRooms.Capacity&#13;&#10;FROM Attendees,&#13;&#10;     Sessions,&#13;&#10;     SessionTimes,&#13;&#10;     LectureRooms&#13;&#10;WHERE Attendees.Username = Sessions.Username AND&#13;&#10;      Sessions.SessionTimesId = SessionTimes.id AND&#13;&#10;      Sessions.LectureRoomsId = LectureRooms.id&#13;&#10;ORDER BY SessionTimes.StartTime"></asp:SqlDataSource>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
            <Columns>
                <asp:BoundField  DataField="StartTime" HtmlEncode="false" DataFormatString="{0:M-dd hh:mm}" HeaderText="StartTime" SortExpression="StartTime" />
                <asp:BoundField DataField="title" HeaderText="title" SortExpression="title" />
                <asp:BoundField DataField="UserLastName" HeaderText="UserLastName" SortExpression="UserLastName" />
                <asp:BoundField DataField="Number" HeaderText="Number" SortExpression="Number" />
                <asp:BoundField DataField="Capacity" HeaderText="Capacity" SortExpression="Capacity" />
            </Columns>
        </asp:GridView>
        <br />
        <br />
    </div>
    </form>
</body>
</html>
