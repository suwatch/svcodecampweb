<%@ Page Title="" Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true" Inherits="TwitterTest1" Codebehind="TwitterTest1.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">



    <asp:ObjectDataSource ID="ObjectDataSourceTwitter" runat="server" 
        DataObjectTypeName="CodeCampSV.TwitterUpdateResult" DeleteMethod="Delete" 
        InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="GetAll" TypeName="CodeCampSV.TwitterUpdateManager">
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
    </asp:ObjectDataSource>
    <asp:Repeater ID="RepeaterTweet" runat="server" 
        DataSourceID="ObjectDataSourceTwitter" 
        onitemcommand="RepeaterTweet_ItemCommand">
        <HeaderTemplate>
            <div id="tweet">
        </HeaderTemplate>
        <FooterTemplate>
            </div></FooterTemplate>
        <ItemTemplate>
            <div>
                <p>
                    <img width="48" height="48" alt="Twitter" src='<%# Eval("AuthorImageUrl") %>' class="profile_icon"><%# Eval("ContentTweet") %>&nbsp;<%# GetCodeCampSessionsString((string)Eval("CodeCampSessionsUrl")) %></p>
            </div>
            <div id="web_intent">
                <span class="time">
                    <%# Eval("Published") %></span>
            </div>
            <hr />
        </ItemTemplate>
    </asp:Repeater>



</asp:Content>
