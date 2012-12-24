<%@ Page Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true" Inherits="EmailOptOut" Title="Untitled Page" Codebehind="EmailOptOut.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="blankSublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" Runat="Server">
    <br />
    <asp:DetailsView ID="DetailsView1" runat="server" AllowPaging="True" 
        AutoGenerateRows="False" DataKeyNames="id" DataSourceID="SqlDataSource2" 
        Height="50px" Width="125px" onitemdeleted="DetailsView1_ItemDeleted" 
    onitemdeleting="DetailsView1_ItemDeleting" 
    oniteminserted="DetailsView1_ItemInserted1" 
    oniteminserting="DetailsView1_ItemInserting">
        <Fields>
            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" 
                ReadOnly="True" SortExpression="id" />
            <asp:BoundField DataField="email" HeaderText="email" SortExpression="email" />
            <asp:BoundField DataField="DateAdded" HeaderText="DateAdded" 
                SortExpression="DateAdded" />
            <asp:BoundField DataField="comment" HeaderText="comment" 
                SortExpression="comment" />
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>
    <br />
    
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
        DeleteCommand="DELETE FROM [EmailOptOut] WHERE [id] = @id" 
        InsertCommand="INSERT INTO [EmailOptOut] ([email], [DateAdded], [comment]) VALUES (@email, @DateAdded, @comment)" 
        SelectCommand="SELECT [id], [email], [DateAdded], [comment] FROM [EmailOptOut]" 
        UpdateCommand="UPDATE [EmailOptOut] SET [email] = @email, [DateAdded] = @DateAdded, [comment] = @comment WHERE [id] = @id">
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="email" Type="String" />
            <asp:Parameter Name="DateAdded" Type="DateTime" />
            <asp:Parameter Name="comment" Type="String" />
            <asp:Parameter Name="id" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="email" Type="String" />
            <asp:Parameter Name="DateAdded" Type="DateTime" />
            <asp:Parameter Name="comment" Type="String" />
        </InsertParameters>
    </asp:SqlDataSource>
    <br />
    <br />
    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" 
        AutoGenerateColumns="False" DataSourceID="SqlDataSourceEmailList">
        <Columns>
            <asp:BoundField DataField="email" HeaderText="email" SortExpression="email" />
            <asp:BoundField DataField="DateAdded" HeaderText="DateAdded" 
                SortExpression="DateAdded" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSourceEmailList" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
        SelectCommand="SELECT [email], [DateAdded] FROM [EmailOptOut] ORDER BY [email]"></asp:SqlDataSource>
    
    <br />
    <br />
    
</asp:Content>

