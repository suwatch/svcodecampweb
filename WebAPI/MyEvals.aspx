<%@ Page Title="My Code Camp Evaluations" Language="C#" MasterPageFile="~/RightNote.master" AutoEventWireup="true" Inherits="MyEvals" Codebehind="MyEvals.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" Runat="Server">

 
 <asp:Menu ID="subMenu" runat="server" DataSourceID="SiteMapProgram" SkinID="subMenu">
    </asp:Menu>
    <%--The next line should be here, but seems to work without because it is in master page--%>
    <asp:SiteMapDataSource ID="SiteMapProgram" runat="server" ShowStartingNode="False"
        StartingNodeUrl="~/Program.aspx" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">


<h1>My Code Camp Evaluations</h1>
   <%-- <em>Complete Overall Evaluation and at least one session evaluation and you are eligible for the Dice IPad raffle</em><br />--%>
&nbsp;(<asp:Literal ID="LiteralEvalsDoneThisYear" runat="server"></asp:Literal> people have evaluated code camp with <asp:Literal ID="LiteralSessionsEvaluatedThisYear" runat="server"></asp:Literal> Sessions Evaluated)

<hr />

<h2>Overall Evaluation</h2>

<asp:HyperLink ID="HyperLinkCodeCampEval" NavigateUrl="~/CodeCampEval.aspx?Return=MyEvals"  runat="server">Code Camp Evaluation</asp:HyperLink>

&nbsp;&nbsp;
    <asp:Label ID="LabelDoneEval" runat="server" Text=""></asp:Label>

<br />
<hr />
<br />



<h2>Sessions I've Not Evaluated</h2>
<i>That I marked as Interested or Plan To Attend</i>


 <asp:SqlDataSource ID="SqlDataSourceEvalsNotDone" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="SELECT SessionId,
       Title,
       UserFirstName,
       UserLastName,
       StartTimeFriendly
FROM SessionsOverview
WHERE SessionId IN (
                     SELECT Sessions_Id
                     FROM SessionAttendee
                     WHERE sessions_id IN (
                                            select id
                                            from Sessions
                                            WHERE CodeCampYearId = @CodeCampYearId
                           ) AND
                           (Interestlevel = 2 OR
                           InterestLevel = 3) AND
                           attendees_username =
                           (
                             SELECT PKID
                             FROM attendees
                             WHERE username = @Username
                           )
      ) AND
      SessionId NOT IN (
                         SELECT dbo.Sessions.Id
                         FROM dbo.SessionEvals
                              INNER JOIN dbo.Sessions ON (
                              dbo.SessionEvals.SessionId = dbo.Sessions.Id)
                         WHERE CodeCampYearId = @CodeCampYearId AND
                               PKID =
                               (
                                 SELECT PKID
                                 FROM Attendees
                                 WHERE Username = @Username
                               )
      )
ORDER BY StartTime,
         Title">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                PropertyName="Text" />
            <asp:ControlParameter ControlID="LabelCurrentUser" Name="Username" 
                PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>




<div style="border-style: 50; padding: 20px; position: relative;">


    <asp:Repeater ID="Repeater1" runat="server" 
        DataSourceID="SqlDataSourceEvalsNotDone">
        <HeaderTemplate><ul></HeaderTemplate>
        <FooterTemplate></ul></FooterTemplate>
        <ItemTemplate>
           <li>
            <asp:HyperLink ID="HyperLinkSession" 
              NavigateUrl='<%# "SessionEval.aspx?id=" + Eval("SessionId") %>'
              Text='<%# Eval("Title") + " " + Eval("UserFirstName") + " " + Eval("UserLastName") + "  " + Eval("StartTimeFriendly") %>'
              runat="server">
            </asp:HyperLink>
        </li>
        </ItemTemplate>


    </asp:Repeater>
</div>



<h2>Sessions I've Evaluated</h2>

<div style="border-style: 50; padding: 20px; position: relative;">


    <asp:Repeater ID="RepeaterEvalsDone" runat="server" 
        DataSourceID="SqlDataSourceEvalsDone">
        <HeaderTemplate><ul></HeaderTemplate>
        <FooterTemplate></ul></FooterTemplate>
        <ItemTemplate>
           <li>
            <asp:HyperLink ID="HyperLinkSession" 
              NavigateUrl='<%# "SessionEval.aspx?Return=MyEvals&id=" + Eval("SessionId") %>'
              Text='<%# Eval("Title") + " " + Eval("UserFirstName") + " " + Eval("UserLastName") + "  " + Eval("StartTimeFriendly") %>'
              runat="server">
            </asp:HyperLink>
        </li>
        </ItemTemplate>


    </asp:Repeater>
</div>


    <asp:SqlDataSource ID="SqlDataSourceEvalsDone" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="SELECT SessionId,
       Title,
       UserFirstName,
       UserLastName,
       StartTimeFriendly
FROM SessionsOverview
WHERE SessionId IN (
SELECT dbo.Sessions.Id
FROM dbo.SessionEvals
     INNER JOIN dbo.Sessions ON (dbo.SessionEvals.SessionId = dbo.Sessions.Id)
WHERE CodeCampYearId = @CodeCampYearId AND
      PKID =
      (
        SELECT PKID
        FROM Attendees
        WHERE Username = @Username
      )
)">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                PropertyName="Text" />
            <asp:ControlParameter ControlID="LabelCurrentUser" Name="Username" 
                PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>


<asp:Label ID="LabelCodeCampYearId" Visible="false" runat="server" Text="Label"></asp:Label>
<asp:Label ID="LabelCurrentUser" Visible="false" runat="server" Text="Label"></asp:Label>








</asp:Content>

