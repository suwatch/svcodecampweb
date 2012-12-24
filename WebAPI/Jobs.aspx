<%@ Page  Language="C#" MasterPageFile="~/Jobs.master" AutoEventWireup="true"  Title="Software Engineering Jobs" Inherits="Jobs1" Codebehind="Jobs.aspx.cs" %>



<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" runat="Server">
    <h1>
        <div style="float: right;">
            <a href="mailto:JobAds@SiliconValley-CodeCamp.com?subject=I'm Interested In Placing a Job Ad on Silicon Valley Code Camp Site">
                Email Me Job Ad Pricing</a> &nbsp;&nbsp;&nbsp;
        </div>
        <div class="mainHeading">
            <asp:Literal ID="LiteralTitle" Text="Jobs.siliconvalley-codecamp.com" runat="server"></asp:Literal>
            &nbsp;&nbsp;&nbsp;
        </div>
    </h1>

     <asp:SqlDataSource ID="SqlDataSourceJobs" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        SelectCommand=" SELECT 
                    Id,
                    JobName,
                    JobCompanyName,
                    JobLocation,
                    JobURL,
                    JobBrief,
                    StartRunDate,
                    EndRunDate,
                    HideListing
                     FROM SponsorListJobListing
                     WHERE GETDATE() &gt;= StartRunDate AND
                           GETDATE() &lt;= EndRunDate AND
                           HideListing = 0 AND
               SponsorListId &gt;= @SponsorListIdStart AND
               SponsorListId &lt;= @SponsorListIdStop
                     ORDER BY StartRunDate DESC">
         <SelectParameters>
             <asp:QueryStringParameter DefaultValue="0" Name="SponsorListIdStart" 
                 QueryStringField="SponsorListIdStart" />
             <asp:QueryStringParameter DefaultValue="999999" Name="SponsorListIdStop" 
                 QueryStringField="SponsorListIdStop" />
         </SelectParameters>



    </asp:SqlDataSource>
    <asp:Repeater ID="RepeaterJobs" Visible="true" runat="server" DataSourceID="SqlDataSourceJobs">
        <HeaderTemplate>
            <div class="jobListWrap">
                <p>
                    <i>Great software jobs, great people. A part of Silicon Valley Code Camp</i></p>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="listItem">
                <div class="postTime">
                    <p>
                        &lt;&nbsp;&nbsp;
                        <asp:Literal Visible="true" ID="Literal1" Text='<%# GetHowLongAgo((DateTime) Eval("StartRunDate")) %>'
                            runat="server"></asp:Literal></p>
                </div>

                 <h3>
                <asp:HyperLink ID="HyperLinkJobURL" NavigateUrl='<%# Eval("JobURL")%>' title='<%# Eval("JobName")%>' Target="_blank" rel="nofollow" 
                      Text='<%# Eval("JobName")%>' onclick='<%# GetJobURLonclick((string) Eval("JobName"),(string) Eval("JobCompanyName"))%>'
                      CssClass="jobTitleLink"
                      runat="server"></asp:HyperLink>
                </h3>
               <%-- <a href="<%# Eval("JobURL")%>" class="jobTitleLink" title="<%# Eval("JobName")%>">
                    <%# Eval("JobName")%>
                </a>--%>

               
                <span class="company" title="<%# Eval("JobCompanyName")%>">
                    <%# Eval("JobCompanyName")%>
                </span>
               

                <p class="location" title="<%# Eval("JobLocation")%>">
                    <%# Eval("JobLocation")%>
                </p>
                <p class="description">
                    <%# Eval("JobBrief")%>
                </p>
            </div>
        </ItemTemplate>
        <FooterTemplate>
            </div>
        </FooterTemplate>
    </asp:Repeater>
   
    <%--    <div class="jobListWrap">

    <p><i>Great software jobs, great people. A part of Silicon Valley Code Camp</i></p>
        
     <div class="listItem">
        <div class="postTime" ><p>&lt; 1 hour ago</p></div>

        <a href="http://siliconvalley-codecamp.com/" class="jobTitleLink" title="Automation Architect ">Automation Architect </a><span class="company" title="BOX.NET">BOX.NET  </span>
        <p class="location" title="Irvine, CA">
            Palo Alto, CA United States
        </p>
        <p class="description">
            As the Automation Architect at Box.net, you will work closely with the Director of Quality Engineering and VP of Technology.  You will be...
        </p>
    </div>

     <div class="listItem">
        <div class="postTime" ><p>&lt; 1 hour ago</p></div>

        <a href="http://siliconvalley-codecamp.com/" class="jobTitleLink" title="Backend Guru ">Backend Guru </a><span class="company" title="BOX.NET">BOX.NET  </span>
        <p class="location" title="Irvine, CA">
            Palo Alto, CA United States
        </p>
        <p class="description">
             We’re looking for a Backend Guru to design and execute projects to implement and optimize our backend PHP architecture. To serve millions...
        </p>
    </div>

    <div class="listItem">
        <div class="postTime" ><p>&lt; 1 hour ago</p></div>

        <a href="http://siliconvalley-codecamp.com/" class="jobTitleLink" title="Web Developer">Web Developer</a><span class="company" title="BOX.NET">BOX.NET  </span>
        <p class="location" title="Irvine, CA">
            Palo Alto, CA United States
        </p>
        <p class="description">
             As a web developer at Box.net, you will have the ability to join in this mission at the ground level. Our development team is still small (less than 20 developers), moves quickly,...
        </p>
    </div>


    </div>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SidebarRight" runat="Server">
</asp:Content>
