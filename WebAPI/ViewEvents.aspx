<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true" Inherits="ViewEvents" Codebehind="ViewEvents.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="blankSublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" Runat="Server">
    
    
    <asp:Button ID="ButtonClearLog" runat="server" Text="Clear Log" OnClick="ButtonClearLog_Click" />

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
        SelectCommand="SELECT [Date], [Message], [ExceptionMessage],[ExceptionStackTrace], [UserName], [MessageLine2] FROM [Log4NetAll] ORDER BY [Id] DESC">
    </asp:SqlDataSource>
    
       <asp:GridView ID="GridView2" runat="server" 
        AllowSorting="True" AutoGenerateColumns="False" DataSourceID="SqlDataSource2">
        <Columns>
            <asp:BoundField DataField="Count" HeaderText="Count"  />
            <asp:BoundField DataField="ExceptionMessage" HeaderText="ExceptionMessage" 
                SortExpression="ExceptionMessage" />
           
        </Columns>
    </asp:GridView>
    
     <br/>
    <br/>

    

    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" PageSize="40"
        AllowSorting="True" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
        <Columns>
            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
            <asp:BoundField DataField="UserName" HeaderText="UserName" 
                SortExpression="UserName" />
            <asp:BoundField DataField="ExceptionMessage" HeaderText="ExceptionMessage" 
                SortExpression="ExceptionMessage" />
             <asp:BoundField DataField="ExceptionStackTrace" HeaderText="ExceptionStackTrace" 
                />
            <asp:BoundField DataField="Message" HeaderText="Message" 
                SortExpression="Message" />
        </Columns>
    </asp:GridView>
    
   
      <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
        SelectCommand="select top 10 count(*) as Count,ExceptionMessage from Log4NetAll group by ExceptionMessage order by count(*) desc">
    </asp:SqlDataSource>
    
   
    

</asp:Content>

