<%@ Page Language="C#" MasterPageFile="~/RightNote.master" AutoEventWireup="true" Inherits="SessionRooms" Title="Sessions By Room" Codebehind="SessionRooms.aspx.cs" %>




<asp:Content ID="SublinksSessionRooms" ContentPlaceHolderID="Sublinks" runat="server">
    <asp:Menu ID="subMenu" runat="server" DataSourceID="SiteMapProgram" SkinID="subMenu">
    </asp:Menu>
    <%--The next line should be here, but seems to work without because it is in master page--%>
    <asp:SiteMapDataSource ID="SiteMapProgram" runat="server" ShowStartingNode="False"
                    StartingNodeUrl="~/Program.aspx" />
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <%--    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
            <strong><span style="font-size: 1.4em">Room Number</span> </strong>
            <asp:DropDownList ID="DropDownListUsedRooms" runat="server" AutoPostBack="True" DataSourceID="SqlDataSourceRoomsUsed"
                DataTextField="Number" DataValueField="id" Font-Size="Large" AppendDataBoundItems="true">
                <asp:ListItem Text="All Rooms" Value="0"></asp:ListItem>
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;
            <asp:SqlDataSource ID="SqlDataSourceRoomsUsed" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
                
        
        SelectCommand="SELECT Number, id FROM LectureRooms WHERE (id IN (SELECT DISTINCT LectureRoomsId FROM Sessions WHERE (CodeCampYearId = @CodeCampYearId) AND (Available = 1 ))) ORDER BY Number">
                <SelectParameters>
                    <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                        PropertyName="Text" />
                </SelectParameters>
            </asp:SqlDataSource>
            <br />
            <br />
            <div runat="server" id="DivOneRoom">




                <asp:GridView ID="GridViewSelectedRoom" runat="server" AutoGenerateColumns="False"
                    DataSourceID="SqlDataSourceSessionsInRoom" Font-Size="Large">
                    <Columns>
                        <asp:TemplateField HeaderText="Class Start Time" SortExpression="StartTime">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# GetNiceDayFormat((DateTime) Eval("StartTime")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Session Title" SortExpression="title">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Width="400" Text='<%# Eval("title") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Presenter Name" SortExpression="UserFirstName">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("UserFirstName") + " " + Eval("UserLastName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>




                <br />
                <br />
            </div>
            <asp:SqlDataSource ID="SqlDataSourceSessionsInRoom" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
                
        SelectCommand="SELECT 
                          Attendees.UserFirstName,
                          Attendees.UserLastName,
                          Sessions.title,
                          SessionTimes.StartTime,
                          SessionTimes.id,
                          Sessions.id AS Expr1
                        FROM
                          Sessions
                          INNER JOIN Attendees ON (Sessions.Attendeesid = Attendees.Id)
                          INNER JOIN SessionTimes ON (Sessions.SessionTimesId = SessionTimes.Id)
                        WHERE
                          Sessions.LectureRoomsId = @LectureRoomId AND 
                          Sessions.Id IN (SELECT SessionsOverview.SessionId FROM SessionsOverview WHERE CodeCampYearId = @CodeCampYearId)
                        ORDER BY
                          SessionTimes.StartTime">
                <SelectParameters>
                    <asp:ControlParameter ControlID="DropDownListUsedRooms" Name="LectureRoomId" PropertyName="SelectedValue" />
                    <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                        PropertyName="Text" />
                </SelectParameters>
            </asp:SqlDataSource>
            <div runat="server" id="DivAllRooms">
                <asp:Repeater ID="RepeaterAllRooms" runat="server" DataSourceID="SqlDataSourceRoomsUsed">
                    <ItemTemplate>
                        <div runat="server" id="SessionPageBreakId" class="page-break"></div>
                        <headertemplate>
                            <asp:Image runat="server" ID="Image3" visible="false"  CssClass="sessionRoomPhoto" ImageUrl='<%# "~/DisplayImage.ashx?roomid=" + Eval("id")  %>'
                            EnableViewState="true" />
                        </headertemplate>
                        <br />
                        <asp:Label ID="HiddenRoomNumber" runat="server" Text='<%# Eval("id") %>' Visible="false" />
                        <strong><span style="font-size: 1.4em">Room Number</span> </strong>
                        <asp:Label ID="LabelRoom" Font-Size="large" Font-Bold="true" runat="server" Text='<%# Eval("number") %>'></asp:Label>
                        <asp:SqlDataSource ID="SqlDataSourceForNestedRepeater" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
                            SelectCommand="SELECT 
                                      dbo.Attendees.UserFirstName,
                                      dbo.Sessions.id as SessionId,     dbo.Attendees.UserLastName,
                                      dbo.Sessions.title,
                                      dbo.SessionTimes.StartTime,
                                      dbo.SessionTimes.id
                                    FROM
                                      dbo.Sessions
                                      INNER JOIN dbo.LectureRooms ON (dbo.Sessions.LectureRoomsId = dbo.LectureRooms.id)
                                      INNER JOIN dbo.SessionTimes ON (dbo.Sessions.SessionTimesId = dbo.SessionTimes.id)
                                      INNER JOIN dbo.Attendees ON (dbo.Sessions.Username = dbo.Attendees.Username)
                                    WHERE
                                      (dbo.LectureRooms.id = @LectureRoomId) AND
                                      (dbo.[Sessions].Id IN (SELECT SessionId FROM SessionsOverview WHERE CodeCampYearId = @CodeCampYearId))
                                    ORDER BY
                                      dbo.SessionTimes.StartTime,
                                      dbo.Attendees.UserLastName,
                                      dbo.Attendees.UserFirstName">
                            <SelectParameters>
                                <asp:ControlParameter Name="LectureRoomId" ControlID="HiddenRoomNumber" />
                                 <asp:ControlParameter Name="CodeCampYearId" ControlID="LabelCodeCampYearId" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:GridView ID="GridViewSelectedRoom" runat="server" AutoGenerateColumns="False"
                            DataSourceID="SqlDataSourceForNestedRepeater" Font-Size="Large">
                            <Columns>
                                <asp:TemplateField HeaderText="Class Start Time" SortExpression="StartTime">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# GetNiceDayFormat((DateTime) Eval("StartTime")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Session Title" SortExpression="title">
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Width="400" Text='<%# Eval("title") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Presenter Name" SortExpression="UserFirstName">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("UserFirstName") + " " + Eval("UserLastName") %>'></asp:Label>
                                        <asp:Label ID="Label2" runat="server" Text='<%# GetWillAttendCount((int)Eval("SessionId")) %>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ItemTemplate>
                    <SeparatorTemplate>
                        <br />
                        <hr />
                        <br />
                    </SeparatorTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
      <%--  </ContentTemplate>
    </asp:UpdatePanel>--%>
    
    
    <%-- <asp:SqlDataSource ID="SqlDataSourcexxx" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
                            SelectCommand="SELECT &#13;&#10;  dbo.Attendees.UserFirstName,&#13;&#10;  dbo.Attendees.UserLastName,&#13;&#10;  dbo.Sessions.title,&#13;&#10;  dbo.SessionTimes.StartTime,&#13;&#10;  dbo.SessionTimes.id&#13;&#10;FROM&#13;&#10;  dbo.Sessions&#13;&#10;  INNER JOIN dbo.LectureRooms ON (dbo.Sessions.LectureRoomsId = dbo.LectureRooms.id)&#13;&#10;  INNER JOIN dbo.SessionTimes ON (dbo.Sessions.SessionTimesId = dbo.SessionTimes.id)&#13;&#10;  INNER JOIN dbo.Attendees ON (dbo.Sessions.Username = dbo.Attendees.Username)&#13;&#10;WHERE&#13;&#10;  (dbo.LectureRooms.id = @LectureRoomId)&#13;&#10;ORDER BY&#13;&#10;  dbo.SessionTimes.StartTime,&#13;&#10;  dbo.Attendees.UserLastName,&#13;&#10;  dbo.Attendees.UserFirstName">
                            <SelectParameters>
                                <asp:ControlParameter Name="LectureRoomId" ControlID="HiddenRoomNumber" />
                            </SelectParameters>
     </asp:SqlDataSource>--%>
     
     
    <asp:Label ID="LabelCodeCampYearId" runat="server" Visible="false" Text="Label"></asp:Label>


    <%--   <asp:SqlDataSource ID="SqlDataSourceForNestedRepeater" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
                            SelectCommand="SELECT 
  dbo.Attendees.UserFirstName,
  dbo.Sessions.id as SessionId,     dbo.Attendees.UserLastName,
  dbo.Sessions.title,
  dbo.SessionTimes.StartTime,
  dbo.SessionTimes.id
FROM
  dbo.Sessions
  INNER JOIN dbo.LectureRooms ON (dbo.Sessions.LectureRoomsId = dbo.LectureRooms.id)
  INNER JOIN dbo.SessionTimes ON (dbo.Sessions.SessionTimesId = dbo.SessionTimes.id)
  INNER JOIN dbo.Attendees ON (dbo.Sessions.Username = dbo.Attendees.Username)
WHERE
  (dbo.LectureRooms.id = @LectureRoomId) AND
  (dbo.[Sessions].Id IN (SELECT SessionId FROM SessionsOverview WHERE CodeCampYearId = @CodeCampYearId))
ORDER BY
  dbo.SessionTimes.StartTime,
  dbo.Attendees.UserLastName,
  dbo.Attendees.UserFirstName">
                            <SelectParameters>
                                <asp:ControlParameter Name="LectureRoomId" ControlID="DropDownListUsedRooms" 
                                    PropertyName="SelectedValue" />
                                <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                                    PropertyName="Text" />
                            </SelectParameters>
                        </asp:SqlDataSource>--%>
    
</asp:Content>
