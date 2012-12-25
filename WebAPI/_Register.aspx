<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultNoColumns.Master" AutoEventWireup="true" CodeBehind="_Register.aspx.cs" Inherits="WebAPI._Register" %>

<%@ Register Namespace="PeterKellner.Utils" TagPrefix="CAPTCHA" %>
<%@ Register Namespace="CodeCampSV" TagPrefix="CST" %>


<asp:Content ID="Content1" ContentPlaceHolderID="blankSublinks" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" runat="server">
    
       <CST:ValidationSummaryWithAddMessage ID="ValidationSummary1" runat="server" HeaderText="Oops! There was a problem.  Please fix the following errors:"
                CssClass="ValidationErrorClass" DisplayMode="BulletList"></CST:ValidationSummaryWithAddMessage>
         

</asp:Content>
