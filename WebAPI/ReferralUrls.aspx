<%@ Page Title="" Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true" Inherits="ReferralUrls" Codebehind="ReferralUrls.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" Runat="Server">
    <asp:Menu ID="subMenu" runat="server" DataSourceID="SiteMapAbout" SkinID="subMenu">
    </asp:Menu>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <br/>
    <div class="mainHeading">
            Claim Referring URL For Cross Links
    </div>
    
    Filter Search:&nbsp;&nbsp;<br/>
    <asp:TextBox runat="server" ID="FilterSearchId"></asp:TextBox>&nbsp;&nbsp;&nbsp;
    <asp:Button ID="ButtonRefreshList" runat="server" Text="Refresh Dropdown List of URLs" />
    <br/><br/>
    

    <asp:DropDownList ID="DropDownListUnClaimedUrls" runat="server" Width="500px">
    </asp:DropDownList>
    <br />
    <br/>
    <asp:Button ID="ButtonClaimReferringUrl" runat="server" 
        Text="Claim The Item Selected In The Above Dropdown as Your Own" 
        onclick="ButtonClaimReferringUrl_Click" Width="500px" />
    <hr/>

    
    <asp:GridView ID="GridViewMyReferringUrls" runat="server" AllowSorting="True" 
        AutoGenerateColumns="False" CellPadding="15" CellSpacing="10" DataKeyNames="Id,AttendeesId" 
        DataSourceID="SqlDataSourceMyReferringUrls" ForeColor="#333333" 
        GridLines="None" onrowdeleted="GridViewMyReferringUrls_RowDeleted" 
        onrowupdated="GridViewMyReferringUrls_RowUpdated">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
            <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" 
                ReadOnly="True" SortExpression="Id" />
            <asp:TemplateField HeaderText="Referring URL" SortExpression="ReferringUrlName">
                
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("ReferringUrlName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="AttendeesId" HeaderText="AttendeesId" 
                SortExpression="AttendeesId" Visible="false" />
            <asp:BoundField DataField="ArticleName" HeaderText="Friendly Name To Display" 
                SortExpression="ArticleName" />
            <asp:BoundField DataField="UserGroup" HeaderText="User Group or Meetup Name" 
                SortExpression="UserGroup" />
            <asp:CheckBoxField DataField="Visible" HeaderText="Show On Site" 
                SortExpression="Visible" />
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
    <asp:SqlDataSource ID="SqlDataSourceMyReferringUrls" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
        DeleteCommand="DELETE FROM [ReferringUrlGroup] WHERE [Id] = @Id" 
        InsertCommand="INSERT INTO [ReferringUrlGroup] ([ReferringUrlName], [AttendeesId], [ArticleName], [UserGroup], [Visible]) VALUES (@ReferringUrlName, @AttendeesId, @ArticleName, @UserGroup, @Visible)" 
        SelectCommand="SELECT [Id], [ReferringUrlName], [AttendeesId], [ArticleName], [UserGroup], [Visible] FROM [ReferringUrlGroup] 
WHERE [AttendeesId] = @AttendeesId
ORDER BY [ArticleName], [ReferringUrlName]" 
        
        UpdateCommand="UPDATE [ReferringUrlGroup] SET [ReferringUrlName] = @ReferringUrlName, [AttendeesId] = @AttendeesId, [ArticleName] = @ArticleName, [UserGroup] = @UserGroup, [Visible] = @Visible WHERE [Id] = @Id">
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="ReferringUrlName" Type="String" />
            <asp:Parameter Name="AttendeesId" Type="Int32" />
            <asp:Parameter Name="ArticleName" Type="String" />
            <asp:Parameter Name="UserGroup" Type="String" />
            <asp:Parameter Name="Visible" Type="Boolean" />
        </InsertParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="Label1" Name="AttendeesId" 
                PropertyName="Text" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="ReferringUrlName" Type="String" />
            <asp:Parameter Name="AttendeesId" Type="Int32" />
            <asp:Parameter Name="ArticleName" Type="String" />
            <asp:Parameter Name="UserGroup" Type="String" />
            <asp:Parameter Name="Visible" Type="Boolean" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>

  

    <asp:Label ID="Label1" runat="server" Text="Label" Visible="False"></asp:Label>
</asp:Content>

