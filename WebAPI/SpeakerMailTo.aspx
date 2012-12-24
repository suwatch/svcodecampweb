  <%@ Page Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true" Inherits="SpeakerMailTo" Title="Send Email To Presenter" Codebehind="SpeakerMailTo.aspx.cs" %>

<%@ Register Assembly="App_Code" Namespace="PeterKellner.Utils"
    TagPrefix="CAPTCHA" %>

<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" runat="Server">
    <CAPTCHA:CaptchaUltimateControl ID="CaptchaUltimateControl1" runat="server" ButtonRedisplayCaptchaText="Generate New Display Number"
        CaptchaBackgroundColor="White" CaptchaBorder="1" CaptchaLength="4" CaptchaType="2"
        CommandArg1="" CommandArg2="" CommandArg3="" CommandArg4="" CommandArg5="" EncryptedValue=""
        FontFamilyString="Courrier New" HeightCaptchaPixels="50" InvalidCaptchaMessage="Try Again."
        PlainValue="" ShowPromo="False" ShowRecalculateCaptchaButton="True" ShowTitle="True"
        Title="" WidthCaptchaPixels="140" OnVerified="CaptchaUltimateControl1_Verified"
        OnVerifying="CaptchaUltimateControl1_Verifying">
        <ItemTemplate>
            <div class="pad">
                <tr>
                    <td class="registerLeft" align="right">
                    </td>
                    <td>
                        Session Name:
                        <asp:Label runat="server" ID="SessionNameID"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="registerLeft" align="right">
                    </td>
                    <td>
                        Presenter Name:
                        <asp:Label runat="server" ID="PresenterNameID"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                
                <tr>
                    <td class="registerLeft" align="right">
                        <span class="required">*</span>Message to Speaker
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxMessageID" Columns="40" TextMode="MultiLine" Height="150" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator Enabled="true" ID="RequiredFieldValidator1" ControlToValidate="TextBoxMessageID"
                            CssClass="required" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                </table>
                <table class="registerTable">
                    <tr>
                        <td class="registerLeft">
                            <div class="subHeading2">
                                Confirm you're human.</div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="registerLeft" align="right">
                            <span class="required">*</span> Enter the letters:
                        </td>
                        <td>
                            <asp:TextBox ID="VerificationID" Style="text-align: left;" runat="server"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorID" runat="server">Try Again.</asp:CustomValidator>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Image ID="CaptchaImageID" runat="server" ImageUrl="~/CaptchaType.ashx" BorderStyle="None"
                                            ImageAlign="Middle" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="ButtonDisplayNextID" runat="server" CausesValidation="false" Text="New Characters" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Button ID="ButtonSend"  runat="server" CausesValidation="true" Text="Send Email" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                    <td colspan="3">
                        <asp:Label ID="LabelStatus" runat="server" Text=""></asp:Label>
                    </td>
                    </tr>
                </table>
            </div>
        </ItemTemplate>
    </CAPTCHA:CaptchaUltimateControl>
    <br />

    <asp:HyperLink ID="HyperLinkHome" Visible="false" NavigateUrl="~/Sessions.aspx" runat="server">Return To Sessions Listings</asp:HyperLink>
    <br />

    <asp:Label runat="server" ID="SpeakerEmailId" ></asp:Label>
    
    <br/>
    
   <%-- <asp:Button runat="server"  Text="Set Speaker Role To Submit 2 sessions" 
       ID="ButtonAddSubmit2SessionsRole" OnClick="ButtonAddSubmit2SessionsRole_Click" 
           />
    <br/>
    
     <asp:Button runat="server"  Text="Set Speaker Role To Submit 3 or more sessions" 
          ID="ButtonAddSubmit3orMoreSessionsRole" 
        OnClick="ButtonAddSubmit3orMoreSessionsRole_Click"   />
    <br/>--%>
    
    


</asp:Content>

