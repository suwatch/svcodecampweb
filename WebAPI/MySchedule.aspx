<%@ Page Title="" Language="C#" MasterPageFile="~/RightNote.master" AutoEventWireup="true" Inherits="MySchedule" Codebehind="MySchedule.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

 <script type="text/javascript" src="MySchedule.aspx.js">
    </script>

    <div id="loading-mask" style="">
    </div>
    <div id="loading">
        <div class="loading-indicator">
            <img src="assets/js/ext-2.2/docs/resources/extanim32.gif"width="32" height="32" style="margin-right: 8px;" align="middle" />Loading...</div>
    </div>


</asp:Content>

