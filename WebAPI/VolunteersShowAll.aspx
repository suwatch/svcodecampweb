<%@ Page Title="" Language="C#" MasterPageFile="~/RightNote.master" AutoEventWireup="true" Inherits="VolunteersShowAll" Codebehind="VolunteersShowAll.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<h2>All People who checked the Volunteer Box on Registration This Code Camp Year</h2>
<br />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="SELECT DISTINCT 
  dbo.Attendees.UserFirstName,
  dbo.Attendees.UserLastName,
  dbo.Attendees.Username,
  dbo.Attendees.Email,
  dbo.Attendees.PhoneNumber
FROM
  dbo.Attendees
  INNER JOIN dbo.AttendeesCodeCampYear ON (dbo.Attendees.Id = dbo.AttendeesCodeCampYear.AttendeesId)
WHERE
  dbo.AttendeesCodeCampYear.Volunteer = 1 AND
 dbo.AttendeesCodeCampYear.CodeCampYearId = @CodeCampYearId 
ORDER BY dbo.Attendees.UserLastName,dbo.Attendees.UserFirstName">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" 
        AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Width="658px" 
        onselectedindexchanged="GridView1_SelectedIndexChanged">
        <Columns>
            <asp:BoundField DataField="UserFirstName" HeaderText="UserFirstName" 
                SortExpression="UserFirstName" />
            <asp:BoundField DataField="UserLastName" HeaderText="UserLastName" 
                SortExpression="UserLastName" />
            <asp:BoundField DataField="Username" HeaderText="Username" 
                SortExpression="Username" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundField DataField="PhoneNumber" HeaderText="PhoneNumber" 
                SortExpression="PhoneNumber" />
        </Columns>
    </asp:GridView>

     <asp:Label ID="LabelCodeCampYearId" Visible="false" runat="server" Text=""></asp:Label>

</asp:Content>

