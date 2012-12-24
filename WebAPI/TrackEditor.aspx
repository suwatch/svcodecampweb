<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true" Inherits="TrackEditor" Codebehind="TrackEditor.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="blankSublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" Runat="Server">

  

<br />
<br />
<h1>This is really only good for adding first track of the year</h1>
<h2>After that, used Track.aspx page logged in as admin</h2>
<h2>Also, update web.config for pretty url</h2>
<h2>Finally, need to assign time to track to get it to come up in right order</h2>
<br />


    <asp:DetailsView ID="DetailsView1" runat="server" Height="50px" Width="125px" 
        AllowPaging="True" AutoGenerateRows="False" DataKeyNames="Id" 
        DataSourceID="SqlDataSource1">
        <Fields>
            <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" 
                ReadOnly="True" SortExpression="Id" />
            <asp:BoundField DataField="CodeCampYearId" HeaderText="CodeCampYearId" 
                SortExpression="CodeCampYearId" />
            <asp:BoundField DataField="OwnerAttendeeId" HeaderText="OwnerAttendeeId" 
                SortExpression="OwnerAttendeeId" />
            <asp:BoundField DataField="Named" HeaderText="Named" SortExpression="Named">
            <ItemStyle Width="150px" />
            </asp:BoundField>
            <asp:CheckBoxField DataField="Visible" HeaderText="Visible" 
                SortExpression="Visible" />
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
        DeleteCommand="DELETE FROM [Track] WHERE [Id] = @Id" 
        InsertCommand="INSERT INTO [Track] ([CodeCampYearId], [OwnerAttendeeId], [Named], [Visible]) VALUES (@CodeCampYearId, @OwnerAttendeeId, @Named, @Visible)" 
        SelectCommand="SELECT [Id], [CodeCampYearId], [OwnerAttendeeId], [Named], [Visible] FROM [Track] ORDER BY [Id] DESC" 
        UpdateCommand="UPDATE [Track] SET [CodeCampYearId] = @CodeCampYearId, [OwnerAttendeeId] = @OwnerAttendeeId, [Named] = @Named, [Visible] = @Visible WHERE [Id] = @Id">
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="CodeCampYearId" Type="Int32" />
            <asp:Parameter Name="OwnerAttendeeId" Type="Int32" />
            <asp:Parameter Name="Named" Type="String" />
            <asp:Parameter Name="Visible" Type="Boolean" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="CodeCampYearId" Type="Int32" />
            <asp:Parameter Name="OwnerAttendeeId" Type="Int32" />
            <asp:Parameter Name="Named" Type="String" />
            <asp:Parameter Name="Visible" Type="Boolean" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>

</asp:Content>

