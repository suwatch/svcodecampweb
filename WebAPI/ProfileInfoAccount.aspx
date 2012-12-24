<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Title="Profile Information" ValidateRequest="false"
    Inherits="ProfileInfoAccount" Codebehind="ProfileInfoAccount.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" runat="Server">
    <br />
    <asp:Label ID="LabelTopMessage" runat="server" Font-Bold="true" Font-Size="Larger"></asp:Label>
    
    <div runat="server" id="FullProfileId">
        &nbsp;<asp:Label ID="LabelUsername" runat="server"></asp:Label>
        <hr />
        <table border="0" cellpadding="5" runat="server" id="TableData">
            <tr>
                <td style="text-align: left;">
                    <asp:Button ID="ButtonUpdate" runat="server" Text="Update Information" OnClick="ButtonUpdate_Click" />
                </td>
                <td style="text-align: right;">
                    <asp:Button ID="ButtonCancelRegistration" runat="server" Text="Cancel Registration"
                        OnClick="ButtonCancelRegistration_Click" />
                </td>
                <td style="text-align: right;">
                    <asp:Button ID="ButtonRegisterCurrentYear" runat="server" Text="Register For Current Year"
                        OnClick="ButtonRegister_Click" />
                </td>
            </tr>
            <tr runat="server" id="ErrorRow" >
                <td colspan="3" style="text-align: left;">
                    <asp:Label ID="LabelErrors" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 44px">
                    Attending On
                    <asp:Label ID="LabelAttendingSaturdayLabel" runat="server" Text="Label" /><?</asp:Label>
                </td>
                <td style="height: 44px">
                    <asp:CheckBox ID="CheckBoxSaturday" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Attending On
                    <asp:Label ID="LabelAttendingSundayLabel" runat="server" Text="Label" />?
                </td>
                <td>
                    <asp:CheckBox ID="CheckBoxSunday" runat="server" />
                </td>
            </tr>

             <tr>
                <td>
                    Volunteer To Help?
                    <asp:Label ID="LabelVolunteering" runat="server" Text="Label" />?
                </td>
                <td>
                    <asp:CheckBox ID="CheckBoxVolunteering" runat="server" />
                </td>
            </tr>

             <tr>
                <td>
                    Contact Phone Number During Camp (For Volunteers)
                    <asp:Label ID="LabelContactPhoneNumber" runat="server" Text="Label" />?
                </td>
                <td>
                    <asp:TextBox ID="TextBoxPhoneNumber" runat="server" />
                </td>
            </tr>
          

            <tr runat="server" id="TableRowSaturdayDinner">
                <td>
                    Attending Saturday Barbeque<br />
                </td>
                <td>
                    <asp:CheckBox ID="CheckBoxSaturdayDinner" Visible="true" runat="server" />
                </td>
            </tr>
            <%--<tr runat="server" id="TableRowOnlyComingVista">
            <td>
                Only Coming for<br />
                Vista Upgrade?<br />
                (No classes)</td>
            <td>
                <asp:CheckBox ID="CheckBoxVistaUpgradeOnly" runat="server" /></td>
        </tr>
        <tr>
            <td>
                Vista Upgrade<br />
                Time Slot</td>
            <td>
                <asp:RadioButtonList ID="RadioButtonListVista" runat="server" DataSourceID="ObjectDataSourceVista"
                    DataTextField="description" DataValueField="id">
                </asp:RadioButtonList>
                <br />
                <asp:ObjectDataSource ID="ObjectDataSourceVista" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetAvailableVistaSlots" TypeName="CodeCampSV.VistaSlotsODS" >
                   
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr runat="server" id="TableRowDesktopOrLaptop">
            <td>
                Bringing Desktop<br />
                or Laptop for<br />
                Vista Upgrade?</td>
            <td>
                <asp:RadioButtonList ID="RadioButtonListDesktopOrLaptop" runat="server" >
                    <asp:ListItem>Desktop</asp:ListItem>
                    <asp:ListItem>Laptop</asp:ListItem>
                    <asp:ListItem>unknown</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        --%>
            <tr>
                <td>
                    First Name
                </td>
                <td>
                    <asp:TextBox ID="TextBoxFirstName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Last Name
                </td>
                <td>
                    <asp:TextBox ID="TextBoxLastName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Email Address
                </td>
                <td>
                    <asp:TextBox ID="TextBoxEmailAddress" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Zipcode
                </td>
                <td>
                    <asp:TextBox ID="TextBoxZipCode" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Share Information on Website
                </td>
                <td>
                    <asp:CheckBox ID="CheckBoxShareInfo" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Web or Blog
                </td>
                <td>
                    <asp:TextBox ID="TextBoxWebBlog" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    If Speaking, Allow Users To Email you?
                </td>
                <td>
                    <asp:CheckBox ID="CheckBoxAllowAttendeesToEmailMe" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="visibility: hidden;">
                    If Speaking, Use WetPaint Wiki To Share Additional Information?
                </td>
                <td>
                    <asp:CheckBox ID="CheckBoxWetPaintWiki" runat="server" Visible="False" />
                </td>
            </tr>
            <tr>
                <td>
                    Bio
                </td>
                <td>
                    <asp:TextBox ID="TextBoxBio" TextMode="MultiLine" Rows="5" Columns="40" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr runat="server" id="PictureRow">
                <td style="width: 160px">
                    Picture&nbsp;
                </td>
                <td>
                    &nbsp;<asp:Image runat="server" ID="ImageUser" /><br />
                    <asp:FileUpload ID="FileUploadImage" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    (To Change your Password, click the link on the top right of the page next to the
                    logout button)
                </td>
            </tr>
             <tr>
                <td colspan="2">
                   <asp:Button ID="ButtonUnsubscribe" runat="server" 
                        Text="Please remove me from Mailing list" onclick="ButtonUnsubscribe_Click" />
                    <asp:Label ID="LabelUnsubscribe" runat="server" Visible = false Text=""></asp:Label>
                </td>
                  </tr>
        </table>
    </div>
</asp:Content>
