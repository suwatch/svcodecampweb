<%@ Page Title="" Language="C#" MasterPageFile="~/RightNote.master" AutoEventWireup="true" Inherits="MySessions" Codebehind="MySessions.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <script type="text/javascript" src="MySessions.aspx.js">
    </script>

    <div id="loading-mask" style="">
    </div>
    <div id="loading">
        <div class="loading-indicator">
            <img src="docs/extanim32.gif" width="32" height="32" style="margin-right: 8px;" align="absmiddle" />Loading...</div>
    </div>
    <textarea id="detailtpl" style="display: none;">
       
       
     <!--   <div class="speakersContainer"> -->
     
     
     
    <!-- Speaker entry -->
    <div class="pad">
        
            <div>{UserBio}</div>
            
    </div>
    
   <%-- <div class="pad">
        <table cellpadding="0" cellspacing="0" width="100%"><tr>
        <td valign="top">
            <table cellpadding="0" cellspacing="0" width="100%"><tr>
                <td><div class="subHeading">{FirstName} {LastName}</div></td>
                <td valign="middle"><div class="slashBullet">
                    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="Sessions.aspx">Sessions</asp:HyperLink></div></td>
            </tr></table>
            <div class="speakerDescription"  >{UserBio}</div>
            <div class="slashBullet"><asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="http://developer.marklogic.com" Target="_blank">Visit web site</asp:HyperLink></div>
        </td>
        <td valign="top">
            <asp:Image ID="Image1" ImageUrl="~/Images/Speakers/RonHitchens.jpg" ImageAlign="middle" runat="server" BorderStyle="None" CssClass="speakerImage" />
        </td>
        </tr></table>
    </div>--%>
       
       
       
       
    </textarea>
    <div id='myDiv'>
    </div>
</asp:Content>
