<%@ Page Language="C#" AutoEventWireup="true" Inherits="MiscPages_TopSqls" Codebehind="TopSqls.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
            SelectCommandType="StoredProcedure"                
                SelectCommand="Top10Sqls"
            
            
            ></asp:SqlDataSource>


    
    </div>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSource1" 
        onselectedindexchanged="GridView1_SelectedIndexChanged">
        <Columns>
            <asp:BoundField DataField="Column1" HeaderText="SQL" ReadOnly="True" 
                SortExpression="Column1">
            <ControlStyle Width="200px" />
            <FooterStyle Width="200px" />
            <HeaderStyle Width="200px" />
            <ItemStyle Width="200px" />
            </asp:BoundField>
            <asp:BoundField DataField="last_worker_time" HeaderText="last_worker_time" 
                SortExpression="last_worker_time" />
            <asp:BoundField DataField="execution_count" HeaderText="execution_count" 
                SortExpression="execution_count" />
            <asp:BoundField DataField="total_logical_reads" 
                HeaderText="total_logical_reads" SortExpression="total_logical_reads" />
            <asp:BoundField DataField="last_logical_reads" HeaderText="last_logical_reads" 
                SortExpression="last_logical_reads" />
            <asp:BoundField DataField="total_logical_writes" 
                HeaderText="total_logical_writes" SortExpression="total_logical_writes" />
            <asp:BoundField DataField="last_logical_writes" 
                HeaderText="last_logical_writes" SortExpression="last_logical_writes" />
            <asp:BoundField DataField="total_worker_time" HeaderText="total_worker_time" 
                SortExpression="total_worker_time" />
            <asp:BoundField DataField="total_elapsed_time_in_S" 
                HeaderText="total_elapsed_time_in_S" ReadOnly="True" 
                SortExpression="total_elapsed_time_in_S" />
            <asp:BoundField DataField="last_elapsed_time_in_S" 
                HeaderText="last_elapsed_time_in_S" ReadOnly="True" 
                SortExpression="last_elapsed_time_in_S" />
            <asp:BoundField DataField="last_execution_time" 
                HeaderText="last_execution_time" SortExpression="last_execution_time" />
            <asp:BoundField DataField="query_plan" HeaderText="query_plan" 
                SortExpression="query_plan" />
        </Columns>
    </asp:GridView>
    </form>
</body>
</html>
