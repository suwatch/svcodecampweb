<%@ Page Language="C#" AutoEventWireup="true" Inherits="RssService" Title="Untitled Page" StylesheetTheme=""  ContentType="text/xml" Codebehind="Rss.aspx.cs" %><?xml version="1.0" ?>


          <asp:Repeater id="Repeater1" runat="server" DataSourceID="ObjectDataSource1">
<HeaderTemplate>
<rss version="2.0">
  <channel>
    <title><%# Server.HtmlEncode(ConfigurationManager.AppSettings["siteTitle"]) %></title>
    <description><%# Server.HtmlEncode(ConfigurationManager.AppSettings["siteDescription"]) %></description>
    <link><%# Request.Url.GetLeftPart(UriPartial.Authority) %><%# Request.ApplicationPath %></link>
    <copyright><%# Server.HtmlEncode(ConfigurationManager.AppSettings["siteCopyright"]) %></copyright>
    <language><%# Server.HtmlEncode(ConfigurationManager.AppSettings["siteLanguage"]) %></language>
</HeaderTemplate>

<ItemTemplate>
  <item>
    <title><%# Eval("post_title") %></title>
    <link><%# Request.Url.GetLeftPart(UriPartial.Authority) %><%# Request.ApplicationPath %><%# "/Default.aspx?p=" + Eval("ID")  %></link>
    <guid><%# Request.Url.GetLeftPart(UriPartial.Authority) %><%# Request.ApplicationPath %><%# "/Default.aspx?p=" + Eval("ID")  %></guid>
    <pubDate><%# Eval("post_date", "{0:R}")%></pubDate>
    <category>No Category</category>
    <description><%# FixDescription((string) Eval("post_content"))%></description>
  </item>
</ItemTemplate>

<FooterTemplate>
  </channel>
</rss>    
</FooterTemplate>

</asp:Repeater>
            
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetByDateDesc"
                TypeName="DataSetPostsTableAdapters.PostsTableAdapter">
            </asp:ObjectDataSource>
       

