<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="SponsorList" Title="Sponsor List Page" Codebehind="SponsorList.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="parentContent" runat="Server">

<h1></h1>

    <asp:SqlDataSource ID="SqlDataSourceSL" runat="server" 
    ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="SELECT 
  dbo.SponsorList.SponsorName,
  dbo.SponsorListCodeCampYear.DonationAmount,
  dbo.SponsorList.CompanyDescriptionShort
FROM
  dbo.SponsorList
  INNER JOIN dbo.SponsorListCodeCampYear ON (dbo.SponsorList.Id = dbo.SponsorListCodeCampYear.SponsorListId)
WHERE
  CodeCampYearId = 6
ORDER BY
  dbo.SponsorList.SponsorName,
  dbo.SponsorListCodeCampYear.DonationAmount"></asp:SqlDataSource>

    <asp:GridView ID="GridViewSL" runat="server" AllowSorting="True" 
    AutoGenerateColumns="False" DataSourceID="SqlDataSourceSL">
        <Columns>
            <asp:BoundField DataField="SponsorName" HeaderText="SponsorName" 
                SortExpression="SponsorName" />
            <asp:BoundField DataField="DonationAmount" HeaderText="DonationAmount" 
                SortExpression="DonationAmount" />
            <asp:BoundField DataField="CompanyDescriptionShort" 
                HeaderText="CompanyDescriptionShort" SortExpression="CompanyDescriptionShort" />
        </Columns>
    </asp:GridView>


</asp:Content>
