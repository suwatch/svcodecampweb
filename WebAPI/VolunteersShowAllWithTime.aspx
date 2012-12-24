<%@ Page Title="" Language="C#" MasterPageFile="~/RightNote.master" AutoEventWireup="true" Inherits="VolunteersShowAllWithTime" Codebehind="VolunteersShowAllWithTime.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<h2>All People who checked the Volunteer Box on Registration This Code Camp Year</h2>
<br />
    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="SELECT 
  dbo.VolunteerJob.JobStartTime,
  dbo.VolunteerJob.JobEndTime,
  dbo.Attendees.UserLastName,
  dbo.Attendees.UserFirstName,
  dbo.Attendees.PhoneNumber,
  dbo.VolunteerJob.Description
FROM
  dbo.VolunteerJob
  INNER JOIN dbo.AttendeeVolunteer ON (dbo.VolunteerJob.Id = dbo.AttendeeVolunteer.VolunteerJobId)
  INNER JOIN dbo.Attendees ON (dbo.AttendeeVolunteer.AttendeeId = dbo.Attendees.Id)
WHERE
  dbo.VolunteerJob.CodeCampYearId = @CodeCampYearId
ORDER BY
  dbo.VolunteerJob.JobStartTime,
  dbo.Attendees.UserLastName,
  dbo.Attendees.UserFirstName">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>

     <asp:Button ID="ButtonExportCSV" runat="server" onclick="ButtonExportCSV_Click" 
        Text="Export To CSV" />
        <br />

    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" 
        AutoGenerateColumns="False" CellPadding="10" CellSpacing="10" 
        DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:BoundField DataField="JobStartTime" HeaderText="JobStartTime" 
                SortExpression="JobStartTime" />
            <asp:BoundField DataField="JobEndTime" HeaderText="JobEndTime" 
                SortExpression="JobEndTime" />
            <asp:BoundField DataField="UserLastName" HeaderText="UserLastName" 
                SortExpression="UserLastName" />
            <asp:BoundField DataField="UserFirstName" HeaderText="UserFirstName" 
                SortExpression="UserFirstName" />
            <asp:BoundField DataField="PhoneNumber" HeaderText="PhoneNumber" 
                SortExpression="PhoneNumber" />
            <asp:BoundField DataField="Description" HeaderText="Description" 
                SortExpression="Description" />
        </Columns>
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>

     <asp:Label ID="LabelCodeCampYearId" Visible="false" runat="server" Text=""></asp:Label>

    <br />
   

</asp:Content>

