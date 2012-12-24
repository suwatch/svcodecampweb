<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true"
MasterPageFile="~/RightRegister.master" AutoEventWireup="true" Inherits="ClassRooms" Codebehind="ClassRooms.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <p>For Adding Rooms:&nbsp; 
    <a href="ClassRoomsForPictures.aspx">ClassRoomsForPictures.aspx</a></p>

    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        CellPadding="4" DataKeyNames="Id" DataSourceID="ObjectDataSource1" 
        ForeColor="#333333" GridLines="None">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
            <asp:CheckBoxField DataField="Available" HeaderText="Available" 
                SortExpression="Available" />
            <asp:BoundField DataField="Number" HeaderText="Number" 
                SortExpression="Number" />
            <asp:BoundField DataField="Capacity" HeaderText="Capacity" 
                SortExpression="Capacity" />
            <asp:BoundField DataField="Description" HeaderText="Description" 
                SortExpression="Description">
            <ItemStyle Width="300px" />
            </asp:BoundField>
            <asp:CheckBoxField DataField="Projector" HeaderText="Projector" 
                SortExpression="Projector" />
            <asp:CheckBoxField DataField="Screen" HeaderText="Screen" 
                SortExpression="Screen" />
            <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" 
                SortExpression="Id" />
        </Columns>
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        DataObjectTypeName="CodeCampSV.LectureRoomsResult" DeleteMethod="Delete" 
        InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="GetAll" TypeName="CodeCampSV.LectureRoomsManager" 
        UpdateMethod="Update">
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
    </asp:ObjectDataSource>

</asp:Content>

