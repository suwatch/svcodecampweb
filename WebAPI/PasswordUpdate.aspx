<%@ Page Language="C#" AutoEventWireup="true" Inherits="PasswordUpdate" Codebehind="PasswordUpdate.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Password Update</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       Last Name:&nbsp;&nbsp;&nbsp;  <asp:TextBox ID="TextBox1" runat="server">UserName</asp:TextBox>
       &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="SearchButton" runat="server" onclick="SearchButton_Click" 
            Text="Search" />
       <hr />
         

        <asp:GridView ID="GridView1" BackColor="White" runat="server" AutoGenerateColumns="False" DataKeyNames="UserName" DataSourceID="ObjectDataSource1">
            <Columns>
                <asp:CommandField ShowEditButton="True" ShowSelectButton="True" />
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
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
           SelectMethod="GetMembersByUsername" 
            TypeName="MembershipUtilities.MembershipUserODS" UpdateMethod="Update"  
            DeleteMethod="Delete" InsertMethod="Insert" 
            OldValuesParameterFormatString="{0}">
            <DeleteParameters>
                <asp:Parameter Name="UserName" Type="String" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="UserName" Type="String" />
                <asp:Parameter Name="email" Type="String" />
                <asp:Parameter Name="isApproved" Type="Boolean" />
                <asp:Parameter Name="comment" Type="String" />
                <asp:Parameter Name="lastActivityDate" Type="DateTime" />
                <asp:Parameter Name="lastLoginDate" Type="DateTime" />
                <asp:Parameter Name="password" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="TextBox1" Name="userName" PropertyName="Text" 
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
        </asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
