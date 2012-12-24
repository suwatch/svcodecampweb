<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" Inherits="StatsRep" Codebehind="StatsRep.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Stats Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <a href="Default.aspx">CodeCamp</a>
        <br />
    
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>&nbsp;<br /><br />
        &nbsp; &nbsp;<br />
        <table border="1" cellpadding="3">
            <tr>
                <td>
                    Code Camp Evaluations Done</td>
                <td>
                    <asp:Label ID="LabelCodeCampEvals" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    Session Evaluations Done</td>
                <td>
                    <asp:Label ID="LabelEvals" runat="server" Text="Label"></asp:Label></td>
            </tr>
             <tr>
                <td>
                    Attendee Both Days</td>
                <td>
                    <asp:Label ID="LabelAttendingBothDays" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Saturday Classes</td>
                <td>
                    <asp:Label ID="LabelSaturdayClasses" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    Sunday Classes</td>
                <td>
                    <asp:Label ID="LabelSundayClasses" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    Total Sessions With Interest</td>
                <td>
                    <asp:Label ID="LabelSessionsInterest" runat="server" Text="Label"></asp:Label></td>
            </tr>
            
              <tr>
                <td>
                    Total Sessions With Plan Attend</td>
                <td>
                    <asp:Label ID="LabelSessionsPlanAttend" runat="server" Text="Label"></asp:Label></td>
            </tr>

            <%-- <tr>
                <td>
                    Speaker Can Email Interested</td>
                <td>
                    <asp:Label ID="LabelSpeakerEmailInterested" runat="server" Text="Label"></asp:Label></td>
            </tr>

              <tr>
                <td>
                    Speaker Can Email PlanToAttend</td>
                <td>
                    <asp:Label ID="LabelSpeakerEmailPlanToAttend" runat="server" Text="Label"></asp:Label></td>
            </tr>--%>
            
            
          

             <tr>
                <td>
                    Number Letting Speaker Send Interested</td>
                <td>
                    <asp:Label ID="LabelSendSpeakerInterested" runat="server" Text="Label"></asp:Label></td>
            </tr>

             <tr>
                <td>
                    Number Letting Speaker Send Plan To Attend</td>
                <td>
                    <asp:Label ID="LabelSendSpeakerPlanToAttend" runat="server" Text="Label"></asp:Label></td>
            </tr>
 
            <tr>
                <td>
                    Number Users With Activity Last 7 days</td>
                <td>
                    <asp:Label ID="LabelLast7DaysActivity" runat="server" Text="Label"></asp:Label></td>
            </tr>
            
        </table>
        <br />
        
           <h4>Registered Over Past 30 days</h4>
         <asp:GridView ID="GridView6" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="SqlDataSource6">
            <Columns>
                <asp:BoundField DataField="DayOfMonth" HeaderText="Day of Month" ReadOnly="True" SortExpression="" />
                <asp:BoundField DataField="Count" HeaderText="Count" ReadOnly="True" SortExpression="" />
            </Columns>
        </asp:GridView>
       
          <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
            SelectCommand="SELECT datepart(day,createDate) as DayOfMonth, count(*) as Count FROM AttendeesCodeCampYear where  createDate > getdate() - 30 group by datepart(day,createDate) order by  datepart(day,createDate) desc">
        </asp:SqlDataSource>
        
        <h4>New Registered</h4>
        <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
            <Columns>
                <asp:BoundField DataField="Number Registered" HeaderText="Number Registered" ReadOnly="True"
                    SortExpression="Number Registered" />
                <asp:BoundField DataField="Total" HeaderText="Total" ReadOnly="True" SortExpression="Total" />
            </Columns>
        </asp:GridView>


         <asp:GridView ID="GridView4" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="SqlDataSource4">
            <Columns>
                <asp:BoundField DataField="Number Registered" HeaderText="Number Registered" ReadOnly="True"
                    SortExpression="Number Registered" />
                <asp:BoundField DataField="Total" HeaderText="Total" ReadOnly="True" SortExpression="Total" />
            </Columns>
        </asp:GridView>

         <asp:SqlDataSource ID="SqlDataSource4" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="SELECT [t2].[value3] AS [Total],
       [t2].[value2] AS [Number Registered]
