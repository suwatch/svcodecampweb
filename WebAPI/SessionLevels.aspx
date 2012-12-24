<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="SessionLevels" Title="Session Levels CodeCamp SFBA" Codebehind="SessionLevels.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" Runat="Server">
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" DeleteCommand="DELETE FROM [SessionLevels] WHERE [id] = @id" InsertCommand="INSERT INTO [SessionLevels] ([description]) VALUES (@description)" SelectCommand="SELECT [id], [description] FROM [SessionLevels] ORDER BY [description]" UpdateCommand="UPDATE [SessionLevels] SET [description] = @description WHERE [id] = @id">
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="description" Type="String" />
            <asp:Parameter Name="id" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="description" Type="String" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:DetailsView ID="DetailsView1" runat="server"  AutoGenerateRows="False" DataKeyNames="id" DataSourceID="SqlDataSource1" AllowPaging="True">
        <Fields>
            <asp:BoundField DataField="id" Visible="False" HeaderText="id" InsertVisible="False" ReadOnly="True"
                SortExpression="id" />
            <asp:BoundField DataField="description" HeaderText="description" SortExpression="description" />
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>
</asp:Content>

