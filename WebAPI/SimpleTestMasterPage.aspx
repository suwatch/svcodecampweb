<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageTest.master" %>

<script runat="server">

</script>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">



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
        

</asp:Content>

