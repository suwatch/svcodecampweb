<%@ Page Language="C#" MasterPageFile="~/RightNote.master" AutoEventWireup="true" Inherits="PasswordUpdateImp" Title="Password Update" Codebehind="PasswordUpdateImp.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<table>
<tr>
<td>
 <asp:Button ID="ButtonSearch" runat="server" Text="Search" 
        onclick="ButtonSearch_Click" />
</td>

<td>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="TextBoxUsername" runat="server"></asp:TextBox>
</td>
</tr>

</table>
   

    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="Id" DataSourceID="ObjectDataSource2" AllowPaging="True" 
        onrowcommand="GridView2_RowCommand">
        <Columns>

         <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                            Text="Update"></asp:LinkButton>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                            Text="Cancel"></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                            Text="Edit"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="MakeCurrentUser"
                            Text="Make Current User"   CommandArgument='<%#Eval("Username") %>' ></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:LinkButton ID="LinkButtonResetPassword" runat="server" CausesValidation="False" CommandName="ResetPassword"
                            Text="Password pass@word"   CommandArgument='<%#Eval("Username") %>' ></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

            <asp:BoundField DataField="PKID" HeaderText="PKID" SortExpression="PKID" Visible="false" />


            <asp:BoundField DataField="Username" HeaderText="Username" 
                SortExpression="Username" />

                   <asp:BoundField DataField="UserFirstName" HeaderText="UserFirstName" 
                SortExpression="UserFirstName" />
            <asp:BoundField DataField="UserLastName" HeaderText="UserLastName" 
                SortExpression="UserLastName" />
           
            <asp:BoundField DataField="Password" HeaderText="Password" Visible="false"
                 />
           
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundField DataField="EmailEventBoard" HeaderText="EmailEventBoard" 
                SortExpression="EmailEventBoard" />
          
            <asp:CheckBoxField DataField="IsApproved" HeaderText="IsApproved" 
                SortExpression="IsApproved" />
          
            
            <asp:CheckBoxField DataField="IsLockedOut" HeaderText="IsLockedOut" 
                SortExpression="IsLockedOut" />

                <asp:BoundField DataField="Id" HeaderText="Id" />
          
         
           
           
          
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
        DataObjectTypeName="CodeCampSV.AttendeesResult" DeleteMethod="Delete" 
        InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="GetByMisc" TypeName="CodeCampSV.AttendeesManager" 
        UpdateMethod="Update">
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="TextBoxUsername" Name="misc" 
                PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>


  <asp:GridView ID="GridView1" BackColor="White" runat="server"
     AutoGenerateColumns="False" DataKeyNames="UserName" 
        DataSourceID="ObjectDataSource1" OnRowCommand="GridView1_RowCommand" 
        AllowPaging="True" Visible="False">
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                            Text="Update"></asp:LinkButton>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                            Text="Cancel"></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                            Text="Edit"></asp:LinkButton>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="MakeCurrentUser"
                            Text="Make Current User"   CommandArgument='<%#Eval("UserName") %>' ></asp:LinkButton>
                             <asp:LinkButton ID="LinkButtonResetPassword" runat="server" CausesValidation="False" CommandName="ResetPassword"
                            Text="Password pass@word"   CommandArgument='<%#Eval("UserName") %>' ></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ProviderName" Visible="False" HeaderText="ProviderName" ReadOnly="True"
                    SortExpression="ProviderName" />
                <asp:CheckBoxField DataField="IsOnline" Visible="False" HeaderText="IsOnline" ReadOnly="True" SortExpression="IsOnline" />
                <asp:BoundField DataField="LastPasswordChangedDate" HeaderText="LastPasswordChangedDate"
                    ReadOnly="True" SortExpression="LastPasswordChangedDate" />
                <asp:BoundField DataField="PasswordQuestion" Visible="False" HeaderText="PasswordQuestion" ReadOnly="True"
                    SortExpression="PasswordQuestion" />
                <asp:BoundField DataField="Password" HeaderText="Password" SortExpression="Password" />
                <asp:BoundField DataField="Comment" Visible="False" HeaderText="Comment" SortExpression="Comment" />
                <asp:BoundField DataField="UserName" HeaderText="UserName" ReadOnly="True" SortExpression="UserName" />
                <asp:CheckBoxField DataField="IsLockedOut" Visible="False" HeaderText="IsLockedOut" ReadOnly="True"
                    SortExpression="IsLockedOut" />
                <asp:BoundField DataField="CreationDate" HeaderText="CreationDate" ReadOnly="True"
                    SortExpression="CreationDate" />
                <asp:CheckBoxField DataField="IsApproved" HeaderText="IsApproved" SortExpression="IsApproved" />
                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                <asp:BoundField DataField="LastLockoutDate" Visible="False" HeaderText="LastLockoutDate" ReadOnly="True"
                    SortExpression="LastLockoutDate" />
                <asp:BoundField DataField="LastLoginDate" Visible="False" HeaderText="LastLoginDate" SortExpression="LastLoginDate" />
                <asp:BoundField DataField="LastActivityDate" Visible="False" HeaderText="LastActivityDate" SortExpression="LastActivityDate" />
            </Columns>
        </asp:GridView>
&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetMembersByUsername" 
        TypeName="MembershipUtilities.MembershipUserODS" UpdateMethod="Update" 
        OldValuesParameterFormatString="original_{0}">
            <DeleteParameters>
                <asp:Parameter Name="UserName" Type="String" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="UserName" Type="String" />
                <asp:Parameter Name="email" Type="String" />
                <asp:Parameter Name="isApproved" Type="Boolean" />
                <asp:Parameter Name="password" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="TextBoxUsername" Name="userName" 
                    PropertyName="Text" Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="userName" Type="String" />
                <asp:Parameter Name="isApproved" Type="Boolean" />
                <asp:Parameter Name="comment" Type="String" />
                <asp:Parameter Name="lastLockoutDate" Type="DateTime" />
                <asp:Parameter Name="creationDate" Type="DateTime" />
                <asp:Parameter Name="email" Type="String" />
                <asp:Parameter Name="lastActivityDate" Type="DateTime" />
                <asp:Parameter Name="providerName" Type="String" />
                <asp:Parameter Name="isLockedOut" Type="Boolean" />
                <asp:Parameter Name="lastLoginDate" Type="DateTime" />
                <asp:Parameter Name="isOnline" Type="Boolean" />
                <asp:Parameter Name="passwordQuestion" Type="String" />
                <asp:Parameter Name="lastPasswordChangedDate" Type="DateTime" />
                <asp:Parameter Name="password" Type="String" />
                <asp:Parameter Name="passwordAnswer" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>

</asp:Content>

