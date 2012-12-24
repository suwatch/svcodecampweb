<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ValidateRequest="false" 
    AutoEventWireup="true" Inherits="AgendaEdit" Title="Edit Agenda File" Codebehind="AgendaEdit.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" Runat="Server">
    &nbsp;
    <br />
    
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
        SelectCommand="SELECT [id], [StartTime], [StartTimeFriendly], [SessionMinutes] FROM [SessionTimes] WHERE ([id] &lt;&gt; @id) ORDER BY [StartTime]">
        <SelectParameters>
            <asp:Parameter DefaultValue="10" Name="id" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="id" DataSourceID="SqlDataSource2">
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" 
                ReadOnly="True" SortExpression="id" />
            <asp:BoundField DataField="StartTime" HeaderText="StartTime" 
                SortExpression="StartTime" />
            <asp:BoundField DataField="StartTimeFriendly" HeaderText="StartTimeFriendly" 
                SortExpression="StartTimeFriendly" />
            <asp:BoundField DataField="SessionMinutes" HeaderText="SessionMinutes" 
                SortExpression="SessionMinutes" />
        </Columns>
    </asp:GridView>
    
    
    <br />
    <br />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        
        
    SelectCommand="SELECT [id], [StartTime], [StartTimeFriendly], [EndTime], [EndTimeFriendly], [SessionMinutes], [Description], [TitleBeforeOnAgenda], [TitleAfterOnAgenda] FROM [SessionTimes] WHERE ([id] = @id) ORDER BY [StartTime]" 
    DeleteCommand="DELETE FROM [SessionTimes] WHERE [id] = @id" 
    InsertCommand="INSERT INTO [SessionTimes] ([StartTime], [StartTimeFriendly], [EndTime], [EndTimeFriendly], [SessionMinutes], [Description], [TitleBeforeOnAgenda], [TitleAfterOnAgenda]) VALUES (@StartTime, @StartTimeFriendly, @EndTime, @EndTimeFriendly, @SessionMinutes, @Description, @TitleBeforeOnAgenda, @TitleAfterOnAgenda)" 
    UpdateCommand="UPDATE [SessionTimes] SET [StartTime] = @StartTime, [StartTimeFriendly] = @StartTimeFriendly, [EndTime] = @EndTime, [EndTimeFriendly] = @EndTimeFriendly, [SessionMinutes] = @SessionMinutes, [Description] = @Description, [TitleBeforeOnAgenda] = @TitleBeforeOnAgenda, [TitleAfterOnAgenda] = @TitleAfterOnAgenda WHERE [id] = @id">
        <SelectParameters>
            <asp:ControlParameter ControlID="GridView1" Name="id" 
                PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="StartTime" Type="DateTime" />
            <asp:Parameter Name="StartTimeFriendly" Type="String" />
            <asp:Parameter Name="EndTime" Type="DateTime" />
            <asp:Parameter Name="EndTimeFriendly" Type="String" />
            <asp:Parameter Name="SessionMinutes" Type="Int32" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="TitleBeforeOnAgenda" Type="String" />
            <asp:Parameter Name="TitleAfterOnAgenda" Type="String" />
            <asp:Parameter Name="id" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="StartTime" Type="DateTime" />
            <asp:Parameter Name="StartTimeFriendly" Type="String" />
            <asp:Parameter Name="EndTime" Type="DateTime" />
            <asp:Parameter Name="EndTimeFriendly" Type="String" />
            <asp:Parameter Name="SessionMinutes" Type="Int32" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="TitleBeforeOnAgenda" Type="String" />
            <asp:Parameter Name="TitleAfterOnAgenda" Type="String" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:DetailsView ID="DetailsView1" runat="server" AllowPaging="True" AutoGenerateRows="False"
        DataKeyNames="id" DataSourceID="SqlDataSource1" Height="50px" Width="125px">
        <Fields>
            <asp:TemplateField HeaderText="id" InsertVisible="False" SortExpression="id">
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="StartTime" HeaderText="StartTime" SortExpression="StartTime" />
            <asp:TemplateField HeaderText="StartTimeFriendly" 
                SortExpression="StartTimeFriendly">
                <ItemTemplate>
                    <asp:TextBox ID="Label2" TextMode="multiLine" runat="server" Enabled="false"
                    Height="100"  Width="500" Text='<%# Bind("StartTimeFriendly") %>'></asp:TextBox>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox3" runat="server"  TextMode="multiLine" Height="100" Width="500"
                        Text='<%# Bind("StartTimeFriendly") %>'></asp:TextBox>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="TextBox3" runat="server" TextMode="multiLine" Height="100" Width="500"
                        Text='<%# Bind("StartTimeFriendly") %>'></asp:TextBox>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="EndTime" HeaderText="EndTime" SortExpression="EndTime" />
            <asp:BoundField DataField="EndTimeFriendly" HeaderText="EndTimeFriendly" SortExpression="EndTimeFriendly" />
            <asp:BoundField DataField="SessionMinutes" HeaderText="SessionMinutes" SortExpression="SessionMinutes" />
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
            <asp:TemplateField HeaderText="TitleBeforeOnAgenda" SortExpression="TitleBeforeOnAgenda">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" TextMode="multiLine" Width="500" Height="200"  runat="server" Text='<%# Bind("TitleBeforeOnAgenda") %>'></asp:TextBox>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="TextBox1" TextMode="multiLine" Width="500" Height="200"   runat="server" Text='<%# Bind("TitleBeforeOnAgenda") %>'></asp:TextBox>
                </InsertItemTemplate>
                <ItemTemplate>
                     <asp:TextBox ID="TextBox1" Enabled="false" TextMode="multiLine" Width="500" Height="200"   runat="server" Text='<%# Bind("TitleBeforeOnAgenda") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="TitleAfterOnAgenda" SortExpression="TitleAfterOnAgenda">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server" TextMode="multiLine" Width="500" Height="200"   Text='<%# Bind("TitleAfterOnAgenda") %>'></asp:TextBox>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server" TextMode="multiLine" Width="500" Height="200"   Text='<%# Bind("TitleAfterOnAgenda") %>'></asp:TextBox>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:TextBox ID="TextBox2" Enabled="false" runat="server" TextMode="multiLine" Width="500" Height="200"   Text='<%# Bind("TitleAfterOnAgenda") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>
</asp:Content>

