<%@ Page MaintainScrollPositionOnPostback="true" ValidateRequest="false" Language="C#"
    MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="PostEdit" Title="Post Editor" Codebehind="PostEdit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" runat="Server">
    <asp:DetailsView ID="DetailsView1" runat="server" AllowPaging="True" 
        AutoGenerateRows="False" DataKeyNames="ID" DataSourceID="SqlDataSource1" 
         CellPadding="5" CellSpacing="5"  >
        <Fields>
            <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" 
                ReadOnly="True" SortExpression="ID" />
            <asp:BoundField DataField="post_date" HeaderText="post_date" 
                SortExpression="post_date" />
            <asp:TemplateField HeaderText="post_content" SortExpression="post_content">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" TextMode="MultiLine" Height="500" Width="750"  runat="server" Text='<%# Bind("post_content") %>'></asp:TextBox>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="TextBox1" TextMode="MultiLine" Height="500" Width="750"   runat="server" Text='<%# Bind("post_content") %>'></asp:TextBox>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" Enabled="false" runat="server" TextMode="MultiLine" Height="500" Width="750"  Text='<%# Bind("post_content") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="post_title" SortExpression="post_title">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox2" Width="550" runat="server" Text='<%# Bind("post_title") %>'></asp:TextBox>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="TextBox2" Width="550" runat="server" Text='<%# Bind("post_title") %>'></asp:TextBox>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label2" Width="550" runat="server" Text='<%# Bind("post_title") %>'></asp:Label>
                </ItemTemplate>
                <ControlStyle Width="100px" />
            </asp:TemplateField>
            <asp:BoundField DataField="post_status" HeaderText="post_status" 
                SortExpression="post_status" />
            <asp:BoundField DataField="post_excerpt" HeaderText="post_excerpt" 
                SortExpression="post_excerpt" />
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        DeleteCommand="DELETE FROM [wp_posts] WHERE [ID] = @ID" 
        InsertCommand="INSERT INTO [wp_posts] ([post_date], [post_content], [post_title], [post_status],[post_excerpt]) VALUES (@post_date, @post_content, @post_title, @post_status,@post_excerpt)"
        SelectCommand="SELECT [ID], [post_date], [post_content], [post_title], [post_status],[post_excerpt]  FROM [wp_posts] ORDER BY [post_date] DESC"
        
        UpdateCommand="UPDATE [wp_posts] SET [post_date] = @post_date, [post_content] = @post_content, [post_title] = @post_title, [post_status] = @post_status, [post_excerpt]=@post_excerpt  WHERE [ID] = @ID">
        <DeleteParameters>
            <asp:Parameter Name="ID" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="post_date" Type="DateTime" />
            <asp:Parameter Name="post_content" Type="String" />
            <asp:Parameter Name="post_title" Type="String" />
            <asp:Parameter Name="post_status" Type="String" />
            <asp:Parameter Name="post_excerpt" />
            <asp:Parameter Name="ID" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="post_date" Type="DateTime" />
            <asp:Parameter Name="post_content" Type="String" />
            <asp:Parameter Name="post_title" Type="String" />
            <asp:Parameter Name="post_status" Type="String" />
            <asp:Parameter Name="post_excerpt" />
        </InsertParameters>
    </asp:SqlDataSource>
</asp:Content>
