<%@ Page Language="C#" AutoEventWireup="true" Inherits="ClouldControl" Codebehind="ClouldControl.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v7.3, Version=7.3.3.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1"
    Namespace="DevExpress.Web.ASPxCloudControl" TagPrefix="dxcc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
<title>Untitled Page</title>
</head>

<body>

<form id="form1" runat="server">
	<div>
		<div style="float: right; width: 25%;margin: 10px;" >
		    col1
			<dxcc:ASPxCloudControl ID="ASPxCloudControl1" runat="server" DataSourceID="SqlDataSource1" ShowValues="true" HorizontalAlign="NotSet" TextField="TagName" ValueField="Column1">
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
        </div>
        <div style="margin: 10px;">
        col2
		<asp:GridView ID="GridView1" runat="server" DataSourceID="ObjectDataSource1" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="TagName" HeaderText="TagName" SortExpression="TagName" />
                <asp:BoundField DataField="TagId" HeaderText="TagId" SortExpression="TagId" />
                <asp:BoundField DataField="TagCount" HeaderText="TagCount" SortExpression="TagCount" />
            </Columns>
        </asp:GridView>
        </div>
		<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="SELECT 
  dbo.Tags.TagName,COUNT(dbo.Tags.TagName)
FROM
  dbo.SessionTags
  INNER JOIN dbo.Tags ON (dbo.SessionTags.tagid = dbo.Tags.id)
  INNER JOIN dbo.Sessions ON (dbo.SessionTags.sessionId = dbo.Sessions.id)
GROUP BY dbo.Tags.TagName">
        </asp:SqlDataSource>
        <br />
        &nbsp;<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetAllTags" TypeName="CodeCampSV.SessionTagsODS"></asp:ObjectDataSource>
	</div>
</form>

</body>

</html>
