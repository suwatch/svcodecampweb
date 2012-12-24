<%@ Page Title="" Language="C#" MasterPageFile="~/RightNote.master" AutoEventWireup="true" Inherits="SessionsJavaScript" Codebehind="SessionsJavaScript.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" Runat="Server">

    <asp:ScriptManagerProxy runat="server" ID="IDScriptManager">
    </asp:ScriptManagerProxy>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<script type="text/javascript" src="js/json_parse.js"></script>

  
            
          
          
<script type="text/javascript" >


function pageLoad() {
            var request = new Sys.Net.WebRequest();
            request.set_url("SessionJSONHandler.ashx");
            request.add_completed(onRequestCompleted);
            request.invoke();
        }
        
         function onRequestCompleted(executor, args) {
             processResults(executor.get_responseData());
         }
         
         function processResults(theData) {
            var dataObject = json_parse(theData);
        }



</script>



</asp:Content>

