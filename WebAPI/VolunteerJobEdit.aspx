<%@ Page Title="" Language="C#" MasterPageFile="~/RightNote.master" AutoEventWireup="true" 
MaintainScrollPositionOnPostback="true" Inherits="VolunteerJobEdit" Codebehind="VolunteerJobEdit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <br />
    <br />
    <hr/>

    <asp:Label ID="LabelProblem" runat="server" Text="Label"></asp:Label>
    <br/>
 <asp:GridView ID="GridViewVolunteerJobs" runat="server" 
        AutoGenerateColumns="False"  DataKeyNames="Id,CodeCampYearId"
        DataSourceID="SqlDataSourceVolunteerJobs" 
        onrowcommand="GridViewVolunteerJobs_RowCommand">
     <Columns>
         <asp:CommandField ShowSelectButton="True" />
         <asp:BoundField DataField="Id" HeaderText="Id" Visible="false" 
             SortExpression="Id" InsertVisible="False" ReadOnly="True" />
         <asp:BoundField DataField="CodeCampYearId" HeaderText="CodeCampYearId" 
             SortExpression="CodeCampYearId" Visible="false"  />
         <asp:BoundField DataField="Description" HeaderText="Description" 
             SortExpression="Description" />
         <asp:BoundField DataField="JobStartTime" HeaderText="Start" 
             SortExpression="JobStartTime" />
         <asp:BoundField DataField="JobEndTime" HeaderText="End" 
             SortExpression="JobEndTime" />
         <asp:BoundField DataField="NumberNeeded" HeaderText="#Needed" 
             SortExpression="NumberNeeded" >
         <ItemStyle Width="150px" />
         </asp:BoundField>
         <asp:TemplateField HeaderText="#Signed Up">
         <ItemTemplate>
         
         <asp:Label runat="server" Text='<%# (string) GetNumberSignedUpByJob((int) Eval("Id")) %>'   ></asp:Label>
         
         </ItemTemplate>
             <ItemStyle Width="150px" />
         </asp:TemplateField>

     </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSourceVolunteerJobs" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
        
        
        SelectCommand="SELECT [Id], [CodeCampYearId], [Description], [JobStartTime], [JobEndTime], [NumberNeeded] FROM [VolunteerJob] WHERE ([CodeCampYearId] = @CodeCampYearId) ORDER BY [JobStartTime]">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                PropertyName="Text" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>

    <br />
    <br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSource2">
        <Columns>
            <asp:BoundField DataField="UserFirstName" HeaderText="FirstName" 
                SortExpression="UserFirstName" />
            <asp:BoundField DataField="UserLastName" HeaderText="LastName" 
                SortExpression="UserLastName" />
            <asp:BoundField DataField="PhoneNumber" HeaderText="Phone" 
                SortExpression="PhoneNumber" />
            <asp:BoundField DataField="Username" HeaderText="Username" 
                SortExpression="Username" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="SELECT DISTINCT 
  dbo.Attendees.UserFirstName,
  dbo.Attendees.UserLastName,
  dbo.Attendees.PhoneNumber,
  dbo.Attendees.Username,
  dbo.Attendees.Email
FROM
  dbo.AttendeeVolunteer
  INNER JOIN dbo.Attendees ON (dbo.AttendeeVolunteer.AttendeeId = dbo.Attendees.Id)
WHERE
  dbo.AttendeeVolunteer.VolunteerJobId = @VolunteerJobId">
        <SelectParameters>
            <asp:ControlParameter ControlID="GridViewVolunteerJobs" Name="VolunteerJobId" 
                PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>

    <br />
    <br />
    <hr />
    <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False"
        DataKeyNames="Id,CodeCampYearId"
        DataSourceID="SqlDataSource1" Height="50px" 
        onitemdeleted="DetailsView1_ItemDeleted" 
        oniteminserted="DetailsView1_ItemInserted" 
        oniteminserting="DetailsView1_ItemInserting" 
        onitemupdated="DetailsView1_ItemUpdated" Width="125px" 
        onitemdeleting="DetailsView1_ItemDeleting">
        <Fields>
            <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" 
                ReadOnly="True" SortExpression="Id" Visible="false" />
            <asp:BoundField DataField="CodeCampYearId" Visible="false" HeaderText="CodeCampYearId" 
                SortExpression="CodeCampYearId" />
            <asp:BoundField DataField="Description" HeaderText="Description" 
                SortExpression="Description" />
            <asp:BoundField DataField="JobStartTime" HeaderText="Start" 
                SortExpression="JobStartTime" />
            <asp:BoundField DataField="JobEndTime" HeaderText="End" 
                SortExpression="JobEndTime" />
            <asp:BoundField DataField="NumberNeeded" HeaderText="Needed" 
                SortExpression="NumberNeeded" />
        </Fields>
    </asp:DetailsView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
        DeleteCommand="DELETE FROM [VolunteerJob] WHERE [Id] = @Id" 
        InsertCommand="INSERT INTO [VolunteerJob] ([CodeCampYearId], [Description], [JobStartTime], [JobEndTime], [NumberNeeded]) VALUES (@CodeCampYearId, @Description, @JobStartTime, @JobEndTime, @NumberNeeded)" 
        oninserting="SqlDataSource1_Inserting" 
        SelectCommand="SELECT [Id], [CodeCampYearId], [Description], [JobStartTime], [JobEndTime], [NumberNeeded] FROM [VolunteerJob] WHERE ([Id] = @Id)" 
        
        UpdateCommand="UPDATE [VolunteerJob] SET [CodeCampYearId] = @CodeCampYearId, [Description] = @Description, [JobStartTime] = @JobStartTime, [JobEndTime] = @JobEndTime, [NumberNeeded] = @NumberNeeded WHERE [Id] = @Id" 
        ondeleting="SqlDataSource1_Deleting">
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="CodeCampYearId" Type="Int32" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="JobStartTime" Type="DateTime" />
            <asp:Parameter Name="JobEndTime" Type="DateTime" />
            <asp:Parameter Name="NumberNeeded" Type="Int32" />
        </InsertParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="GridViewVolunteerJobs" Name="Id" 
                PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="CodeCampYearId" Type="Int32" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="JobStartTime" Type="DateTime" />
            <asp:Parameter Name="JobEndTime" Type="DateTime" />
            <asp:Parameter Name="NumberNeeded" Type="Int32" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <br />
    <br />
    <br />
    <br />
    <asp:Label ID="LabelCodeCampYearId" Visible="false" runat="server" Text=""></asp:Label>
</asp:Content>

