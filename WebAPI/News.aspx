<%@ Page Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true" Inherits="News" Title="News" Codebehind="News.aspx.cs" %>

<asp:Content ID="SessionsContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Begin container for news listings -->
    
    <div class="mainHeading">News</div>
    
    <div class="pad"  runat="server" id="MainHeaderCountID" >
         <asp:Label ID="LabelStatus" runat="server" />
    </div>
        <asp:ObjectDataSource ID="ObjectDataSourceArticle" runat="server" SelectMethod="GetByDateDesc"
            TypeName="DataSetPostsTableAdapters.PostsTableAdapter" EnableCaching="true" CacheDuration="60">
        </asp:ObjectDataSource>
       

        <asp:Repeater ID="RepeaterArticle" runat="server" DataSourceID="ObjectDataSourceArticle"
            OnItemDataBound="RepeaterArticle_ItemDataBound">
            <ItemTemplate>
                <%--<table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <div class="mainHeading">
                                Update:
                                <asp:Label ID="Label1" runat="server" Text='<%# GetDateTimeOfPost(Convert.ToDateTime( Eval("post_date"))) %>'></asp:Label></div>
                        </td>
                        <td class="pad" >
                            Posted by
                            <asp:Label ID="LabelAuthor" runat="server" Text='<%# Eval("post_excerpt") %>'  ></asp:Label>
                        </td>
                    </tr>
                </table>
                
                <div class="subHeading" >
                <asp:Label runat="server" ID="PostTitleID" Text='<%# Eval("post_title") %>'   ></asp:Label>
                </div>
                <br />
                <div class="pad">
                <asp:Label ID="LabelShowContent" runat="server" Text='<%# Eval("post_content") %>'></asp:Label>
                </div>--%>
                
                <div class="mainHeading">
                    Update:
                    <asp:Label ID="Label1" runat="server" Text='<%# GetDateTimeOfPost(Convert.ToDateTime( Eval("post_date"))) %>'></asp:Label>
                 </div>
            
                <div class="pad">
                    Posted by
                    <asp:Label ID="LabelAuthor" runat="server" Text='<%# Eval("post_excerpt") %>'  ></asp:Label>
                    
                    
                    <div class="subHeading" >
                    <asp:Label runat="server" ID="PostTitleID" Text='<%# Eval("post_title") %>'   ></asp:Label>
                    </div>
                    
                    
                    <asp:Label ID="LabelShowContent" runat="server" Text='<%# Eval("post_content") %>'></asp:Label>
                    
                                
                </div>
                
                </ItemTemplate>
                
         </asp:Repeater>
    
    <!-- End container for news listings -->
</asp:Content>
