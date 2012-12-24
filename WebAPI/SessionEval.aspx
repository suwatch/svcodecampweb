<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"  Inherits="SessionEval" Title="Session Evaluation" Codebehind="SessionEval.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" runat="Server">
    Please answer the following questions as honestly as you can.&nbsp; This is CodeCamp,
    which means all instructors are volunteers and are here to improve themselves and
    help the community.&nbsp; No feedback will be made public.&nbsp; It is primarily
    for the instructors benefit.<br />
    <br />
    Session Name:&nbsp;<asp:Label ID="LabelSessionName" runat="server" Text=""></asp:Label>
    <br />
    Instructor Name:&nbsp;<asp:Label ID="LabelPresenterName" runat="server" Text=""></asp:Label>
    <br />
    <asp:HyperLink Visible="false" ID="HyperLinkReturn1" runat="server">Return to Session</asp:HyperLink><br />
     <asp:HyperLink Visible="false" ID="HyperLinkReturn3" runat="server">Return to Session Overview</asp:HyperLink><br />
     <asp:HyperLink Visible="false" ID="HyperLink1" runat="server">Return to My Session Evals</asp:HyperLink><br />

    <br />
    <asp:Button ID="ButtonUpdate1" runat="server" Text="Submit Evaluation" Enabled="False" OnClick="ButtonUpdate1_Click" />
    <asp:Label ID="Label1" runat="server"></asp:Label><br />
    <br />
    <table width="700" border="0"  bgcolor="#ccffff" cellpadding="3">
        <tr>
            <td style="float: right">
                Course As A Whole
            </td>
             <td style="width: 139px" >
                <asp:RadioButtonList ID="RBLCourseAsWhole" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td style="float: right;">
                Course Content
            </td>
             <td style="width: 139px" >
                <asp:RadioButtonList ID="RBLCourseContent" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
            </td>
        </tr>
         
        <tr>
            <td style="float: right;">
                Instructors ability to explain material
            </td>
             <td style="width: 139px" >
                <asp:RadioButtonList ID="RBLInstructorAbilityExplain" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td style="float: right;">
                Effectiveness of Instructor
            </td>
             <td style="width: 139px" >
                <asp:RadioButtonList ID="RBLInstructorEffective" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td style="float: right;">
                Instructors Knowledge of Material
            </td>
             <td style="width: 139px" >
                <asp:RadioButtonList ID="RBLInstructorKnowledge" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td style="float: right;">
                Quality of Facility
            </td>
             <td style="width: 139px" >
                <asp:RadioButtonList ID="RBLQualityOfFacility" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td style="float: right;">
                Was Content Level As Advertised?
            </td>
             <td style="width: 139px" >
                <asp:RadioButtonList ID="RBLContentLevel" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
            </td>
        </tr>
        
        <tr>
            <td style="float: right;">
                Overall Feeling How Code Camp is Going?
            </td>
             <td style="width: 139px" >
                <asp:RadioButtonList ID="RBLOverallCodeCamp" runat="server" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
            </td>
        </tr>
        
        <tr>
            <td style="float: right;">
                Would You Like Your Name and Email Disclosed to the Presenter With Your Evaluation?
            </td>
             <td style="width: 139px" >
                 <asp:CheckBox ID="CheckBoxDiscloseName" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="float: right">
             <br />
                What was your favorite part of the session?
            </td>
        </tr>
        
        <tr>
            <td style="float: right;" colspan="2">
                 <asp:TextBox ID="TextBoxFavorite" TextMode="MultiLine" Width="600px" Height="80" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="float: right">
             <br />
                How could the session be improved?</td>
        </tr>
        
         <tr>
            <td style="float: right;" colspan="2">
               <asp:TextBox ID="TextBoxImproved" TextMode="MultiLine" Width="600px" Height="80" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="float: right">
            <br />
                General Comments For the Instructor Below</td>
        </tr>
        
          <tr>
            <td style="float: right;" colspan="2">
                
                 <asp:TextBox ID="TextBoxGeneralComments" TextMode="MultiLine" Width="600px" Height="80" runat="server"></asp:TextBox></td>
        </tr>
    </table>
    <br />
    <asp:Button ID="ButtonUpdate2" runat="server" Text="Submit Evaluation" Enabled="False" OnClick="ButtonUpdate2_Click" />
    
    
    <asp:Label ID="Label2" runat="server"></asp:Label><br />
    
    
    <br />
    <asp:HyperLink  Visible="false" ID="HyperLinkReturn2" runat="server">Return to Session</asp:HyperLink><br />
     <asp:HyperLink  Visible="false" ID="HyperLinkReturn4" runat="server">Return to Session Overview</asp:HyperLink><br />
        <asp:HyperLink Visible="false" ID="HyperLink2" runat="server">Return to My Session Evals</asp:HyperLink><br />
    <br />
</asp:Content>
