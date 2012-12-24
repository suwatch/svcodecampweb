<%@ Page Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true" Inherits="DefaultDeleteTags" Title="Untitled Page" Codebehind="DefaultDeleteTags.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="blankSublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" Runat="Server">
    <br />
    <p>This shows for deleting just the tags that are not used in any session currently</p>
    <br />

    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server"></asp:ObjectDataSource>
    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" 
        AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSource1">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" />
            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" 
                ReadOnly="True" SortExpression="id" />
            <asp:BoundField DataField="TagName" HeaderText="TagName" 
                SortExpression="TagName" />
        </Columns>
    </asp:GridView>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
        DeleteCommand="DELETE FROM [Tags] WHERE [id] = @id" 
        InsertCommand="INSERT INTO [Tags] ([TagName]) VALUES (@TagName)" 
        SelectCommand="SELECT [id], [TagName] FROM [Tags] WHERE id not in (
select distinct tagid from sessiontags) ORDER BY [TagName]" 
        UpdateCommand="UPDATE [Tags] SET [TagName] = @TagName WHERE [id] = @id">
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="TagName" Type="String" />
            <asp:Parameter Name="id" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="TagName" Type="String" />
        </InsertParameters>
    </asp:SqlDataSource>

</asp:Content>

