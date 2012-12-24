<%@ Page validateRequest="false" Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true" Inherits="SpreadTheWord" Title="Spread the Word About SV CodeCamp" Codebehind="SpreadTheWord.aspx.cs" %>

<asp:Content ID="SublinksPrevious" ContentPlaceHolderID="Sublinks" runat="server">
    <asp:Menu ID="previousSubMenu" runat="server" DataSourceID="SiteMapAbout" SkinID="subMenu">
    </asp:Menu>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <%--<h1>
        Spread the Word!</h1>--%>
        
        
<div class="mainHeading">Spread the Word!</div>
        
    <h2>
        Silicon Valley Code Camp, 
        <asp:Label ID="LabelCodeCampDate" runat="server" Text=""></asp:Label></h2>

        
    <p>Help let everyone know about codecamp!  You can cut and paste the below HTML and put them on
    your own blog or web site.  It would also be a great help if you know of other web sites 
    developers look at such as internal web sites, please see if you can get the HTML below
    pasted there.  All the HTML does is display a jpg with an anchor tag for linking.  Thanks, from the 
    SV Codecamp Team.</p>
    
    <p>(If you are logged on, the HTML below includes a reference to you so that when someone
    signs up, you get credit for the referral.  You can see the number of referrals you've generated
    by selecting "Update Profile" in the upper left when you are logged in.</p>
    <br /><br />
    <table>
        <tr>
            <td style="width: 10px" rowspan="10"></td>
            <td>
                How it Will Look
            </td>
            <td>
                HTML to use
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td>    
                <br /><b>Simple Graphic No Status</b>
            </td>
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="ImageAd1"   ImageUrl="~/DisplayAd.ashx?ImageType=1"  runat="server" />
            </td>
            <td>
                <asp:TextBox ID="TextBoxAd1" runat="server" Rows="5" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>    
                <br /><b>Simple Graphic With Status</b>
            </td>
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="ImageAd1a"   ImageUrl="~/DisplayAd.ashx?ImageType=2"  runat="server" />
            </td>
            <td>
                <asp:TextBox ID="TextBoxAd1a" runat="server" Rows="5" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>   
                <br /><b>Larger Graphic No Status (not to scale)</b>                 
            </td>
            <td></td>
        </tr>
        <tr>
            
            <td>
                <asp:Image ID="ImageAd2" Width="300"    ImageUrl="~/DisplayAd.ashx?ImageType=3"  runat="server" />
            </td>
            <td>
                <asp:TextBox ID="TextBoxAd2"  runat="server" Rows="5" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>    
                <br /><b>Larger Graphic With Status (not to scale)</b>                                 
            </td>
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="ImageAd2a"  Width="300"  ImageUrl="~/DisplayAd.ashx?ImageType=4"  runat="server" />
            </td>
            <td>
                <asp:TextBox ID="TextBoxAd2a" runat="server" Rows="5" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <td colspan="3">
            <br />
                Simple URL to Cut and Paste the URL below and Email it to someone
                <br />
            </td>
            </tr>
            
            <tr>
            <td colspan="3">
                <asp:TextBox ID="TextBoxURL" runat="server" Width="600" Rows="3" TextMode="MultiLine"></asp:TextBox>
            <br />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:HyperLink ID="HyperLinkURL" runat="server"></asp:HyperLink>
            <br />
            </td>
            </tr>
    </table>
</asp:Content>
