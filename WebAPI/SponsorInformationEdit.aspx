<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true" ValidateRequest="false" MaintainScrollPositionOnPostback="true" Inherits="SponsorInformationEdit" Codebehind="SponsorInformationEdit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="blankSublinks" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" runat="Server">





    <tr>
        <td>





            <script language="javascript" type="text/javascript">

                function CountShort(text, long) {
                    var maxlength = new Number(long); // Change number to your max length.
                    if (document.getElementById('<%=TextBoxShortDescription.ClientID%>').value.length > maxlength) {
            text.value = text.value.substring(0, maxlength);
            alert(" Only " + long + " chars");
        }
    }

    function CountLong(text, long) {
        var maxlength = new Number(long); // Change number to your max length.
        if (document.getElementById('<%=TextBoxFullDescription.ClientID%>').value.length > maxlength) {
            text.value = text.value.substring(0, maxlength);
            alert(" Only " + long + " chars");
        }
    }

            </script>


            <br />

            Back To:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:HyperLink ID="HyperLinkSponsorInformation" NavigateUrl="~/SponsorInformation.aspx" runat="server">Sponsor Information Page</asp:HyperLink>
            <br />
            <br />
            <asp:Button ID="ButtonUpdate" runat="server" Text="Update"
                OnClick="ButtonUpdate_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="LabelStatus" runat="server" Text=""></asp:Label>
            <br />

            <table cellpadding="5" cellspacing="15" border="0">
                <tr>
                    <td colspan="2" style="font-size: x-large">
                        <asp:Label ID="LabelSponsorName" runat="server" Text=""></asp:Label>&nbsp;&nbsp;
                <strong style="text-align: center">Sponsorship Information</strong>&nbsp;&nbsp;
                 (no HTML please)
                    </td>
                </tr>

                <tr>

                    <td colspan="2" style="font-size: x-large">
                        <asp:HyperLink ID="HyperLinkEdit" runat="server">Edit</asp:HyperLink>
                        &nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label1" runat="server" Text="" Font-Size="X-Large" Font-Bold="True"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td style="width: 250px;">Platinum Table? (If you are a platinum sponsor, will you be need a table in the sponsor area)
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBoxIDPlatinumTable" runat="server" />
                    </td>
                </tr>

                <tr>
                    <td>Platinum Flier For Giveaway bag? (If you are a platinum sponsor, will you be providing an single sheet flier for attendee bag)
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBoxIdPlatinumFlier" runat="server" />
                    </td>
                </tr>

              
                <tr>
                    <td>Are You Shipping Anything To Code Camp?
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBoxIdShippingItemstoCodeCamp" runat="server" />
                        <br />
                        What:  &nbsp;
                        <asp:TextBox ID="TextBoxShippingWhatToCodeCamp" Width="400px" MaxLength="256"
                            runat="server"></asp:TextBox>


                    </td>
                </tr>


                <tr>
                    <td>Address Line 1
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxAddressLine1" runat="server" MaxLength="120" Width="500px"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>Address Line 2
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxAddressLine2" runat="server" MaxLength="120" Width="500px"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>City
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxCity" runat="server" MaxLength="120" Width="500px"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>State
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxState" runat="server" MaxLength="120" Width="500px"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>Zip
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxZip" runat="server" MaxLength="120" Width="500px"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>Company Phone Number
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxPhone" runat="server" MaxLength="120" Width="500px"></asp:TextBox>
                    </td>
                </tr>


                <tr>

                    <td>Full Company Description (Limit 500 characters)
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxFullDescription" runat="server" Height="200"
                            onKeyUp="javascript:CountLong(this,500);" onChange="javascript:CountLong(this,500);"
                            TextMode="MultiLine" Width="500px"></asp:TextBox>

                    </td>
                </tr>
                <tr>

                    <td>Short Company Description (Limit 140 characters)
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxShortDescription" runat="server" Height="100"
                            onKeyUp="javascript:CountShort(this,140);" onChange="javascript:CountShort(this,140);"
                            TextMode="MultiLine" Width="500px"></asp:TextBox>
                    </td>
                </tr>
                
                
                  <tr>

                   
                    <td colspan="2">
                       <div runat="server" id="CodeCampNoteId">
                           

                       
                        <i>CodeCamp Use Only: fill in below line, normal sponsor does not see this </i> &nbsp;
                        <asp:TextBox ID="TextBoxCodeCampNote" runat="server" Height="100"
                            TextMode="MultiLine" Width="500px"></asp:TextBox>
                           </div>
                    </td>
                </tr>
                

            </table>


            <asp:Label ID="LabelSponsorId" runat="server" Visible="false" Text="Label"></asp:Label>



        </td>
    </tr>



</asp:Content>
