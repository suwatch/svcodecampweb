<%@ Page Title="" Language="C#" MasterPageFile="~/RightRegister.master" 
MaintainScrollPositionOnPostback="true"
AutoEventWireup="true" Inherits="VolunteerJobSetup" Theme="" Codebehind="VolunteerJobSetup.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" Runat="Server">
    <p>
        <br />
    </p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<div class="mainHeading">Volunteer Job Setup</div>
    <br/>




    <asp:CheckBox ID="CheckBoxSaturday" Text="Saturday" runat="server"  Visible="false"
        AutoPostBack="True" Checked="True" 
        oncheckedchanged="CheckBoxSaturday_CheckedChanged" />    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;    
    <asp:CheckBox ID="CheckBoxSunday" Text="Sunday" runat="server"  Visible="false"
        AutoPostBack="True" Checked="True" 
        oncheckedchanged="CheckBoxSunday_CheckedChanged" />    
    <br />
    <br/>

    
    <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" 
        CellPadding="4" DataKeyNames="Id,CodeCampYearId" 
        DataSourceID="SqlDataSource1" Height="50px" 
        onitemdeleted="DetailsView1_ItemDeleted" 
        oniteminserted="DetailsView1_ItemInserted" 
        onitemupdated="DetailsView1_ItemUpdated" Width="532px" ForeColor="#333333" 
        GridLines="None" oniteminserting="DetailsView1_ItemInserting" 
        onitemupdating="DetailsView1_ItemUpdating">
        <AlternatingRowStyle BackColor="White" />
        <CommandRowStyle BackColor="#D1DDF1" Font-Bold="True" />
        <EditRowStyle BackColor="#2461BF" />
        <FieldHeaderStyle BackColor="#DEE8F5" Font-Bold="True" />
        <Fields>
            <asp:BoundField DataField="JobStartTime" HeaderText="JobStartTime" 
                SortExpression="JobStartTime" />
            <asp:BoundField DataField="JobEndTime" HeaderText="JobEndTime" 
                SortExpression="JobEndTime" />
            <asp:TemplateField HeaderText="Description" SortExpression="Description" >
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" Width="400" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                </ItemTemplate>
                <ControlStyle Width="500px" />
                <FooterStyle Width="500px" />
                <HeaderStyle Width="500px" />
                <ItemStyle Width="500px" />
            </asp:TemplateField>
            <asp:BoundField DataField="NumberNeeded" HeaderText="NumberNeeded" 
                SortExpression="NumberNeeded" />
            <asp:BoundField DataField="CodeCampYearId" HeaderText="CodeCampYearId" 
                SortExpression="CodeCampYearId" Visible="False" />
            <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" 
                ReadOnly="True" SortExpression="Id" Visible="False" />
            <asp:CommandField ShowEditButton="True" ShowInsertButton="True" 
                ShowDeleteButton="True" />
        </Fields>
        <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
    </asp:DetailsView>
    (When Putting in Dates, Always Use this exact format:&nbsp;
    <asp:Literal ID="LiteralSaturdayDateFormat" runat="server"></asp:Literal>
    <br/>
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    <br />
    <br/>

    <asp:GridView ID="GridViewVolunteerJobs" runat="server"  SkinID="gridviewSkin"
        DataSourceID="SqlDataSourceVolunteerJobs" AutoGenerateColumns="False" 
        DataKeyNames="Id" CellPadding="4" ForeColor="#333333" GridLines="None">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:CommandField ShowSelectButton="True" >
            <ControlStyle Width="80px" />
            <FooterStyle Width="80px" />
            <HeaderStyle Width="80px" />
            <ItemStyle Width="80px" />
            </asp:CommandField>
            <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" 
                ReadOnly="True"  Visible="False" />

            <asp:TemplateField HeaderText="Job Time" >
               
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# (string) GetJobFullTime((DateTime) Eval("JobStartTime"),(DateTime) Eval("JobEndTime")) %>'></asp:Label>
                </ItemTemplate>
               
                <ControlStyle Width="230px" />
                  <FooterStyle Width="230px" />
            <HeaderStyle Width="230px" />
            <ItemStyle Width="230px" />
               
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Start"  
                Visible="False">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("JobStartTime") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("JobStartTime") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="350px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="End"  Visible="False">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("JobEndTime") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("JobEndTime") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="300px" />
            </asp:TemplateField>
            <asp:BoundField DataField="Description" HeaderText="Description" 
                 >
            <ControlStyle Width="600px" />
                  <FooterStyle Width="600px" />
            <HeaderStyle Width="600px" />
            <ItemStyle Width="600px" />
            </asp:BoundField>
            <asp:BoundField DataField="NumberNeeded" HeaderText="People Needed" 
                SortExpression="NumberNeeded" />
        </Columns>
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSourceVolunteerJobs" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
        DeleteCommand="DELETE FROM [VolunteerJob] WHERE [Id] = @Id" 
        InsertCommand="INSERT INTO [VolunteerJob] ([Description], [JobStartTime], [JobEndTime], [NumberNeeded]) VALUES (@Description, @JobStartTime, @JobEndTime, @NumberNeeded)" 
        
        SelectCommand=
        "SELECT [Id], [Description], [JobStartTime], [JobEndTime], [NumberNeeded] FROM [VolunteerJob] 
        WHERE ([JobStartTime] = @JobStartTime) 
        ORDER BY [JobStartTime], [Description]"
        
        UpdateCommand="UPDATE [VolunteerJob] SET [Description] = @Description, [JobStartTime] = @JobStartTime, [JobEndTime] = @JobEndTime, [NumberNeeded] = @NumberNeeded WHERE [Id] = @Id">
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="JobStartTime" Type="DateTime" />
            <asp:Parameter Name="JobEndTime" Type="DateTime" />
            <asp:Parameter Name="NumberNeeded" Type="Int32" />
        </InsertParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelSaturday" Name="JobStartTime" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="LabelSunday" Name="JobEndTime" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="LabelCodeCampStartDate" Name="CodeCampStartDate" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="LabelCodeCampEndDate" Name="CodeCampEndDate" PropertyName="Text" Type="DateTime" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="JobStartTime" Type="DateTime" />
            <asp:Parameter Name="JobEndTime" Type="DateTime" />
            <asp:Parameter Name="NumberNeeded" Type="Int32" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <br />
    <br />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
        DeleteCommand="DELETE FROM [VolunteerJob] WHERE [Id] = @Id" 
        InsertCommand="INSERT INTO [VolunteerJob] ([JobStartTime], [JobEndTime], [Description], [NumberNeeded], [CodeCampYearId]) VALUES (@JobStartTime, @JobEndTime, @Description, @NumberNeeded, @CodeCampYearId)" 
        SelectCommand="SELECT [JobStartTime], [JobEndTime], [Description], [NumberNeeded], [CodeCampYearId], [Id] FROM [VolunteerJob] WHERE ([Id] = @Id)" 
        UpdateCommand="UPDATE [VolunteerJob] SET [JobStartTime] = @JobStartTime, [JobEndTime] = @JobEndTime, [Description] = @Description, [NumberNeeded] = @NumberNeeded, [CodeCampYearId] = @CodeCampYearId WHERE [Id] = @Id">
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="JobStartTime" Type="DateTime" />
            <asp:Parameter Name="JobEndTime" Type="DateTime" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="NumberNeeded" Type="Int32" />
            <asp:Parameter Name="CodeCampYearId" Type="Int32" />
        </InsertParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="GridViewVolunteerJobs" Name="Id" 
                PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="JobStartTime" Type="DateTime" />
            <asp:Parameter Name="JobEndTime" Type="DateTime" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="NumberNeeded" Type="Int32" />
            <asp:Parameter Name="CodeCampYearId" Type="Int32" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <br />
    <asp:Button ID="ButtonCopyPreviousYearsJobs" runat="server" 
        onclick="ButtonCopyPreviousYearsJobs_Click" 
        Text="Copy Previous Years Jobs to this year" />
    <br />
    <br />
    <asp:Label ID="LabelCodeCampYearId" runat="server" Text="Label"></asp:Label>
    <asp:Label ID="LabelSaturday" Visible="false" runat="server" Text=""></asp:Label>
    <asp:Label ID="LabelSunday" Visible="false" runat="server" Text=""></asp:Label>
       <asp:Label ID="LabelCodeCampStartDate" Visible="false" runat="server" Text=""></asp:Label>
    <asp:Label ID="LabelCodeCampEndDate" Visible="false" runat="server" Text=""></asp:Label>

    <br />

</asp:Content>

