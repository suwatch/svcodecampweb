<%@ Page Title="" Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true" Inherits="ReferralUrlLinks" Codebehind="ReferralUrlLinks.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" Runat="Server">
    <asp:Menu ID="subMenu" runat="server" DataSourceID="SiteMapAbout" SkinID="subMenu">
    </asp:Menu>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

 <br/>
    <div class="mainHeading">
            Incoming Links From Code Camp Speakers and Attendees
    </div>


    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        CellPadding="8" CellSpacing="10" DataSourceID="ObjectDataSourceReferrings" 
        ForeColor="#333333" GridLines="None">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:TemplateField HeaderText="Attendees Name and Link Author" >
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("FirstName") + " " + Eval("LastName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Referring Link" >
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink2" runat="server" Text='<%# Bind("ArticleName") %>' NavigateUrl='<%# Bind("ReferringUrlName") %>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
              <asp:BoundField DataField="ReferralCountAllTime" HeaderText="Referral Count" 
                SortExpression="ReferralCountAllTime" />
            <asp:BoundField DataField="UserGroup" HeaderText="Group" 
                SortExpression="UserGroup" />
          
            <asp:TemplateField HeaderText="Attendees Web Site" SortExpression="UserWebSite">
               
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink2" runat="server" Text='<%# (string) CleanupUrl((string) Eval("UserWebSite")) %>' NavigateUrl='<%# Bind("UserWebSite") %>'></asp:HyperLink>
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
    <br />


    <br/>



    <asp:ObjectDataSource ID="ObjectDataSourceReferrings" runat="server" 
         SelectMethod="Get"  CacheDuration="15" 
        TypeName="CodeCampSV.ReferringUrlAttendeeInfoDo"></asp:ObjectDataSource>


    <asp:Label ID="Label1" runat="server" Text="Label" Visible="False"></asp:Label>



</asp:Content>

