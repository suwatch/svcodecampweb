<%@ Control Language="C#" AutoEventWireup="true" Inherits="SessionTweetList" Codebehind="SessionTweetList.ascx.cs" %>


<asp:Repeater ID="RepeaterSessions" runat="server">

    <HeaderTemplate>
           <table>
    </HeaderTemplate>

    <ItemTemplate>

      <tr>
          <td>
              
         
          
            
            <%-- <asp:HyperLink ID="HyperLinkReTweet" runat="server" 
                NavigateUrl="http://twitter.com/home?status=RT+@pkellner+Release+2+in+store+today+http://agelessemail.com"
                Text="RT+@pkellner+Release+2+in+store+today+http://agelessemail.com"
                  >
                
            </asp:HyperLink>--%>
            
             <asp:HyperLink ID="HyperLinkReTweet" runat="server" 
                onclick='<%# GetURLonclick((int) Eval("SessionId"),(string) Eval("SessionTitle"))%>'
                NavigateUrl='<%# GetNavigateUrl((int) Eval("SessionId")) %>'
                Text='<%# GetNavigateText((int) Eval("SessionId")) %>'
                  >
                
            </asp:HyperLink>
            

         </td>

      </tr>



    </ItemTemplate>



    <FooterTemplate>
       </table>
    </FooterTemplate>


</asp:Repeater>