FROM (
       SELECT COUNT(*) AS [value],
              COUNT(*) AS [value2],
              [t1].[value] AS [value3]
       FROM (
              SELECT CONVERT (DATETIME, CONVERT (NCHAR (2), DATEPART(Month,
              [t0].[CreateDate])) +('/' +(CONVERT (NCHAR (2), DATEPART(Day,
              [t0].[CreateDate])) +('/' + CONVERT (NCHAR (4), DATEPART(Year,
              [t0].[CreateDate]))))), 101) AS [value]
              FROM [AttendeesCodeCampYear] AS [t0]
            ) AS [t1]
       GROUP BY [t1].[value]
     ) AS [t2]
WHERE [t2].[value] > 0
ORDER BY [t2].[value3] DESC"></asp:SqlDataSource>




        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="SELECT [t2].[value3] AS [Total], [t2].[value2] AS [Number Registered]
FROM (
    SELECT COUNT(*) AS [value], COUNT(*) AS [value2], [t1].[value] AS [value3]
    FROM (
        SELECT CONVERT(DATETIME, CONVERT(NCHAR(2), DATEPART(Month, [t0].[CreationDate])) + ('/' + (CONVERT(NCHAR(2), DATEPART(Day, [t0].[CreationDate])) + ('/' + CONVERT(NCHAR(4), DATEPART(Year, [t0].[CreationDate]))))), 101) AS [value]
        FROM [Attendees] AS [t0]
        ) AS [t1]
    GROUP BY [t1].[value]
    ) AS [t2]
WHERE [t2].[value] &gt; 0
ORDER BY [t2].[value3] DESC"></asp:SqlDataSource>
        <br />
        <br />
        <asp:GridView ID="GridView2" runat="server" AllowSorting="True" AutoGenerateColumns="False"
            DataSourceID="SqlDataSource2">
            <Columns>
                <asp:BoundField DataField="Number Sessions" HeaderText="Number Sessions" ReadOnly="True"
                    SortExpression="Number Sessions" />
                <asp:BoundField DataField="Total" HeaderText="Total" ReadOnly="True" SortExpression="Total" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
            SelectCommand="&#13;&#10;SELECT convert (varchar, createdate, 101) as 'Number Sessions',&#13;&#10;       count(*) as Total&#13;&#10;FROM Sessions&#13;&#10;Group By convert (varchar, createdate, 101)&#13;&#10;ORDER by convert (varchar, createdate, 101) desc">
        </asp:SqlDataSource>
        <br />
        
        <asp:GridView ID="GridView3" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="SqlDataSource3">
            <Columns>
                <asp:BoundField DataField="Column1" HeaderText="Attendee Domain" ReadOnly="True" SortExpression="Column1" />
                <asp:BoundField DataField="Column2" HeaderText="Count" ReadOnly="True" SortExpression="Column2" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="SELECT &#13;&#10;  SUBSTRING(Email,PATINDEX('%@%',Email)+1,30),Count(*)&#13;&#10;FROM&#13;&#10;  Attendees&#13;&#10;Group By SUBSTRING(Email,PATINDEX('%@%',Email)+1,30)&#13;&#10;Order By SUBSTRING(Email,PATINDEX('%@%',Email)+1,30)"></asp:SqlDataSource>
        
        <br />
        
         <h4>session interest updates over last 30 days</h4>
         <asp:GridView ID="GridView5" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="SqlDataSource5">
            <Columns>
                <asp:BoundField DataField="DayOfMonth" HeaderText="Day of Month" ReadOnly="True" SortExpression="" />
                <asp:BoundField DataField="Count" HeaderText="Count" ReadOnly="True" SortExpression="" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
            SelectCommand="SELECT datepart(day,lastUpdatedDate) as DayOfMonth,  count(*) as Count FROM SessionAttendee where  LastUpdatedDate > getdate() - 30 group by datepart(day,lastUpdatedDate) order by  datepart(day,lastUpdatedDate) desc">
        </asp:SqlDataSource>
        
        <br />

     
        <asp:Button ID="ButtonTemp" runat="server" Text="refresh" 
            onclick="ButtonTemp_Click" />


    </div>
    </form>
</body>
</html>
