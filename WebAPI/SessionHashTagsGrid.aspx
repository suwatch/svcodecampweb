<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultNoColumns.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" Inherits="SessionHashTagsGrid" Codebehind="SessionHashTagsGrid.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="blankSublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" Runat="Server">
    
      <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DataObjectTypeName="CodeCampSV.SessionsResult" 
             SelectMethod="GetByCodeCampYearId" TypeName="CodeCampSV.SessionsManager" UpdateMethod="UpdateHashTag" DeleteMethod="Delete" InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" >
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:Parameter DefaultValue="7" Name="codeCampYearId" Type="Int32" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="Id" Type="Int32" />
                <asp:Parameter Name="TwitterHashTags" Type="String" />
            </UpdateParameters>
        </asp:ObjectDataSource>
        <asp:GridView ID="GridView1" CellSpacing="5" runat="server" AutoGenerateColumns="False" DataKeyNames="Id,CodeCampYearId,Attendeesid,SessionLevel_id,LectureRoomsId,SessionTimesId" DataSourceID="ObjectDataSource1" CellPadding="4" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:CommandField ShowEditButton="True" />
                <%--<asp:BoundField DataField="CodeCampYearId" HeaderText="CodeCampYearId" SortExpression="CodeCampYearId" />--%>
               <%-- <asp:BoundField DataField="Attendeesid" HeaderText="Attendeesid" SortExpression="Attendeesid" />
                <asp:BoundField DataField="SessionLevel_id" HeaderText="SessionLevel_id" SortExpression="SessionLevel_id" />
                <asp:BoundField DataField="SponsorId" HeaderText="SponsorId" SortExpression="SponsorId" />
                <asp:BoundField DataField="LectureRoomsId" HeaderText="LectureRoomsId" SortExpression="LectureRoomsId" />
                <asp:BoundField DataField="SessionTimesId" HeaderText="SessionTimesId" SortExpression="SessionTimesId" />--%>
                <asp:BoundField DataField="TwitterHashTags" HeaderText="Twitter Hash Tags (#json,@javascript)" SortExpression="TwitterHashTags" />
                <asp:BoundField DataField="Title" ReadOnly="true" HeaderText="Session Title" />
                 <asp:BoundField DataField="PresenterName" ReadOnly="true" HeaderText="Presenter" />
               <%-- <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" SortExpression="Id" />--%>
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
    

</asp:Content>

