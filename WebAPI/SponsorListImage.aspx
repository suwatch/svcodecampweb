<%@ Page Language="C#" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" Inherits="SponsorListImage" Codebehind="SponsorListImage.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" DataSourceID="SqlDataSource1" OnRowUpdating="GridView1_RowUpdating" Width="778px">
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="SponsorName" HeaderText="SponsorName" SortExpression="SponsorName" />
                <asp:BoundField DataField="ImageURL" HeaderText="ImageURL" SortExpression="ImageURL" />
                <asp:TemplateField HeaderText="Id" InsertVisible="False" SortExpression="Id">
                    <EditItemTemplate>
                        <asp:Label ID="LabelId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="LabelId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Image Type">
                    
                    <EditItemTemplate>
                        
                        <asp:FileUpload ID="FileUploadImageId" runat="server" />
                        

                    </EditItemTemplate>
                    
                    <ItemTemplate>
                        
                      <asp:Label ID="LabelIdType" runat="server" Text='<%# Bind("CompanyImageType") %>'></asp:Label>

                    </ItemTemplate>

                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
            DeleteCommand="DELETE FROM [SponsorList] WHERE [Id] = @Id" 
            InsertCommand="INSERT INTO [SponsorList] ([SponsorName], [ImageURL]) VALUES (@SponsorName, @ImageURL)" 
            SelectCommand="SELECT [SponsorName], [ImageURL],[CompanyImageType], [Id] FROM [SponsorList] ORDER BY [SponsorName]" 
            UpdateCommand="UPDATE [SponsorList] SET [SponsorName] = @SponsorName, [ImageURL] = @ImageURL WHERE [Id] = @Id">
            <DeleteParameters>
                <asp:Parameter Name="Id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="SponsorName" Type="String" />
                <asp:Parameter Name="ImageURL" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="SponsorName" Type="String" />
                <asp:Parameter Name="ImageURL" Type="String" />
                <asp:Parameter Name="Id" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
