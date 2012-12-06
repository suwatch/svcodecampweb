<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SponsorsList.ascx.cs" Inherits="WebAPI.SponsorsList" %>

 <div class="sponsors">

      <%-- update sponsorlist set imageurl = '' where imageurl is null--%>
      
        <div class="sponsorBar"><div class="right"></div><div class="left"></div><h3>Platinum Sponsors</h3></div>
        <asp:Repeater ID="RepeaterSponsors" runat="server" DataSourceID="SqlDataSourceSponsorsPlatinum">
            <HeaderTemplate>
            <ul>
            </HeaderTemplate>
            <FooterTemplate>
            </ul>
            </FooterTemplate>
            <ItemTemplate>
                <li>

                <a id="A1" runat="server" href='<%# Eval("NavigateURL")  %>' target="_blank" >
                   <asp:Image ID="ImageSprite2" Visible='<%# (bool) ShowImageIfGood((string) Eval("ImageURL")) %>' CssClass="imgLink"
                       runat="server" ImageUrl='<%# GetSpriteLocation((string) Eval("ImageURL"),(int) Eval("Id"),(string) Eval("CompanyImageType"),"Platinum") %>' ToolTip='<%# Eval("HoverOverText") %>'  /></a>
               

                     <%--  
                    <asp:HyperLink ID="HyperLink1" CssClass="imgLink" Target="_blank" ToolTip='<%# Eval("HoverOverText") %>' ImageUrl='<%# Eval("ImageURL") %>'
                        Visible='<%# (bool) ShowImageIfGood((string) Eval("ImageURL")) %>' NavigateUrl='<%# Eval("NavigateURL")  %>'
                        runat="server"></asp:HyperLink>--%>
                    <br /><asp:HyperLink ID="HyperLinkSponsor" Target="_blank" ToolTip='<%# Eval("HoverOverText") %>' Text='<%# Eval("HoverOverText") %>'
                        NavigateUrl='<%# Eval("NavigateURL")  %>' 
                        onclick='<%# (string) GetJobURLonclick((string) Eval("HoverOverText"))%>'
                        runat="server"></asp:HyperLink>
                </li>
            </ItemTemplate>
        </asp:Repeater>
        <div class="sponsorBar"><div class="right"></div><div class="left"></div><h3>Gold Sponsors</h3></div>
        <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSourceSponsorsGold">
            <HeaderTemplate>
            <ul class="imgSmall">
            </HeaderTemplate>
            <FooterTemplate>
            </ul>
            </FooterTemplate>
            
            <ItemTemplate>
              <li>
                     <a id="A1" runat="server" href='<%# Eval("NavigateURL")  %>' target="_blank" >
                       <asp:Image ID="ImageSprite2" Visible='<%# (bool) ShowImageIfGood((string) Eval("ImageURL")) %>' CssClass="imgLink"
                           runat="server" ImageUrl='<%# GetSpriteLocation((string) Eval("ImageURL"),(int) Eval("Id"),(string) Eval("CompanyImageType"),"Gold") %>' ToolTip='<%# Eval("HoverOverText") %>'  /></a>
                   <%-- <asp:HyperLink ID="HyperLink1" CssClass="imgLink" Target="_blank" ToolTip='<%# Eval("HoverOverText") %>' ImageUrl='<%# Eval("ImageURL") %>'
                        Visible='<%# (bool) ShowImageIfGood((string) Eval("ImageURL")) %>' NavigateUrl='<%# Eval("NavigateURL")  %>'
                        runat="server"></asp:HyperLink>--%>
                    <br /><asp:HyperLink ID="HyperLinkSponsor" Target="_blank" ToolTip='<%# Eval("HoverOverText") %>' Text='<%# Eval("HoverOverText") %>'
                        NavigateUrl='<%# Eval("NavigateURL")  %>' runat="server"></asp:HyperLink>
                </li>
               
            </ItemTemplate>
        </asp:Repeater>
        
       
        <div class="sponsorBar"><div class="right"></div><div class="left"></div><h3>Silver Sponsors</h3></div>

            <asp:Repeater ID="Repeater2" runat="server" DataSourceID="SqlDataSourceSponsorsSilver">
            <HeaderTemplate>
            <ul class="imgSmall">
            </HeaderTemplate>
            <FooterTemplate>
            </ul>
            </FooterTemplate>
            
            <ItemTemplate>
            
             <li>
                     <a id="A1" runat="server" href='<%# Eval("NavigateURL")  %>' target="_blank" >
                       <asp:Image ID="ImageSprite2" Visible='<%# (bool) ShowImageIfGood((string) Eval("ImageURL")) %>' CssClass="imgLink"
                           runat="server" ImageUrl='<%# GetSpriteLocation((string) Eval("ImageURL"),(int) Eval("Id"),(string) Eval("CompanyImageType"),"Silver") %>' ToolTip='<%# Eval("HoverOverText") %>'  /></a>
                   <%-- <asp:HyperLink ID="HyperLink1" CssClass="imgLink" Target="_blank" ToolTip='<%# Eval("HoverOverText") %>' ImageUrl='<%# Eval("ImageURL") %>'
                        Visible='<%# (bool) ShowImageIfGood((string) Eval("ImageURL")) %>' NavigateUrl='<%# Eval("NavigateURL")  %>'
                        runat="server"></asp:HyperLink>--%>
                    <br /><asp:HyperLink ID="HyperLinkSponsor" Target="_blank" ToolTip='<%# Eval("HoverOverText") %>' Text='<%# Eval("HoverOverText") %>'
                        NavigateUrl='<%# Eval("NavigateURL")  %>' runat="server"></asp:HyperLink>
                </li>
            </ItemTemplate>
        </asp:Repeater>

         <div class="sponsorBar"><div class="right"></div><div class="left"></div><h3>Bronze Sponsors</h3></div>

            <asp:Repeater ID="Repeater3" runat="server" DataSourceID="SqlDataSourceSponsorsBronze">
            <HeaderTemplate>
            <ul class="imgSmall">
            </HeaderTemplate>
            <FooterTemplate>
            </ul>
            </FooterTemplate>
            
            <ItemTemplate>
            
            <li>
                     <a id="A1" runat="server" href='<%# Eval("NavigateURL")  %>' target="_blank" >
                       <asp:Image ID="ImageSprite2" Visible='<%# (bool) ShowImageIfGood((string) Eval("ImageURL")) %>' CssClass="imgLink"
                           runat="server" ImageUrl='<%# GetSpriteLocation((string) Eval("ImageURL"),(int) Eval("Id"),(string) Eval("CompanyImageType"),"Bronze") %>' ToolTip='<%# Eval("HoverOverText") %>'  /></a>
                    
                 <%--   <asp:HyperLink ID="HyperLink1" CssClass="imgLink" Target="_blank" ToolTip='<%# Eval("HoverOverText") %>' ImageUrl='<%# Eval("ImageURL") %>'
                        Visible='<%# (bool) ShowImageIfGood((string) Eval("ImageURL")) %>' NavigateUrl='<%# Eval("NavigateURL")  %>'
                        runat="server"></asp:HyperLink>--%>
                    <br /><asp:HyperLink ID="HyperLinkSponsor" Target="_blank" ToolTip='<%# Eval("HoverOverText") %>' Text='<%# Eval("HoverOverText") %>'
                        NavigateUrl='<%# Eval("NavigateURL")  %>' runat="server"></asp:HyperLink>
                </li>
            </ItemTemplate>
        </asp:Repeater>
        
        <div class="sponsorBar"><div class="right"></div><div class="left"></div><h3>Community Sponsors</h3></div>

            <asp:Repeater ID="RepeaterCommunity" runat="server" DataSourceID="SqlDataSourceSponsorsCommunity">
            <HeaderTemplate>
            <ul class="imgSmall">
            </HeaderTemplate>
            <FooterTemplate>
            </ul>
            </FooterTemplate>
            
            <ItemTemplate>
            
           <li>
                     <a id="A1" runat="server" href='<%# Eval("NavigateURL")  %>' target="_blank" >
                       <asp:Image ID="ImageSprite2" Visible='<%# (bool) ShowImageIfGood((string) Eval("ImageURL")) %>' CssClass="imgLink"
                           runat="server" ImageUrl='<%# GetSpriteLocation((string) Eval("ImageURL"),(int) Eval("Id"),(string) Eval("CompanyImageType"),"Community") %>' ToolTip='<%# Eval("HoverOverText") %>'  /></a>
                    
                  <%--  <asp:HyperLink ID="HyperLink1" CssClass="imgLink" Target="_blank" ToolTip='<%# Eval("HoverOverText") %>' ImageUrl='<%# Eval("ImageURL") %>'
                        Visible='<%# (bool) ShowImageIfGood((string) Eval("ImageURL")) %>' NavigateUrl='<%# Eval("NavigateURL")  %>'
                        runat="server"></asp:HyperLink>--%>
                    <br /><asp:HyperLink ID="HyperLinkSponsor" Target="_blank" ToolTip='<%# Eval("HoverOverText") %>' Text='<%# Eval("HoverOverText") %>'
                        NavigateUrl='<%# Eval("NavigateURL")  %>' runat="server"></asp:HyperLink>
                </li>
            </ItemTemplate>
        </asp:Repeater>

    </div>


    <asp:SqlDataSource ID="SqlDataSourceSponsorsPlatinum" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        SelectCommand="SEE CODE BEHIND" 
          CacheDuration="5" EnableCaching="True">
    <SelectParameters>
             <asp:ControlParameter ControlID="LabelCodeCampYearId" DefaultValue="" 
                 Name="CodeCampYearId" PropertyName="Text" />
         </SelectParameters>
    </asp:SqlDataSource>
   
    
    <asp:SqlDataSource ID="SqlDataSourceSponsorsGold" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        SelectCommand="SEE CODE BEHIND" 
          CacheDuration="5" EnableCaching="True">
    <SelectParameters>
             <asp:ControlParameter ControlID="LabelCodeCampYearId" DefaultValue="" 
                 Name="CodeCampYearId" PropertyName="Text" />
         </SelectParameters>
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="SqlDataSourceSponsorsSilver" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        SelectCommand="SEE CODE BEHIND" 
          CacheDuration="5" EnableCaching="True">
    <SelectParameters>
             <asp:ControlParameter ControlID="LabelCodeCampYearId" DefaultValue="" 
                 Name="CodeCampYearId" PropertyName="Text" />
         </SelectParameters>
    </asp:SqlDataSource>

     <asp:SqlDataSource ID="SqlDataSourceSponsorsBronze" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        SelectCommand="SEE CODE BEHIND" 
          CacheDuration="5" EnableCaching="True">
    <SelectParameters>
             <asp:ControlParameter ControlID="LabelCodeCampYearId" DefaultValue="" 
                 Name="CodeCampYearId" PropertyName="Text" />
         </SelectParameters>
    </asp:SqlDataSource>
    
     <asp:SqlDataSource ID="SqlDataSourceSponsorsCommunity" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        SelectCommand="SEE CODE BEHIND" 
          CacheDuration="5" EnableCaching="True">
             <SelectParameters>
                 <asp:ControlParameter ControlID="LabelCodeCampYearId" DefaultValue="" 
                     Name="CodeCampYearId" PropertyName="Text" />
             </SelectParameters>
    </asp:SqlDataSource>

    <asp:Label ID="LabelCodeCampYearId" runat="server" Text="" Visible="false" ></asp:Label>



