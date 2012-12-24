<%@ Page Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" Inherits="AgendaUpdate"
    Title="Agenda Update" Codebehind="AgendaUpdate.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="blankContent" runat="Server">
 
    <a href="AgendaUpdateByTime.aspx">AgendaUpdateByTime.aspx (assigns room 0)</a> &nbsp;&nbsp;


    --------

    <a href="AgendaUpdateSpeakersMultipleSessions.aspx">AgendaUpdateSpeakersMultipleSessions.aspx</a>

    <asp:SqlDataSource ID="SqlDataSourceSessionTimes" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        
        SelectCommand="SELECT [id], [Description] FROM [SessionTimes] WHERE id &lt;&gt; 10 AND CodeCampYearId = @CodeCampYearId ORDER BY [StartTime]">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:Label ID="LabelCodeCampYearId" Visible="false" runat="server" Text="Label"></asp:Label>

    <br />
    <asp:DropDownList ID="DropDownListSessionTimes" runat="server" DataSourceID="SqlDataSourceSessionTimes"
        DataTextField="Description" DataValueField="id" AutoPostBack="True" OnSelectedIndexChanged="DropDownListSessionTimes_SelectedIndexChanged">
    </asp:DropDownList>
    <asp:CheckBox ID="CheckBoxShowRoomPictures" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBoxShowRoomPictures_CheckedChanged"
        Text="Show Rooms" Checked="true" />

    <br/>
    <asp:Button ID="Button1" runat="server" OnClientClick="return confirm('OK to Unassign all Rooms?');"
        OnClick="ButtonUnassignAllRooms_Click" Text="UnAssign All Rooms For All Time Slots" />

    <asp:Button ID="Button2" runat="server" OnClientClick="return confirm('OK to Unassign all Times?');"
        OnClick="ButtonUnassignAllTimes_Click" Text="UnAssign All Times For All Time Slots" />

    <asp:Button ID="Button3" runat="server" OnClientClick="return confirm('OK to AutoAssign All Rooms?');"
        OnClick="ButtonAutoAssignRooms_Click" Text="Auto Assign Rooms For All Time Slots" />

   
    <asp:GridView ID="GridViewRooms" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSourceRooms" CellSpacing="3"
        OnRowCommand="GridViewRooms_RowCommand" 
        EnableSortingAndPagingCallbacks="False">
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <table cellpadding="3">
                        <tr>
                            <td>
                                <asp:LinkButton ID="LinkButtonAssignSession" runat="server" CausesValidation="False"
                                    CommandName="AssignSession" Text="Assign Session" CommandArgument='<%# Eval("id") + "^all"  %>'>
                                </asp:LinkButton>
                                <asp:LinkButton ID="LinkButtonClearSession" runat="server" CausesValidation="False"
                                    CommandName="ClearSession" Text="Clear Session" CommandArgument='<%# Eval("id") + "^all"  %>'>
                                </asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LabelPresenter" runat="server" Text='<%# (string) GetPresenterByRoomId( (int) Eval("id") ) %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LabelTitle" runat="server" Text='<%# (string) GetSessionTitleByRoomId( (int) Eval("id") ) %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LabelInterestLevel" runat="server" Text='<%# (string) GetInterestedByRoomId( (int) Eval("id") ) %>'></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Number" HeaderText="Number" SortExpression="Number" />
            <asp:BoundField DataField="Capacity" HeaderText="Capacity" SortExpression="Capacity" />
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Image runat="server" Visible='<%# IsImageVisible() %>' ID="ImageUser" ImageUrl='<%# "~/DisplayImage.ashx?roomid=" + Eval("id")  %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSourceRooms" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        DeleteCommand="DELETE FROM [LectureRooms] WHERE [id] = @id" InsertCommand="INSERT INTO [LectureRooms] ([Number], [Capacity], [Description]) VALUES (@Number, @Capacity, @Description)"
        SelectCommand="SELECT [id], [Number], [Capacity], [Description] FROM [LectureRooms] WHERE [Available] = 1 ORDER BY Capacity DESC"
        UpdateCommand="UPDATE [LectureRooms] SET [Number] = @Number, [Capacity] = @Capacity, [Description] = @Description WHERE [id] = @id">
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Number" Type="String" />
            <asp:Parameter Name="Capacity" Type="Int32" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="id" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="Number" Type="String" />
            <asp:Parameter Name="Capacity" Type="Int32" />
            <asp:Parameter Name="Description" Type="String" />
        </InsertParameters>
    </asp:SqlDataSource>
</asp:Content>
