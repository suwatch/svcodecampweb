<%@ Page Language="C#" ValidateRequest="false"   AutoEventWireup="true" Inherits="TrackEditorImage" Codebehind="TrackEditorImage.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" DeleteCommand="DELETE FROM [Track] WHERE [Id] = @Id" InsertCommand="INSERT INTO [Track] ([CodeCampYearId], [Named], [Visible]) VALUES (@CodeCampYearId, @Named, @Visible)" SelectCommand="SELECT [Id], [CodeCampYearId], [Named], [Visible] FROM [Track] ORDER BY [Id] DESC" UpdateCommand="UPDATE [Track] SET [CodeCampYearId] = @CodeCampYearId, [Named] = @Named,  [Visible] = @Visible WHERE [Id] = @Id">
            <DeleteParameters>
                <asp:Parameter Name="Id" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="CodeCampYearId" Type="Int32" />
                <asp:Parameter Name="Named" Type="String" />
               
                <asp:Parameter Name="Visible" Type="Boolean" />
                
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="CodeCampYearId" Type="Int32" />
                <asp:Parameter Name="Named" Type="String" />
               
                <asp:Parameter Name="Visible" Type="Boolean" />
                <asp:Parameter Name="Id" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
    </div>
        <p>
            &nbsp;</p>
        <asp:DetailsView ID="DetailsView1" runat="server" Height="50px" Width="125px" AutoGenerateRows="False" DataKeyNames="Id" DataSourceID="SqlDataSource1" AllowPaging="True" OnItemUpdating="DetailsView1_ItemUpdating">
            <Fields>
                <asp:TemplateField HeaderText="Id" InsertVisible="False" SortExpression="Id">
                    <EditItemTemplate>
                        <asp:Label ID="LabelIdField" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CodeCampYearId" HeaderText="CodeCampYearId" SortExpression="CodeCampYearId" />
                <asp:BoundField DataField="Named" HeaderText="Named" SortExpression="Named" />
               
                <asp:CheckBoxField DataField="Visible" HeaderText="Visible" SortExpression="Visible" />
                <asp:TemplateField ConvertEmptyStringToNull="False" HeaderText="TrackImage" >
                    <EditItemTemplate>
                        <asp:FileUpload ID="FileUploadImageId"  runat="server" />
                    </EditItemTemplate>
                  
                    <ItemTemplate>
                       <asp:Image runat="server" ImageUrl=<%# GetImageUrl((int) Eval("Id")) %>  />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" />
            </Fields>
        </asp:DetailsView>
    </form>
</body>
</html>
