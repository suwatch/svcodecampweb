<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="SessionsRoomUpdate" Codebehind="SessionsRoomUpdate.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    <br />
    <br />
    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" DataSourceID="SqlDataSourceTimes"
        DataTextField="StartTimeFriendly" DataValueField="Id">
    </asp:DropDownList>
    <asp:SqlDataSource ID="SqlDataSourceTimes" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        SelectCommand="SELECT [Id], [StartTimeFriendly] FROM [SessionTimes] WHERE ([CodeCampYearId] = @CodeCampYearId) ORDER BY [StartTime]">
        <SelectParameters>
            <asp:Parameter DefaultValue="5" Name="CodeCampYearId" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <p>
        &nbsp;&nbsp;
        &nbsp;</p>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        SelectCommand="SELECT 
  dbo.Sessions.Id,
  count(*) AS  CountPlannedToAttend
FROM
  dbo.LectureRooms
  INNER JOIN dbo.Sessions ON (dbo.LectureRooms.Id = dbo.Sessions.LectureRoomsId)
  INNER JOIN dbo.SessionTimes ON (dbo.Sessions.SessionTimesId = dbo.SessionTimes.Id)
  INNER JOIN dbo.SessionAttendee ON (dbo.Sessions.Id = dbo.SessionAttendee.Sessions_id)
WHERE
  dbo.Sessions.CodeCampYearId = @CodeCampYearId AND 
  dbo.SessionTimes.Id = @SessionTimesId AND 
  dbo.SessionAttendee.Interestlevel = 3
GROUP BY
  dbo.Sessions.Id
ORDER BY CountPlannedToAttend desc">
        <SelectParameters>
            <asp:Parameter DefaultValue="5" Name="CodeCampYearId" />
            <asp:ControlParameter ControlID="DropDownList1" DefaultValue="" Name="SessionTimesId"
                PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
        <Columns>
           <asp:BoundField DataField="Id" HeaderText="Id" />




            <asp:TemplateField>
                <ItemTemplate>
                   &nbsp;&nbsp;  &nbsp;&nbsp;  <asp:Label ID="Label1" runat="server" Text='<%# (string) GetSessionRoom((int) Eval("Id")) %>'></asp:Label>
                 &nbsp;&nbsp;  &nbsp;&nbsp;
               
               </ItemTemplate>
                <ItemStyle Width="100px" />
            </asp:TemplateField>

             <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# (string) GetSessionTitle((int) Eval("Id")) %>'></asp:Label>
                 &nbsp;&nbsp;  &nbsp;&nbsp;
               
                </ItemTemplate>
            </asp:TemplateField>

              <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# (string) GetSessionPresenter((int) Eval("Id")) %>'></asp:Label>
                &nbsp;&nbsp;  &nbsp;&nbsp;
               
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# (string) GetNumberPlannedToAttend((int) Eval("CountPlannedToAttend")) %>'></asp:Label>
               &nbsp;&nbsp;  &nbsp;&nbsp;
               
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br />
    <br />
    syntax:&nbsp; SessionId,Room;SessionId,Room;SessionId,Room;...<br />
    <asp:TextBox ID="TextBox1" runat="server" Width="776px"></asp:TextBox>
    <br />
    <asp:Button ID="ButtonUpdate" runat="server" Text="Process Room Changes" 
        onclick="ButtonUpdateClick" />
    <br />
    <br />
    <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
    <br />
    </form>
</body>
</html>
