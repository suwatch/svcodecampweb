<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    ValidateRequest="false" MaintainScrollPositionOnPostback="true"
    Inherits="SessionPicturesDescription" Title="Session Pictures Description" Codebehind="SessionPicturesDescription.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <table>
        <tr>
            <td>
                <asp:HyperLink ID="HyperLinkReturnToPictures" runat="server">Return to Pictures</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                Session Name:&nbsp;<asp:Label ID="LabelSessionName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Instructor Name:&nbsp;<asp:Label ID="LabelPresenterName" runat="server" Text=""></asp:Label>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="Image1" runat="server" />
                
            </td>
        </tr>
        <tr>
            <td>
               
                (<asp:HyperLink ID="HyperLinkFullRes" runat="server">Full Resolution Link</asp:HyperLink>)
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            
            <td>
            <asp:Label ID="LabelDescription" runat="server" Text="Description"></asp:Label>
            <br />
                <asp:TextBox ID="TextBoxDescription" TextMode="MultiLine" Width="500" Height="100"
                    runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="ButtonUpdate" runat="server" Text="Update Description" OnClick="ButtonUpdate_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
