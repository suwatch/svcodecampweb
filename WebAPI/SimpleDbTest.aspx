<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            DataKeyNames="Id" DataSourceID="SqlDataSource1">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" 
                    SortExpression="Id" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="CodeCampDateString" HeaderText="CodeCampDateString" 
                    SortExpression="CodeCampDateString" />
                <asp:BoundField DataField="CampStartDate" HeaderText="CampStartDate" 
                    SortExpression="CampStartDate" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
            SelectCommand="SELECT [Id], [Name], [CodeCampDateString], [CampStartDate] FROM [CodeCampYear] ORDER BY [CampStartDate]"></asp:SqlDataSource>
        
    
    
    </div>
    </form>
</body>
</html>
