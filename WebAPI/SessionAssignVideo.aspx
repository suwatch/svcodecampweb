<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true" Inherits="SessionAssignVideo" Codebehind="SessionAssignVideo.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="blankSublinks" Runat="Server">
    
       
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" Runat="Server">

    <tr>
        <td>

 <br />
 <br />
 <br />
 <br />
 <br />
        <table style="width: 100%">
            <tr>
                <td style="width: 176px">
                    Static Image&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td>
                    <asp:FileUpload ID="FileUploadStaticImage" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 176px">
                    Youtube URL</td>
                <td>
                    <asp:TextBox ID="TextBoxYoutubeURL" runat="server" Width="590px"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td style="width: 176px">
                </td>
                <td>
                     http://www.youtube.com/watch?v=ZN3j_C3QNJE
                </td>
            </tr>


           


            <tr>
                <td style="width: 176px">
                    Descriptive Text</td>
                <td>
                    <asp:TextBox ID="TextBoxDescrText" runat="server" Height="80px" TextMode="MultiLine" 
                        Width="591px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 176px">
                    <asp:Button ID="ButtonUpload" runat="server" Text="Upload" 
                        onclick="ButtonUploadClick" />
                </td>
                <td>
                    <asp:Button ID="ButtonDelete" runat="server" onclick="ButtonDeleteClick" 
                        Text=" Delete Current Video From Session" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                    <br />
                    <asp:Image ID="ImageStaticVideo" runat="server" />
                    <br />
                    <br />
                    <asp:HyperLink ID="HyperLinkSession" runat="server">Back To Session</asp:HyperLink>
                    <br />
                </td>
            </tr>
        </table>







        </td>
    </tr>







</asp:Content>

