<%@ Page Language="C#" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" Inherits="_Default" Title="Membership CodeCamp SFBA" Codebehind="Membership.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="ButtonAssignPresentersRolePresenter" runat="server" Text="Assign Role presenter to all presenters"
            OnClick="ButtonAssignPresentersRolePresenter_Click" />
        <br />
        <br />
        Username:&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="TextBoxUsernameSearch" runat="server" Width="272px"></asp:TextBox>
        &nbsp;&nbsp;
        <asp:Button ID="ButtonUsernameSearch" runat="server" OnClick="ButtonUsernameSearch_Click"
            Text="Search" />
        &nbsp;&nbsp; (remember to press select below after search)
        <br />
        <table border="0">
            <tr>
                <td>
                    <asp:GridView ID="GridViewMemberUser" runat="server" OnSelectedIndexChanged="GridViewMembershipUser_SelectedIndexChanged"
                        OnRowDeleted="GridViewMembership_RowDeleted" AllowPaging="True" AutoGenerateColumns="False"
                        DataKeyNames="UserName" DataSourceID="ObjectDataSourceMembershipUser" AllowSorting="True"
                        PageSize="20">
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                        Text="Edit"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Select"
                                        Text="Select"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                                        Text="Update"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                        Text="Cancel"></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="UserName" HeaderText="UserName" ReadOnly="True" SortExpression="UserName" />
                            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                            <asp:BoundField DataField="PasswordQuestion" HeaderText="PasswordQuestion" ReadOnly="True"
                                SortExpression="PasswordQuestion" />
                            <asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment" />
                            <asp:BoundField DataField="CreationDate" HeaderText="CreationDate" ReadOnly="True"
                                SortExpression="CreationDate" />
                            <asp:CheckBoxField DataField="IsApproved" HeaderText="IsApproved" SortExpression="IsApproved" />
                            <asp:BoundField DataField="LastLockoutDate" Visible="False" HeaderText="LastLockoutDate"
                                ReadOnly="True" SortExpression="LastLockoutDate" />
                            <asp:BoundField DataField="LastLoginDate" HeaderText="LastLoginDate" SortExpression="LastLoginDate" />
                            <asp:CheckBoxField DataField="IsOnline" Visible="False" HeaderText="IsOnline" ReadOnly="True"
                                SortExpression="IsOnline" />
                            <asp:CheckBoxField DataField="IsLockedOut" HeaderText="IsLockedOut" ReadOnly="True"
                                SortExpression="IsLockedOut" Visible="False" />
                            <asp:BoundField DataField="LastActivityDate" HeaderText="LastActivityDate" SortExpression="LastActivityDate"
                                Visible="False" />
                            <asp:BoundField DataField="LastPasswordChangedDate" HeaderText="LastPasswordChangedDate"
                                Visible="False" ReadOnly="True" SortExpression="LastPasswordChangedDate" />
                            <asp:BoundField DataField="ProviderName" HeaderText="ProviderName" ReadOnly="True"
                                Visible="False" SortExpression="ProviderName" />
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="ObjectDataSourceMembershipUser" runat="server" DeleteMethod="Delete"
                        InsertMethod="Insert" SelectMethod="GetMembersByUsername" TypeName="MembershipUtilities.MembershipUserODS"
                        SortParameterName="SortData" OnInserted="ObjectDataSourceMembershipUser_Inserted"
                        UpdateMethod="Update">
                        <DeleteParameters>
                            <asp:Parameter Name="UserName" Type="String" />
                        </DeleteParameters>
                        <SelectParameters>
                            <asp:ControlParameter ControlID="TextBoxUsernameSearch" Name="userName" PropertyName="Text"
                                Type="String" />
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
                        <UpdateParameters>
                            <asp:Parameter Name="UserName" Type="String" />
                            <asp:Parameter Name="email" Type="String" />
                            <asp:Parameter Name="isApproved" Type="Boolean" />
                            <asp:Parameter Name="comment" Type="String" />
                            <asp:Parameter Name="lastActivityDate" Type="DateTime" />
                            <asp:Parameter Name="lastLoginDate" Type="DateTime" />
                            <asp:Parameter Name="password" Type="String" />
                        </UpdateParameters>
                    </asp:ObjectDataSource>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <asp:GridView ID="GridViewRole" runat="server" AutoGenerateColumns="False" DataSourceID="ObjectDataSourceRoleObject"
                        DataKeyNames="RoleName" CellPadding="3" CellSpacing="3">
                        <Columns>
                            <asp:CommandField ShowDeleteButton="True" DeleteText="Delete Role" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="Button1" runat="server" CausesValidation="false" Width="500px" OnClick="ToggleInRole_Click"
                                        Text='<%# ShowInRoleStatus( (string) Eval("UserName"),(string) Eval("RoleName")) %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="NumberOfUsersInRole" HeaderText="Number Of Users In Role"
                                SortExpression="NumberOfUsersInRole" />
                            <asp:BoundField DataField="RoleName" ReadOnly="True" Visible="False" HeaderText="RoleName"
                                SortExpression="RoleName" />
                            <asp:CheckBoxField DataField="UserInRole" HeaderText="UserInRole" Visible="False"
                                SortExpression="UserInRole" />
                        </Columns>
                    </asp:GridView>
                </td>
                <td>
                    <asp:CheckBox ID="CheckBoxShowRolesAssigned" runat="server" AutoPostBack="True" Text="Show Roles Assigned Only" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="ButtonCreateNewRole" runat="server" OnClick="ButtonCreateNewRole_Click"
                        Text="Create New Role" />
                    <asp:TextBox ID="TextBoxCreateNewRole" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <br />
        <asp:Panel ID="PanelCreateUser" runat="server" Height="50px" Width="125px" BorderColor="Black"
            BorderWidth="1px">
            <table cellpadding="3" cellspacing="3">
                <tr>
                    <td style="height: 32px">
                        <asp:Label ID="Label3" Text="UserName" runat="server"></asp:Label>
                    </td>
                    <td style="height: 32px">
                        <asp:TextBox ID="TextBoxUserName" runat="server"></asp:TextBox>
                    </td>
                    <td style="height: 32px">
                        <asp:Label ID="Label4" Text="Password" runat="server"></asp:Label>
                    </td>
                    <td style="height: 32px">
                        <asp:TextBox ID="TextBoxPassword" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" Text="PasswordQuestion" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxPasswordQuestion" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label6" Text="PasswordAnswer" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxPasswordAnswer" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" Text="Email" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxEmail" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label9" Text="Approved" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckboxApproval" runat="server"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="ButtonNewUser" runat="server" Text="Create New User" OnClick="ButtonNewUser_Click"
                            Enabled="False" />
                    </td>
                </tr>
            </table>
            <asp:Label ID="LabelInsertMessage" runat="server"></asp:Label></asp:Panel>
        <asp:ObjectDataSource ID="ObjectDataSourceRoleObject" runat="server" SelectMethod="GetRoles"
            TypeName="MembershipUtilities.RoleDataObject" InsertMethod="Insert" DeleteMethod="Delete">
            <SelectParameters>
                <asp:ControlParameter ControlID="GridViewMemberUser" Name="UserName" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="CheckBoxShowRolesAssigned" Name="ShowOnlyAssignedRolls"
                    PropertyName="Checked" Type="Boolean" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="RoleName" Type="String" />
            </InsertParameters>
            <DeleteParameters>
                <asp:Parameter Name="RoleName" Type="String" />
            </DeleteParameters>
        </asp:ObjectDataSource>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:Button ID="ButtonRemoveSubmitSessionFromAll" runat="server" Text="ButtonRemoveSubmitSessionFromAll"
            OnClick="ButtonRemoveSubmitSessionFromAll_Click" />
        <asp:Button ID="ButtonRemoveAddMoreThanTwoSessionsRoleName" runat="server" Text="ButtonRemoveAddMoreThanTwoSessionsRoleName"
            OnClick="ButtonRemoveAddMoreThanTwoSessionsRoleName_Click" />
        <asp:Button ID="ButtonRemoveAddTwoSessionsRoleName" runat="server" Text="ButtonRemoveAddTwoSessionsRoleName"
            OnClick="ButtonRemoveAddTwoSessionsRoleName_Click" />
        <asp:Button ID="Button2" runat="server" 
            Text="ButtonRemove(NOT DONE)SpeakerRoleFromAllSpeakers" onclick="Button2_Click"
             />
    </div>
    </form>
</body>
</html>
