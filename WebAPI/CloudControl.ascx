<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CloudControl.ascx.cs" Inherits="WebAPI.CloudControl" %>


<%@ Register Assembly="DevExpress.Web.v7.3, Version=7.3.3.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1"
    Namespace="DevExpress.Web.ASPxCloudControl" TagPrefix="dxcc" %>

<%--<%@ Register Assembly="App_Code" Namespace="CodeCampSV" TagPrefix="RBUTIL" %>--%>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<asp:UpdatePanel runat="server" ID="UpdatePanelCloud">
    <ContentTemplate>
        
            <div runat="server" visible="true" id="DivCloud" >
                <dxcc:ASPxCloudControl ID="ASPxCloudControl1" runat="server" DataSourceID="ObjectDataSourceCloudControl"
                    ShowValues="True" HorizontalAlign="NotSet" TextField="TagName" ValueField="TagCount" 
                    NavigateUrlField="TagId" NavigateUrlFormatString="Sessions.aspx?sortby=title&by=category&tag={0}"
                    ToolTip="CloudControl Provided From DevExpress">
                    <RankProperties>
                        <dxcc:RankProperties />
                        <dxcc:RankProperties />
                        <dxcc:RankProperties />
                        <dxcc:RankProperties />
                        <dxcc:RankProperties />
                        <dxcc:RankProperties />
                        <dxcc:RankProperties />
                    </RankProperties>
                </dxcc:ASPxCloudControl>
                <asp:ObjectDataSource ID="ObjectDataSourceCloudControl" runat="server" SelectMethod="GetAllTagsShorten"
                    TypeName="CodeCampSV.SessionTagsODS" >
                    <SelectParameters>
                        <asp:Parameter DefaultValue="0" Name="minCount" Type="Int32" />
                         <asp:ControlParameter ControlID="LabelCodeCampYearId" DefaultValue="" 
                                 Name="CodeCampYearId" PropertyName="Text" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
           <asp:Label ID="LabelCodeCampYearId" runat="server" Text="" Visible="false" ></asp:Label>

    </ContentTemplate>
</asp:UpdatePanel>
