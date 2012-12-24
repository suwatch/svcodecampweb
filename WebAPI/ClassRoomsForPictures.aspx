<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="ClassRoomsForPictures" Title="Class Rooms For Pictures" Codebehind="ClassRoomsForPictures.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" runat="Server">

    Only Shows Available Rooms<br />

    <br />


    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
    SelectCommand="SELECT [Number], [id], [Description], [Capacity] FROM [LectureRooms] 
WHERE [Available] = 1
ORDER BY [Number]"></asp:SqlDataSource>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
    DataKeyNames="id" DataSourceID="SqlDataSource2" AllowSorting="True">
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="Number" HeaderText="Number" 
                SortExpression="Number" />
            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" 
                ReadOnly="True" SortExpression="id" />
            <asp:BoundField DataField="Description" HeaderText="Description" 
                SortExpression="Description" />
            <asp:BoundField DataField="Capacity" HeaderText="Capacity" 
                SortExpression="Capacity" />
        </Columns>
    </asp:GridView>










    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        DeleteCommand="DELETE FROM [LectureRooms] WHERE [id] = @id" 
        InsertCommand="INSERT INTO [LectureRooms] ([Number], [Description], [Style], [Capacity], [Projector], [Screen]) VALUES (@Number, @Description, @Style, @Capacity, @Projector, @Screen)"
        SelectCommand="SELECT [id], [Number], [Description], [Style], [Capacity], [Projector], [Screen] FROM [LectureRooms] WHERE ([id] = @id) ORDER BY [Number]"
        
    UpdateCommand="UPDATE [LectureRooms] SET [Number] = @Number, [Description] = @Description, [Style] = @Style, [Capacity] = @Capacity, [Projector] = @Projector, [Screen] = @Screen WHERE [id] = @id">
        <SelectParameters>
            <asp:ControlParameter ControlID="GridView1" Name="id" 
                PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Number" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="Style" Type="String" />
            <asp:Parameter Name="Capacity" Type="Int32" />
            <asp:Parameter Name="Projector" Type="Boolean" />
            <asp:Parameter Name="Screen" Type="Boolean" />
            <asp:Parameter Name="id" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="Number" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="Style" Type="String" />
            <asp:Parameter Name="Capacity" Type="Int32" />
            <asp:Parameter Name="Projector" Type="Boolean" />
            <asp:Parameter Name="Screen" Type="Boolean" />
        </InsertParameters>
    </asp:SqlDataSource>
    <br />
<br />
    <asp:DetailsView ID="DetailsView1" runat="server" Height="50px" 
    Width="125px" AllowPaging="True"
        AutoGenerateRows="False" DataKeyNames="id" 
    DataSourceID="SqlDataSource1" OnItemUpdated="DetailsView1_ItemUpdated" 
    OnModeChanged="DetailsView1_ModeChanged" 
    onitemdeleted="DetailsView1_ItemDeleted" 
    oniteminserted="DetailsView1_ItemInserted" 
    onitemupdating="DetailsView1_ItemUpdating">
        <Fields>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowInsertButton="True" />
            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True"
                SortExpression="id" />
            <asp:BoundField DataField="Number" HeaderText="Number" SortExpression="Number" />
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
            <asp:BoundField DataField="Style" HeaderText="Style" SortExpression="Style" />
            <asp:BoundField DataField="Capacity" HeaderText="Capacity" SortExpression="Capacity" />
            <asp:CheckBoxField DataField="Projector" HeaderText="Projector" SortExpression="Projector" />
            <asp:CheckBoxField DataField="Screen" HeaderText="Screen" SortExpression="Screen" />
            <asp:TemplateField HeaderText="Picture">
                <ItemTemplate>
                    <asp:Image runat="server" ID="ImageUser" ImageUrl='<%# "~/DisplayImage.ashx?roomid=" + Eval("id")  %>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:FileUpload ID="FileUploadImage" runat="server" />
                </EditItemTemplate>
            </asp:TemplateField>
            
        </Fields>
    </asp:DetailsView>
    <br />
    
    <br />
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    
</asp:Content>
