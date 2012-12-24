<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true" Inherits="MiscPages_SponsorImagesToSql" Codebehind="SponsorImagesToSql.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="blankSublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" Runat="Server">
    <br/>
    <asp:Button ID="Button1" runat="server" Text="Convert All Sponsor Images To Sql" OnClick="Button1_Click" />
    <br/>
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" DataSourceID="ObjectDataSource1">
        <Columns>
            <asp:BoundField DataField="SponsorName" HeaderText="SponsorName" SortExpression="SponsorName" />
            <asp:BoundField DataField="ImageURL" HeaderText="ImageURL" SortExpression="ImageURL" />
            <asp:BoundField DataField="NavigateURL" HeaderText="NavigateURL" SortExpression="NavigateURL" />
            

            <asp:TemplateField HeaderText="Id" SortExpression="Id">
               
                <ItemTemplate>
                    <asp:Image runat="server" ImageUrl='<%# GetImageUrl((int) Eval("Id")) %>'   />
                   
                </ItemTemplate>

            </asp:TemplateField>
            
            
            

          
        </Columns>
    </asp:GridView>
    
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DataObjectTypeName="CodeCampSV.SponsorListResult" DeleteMethod="Delete" InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" SelectMethod="GetAll" TypeName="CodeCampSV.SponsorListManager">
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
    </asp:ObjectDataSource>
    

</asp:Content>

