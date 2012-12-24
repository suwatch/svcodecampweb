<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="SessionsEvalReview" Title="Untitled Page"
    MaintainScrollPositionOnPostback="true" Codebehind="SessionsEvalReview.aspx.cs" %>

<%@ Register Assembly="DevExpress.XtraCharts.v7.3, Version=7.3.3.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v7.3.Web, Version=7.3.3.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1"
    Namespace="DevExpress.XtraCharts" TagPrefix="dxchartsui" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" runat="Server">
    <asp:SqlDataSource ID="SqlDataSourceSessionInfo" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        SelectCommand="SELECT [Username], [title],[id] FROM [Sessions] WHERE ([id] = @id)">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelSessionId" Name="id" PropertyName="Text" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:Repeater ID="RepeaterSessionInfo" runat="server" DataSourceID="SqlDataSourceSessionInfo">
        <ItemTemplate>
            <table cellpadding="5" style="font-weight: bolder; font-size: large;">
                <tr>
                <td colspan="2" >
                   <%# GetCodeCampTitle() %>
                </td>
                </tr>
                <tr>
                    <td>
                        Presenter:
                    </td>
                    <td>
                        <%#  GetAttendeeNameFromSessionId((int) Eval("id")) %>
                    </td>
                </tr>
                <tr>
                    <td>
                        Session Title:
                    </td>
                    <td>
                        <%# Eval("Title") %>
                    </td>
                </tr>
                <tr>
                    <td>
                        Evaluations Submitted:
                    </td>
                    <td>
                       <%# GetSessionEvalsSubmittedCount((int)Eval("id"))%>
                    </td>
                </tr>
            </table>
         
        </ItemTemplate>
    </asp:Repeater>
    <div runat="server" id="IDShowAvailable" visible="false">
        <p>
            Session evaluations will be available for review by presenter only shortly after
            Codecamp is completed.
        </p>
        <p>
            Thanks for Presenting!</p>
    </div>
    <asp:Label runat="server" ID="LabelSessionId" Visible="false" />&nbsp;<br />
    &nbsp; &nbsp; &nbsp;&nbsp; (Rating Key: 1-poor;2-average;3-good;4-excellent)<br />
    <dxchartsui:WebChartControl ID="WebChartControl1" runat="server" DataSourceID="ObjectDataSourceGraph"
        DiagramTypeName="XYDiagram" Height="400px" Width="694px">
        <Diagram>
            <axisx>
<Label Angle="25" Antialiasing="True"></Label>

<Range SideMarginsEnabled="True"></Range>
</axisx>
            <axisy>
<Range SideMarginsEnabled="True"></Range>
</axisy>
        </Diagram>
        <FillStyle FillOptionsTypeName="SolidFillOptions">
            <Options HiddenSerializableString="to be serialized" />
        </FillStyle>
        <SeriesTemplate LabelTypeName="SideBySideBarSeriesLabel" PointOptionsTypeName="PointOptions"
            SeriesViewTypeName="SideBySideBarSeriesView">
            <View HiddenSerializableString="to be serialized">
            </View>
            <Label Antialiasing="True" HiddenSerializableString="to be serialized">
                <FillStyle FillOptionsTypeName="SolidFillOptions">
                    <Options HiddenSerializableString="to be serialized" />
                </FillStyle>
            </Label>
            <PointOptions HiddenSerializableString="to be serialized">
            </PointOptions>
        </SeriesTemplate>
        <SeriesSerializable>
            <cc1:Series ArgumentDataMember="Question" LabelTypeName="SideBySideBarSeriesLabel"
                Name="Your Session" PointOptionsTypeName="PointOptions" SeriesViewTypeName="SideBySideBarSeriesView"
                ValueDataMembersSerializable="AverageResponse">
                <view hiddenserializablestring="to be serialized"></view>
                <label hiddenserializablestring="to be serialized" visible="False">
                    <fillstyle filloptionstypename="SolidFillOptions">
<Options HiddenSerializableString="to be serialized"></Options>
</fillstyle>
                </label>
                <pointoptions hiddenserializablestring="to be serialized"></pointoptions>
            </cc1:Series>
            <cc1:Series ArgumentDataMember="Question" LabelTypeName="SideBySideBarSeriesLabel"
                visible="false" Name="All Sessions Average" PointOptionsTypeName="PointOptions"
                SeriesViewTypeName="SideBySideBarSeriesView" ValueDataMembersSerializable="OverallAverageResponse">
                <view hiddenserializablestring="to be serialized"></view>
                <label hiddenserializablestring="to be serialized" visible="False">
                    <fillstyle filloptionstypename="SolidFillOptions">
<Options HiddenSerializableString="to be serialized"></Options>
</fillstyle>
                </label>
                <pointoptions hiddenserializablestring="to be serialized"></pointoptions>
            </cc1:Series>
        </SeriesSerializable>
        <Legend AlignmentHorizontal="Center" Antialiasing="True" AlignmentVertical="TopOutside">
        </Legend>
    </dxchartsui:WebChartControl>
    <asp:ObjectDataSource ID="ObjectDataSourceGraph" runat="server" SelectMethod="GetSessionResponses"
        TypeName="CodeCampSV.SessionEvalsODS">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelSessionId" Name="sessionId" PropertyName="Text"
                Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <br />
    <%-- <asp:Repeater ID="RepeaterEvalQuestions" runat="server" DataSourceID="ObjectDataSourceSessionEvals">
        <HeaderTemplate>
            <table>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("Question")%>
                </td>
                <td>
                    <%# Eval("AverageResponse")%>
                </td>
                <td>
                    <%# Eval("OverallAverageResponse")%>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
  
    <asp:ObjectDataSource ID="ObjectDataSourceSessionEvals" runat="server" SelectMethod="GetSessionResponses"
        TypeName="CodeCampSV.SessionEvalsODS">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelSessionId" Name="sessionId" PropertyName="Text"
                Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    --%>
    <br />
    <asp:Repeater ID="RepeaterGeneralComments" runat="server" DataSourceID="ObjectDataSourceSessionEvalById">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li>
                <%# GetEvalName((bool)Eval("Discloseeval"), (Guid)Eval("Pkid"))%>
                <ul>
                    <li><i>General Comments: </i>
                        <%# Eval("Generalcomments")%>
                    </li>
                    <li><i>Improvements: </i>
                        <%# Eval("Improved")%>
                    </li>
                    <li><i>Favorite Part: </i>
                        <%# Eval("Favorite")%>
                    </li>
                </ul>
            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
    <asp:ObjectDataSource ID="ObjectDataSourceSessionEvalById" runat="server" SelectMethod="GetBySessionId"
        TypeName="CodeCampSV.SessionEvalsODS">
        <SelectParameters>
            <asp:Parameter Name="sortData" Type="String" />
            <asp:ControlParameter ControlID="LabelSessionId" Name="sessionId" PropertyName="Text"
                Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <br />
</asp:Content>
