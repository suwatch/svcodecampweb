<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="SessionEvalResults" Title="Session Eval Results" Codebehind="SessionEvalResults.aspx.cs" %>

<%@ Register Assembly="DevExpress.XtraCharts.v7.3, Version=7.3.3.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v7.3.Web, Version=7.3.3.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1"
    Namespace="DevExpress.XtraCharts" TagPrefix="dxchartsui" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" Runat="Server">
    <asp:ObjectDataSource ID="ObjectDataSourceSessionEvals" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetSessionResponses" TypeName="CodeCampSV.SessionEvalsODS" DeleteMethod="Delete" UpdateMethod="UpdateAllSessionEvals">
        <SelectParameters>
            <asp:Parameter DefaultValue="125" Name="sessionId" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
            <asp:Parameter Name="original_id" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="username" Type="String" />
            <asp:Parameter Name="sessionId" Type="Int32" />
            <asp:Parameter Name="updatedate" Type="DateTime" />
            <asp:Parameter Name="courseaswhole" Type="Int32" />
            <asp:Parameter Name="coursecontent" Type="Int32" />
            <asp:Parameter Name="instructoreff" Type="Int32" />
            <asp:Parameter Name="instructorabilityexplain" Type="Int32" />
            <asp:Parameter Name="instructoreffective" Type="Int32" />
            <asp:Parameter Name="instructorknowledge" Type="Int32" />
            <asp:Parameter Name="qualityoffacility" Type="Int32" />
            <asp:Parameter Name="overallcodecamp" Type="Int32" />
            <asp:Parameter Name="contentlevel" Type="Int32" />
            <asp:Parameter Name="favorite" Type="String" />
            <asp:Parameter Name="improved" Type="String" />
            <asp:Parameter Name="generalcomments" Type="String" />
            <asp:Parameter Name="discloseeval" Type="Boolean" />
        </UpdateParameters>
    </asp:ObjectDataSource>
    <br />
    

    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="ObjectDataSourceSessionEvals">
        <Columns>
            <asp:BoundField DataField="Question" HeaderText="Question" SortExpression="Question" />
            <asp:BoundField DataField="NumberResponses" HeaderText="NumberResponses" SortExpression="NumberResponses" />
            <asp:BoundField DataField="AverageResponse" HeaderText="AverageResponse" SortExpression="AverageResponse" />
            <asp:BoundField DataField="OverallAverageResponse" HeaderText="OverallAverageResponse"
                SortExpression="OverallAverageResponse" />
        </Columns>
    </asp:GridView>




</asp:Content>

