<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="AgendaUpdateByTime" Codebehind="AgendaUpdateByTime.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

    <h1>Assign Session to a Given Time Slot (Keep all in Room 0)</h1>

    <div>
        <asp:SqlDataSource ID="SqlDataSourceTimeList" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
            
            
            
            SelectCommand="SELECT [Id], [StartTimeFriendly] FROM [SessionTimes] WHERE ([CodeCampYearId] = @CodeCampYearId) ORDER BY [StartTime]">
            <SelectParameters>
                <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                    PropertyName="Text" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:DropDownList ID="DropDownListTimeList" runat="server" 
            DataSourceID="SqlDataSourceTimeList" DataTextField="StartTimeFriendly" 
            DataValueField="Id" AutoPostBack="True">
        </asp:DropDownList>
       
       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
       Default 
        Time To Unassign to&nbsp;&nbsp;
        <asp:DropDownList ID="DropDownListDefaultTimeToUnassign" runat="server" 
            AutoPostBack="True" DataSourceID="SqlDataSourceDefaultTimeToUnassign" 
            DataTextField="StartTime" DataValueField="Id">
        </asp:DropDownList>
        <asp:SqlDataSource ID="SqlDataSourceDefaultTimeToUnassign" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
            SelectCommand="SELECT [Id], [StartTime] FROM [SessionTimes] ORDER BY [StartTime] DESC"></asp:SqlDataSource>

        <br />
    </div>
    <div style="float: right; width: 45%;">
        <b>Assigned</b>
        <hr />
        <asp:GridView ID="GridViewAssigned" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            DataSourceID="SqlDataSourceAssigned" AllowSorting="True" 
            onrowcommand="GridView1_RowCommand">
            <Columns>
               <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                            CommandArgument='<%# Eval("Id") %>'
                            CommandName="Select" Text="Remove"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                <asp:BoundField DataField="UserLastName" HeaderText="UserLastName" SortExpression="UserLastName" />
                <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True"
                    SortExpression="Id" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSourceAssigned" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
            SelectCommand="      
            SELECT dbo.Sessions.Title,
                   dbo.Attendees.UserLastName,
                   dbo.Sessions.Id
            FROM dbo.Attendees
                 INNER JOIN dbo.Sessions ON (dbo.Attendees.Id = dbo.Sessions.Attendeesid)
            WHERE dbo.Sessions.CodeCampYearId = @CodeCampYearId AND
                  dbo.[Sessions].Id  IN (
                                             SELECT Id
                                             FROM dbo.Sessions
                                             WHERE dbo.Sessions.CodeCampYearId = @CodeCampYearId AND
                                                   dbo.Sessions.SessionTimesId = @Id
                                )
            
               ">
            <SelectParameters>
                <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" PropertyName="Text" />
                <asp:ControlParameter ControlID="DropDownListTimeList" Name="Id" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <div>
        <b>Not Assigned</b>
        <hr />
        <asp:GridView ID="GridViewNotAssigned" runat="server" AutoGenerateColumns="False"
            DataKeyNames="Id" DataSourceID="SqlDataSourceNotAssigned" 
            onrowcommand="GridViewNotAssigned_RowCommand" AllowSorting="True">
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                            CommandArgument='<%# Eval("Id") %>'
                            CommandName="Select" Text="AssignToTime"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                <asp:BoundField DataField="UserLastName" HeaderText="UserLastName" SortExpression="UserLastName" />
                <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True"
                    SortExpression="Id" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSourceNotAssigned" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
            SelectCommand="      
            SELECT dbo.Sessions.Title,
                   dbo.Attendees.UserLastName,
                   dbo.Sessions.Id
            FROM dbo.Attendees
                 INNER JOIN dbo.Sessions ON (dbo.Attendees.Id = dbo.Sessions.Attendeesid)
            WHERE dbo.Sessions.CodeCampYearId = @CodeCampYearId AND
                  dbo.[Sessions].Id NOT IN (
                                             SELECT Id
                                             FROM dbo.Sessions
                                             WHERE dbo.Sessions.CodeCampYearId = @CodeCampYearId AND
                                                   dbo.Sessions.SessionTimesId = @Id
                                )
                                ORDER BY dbo.Sessions.Title
            
               ">
            <SelectParameters>
                <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" PropertyName="Text" />
                <asp:ControlParameter ControlID="DropDownListTimeList" Name="Id" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <asp:Label ID="LabelCodeCampYearId" runat="server" Text=""></asp:Label>
    <asp:Label ID="LabelLectureRoomId" runat="server" Text=""></asp:Label>
    </form>
</body>
</html>
