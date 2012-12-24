<%@ Page MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/MasterPage.master"  ValidateRequest="false"
    AutoEventWireup="true" Inherits="EmailerAdmin" 
    Title="Email Admin" Codebehind="EmailerAdmin.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" runat="Server">
    <table border="1">
        <tr>
            <td rowspan="2" style="width: 120px">
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                    <asp:ListItem>All Attendees</asp:ListItem>
                    <asp:ListItem>Custom</asp:ListItem>
                    <asp:ListItem Selected="True">All Attendees Registered This Year</asp:ListItem>
                    <asp:ListItem>All Attendees Not Registered This Year</asp:ListItem>
                    <asp:ListItem>All Presenters This Year No Shirt Size</asp:ListItem>
                    <asp:ListItem>All Presenters This Year With Shirt Size</asp:ListItem>
                    <asp:ListItem>All Presenters</asp:ListItem>
                    <asp:ListItem>GetDataByPresentersAllYears</asp:ListItem>
                    <asp:ListItem>GetAllPresentersByCodeCampYearIdNotRegistered</asp:ListItem>
                    <asp:ListItem>GetAllPresentersByCodeCampYearId</asp:ListItem>
                    <asp:ListItem>GetAllAttendeesWhereAttendeeVolunteerChecked</asp:ListItem>
                    <asp:ListItem>GetAllUsersByCodeCampYearIdWhoActuallyVolunteeredNotPresenter</asp:ListItem>
                    <asp:ListItem>GetAllUsersByCodeCampYearIdWhoActuallyNotVolunteeredNotPresenter</asp:ListItem>
                  
                 
                </asp:RadioButtonList></td>
            <td rowspan="1" style="height: 53px">
            </td>
        </tr>
        <tr>
            <td rowspan="5" style="width: 400px">
                <asp:Label ID="LabelFrom" Width="100" runat="server" Text="From: "></asp:Label>
                <asp:TextBox ID="TextBoxFrom" Width="290" runat="server"></asp:TextBox>
                <asp:Label ID="LabelSubject" Width="100" runat="server" Text="Subject: "></asp:Label>
                <asp:TextBox ID="TextBoxSubject" Width="290" runat="server"></asp:TextBox>
                <asp:TextBox ID="TextBoxNote" runat="server" Height="400px" TextMode="MultiLine"
                    Width="400px"></asp:TextBox>
                    
                    <asp:Label Text=" Sample: http://www.siliconvalley-codecamp.com/Sessions.aspx?PKID={PKID}" runat="server" />
                    
                    <br />
                <asp:Literal ID="LiteralCount" runat="server"></asp:Literal>
                    
             </td>
        </tr>
        <tr>
            <td style="width: 100px">
                <asp:Button ID="ButtonSelectAll" runat="server" Text="Select Range" OnClick="ButtonSelectAll_Click" />
                <br />
                <asp:CheckBox ID="CheckBoxIncludeNoMailAndBouncing" runat="server" 
                    oncheckedchanged="CheckBoxIncludeNoMailAndBouncing_CheckedChanged" 
                    Text="Include No Mail &amp; Bouncing" />
                <br />
                Range Start<br />
                <asp:TextBox ID="TextBoxSelectStart" Width="75" runat="server">0</asp:TextBox>
                <br />
                
                Range End
                <br />
                <asp:TextBox ID="TextBoxSelectEnd" Width="75" runat="server">99999</asp:TextBox>
                <br />
               <%-- <asp:CheckBox ID="CheckBoxIncludePacketOnlyPeople" runat="server" 
                    AutoPostBack="True" Text="Include Packet Only People" />--%>
                 <br />
                 <asp:TextBox ID="TextBoxStartAlpha" Width="25" runat="server">A</asp:TextBox><br />
                 <asp:TextBox ID="TextBoxEndAlpha" Width="25" runat="server">Z</asp:TextBox>

            
            </td>
              
        </tr>
        <tr>
            <td style="width: 100px">
                <asp:Button ID="ButtonUnSelectAll" runat="server" Text="Deselect All" OnClick="ButtonUnSelectAll_Click" /></td>
        </tr>
        <tr>
            <td style="width: 100px">
                <asp:Label ID="LabelCount" runat="server" Text="Label"></asp:Label></td>
        </tr>
        <%--<tr>
            <td style="width: 100px">
                <asp:CheckBox ID="CheckBoxIncludeCalandarAttachment" runat="server" Text="Include Calendar Attach" /></td>
        </tr>--%>
        <tr>
            <td style="width: 100px">
                Emails/Hr: 
                <asp:TextBox ID="TextBoxEmailsPerHour" Width="25" runat="server">200</asp:TextBox>
                <br />
                Repeat Cnt: 
                <asp:TextBox ID="TextBoxRepeatEachEmail" Width="25"  runat="server">1</asp:TextBox>
            </td>
        </tr>
    </table>
    <asp:Button ID="ButtonProcessSelection" runat="server" Text="Send All Mail" OnClick="ButtonProcessSelection_Click" />
    <br />
    <asp:Button ID="ButtonProcessSelectionToEmailDetail" runat="server" 
        Text="Send All Mail To EmailDetail" 
        OnClick="ButtonProcessSelectionToEmailDetail_Click"  />
    
    <asp:Button runat="server" ID="ButtonClearEmailDetailRecordsAll" Text="Delete All EmailDetails Records"  OnClick="ButtonClearEmailDetailRecordsAll_OnClick"/>
    <br />
    <asp:Button ID="ExcelExcelDownload" runat="server" 
        onclick="ExcelExcelDownload_Click" Text="Excel Download" />
    <br />
    <asp:Button ID="XMLDownload0" runat="server" 
        onclick="XMLDownload0_Click" Text="XML download" />
    <br />
    <asp:Button ID="ButtonResetSend" runat="server" Text="Re-enable Send Mail Button"
        OnClick="ButtonResetSend_Click" /><br />
    <asp:Button ID="ButtonShowStatus" runat="server" Text="Show Sent Status" OnClick="ButtonShowStatus_Click" /><br />
    <asp:Button ID="ButtonCancel" runat="server" Text="Abort Mail Sending" OnClick="ButtonCancel_Click" /><br />
    <asp:Label ID="LabelSentStatus" runat="server" Text=""></asp:Label><br />
    <asp:Label ID="LabelCurrentStatus" runat="server" Text=""></asp:Label><br />
    <br />
    <asp:Button ID="ButtonPutEmailInTextBox" runat="server" Text="Put Selected Email In Textbox" OnClick="ButtonPutEmailInTextBox_Click" /><br />
    <br />
    <asp:TextBox ID="TextBoxEmails" runat="server" TextMode="MultiLine" Width="400" Height="300" Visible="False" ></asp:TextBox><br />
    <br />
    Custom Sql:<br/>
    <asp:TextBox ID="TextBoxCustomSqlWhere" runat="server" Height="200px" TextMode="MultiLine"
        Width="800px" ontextchanged="TextBoxCustomSqlWhere_TextChanged"></asp:TextBox>
    <br />
    
    ---
    <br/>
    <asp:CheckBoxList ID="CheckBoxListEmail" runat="server" CellPadding="2" DataSourceID="ObjectDataSourceAttendees"
        DataTextField="Email" DataValueField="Email" 
        ondatabinding="CheckBoxListEmail_DataBinding" 
        ondatabound="CheckBoxListEmail_DataBound" >
    </asp:CheckBoxList><br />
    <asp:ObjectDataSource ID="ObjectDataSourceAttendees" runat="server"
        SelectMethod="GetAllUsersByCodeCampYearId" 
        TypeName="DataSetAttendeesTableAdapters.AttendeesTableAdapter" >
         <SelectParameters>
             <asp:ControlParameter ControlID="LabelCurrentCodeCampYearId" Name="CodeCampYearId" 
                 PropertyName="Text" Type="Int32" />
          </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Label ID="LabelCurrentCodeCampYearId" runat="server" Text="Label"></asp:Label>
  
    <br />
    <br />
  
    
    <br />
    ..select id from attendees where id &gt; 6000&nbsp;&nbsp;&nbsp; and check custom in 
    radiobox on top&nbsp; (3x)<br />


</asp:Content>
