<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true" Inherits="SponsorManager" Codebehind="SponsorManager.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="blankSublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" Runat="Server">
    
      <a href="SiteAdmin.aspx">SiteAdmin.aspx</a>
    
     
    
    
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DataObjectTypeName="CodeCampSV.SponsorListResult" DeleteMethod="Delete" InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" SelectMethod="GetAllCurrentYear7" TypeName="CodeCampSV.SponsorListManager">
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
    </asp:ObjectDataSource>
    
    <br/>
    

    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="Id" DataSourceID="ObjectDataSource1" ForeColor="#333333" GridLines="None">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" Text="select"
                                   NavigateUrl='<%# GetHyperLink((int) Eval("Id")) %>'
                                   runat="server"/>
                   

                    <%--<asp:LinkButton ID="LinkButton1"  runat="server" CausesValidation="False" CommandName="Select" Text="Select"></asp:LinkButton>--%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="SponsorName" HeaderText="SponsorName" SortExpression="SponsorName">
            <ItemStyle Width="300px" />
            </asp:BoundField>
            <asp:BoundField DataField="CompanyCity" HeaderText="CompanyCity" SortExpression="CompanyCity" />
            <asp:BoundField DataField="CompanyState" HeaderText="CompanyState" SortExpression="CompanyState" />
            <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" SortExpression="Id" />
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
    

</asp:Content>

