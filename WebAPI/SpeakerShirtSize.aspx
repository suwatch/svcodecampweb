<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LabelCodeCampYearId.Text = CodeCampSV.Utils.GetCurrentCodeCampYear().ToString();

            LabelShirtSizes.Text = ConfigurationManager.AppSettings["SpeakerShirtSizes"].ToString();
            
        }
    }

    protected void GridViewShirtSizeEdit_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        GridViewShirtSizeEdit.DataBind();
        GridViewShirtList.DataBind();
        GridViewCounts.DataBind();
        GridView1.DataBind();
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

        <a href="About.aspx">Back to Main Site</a>
        <br />

        <asp:SqlDataSource ID="SqlDataSourceCounts" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="SELECT ShirtSize,Count(*) 
FROM Attendees
WHERE (Username IN (
                     SELECT DISTINCT Attendees_1.Username
                     FROM SessionPresenter
                          INNER JOIN Sessions ON SessionPresenter.SessionId =
                          Sessions.Id
                          INNER JOIN Attendees AS Attendees_1 ON
                          SessionPresenter.AttendeeId = Attendees_1.Id
                     WHERE (Sessions.CodeCampYearId = @CodeCampYearId)
      ) AND
      Attendees.Id IN (
                        select Attendees.Id
                        From Attendees
                        WHERE Attendees.Id IN (
                                                SELECT DISTINCT
                                                dbo.SessionPresenter.AttendeeId
                                                FROM dbo.SessionPresenter
                                                     INNER JOIN dbo.Attendees ON
                                                     (
                                                     dbo.SessionPresenter.AttendeeId
                                                     = dbo.Attendees.Id)
                                                     INNER JOIN dbo.Sessions ON
                                                     (
                                                     dbo.SessionPresenter.SessionId
                                                     = dbo.Sessions.Id)
                                                WHERE
                                                dbo.Sessions.CodeCampYearId = @CodeCampYearId
                              ) 
      ))
GROUP BY ShirtSize
ORDER BY ShirtSize">

 <SelectParameters>
                <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                    PropertyName="Text" />
            </SelectParameters>

</asp:SqlDataSource>
        (first two rows are blanks and nulls.  Mailing List Generator lets you email those with shirts not in)
        <asp:GridView ID="GridViewCounts" runat="server" AllowSorting="True" 
            AutoGenerateColumns="False" DataSourceID="SqlDataSourceCounts">
            <Columns>
                <asp:BoundField DataField="ShirtSize" HeaderText="Shirt Size" 
                    SortExpression="ShirtSize" />
                <asp:BoundField DataField="Column1" HeaderText="Count" ReadOnly="True" 
                    SortExpression="Column1" />
            </Columns>
        </asp:GridView>

    <hr />
        <b>Shirt Sizes</b>
        <asp:SqlDataSource ID="SqlDataSourceShirtList" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="SELECT Id,ShirtSize,
       UserFirstName,
       UserLastName,
       Email,
       Username,
       TwitterHandle,
       CreationDate
FROM Attendees
WHERE (Username IN (
                     SELECT DISTINCT Attendees_1.Username
                     FROM SessionPresenter
                          INNER JOIN Sessions ON SessionPresenter.SessionId =
                          Sessions.Id
                          INNER JOIN Attendees AS Attendees_1 ON
                          SessionPresenter.AttendeeId = Attendees_1.Id
                     WHERE (Sessions.CodeCampYearId = @CodeCampYearId)
      ) AND
      Attendees.Id IN (
                        select Attendees.Id
                        From Attendees
                        WHERE Attendees.Id IN (
                                                SELECT DISTINCT
                                                dbo.SessionPresenter.AttendeeId
                                                FROM dbo.SessionPresenter
                                                     INNER JOIN dbo.Attendees ON
                                                     (
                                                     dbo.SessionPresenter.AttendeeId
                                                     = dbo.Attendees.Id)
                                                     INNER JOIN dbo.Sessions ON
                                                     (
                                                     dbo.SessionPresenter.SessionId
                                                     = dbo.Sessions.Id)
                                                WHERE
                                                dbo.Sessions.CodeCampYearId = @CodeCampYearId
                              ) AND
                              NOT (shirtsize is null OR
                              shirtsize = '')
      ))
