<%@ Page Language="C#" AutoEventWireup="true" Inherits="TwitterTest" Codebehind="TwitterTest.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

 <style type="text/css" >
    
     
        #content .twitterfeed {
        float: left;
        background-color: #fff;
        width: 385px;
        margin: 10px 0px 30px 10px;
        border: 1px solid #d8d8d8;
        padding: 5px 5px 15px 5px;
        }
        #content img {
        margin: 5px 0px 0px 10px;
        }
       
        #tweet {
        float: none;
        clear: both;
        }
        #tweet p {
        margin: 15px 15px 0px 15px;
        }
        #tweet .profile_icon {
        float: left;
        margin: 0px 10px 10px 0px;
        }
      
    </style>

</head>





<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
        <br />


        <asp:Repeater ID="RepeaterTweet" runat="server">
        <HeaderTemplate><div id="tweet"></HeaderTemplate>
        <FooterTemplate></div></FooterTemplate>

        <ItemTemplate>
        
           <div>
                <p>
                    <img width="48" height="48" alt="Twitter" src='<%# Eval("AuthorImageUrl") %>'
                        class="profile_icon"><%# Eval("ContentTweet") %>&nbsp;<%# GetCodeCampSessionsString((string)Eval("CodeCampSessionsUrl")) %></p>
            </div>
            <div id="web_intent">
                <span class="time"><%# Eval("Published") %></span>
            </div>
        
        </ItemTemplate>
        </asp:Repeater>

      <%--  <div id="tweet">
            <div>
                <p>
                    <img width="48" height="48" alt="Twitter" src="http://a3.twimg.com/profile_images/1189513240/web_dev_normal.png"
                        class="profile_icon">RT @rbates: Introducing RailsCasts Pro! More RailsCasts
                    goodness for just $9 per month! <a href="http://t.co/0TuBJOfG">http://t.co/0TuBJOfG</a></p>
            </div>
            <div id="web_intent">
                <span class="time">14 hours ago</span>
            </div>
           
        </div>--%>
    </div>
    </form>
    <p>

        &nbsp;</p>
</body>
</html>
