<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultNoColumns.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="WebAPI.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="blankSublinks" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" runat="server">
    
    <div style="margin: 50px 50px 50px 50px;">

   <%-- <iframe src="/ExtJSApps/RegistrationProd"  width="600" height="700">--%>
        
         <iframe src="/ExtJSApps/Registration/app.html"  width="600" height="700">
    
    <h3>Registration Page Loading...</h3>
    <img src="Images/loadingAnimation.gif" alt="Registration Page Loading, Hold on..."/>

</iframe>    
        
        </div>

    

</asp:Content>
