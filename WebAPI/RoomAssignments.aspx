<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true" Inherits="RoomAssignments" Codebehind="RoomAssignments.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="blankSublinks" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" runat="Server">
    <strong>Session Time </strong>
    <asp:DropDownList ID="DropDownListSessionTimes" runat="server" AutoPostBack="True"
        DataSourceID="SqlDataSourceSessionTime" DataTextField="StartTimeFriendly" DataValueField="id"
        Font-Size="Large" AppendDataBoundItems="True">
       <%-- <asp:ListItem Text="All Times" Value="0"></asp:ListItem>--%>
    </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;
    <asp:SqlDataSource ID="SqlDataSourceSessionTime" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        SelectCommand="SELECT [id], [StartTime], [StartTimeFriendly] FROM [SessionTimes] ORDER BY [StartTime]">
    </asp:SqlDataSource>
   
    <div runat="server" id="DivOneTime">
      
        <asp:GridView ID="GridViewSessionTimes" runat="server" 
        AutoGenerateColumns="False" 
        DataSourceID="ObjectDataSourceSessionsByTime">
            <Columns>
                <asp:BoundField DataField="Title" HeaderText="Title" 
                    SortExpression="Title" />
                <asp:TemplateField HeaderText="SessionRoomId" SortExpression="SessionRoomId">
                    <ItemTemplate>
                      <%--  <asp:DropDownList ID="DropDownListUser" runat="server" AutoPostBack="False" 
                            SelectedValue='<%# Bind("Approved") %>'>
                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                        </asp:DropDownList>--%>

                    
                    
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("SessionRoomId") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("SessionRoomId") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
               <%-- <asp:TemplateField HeaderText="SessionStartTime" 
                    SortExpression="SessionStartTime">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("SessionStartTime") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" 
                            Text='<%# Bind("SessionStartTime") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>--%>
            </Columns>
        </asp:GridView>
      
      
      
      
    <asp:ObjectDataSource ID="ObjectDataSourceSessionsByTime" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetBySessionTime" 
        TypeName="CodeCampSV.SessionSpecialODS">
        <SelectParameters>
            <asp:ControlParameter ControlID="DropDownListSessionTimes" Name="sessionTimeId" 
                PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <br />
      
      
      
      
    </div>
  
  
  
    <div runat="server" id="DivAllRooms">
    </div>
    <asp:Label ID="LabelCodeCampYearId" runat="server" Visible="false" Text="Label"></asp:Label>
</asp:Content>