ORDER BY ShirtSize,
         UserLastName,
         UserFirstName">
            <SelectParameters>
                <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                    PropertyName="Text" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:GridView ID="GridViewShirtList" runat="server" AutoGenerateColumns="False"  DataKeyNames="Id"
            DataSourceID="SqlDataSourceShirtList" AllowSorting="True">
            <Columns>
                <asp:BoundField DataField="ShirtSize" HeaderText="ShirtSize" 
                    SortExpression="ShirtSize">
                <ItemStyle Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="UserFirstName" HeaderText="UserFirstName" 
                    SortExpression="UserFirstName" />
                <asp:BoundField DataField="UserLastName" HeaderText="UserLastName" 
                    SortExpression="UserLastName" />
                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email">
                <ItemStyle Width="200px" />
                </asp:BoundField>
                <asp:BoundField DataField="Username" HeaderText="Username" 
                    SortExpression="Username" />
                <asp:BoundField DataField="CreationDate" HeaderText="CreationDate" 
                    SortExpression="CreationDate" />
            </Columns>
        </asp:GridView>
        <br />
       <hr />

        <br />NO Shirt Sizes<br />
        <asp:SqlDataSource ID="SqlDataSourceNoShirtSize" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="SELECT Id,ShirtSize,
       UserFirstName,
       UserLastName,
       Email,
       Username,
       CreationDate
FROM Attendees
WHERE (Username IN (
                     SELECT DISTINCT Attendees_1.Username
                     FROM SessionPresenter
                          INNER JOIN Sessions ON SessionPresenter.SessionId =
                          Sessions.Id
                          INNER JOIN Attendees AS Attendees_1 ON
                          SessionPresenter.AttendeeId = Attendees_1.Id
                     WHERE (Sessions.CodeCampYearId = @CodeCampYearId)
      ) AND
      Attendees.Id IN (
                        select Attendees.Id
                        From Attendees
                        WHERE Attendees.Id IN (
                                                SELECT DISTINCT
                                                dbo.SessionPresenter.AttendeeId
                                                FROM dbo.SessionPresenter
                                                     INNER JOIN dbo.Attendees ON
                                                     (
                                                     dbo.SessionPresenter.AttendeeId
                                                     = dbo.Attendees.Id)
                                                     INNER JOIN dbo.Sessions ON
                                                     (
                                                     dbo.SessionPresenter.SessionId
                                                     = dbo.Sessions.Id)
                                                WHERE
                                                dbo.Sessions.CodeCampYearId = @CodeCampYearId
                              ) AND
                              (shirtsize is null OR
                              shirtsize = '')
      ))
