<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/RightNote.master" AutoEventWireup="true" Inherits="VolunteerForJob" Codebehind="VolunteerForJob.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
   <div id="Div3" runat="server">
     <h2 class="mainHeading">
            <asp:Label ID="VolunteerCountsId" runat="server" Font-Bold="True" Font-Size="Large"  ></asp:Label>
    </h2>
   </div>

   
   <div id="Div1" runat="server">
     <h2 class="mainHeading">
            My Jobs I&#39;ve Volunteered For:&nbsp;&nbsp;&nbsp;
    </h2>
   </div>
  
    <div class="dataGrid">
    <asp:Label ID="LabelProblemText" BackColor="Yellow"  ForeColor="Red" 
        runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>

    <asp:GridView ID="GridViewMyJobs" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSourceMyJobs" Width="800px"  DataKeyNames="Id" CellPadding="4"
        EmptyDataText="You Have Not Volunteered For Any Jobs Yet This Year" 
        onrowcommand="GridViewMyJobs_RowCommand" ForeColor="#333333" GridLines="None">

         <EditRowStyle BackColor="#999999" />

         <emptydatarowstyle backcolor="LightBlue"
          forecolor="Red"/>
         
          
     
         <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
         
          
     
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                        CommandArgument='<%# Eval("Id") %>'
                        CommandName="Select" Text="UnVolunteer For This Job"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="JobStartTime" HeaderText="Start Time" 
                SortExpression="JobStartTime">
            <ControlStyle Width="150px" />
            <HeaderStyle Width="150px" />
            <ItemStyle Width="150px" />
            </asp:BoundField>
            <asp:BoundField DataField="JobEndTime" HeaderText="End Time" 
                SortExpression="JobEndTime">
            <ItemStyle Width="150px" />
            </asp:BoundField>
            <asp:BoundField DataField="Description" HeaderText="Description" 
                SortExpression="Description">
            <ControlStyle Width="350px" />
            <FooterStyle Width="350px" />
            <HeaderStyle Width="350px" />
            <ItemStyle Width="350px" />
            </asp:BoundField>
            <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" 
                Visible="False">
            <ItemStyle Width="250px" />
            </asp:BoundField>
            
            
             <asp:TemplateField HeaderText="#Volunteered">
                    <ItemStyle Width="325px" />
                    <ItemTemplate>
                       Volunteers Assigned/Needed: (
                       <asp:Label ID="Label1" runat="server" Text='<%# (string) GetNumberAlreadyVolunteeredByJob((int) Eval("VolunteerJobId")) %>' ></asp:Label>
                        /
                         <asp:Label ID="Label2" runat="server" Text='<%# Eval("NumberNeeded") %>' ></asp:Label>
                        )
                    </ItemTemplate>
                </asp:TemplateField>
            

        </Columns>
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
    <asp:SqlDataSource ID="SqlDataSourceMyJobs" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
        SelectCommand="SELECT  
       dbo.VolunteerJob.JobStartTime,
       dbo.VolunteerJob.JobEndTime,
       dbo.VolunteerJob.NumberNeeded,
       dbo.VolunteerJob.Description,
       dbo.AttendeeVolunteer.Notes,
       dbo.AttendeeVolunteer.Id,
       dbo.AttendeeVolunteer.VolunteerJobId
        FROM dbo.AttendeeVolunteer
             INNER JOIN dbo.VolunteerJob ON (dbo.AttendeeVolunteer.VolunteerJobId =
             dbo.VolunteerJob.Id)
        WHERE dbo.VolunteerJob.CodeCampYearId = @CodeCampYearId AND
              dbo.AttendeeVolunteer.AttendeeId = @AttendeeId
        ORDER BY dbo.VolunteerJob.JobStartTime
        ">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                PropertyName="Text" />
            <asp:ControlParameter ControlID="LabelMyAttendeeId" Name="AttendeeId" 
                PropertyName="Text" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>

    <div id="Div2" runat="server">
    <hr />
    <h2 class="mainHeading">
        Available Jobs (Choose By Pressing "Volunteer For This")
    </h2>
    </div>
    
      <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
            SelectCommand="
            SELECT [Id], [CodeCampYearId], [Description], [JobStartTime], [JobEndTime], [NumberNeeded] 
            FROM [VolunteerJob] 
            WHERE ([CodeCampYearId] = @CodeCampYearId) 
            AND Id NOT IN (SELECT VolunteerJobId FROM AttendeeVolunteer WHERE AttendeeId = @AttendeeId)
            ORDER BY [JobStartTime]">
            <SelectParameters>
                <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                    PropertyName="Text" Type="Int32" />
                <asp:ControlParameter ControlID="LabelMyAttendeeId" Name="AttendeeId" 
                    PropertyName="Text" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"  CellPadding="4"
            DataKeyNames="Id" DataSourceID="SqlDataSource1" 
        onrowcommand="GridView1_RowCommand" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id") %>'
                            Enabled='<%# (bool) GetEnabledForVolunteerForThisButton((int) Eval("Id")) %>'
                            CommandName="Select" Text='<%# (string) GetTextForVolunteerForThisButton((int) Eval("Id")) %>'   ></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" 
                    ReadOnly="True" SortExpression="Id" Visible="False" ItemStyle-Width="120" >
<ItemStyle Width="120px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CodeCampYearId" HeaderText="CodeCampYearId" 
                    SortExpression="CodeCampYearId" Visible="False" />
                <asp:BoundField DataField="Description" HeaderText="Description" 
                    SortExpression="Description" ItemStyle-CssClass="Description" >
<ItemStyle CssClass="Description"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="JobStartTime" HeaderText="Start Time" 
                    SortExpression="JobStartTime" />
                <asp:BoundField DataField="JobEndTime" HeaderText="End Time" 
                    SortExpression="JobEndTime" />
                <asp:BoundField DataField="NumberNeeded" HeaderText="#Needed" 
                    SortExpression="NumberNeeded" />
                <asp:TemplateField HeaderText="#Volunteered">
                    <ItemStyle Width="200px" />
                    <ItemTemplate>
                       <asp:Label runat="server" Text='<%# GetNumberAlreadyVolunteeredByJob((int) Eval("Id")) %>' ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
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
    </div> 
    <br />
    <asp:Label ID="LabelCodeCampYearId" Visible="false" runat="server" Text=""></asp:Label>
    <asp:Label ID="LabelMyAttendeeId" Visible="false" runat="server" Text=""></asp:Label>
 
    <br />

</asp:Content>

