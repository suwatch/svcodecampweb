<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebAPI._test.WebForm1" %>
<%@ Register TagPrefix="dxcc" Namespace="DevExpress.Web.ASPxCloudControl" Assembly="DevExpress.Web.v7.3, Version=7.3.3.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ObjectDataSource ID="ObjectDataSourceCloudControl" runat="server" 
             SelectMethod="GetAllTagsShorten" TypeName="CodeCampSV.SessionTagsODS">
            
              <SelectParameters>
                        <asp:Parameter DefaultValue="0" Name="minCount" Type="Int32" />
                         <asp:ControlParameter ControlID="LabelCodeCampYearId" DefaultValue="" 
                                 Name="CodeCampYearId" PropertyName="Text" />
                    </SelectParameters>
            

        </asp:ObjectDataSource>
        

         
    </div>
        <asp:Label ID="LabelCodeCampYearId" runat="server" Text="Label"></asp:Label>
        
    </form>
</body>
</html>
