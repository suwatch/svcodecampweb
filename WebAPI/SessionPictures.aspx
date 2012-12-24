<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="SessionPictures" Title="Untitled Page" Codebehind="SessionPictures.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<h1>Enter Sessions Pictures</h1>
    <asp:Button ID="ButtonUpload" runat="server" OnClick="ButtonUpload_Click" Text="Upload File" />
<asp:FileUpload ID="FileUploadImages" runat="server" />
    <br />
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" /><br />
    <br />


    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" DataSourceID="SqlDataSource1" AutoGenerateColumns="False">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
            <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" SortExpression="DateCreated" />
            <asp:BoundField DataField="FileName" HeaderText="FileName" SortExpression="FileName" />
            <asp:TemplateField HeaderText="id" InsertVisible="False" SortExpression="id">
                <EditItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Image ID="Image1" runat="server"  ImageUrl='<%# "~/DisplayImage.ashx?sizex=150&pictureid=" + Eval("id")  %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
        </Columns>
    </asp:GridView>
  <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" DeleteCommand="DELETE FROM [Pictures] WHERE [id] = @id" InsertCommand="INSERT INTO [Pictures] ([AttendeePKID], [DateCreated], [DateUpdated], [PictureBytes], [FileName], [Description]) VALUES (@AttendeePKID, @DateCreated, @DateUpdated, @PictureBytes, @FileName, @Description)" SelectCommand="SELECT [id], [AttendeePKID], [DateCreated], [DateUpdated], [PictureBytes], [FileName], [Description] FROM [Pictures]" UpdateCommand="UPDATE [Pictures] SET [AttendeePKID] = @AttendeePKID, [DateCreated] = @DateCreated, [DateUpdated] = @DateUpdated, [PictureBytes] = @PictureBytes, [FileName] = @FileName, [Description] = @Description WHERE [id] = @id">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="AttendeePKID" Type="Object" />
                <asp:Parameter Name="DateCreated" Type="DateTime" />
                <asp:Parameter Name="DateUpdated" Type="DateTime" />
                <asp:Parameter Name="PictureBytes" Type="Object" />
                <asp:Parameter Name="FileName" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:Parameter Name="id" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="AttendeePKID" Type="Object" />
                <asp:Parameter Name="DateCreated" Type="DateTime" />
                <asp:Parameter Name="DateUpdated" Type="DateTime" />
                <asp:Parameter Name="PictureBytes" Type="Object" />
                <asp:Parameter Name="FileName" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>

</asp:Content>

