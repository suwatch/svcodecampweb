<%@ Page Title="" Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true" Inherits="ShirtSizeSet" Codebehind="ShirtSizeSet.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="mainHeading">
        Speaker Shirt Size Selection
    </div>
    
    <div style="margin: 20px 20px 20px 20px">

        <div runat="server" id="HaveShirtSizeId">

            <p>We have your shirt size already!  If you would like to change it, go to the Registration page and update it there please.</p>

            <a href="Register.aspx">Register.aspx</a>


        </div>

        <div runat="server" visible="true" id="SpeakerShirtSizeDiv">
            
            <p>We provide shirts at no cost to Silicon Valley Code Camp speakers.  In order to do this, we need to know all speakes shirt sizes who actually plan on 
                using one of our shirts.  If you are not interested in a shirt, <strong>please change the dropdown below</strong> to N/A and we will stop pestering you with this message.</p>
            <p>&nbsp;</p>
            <p>&nbsp;Thanks
                for volunteering your time to speak this year.
            </p>
            <br/>
            <table>
                <tr>
                    <td>
                        <strong>Shirt Size If Speaker:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong>
                    </td>
                    <td style="alignment-adjust: before-edge">
                        <asp:DropDownList ID="DropDownListSpeakerShirtSize" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>

               
                <tr>
                    <td colspan="2">

                        <asp:Button runat="server" ID="submitbuttonId" Text="Press To Update Shirt Size" OnClick="submitbuttonId_Click" />

                    </td>

                </tr>
            </table>
        </div>
    </div>


</asp:Content>

