<%@ Page Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true"
    ValidateRequest="false" MaintainScrollPositionOnPostback="true"
    Inherits="SubmitSession" Title="Submit Session CodeCamp SFBA" Codebehind="SubmitSession.aspx.cs" %>


<%@ Register Namespace="PeterKellner.Utils"  TagPrefix="CAPTCHA"    %>


<asp:Content ID="SublinksSessions" ContentPlaceHolderID="blankSublinks" runat="server">
    <asp:Menu ID="subMenu" runat="server" DataSourceID="SiteMapProgram" SkinID="subMenu">
    </asp:Menu>
    <%--The next line should be here, but seems to work without because it is in master page--%>
    <asp:SiteMapDataSource ID="SiteMapProgram" runat="server" ShowStartingNode="False"
        StartingNodeUrl="~/Program.aspx" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="blankContent" runat="Server">
    <tr>
        <td>
    <br />
    <div id="DIVOverSessionLimit" runat="server">
        <p>
            Each presenter is limited to one sessions. If you would like to submit more, please
            contact info@siliconvalley-codecamp.com for further information.
        </p>
    </div>
    <div runat="server" id="DIVMain">
        <h2>Submit a New Session</h2>
        <hr />
        <table border="0" cellpadding="3">
            <tr>
                <td colspan="2">Title:
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="TextBoxTitle" runat="server" Width="461px"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td>Session Hash Tags for Twitter (ex:   #sqlserver #vsstudio)  (no ,:; please, just spaces and #'s)
                </td>
            </tr>
            <tr>
                <td>
                    <div class="title">
                        <asp:TextBox ID="TextBoxHashTags" Width="400" runat="server"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </div>
                </td>
            </tr>




            <tr>
                <td colspan="1">
                    <br />
                    Description:     (Please don't include any HTML, 1200 character limit)
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <asp:TextBox ID="TextBoxDescription" runat="server" MaxLength="1200" Columns="60"
                        Rows="6" TextMode="MultiLine">
                    </asp:TextBox>
                </td>
            </tr>


            <tr>
                <td colspan="2">Level:
                    <asp:DropDownList ID="DropDownListLevel" runat="server" DataSourceID="SqlDataSource1"
                        DataTextField="description" DataValueField="id">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
                        SelectCommand="SELECT [id], [description] FROM [SessionLevels] ORDER BY [description]"></asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;<asp:CheckBox TextAlign="left" ID="CheckBoxInternetAccess" runat="server" /><asp:Label
                    ID="Label1" Text=" Internet Access Required" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                    <p>
                        Choose Up to&nbsp;<asp:Label ID="LabelMaxCategories" runat="server" Text="###"></asp:Label>&nbsp;Categories
                        Below as is appropriate for your talk. You may also add your own categories in the
                        text box below that are not shown in the above list. Please check thoroughly before
                        adding your own topic.
                    </p>
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DeleteMethod="Delete"
                        SelectMethod="GetAllTags" TypeName="CodeCampSV.TagsODS" UpdateMethod="UpdateChecked">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="1" Name="sortData" Type="String" />
                            <asp:Parameter Name="searchsessionid" Type="Int32" DefaultValue="" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <br />
                    <asp:CheckBoxList ID="CheckBoxListTags" RepeatColumns="4" RepeatLayout="Table" RepeatDirection="Vertical"
                        runat="server" DataSourceID="ObjectDataSource1" DataTextField="TagName" DataValueField="id">
                    </asp:CheckBoxList>
                </td>
                <tr>
                    <td colspan="2">
                        <div id="newtag" runat="server" visible="true">
                            New Categories for your topic separated by ".,;:" if multiple:&nbsp;
                            <asp:TextBox ID="TextBoxAddCategories" runat="server"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                        <CAPTCHA:CaptchaUltimateControl ID="CaptchaUltimateControl1" runat="server" CaptchaBorder="1"
                            CaptchaLength="5" OnVerified="CaptchaUltimateControl1_Verified" ShowPromo="False"
                            OnVerifying="CaptchaUltimateControl1_Verifying">
                        </CAPTCHA:CaptchaUltimateControl>
                        <br />
                        <br />
                        <asp:Button ID="ButtonSubmitSession" runat="server" Text="Submit Session" /><br />
                        <asp:Label ID="LabelStatus" ForeColor="Red" runat="server"></asp:Label>
                    </td>
                </tr>
        </table>
    </div>
    <div runat="server" id="DIVNOTLOGGEDIN">
        <asp:HyperLink Font-Size="1.4em" NavigateUrl="~/attendeeRegistration.aspx" ID="HyperLinkRegister"
            runat="server">You Must Register and Login Before Submitting A Session</asp:HyperLink>
    </div>
        </td>
    </tr>
</asp:Content>
