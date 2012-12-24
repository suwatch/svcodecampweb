<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true" Inherits="AgendaUpdateSpeakersMultipleSessions" Codebehind="AgendaUpdateSpeakersMultipleSessions.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="blankSublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" Runat="Server">

    <h3>Sessions With Speakers Back to Back</h3>

    <asp:Button ID="Button1" runat="server" 
        Text="Retreive Speakers With Multiple Sessions" onclick="Button1_Click" />

    <br />

    <asp:GridView ID="GridViewSpeakerTargets" runat="server" 
        AutoGenerateColumns="False" DataKeyNames="Id" 
        DataSourceID="ObjectDataSourceSpeakerTargets" CellPadding="10" 
        CellSpacing="5" ForeColor="#333333" GridLines="None" 
        onrowcommand="GridViewSpeakerTargets_RowCommand">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                        CommandName="Select" CommandArgument='<%# (int) Eval("Id") %>' Text="Select"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="PKID" HeaderText="PKID" SortExpression="PKID" 
                Visible="False" />
            <asp:BoundField DataField="Username" HeaderText="Username" 
                SortExpression="Username" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundField DataField="CreationDate" HeaderText="CreationDate" 
                SortExpression="CreationDate" />
            <asp:BoundField DataField="UserFirstName" HeaderText="UserFirstName" 
                SortExpression="UserFirstName" />
            <asp:BoundField DataField="UserLastName" HeaderText="UserLastName" 
                SortExpression="UserLastName" />
            <asp:BoundField DataField="Id" HeaderText="AttendeeId" ReadOnly="True" 
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
    <asp:ObjectDataSource ID="ObjectDataSourceSpeakerTargets" runat="server" 
        DataObjectTypeName="CodeCampSV.AttendeesResult" DeleteMethod="Delete" 
        InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="GetByStringIds" TypeName="CodeCampSV.AttendeesManager">
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelAttendeesId" Name="listOfAttendeeIds" 
                PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:ObjectDataSource ID="ObjectDataSourceSessions" runat="server" 
        DataObjectTypeName="CodeCampSV.SessionsResult" DeleteMethod="Delete" 
        InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="GetByStringIds" TypeName="CodeCampSV.SessionsManager">
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelSessionIds" Name="listOfIds" 
                PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="codeCampYearId" 
                PropertyName="Text" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <br />

    <asp:GridView ID="GridViewSessions" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="Id" DataSourceID="ObjectDataSourceSessions" CellPadding="4" 
        CellSpacing="5" ForeColor="#333333" GridLines="None">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />


              <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                   <asp:Label runat="server" text='<%# GetRoomAndTime((int) Eval("Id")) %>' ></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>



            <asp:BoundField DataField="PlanAheadCountInt" HeaderText="PlanAheadCountInt" 
                SortExpression="PlanAheadCountInt" />
            <asp:BoundField DataField="InterestCountInt" HeaderText="InterestCountInt" 
                SortExpression="InterestCountInt" />
            <asp:BoundField DataField="Id" HeaderText="SessionId" ReadOnly="True" 
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


    <br />
    <br />

    Rooms:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
    <asp:DropDownList ID="DropDownListActiveRooms" runat="server" 
        DataSourceID="ObjectDataSourceActiveRooms" DataTextField="RoomNumberWithCapacity"
        DataValueField="Number">
    </asp:DropDownList>
    <asp:ObjectDataSource ID="ObjectDataSourceActiveRooms" runat="server" 
        DataObjectTypeName="CodeCampSV.LectureRoomsResult" DeleteMethod="Delete" 
        InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="GetAvailableRooms" TypeName="CodeCampSV.LectureRoomsManager">
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
    </asp:ObjectDataSource>

    <br />

    List SessionIds Here With Commas: (checkbox gridview next year)
    <asp:TextBox ID="TextBoxSessionIdsForAssignment" runat="server"></asp:TextBox>
      <br />

    <asp:Button ID="ButtonAssignToRoom" runat="server" 
    Text="Assign Sessions In TextBox to Selected Room" 
    onclick="ButtonAssignToRoom_Click1" />

    <br />
    <br />
    <br />



    <asp:Label ID="LabelAttendeesId" runat="server" Visible="False"></asp:Label>
    
    <asp:Label ID="LabelSessionIds" runat="server" Visible="False"></asp:Label>

    <asp:Label ID="LabelCodeCampYearId" runat="server" Visible="False"></asp:Label>

</asp:Content>

