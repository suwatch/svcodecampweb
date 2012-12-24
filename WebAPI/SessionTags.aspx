<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="SessionTags" Title="Session Tags CodeCamp SFBA" Codebehind="SessionTags.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" Runat="Server">
    <br />
    <asp:TextBox ID="TextBoxTag" runat="server"></asp:TextBox>
    <asp:Button ID="ButtonAddTag" runat="server" OnClick="ButtonAddTag_Click" Text="Add Tag" /><br />

    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" 
    DataSourceID="SqlDataSource1" DataKeyNames="id" AutoGenerateColumns="False">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
            <asp:BoundField DataField="TagName" HeaderText="TagName" SortExpression="TagName" />
            <asp:BoundField DataField="TagDescription" HeaderText="TagDescription" SortExpression="TagDescription" />
        </Columns>
    </asp:GridView>
    <br />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        DeleteCommand="DELETE FROM Tags WHERE id = @id;delete from sessiontags where tagid = @id" 
        InsertCommand="INSERT INTO [Tags] ([TagName], [TagDescription]) VALUES (@TagName, @TagDescription)"
        SelectCommand="SELECT [id], [TagName], [TagDescription] FROM [Tags] ORDER BY [TagName]"
        UpdateCommand="UPDATE [Tags] SET [TagName] = @TagName, [TagDescription] = @TagDescription WHERE [id] = @id">
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="TagName" Type="String" />
            <asp:Parameter Name="TagDescription" Type="String" />
            <asp:Parameter Name="id" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="TagName" Type="String" />
            <asp:Parameter Name="TagDescription" Type="String" />
        </InsertParameters>
    </asp:SqlDataSource>
</asp:Content>