ORDER BY ShirtSize,
         UserLastName,
         UserFirstName">
            <SelectParameters>
                <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                    PropertyName="Text" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:GridView ID="GridView1" runat="server" AllowSorting="True" 
            AutoGenerateColumns="False" DataSourceID="SqlDataSourceNoShirtSize">
            <Columns>
                <asp:BoundField DataField="UserFirstName" HeaderText="UserFirstName" 
                    SortExpression="UserFirstName" />
                <asp:BoundField DataField="UserLastName" HeaderText="UserLastName" 
                    SortExpression="UserLastName" />
                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                <asp:BoundField DataField="Username" HeaderText="Username" 
                    SortExpression="Username" />
                <asp:BoundField DataField="CreationDate" HeaderText="CreationDate" 
                    SortExpression="CreationDate" />
            </Columns>
        </asp:GridView>

        <hr />
         <h2>Make Sure to enter shirtsize exactly as is in list</h2>
       
         <asp:Label ID="LabelShirtSizes" runat="server" Text=""></asp:Label>
        <asp:GridView ID="GridViewShirtSizeEdit" runat="server" AllowSorting="True" 
            AutoGenerateColumns="False" DataSourceID="SqlDataSourceShirtSizeEdit" DataKeyNames="Id"
            onrowupdated="GridViewShirtSizeEdit_RowUpdated">
            <Columns>
                <asp:CommandField ShowEditButton="True" />
                <asp:BoundField DataField="ShirtSize" HeaderText="ShirtSize" 
                    SortExpression="ShirtSize" />
                <asp:BoundField DataField="UserFirstName" HeaderText="UserFirstName"  
                    SortExpression="UserFirstName" ReadOnly="True" />
                <asp:BoundField DataField="UserLastName" HeaderText="UserLastName" 
                    SortExpression="UserLastName" ReadOnly="True" />
                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" 
                    ReadOnly="True"  />
                <asp:BoundField DataField="Username" HeaderText="Username" 
                    SortExpression="Username" ReadOnly="True" />
                <asp:BoundField DataField="CreationDate" HeaderText="CreationDate" 
                    SortExpression="CreationDate" ReadOnly="True" />
                 <asp:BoundField DataField="TwitterHandle" HeaderText="TwitterHandle" 
                    SortExpression="TwitterHandle" ReadOnly="False" />
            </Columns>
        </asp:GridView>

        <asp:SqlDataSource ID="SqlDataSourceShirtSizeEdit" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
            DeleteCommand="DELETE FROM [Attendees] WHERE [Id] = @Id" 
            InsertCommand="INSERT INTO [Attendees] ([Email], [UserFirstName], [UserLastName], [ShirtSize]) VALUES (@Email, @UserFirstName, @UserLastName, @ShirtSize)" 
            SelectCommand="SELECT Id,ShirtSize,
       UserFirstName,
       UserLastName,
       Email,
       Username,
       TwitterHandle,
       CreationDate
FROM Attendees
WHERE (Username IN (
                     SELECT DISTINCT Attendees_1.Username
                     FROM SessionPresenter
                          INNER JOIN Sessions ON SessionPresenter.SessionId =
                          Sessions.Id
                          INNER JOIN Attendees AS Attendees_1 ON
                          SessionPresenter.AttendeeId = Attendees_1.Id
                     WHERE (Sessions.CodeCampYearId = @CodeCampYearId)
      ) AND
      Attendees.Id IN (
                        select Attendees.Id
                        From Attendees
                        WHERE Attendees.Id IN (
                                                SELECT DISTINCT
                                                dbo.SessionPresenter.AttendeeId
                                                FROM dbo.SessionPresenter
                                                     INNER JOIN dbo.Attendees ON
                                                     (
                                                     dbo.SessionPresenter.AttendeeId
                                                     = dbo.Attendees.Id)
                                                     INNER JOIN dbo.Sessions ON
                                                     (
                                                     dbo.SessionPresenter.SessionId
                                                     = dbo.Sessions.Id)
                                                WHERE
                                                dbo.Sessions.CodeCampYearId = @CodeCampYearId
                              ) 
      ))
ORDER BY 
         UserLastName,
         UserFirstName" 
            
            
            
            UpdateCommand="UPDATE [Attendees] SET  [ShirtSize] = @ShirtSize, [TwitterHandle] = @TwitterHandle WHERE [Id] = @Id">
            <DeleteParameters>
                <asp:Parameter Name="Id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="Email" Type="String" />
                <asp:Parameter Name="UserFirstName" Type="String" />
                <asp:Parameter Name="UserLastName" Type="String" />
                <asp:Parameter Name="ShirtSize" Type="String" />
            </InsertParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                    PropertyName="Text" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="ShirtSize" Type="String" />
                <asp:Parameter Name="TwitterHandle" />
                <asp:Parameter Name="Id" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>


       
        <br />
         <asp:Label ID="LabelCodeCampYearId" runat="server" Text="Label"></asp:Label>
    </div>
    </form>
</body>

</html>
