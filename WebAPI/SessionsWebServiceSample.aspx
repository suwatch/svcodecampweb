<%@ Page Title="Sessions - Web Service Sample" Language="C#" MasterPageFile="~/RightNote.master"
    AutoEventWireup="true" Inherits="SessionsWebServiceSample" Codebehind="SessionsWebServiceSample.aspx.cs" %>
<%@ Import Namespace="System.Security.Policy"%>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <script type="text/javascript" src="js/json.js?proxy" >
    </script>
     <script type="text/javascript" src="WebServiceCodeCamp.ashx?proxy" >
    </script>
    
   

    <script type="text/javascript">

        function ReturnAllData(dataHere) {
        
            var divObj = document.getElementById('SessionListId');

            var tableStart =
                   '<table cellspacing="5" cellpadding="0" border="1">' +
                   '<tbody><tr><th>Presenter</th><th>Session Title</th><th>Picture</th></tr>';
            var tableContent = '';
            var tableEnd = '</tbody></table>';
            for (var i = 0; i < dataHere.result.sessionDetails.length; i++) {
                var presenterName = dataHere.result.sessionDetails[i].speakerFirstName + ' ' + dataHere.result.sessionDetails[i].speakerLastName
                var sessionTitle = dataHere.result.sessionDetails[i].sessionInfo.title
                var sessionPictureURL = dataHere.result.sessionDetails[i].speakerPictureUrl;
                var sessionPictureURLTag = "<img alt=\'Speaker: " + presenterName + "\'  src=\'" + sessionPictureURL + "\' \/>";
                tableContent += '<tr><td>' + presenterName
                 + '</td><td>' + sessionTitle + '</td><td>' + sessionPictureURLTag + '</td></tr>';
            }

            var userHere = dataHere.result.loggedInUsername;
            divObj.innerHTML = '<h1>Logged In Username: ' + userHere + '</h1>' + tableStart + tableContent + tableEnd;
        }

        // DisplayImage.ashx?sizex=100&amp;PKID=5843e256-4368-4b03-b20e-159aefab850e"

        function pageLoad() {
           
            var webServiceCC = new WebServiceCodeCamp();
            webServiceCC.RetrieveAllData(ReturnAllData);
        }

        
        

    </script>


    <div id='SessionListId'>
        
        </div>
</asp:Content>
