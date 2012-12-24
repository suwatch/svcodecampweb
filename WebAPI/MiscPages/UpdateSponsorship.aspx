<%@ Page Language="C#" AutoEventWireup="true"   MaintainScrollPositionOnPostback="true" ValidateRequest="false" Inherits="MiscPages_UpdateSponsorship" Codebehind="UpdateSponsorship.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       

     
        

    </div>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        CellPadding="10" CellSpacing="10" 
        DataKeyNames="Id,CodeCampYearId,SponsorListId" DataSourceID="ObjectDataSource1" 
        ForeColor="#333333" GridLines="None">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:CommandField ShowEditButton="True" />
            <asp:BoundField DataField="SponsorName" HeaderText="SponsorName"  ReadOnly="true"
                SortExpression="SponsorName" />
         
            <asp:CheckBoxField DataField="TableRequired" HeaderText="TableRequired" 
                SortExpression="TableRequired" />
            
            <asp:CheckBoxField DataField="AttendeeBagItem" HeaderText="AttendeeBagItem" 
                SortExpression="AttendeeBagItem" />
           
            <asp:CheckBoxField DataField="ItemSentToFoothill" 
                HeaderText="ItemSentToFoothill" SortExpression="ItemSentToFoothill" />
            
             <asp:BoundField DataField="ItemsShippingDescription" 
                HeaderText="Items Detail" SortExpression="ItemSentToFoothill" />
            
               <asp:TemplateField HeaderText="NoteFromCodeCamp" ControlStyle-Width="400" FooterStyle-Width="400" HeaderStyle-Width="400" ItemStyle-Width="400">
                   <EditItemTemplate>
                       <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("NoteFromCodeCamp") %>'></asp:TextBox>
                   </EditItemTemplate>
                   <ItemTemplate>
                       <asp:Label ID="Label2" runat="server" Text='<%# Bind("NoteFromCodeCamp") %>'></asp:Label>
                   </ItemTemplate>
            </asp:TemplateField>
            
            
            <asp:TemplateField HeaderText="Comments" SortExpression="Comments" ControlStyle-Width="300" FooterStyle-Width="300" HeaderStyle-Width="300" ItemStyle-Width="300">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" TextMode="MultiLine" Height="200" runat="server" Text='<%# Bind("Comments") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Visible="False" Text='<%# Bind("Comments") %>'></asp:Label>
                </ItemTemplate>

<ControlStyle Width="300px"></ControlStyle>

<FooterStyle Width="300px"></FooterStyle>

<HeaderStyle Width="300px"></HeaderStyle>

<ItemStyle Width="300px"></ItemStyle>
            </asp:TemplateField>
            
               <asp:BoundField DataField="NextActionDate"  
                HeaderText="NextActionDate" SortExpression="NextActionDate" Visible="False"  />
            
            
            <asp:BoundField DataField="Comment" HeaderText="Comment" 
                SortExpression="Comment" Visible="False" />
            <asp:BoundField DataField="CurrentCodeCampYearId" 
                HeaderText="CurrentCodeCampYearId" SortExpression="CurrentCodeCampYearId" 
                Visible="False" />
            <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" 
                SortExpression="Id" Visible="False" />
            <asp:BoundField DataField="CodeCampYearId" HeaderText="CodeCampYearId" 
                SortExpression="CodeCampYearId" Visible="False" />
            <asp:CheckBoxField DataField="Visible" HeaderText="Visible" 
                SortExpression="Visible" Visible="False" />
            <asp:BoundField DataField="SortIndex" HeaderText="SortIndex"  
                SortExpression="SortIndex" Visible="False" />
            <asp:BoundField DataField="SponsorListId" HeaderText="SponsorListId" 
                SortExpression="SponsorListId" Visible="False" />
            <asp:BoundField DataField="WhoOwns" HeaderText="WhoOwns"  Visible="False"
                SortExpression="WhoOwns" />
            <asp:BoundField DataField="Status" HeaderText="Status"  Visible="False"
                SortExpression="Status" />
               <asp:BoundField DataField="DonationAmount" HeaderText="DonationAmount"  Visible="False"
                SortExpression="DonationAmount" />
            
             <asp:CheckBoxField DataField="ItemSentToUPS" HeaderText="ItemSentToUPS"  Visible="False"
                SortExpression="ItemSentToUPS" />


        </Columns>
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        DataObjectTypeName="CodeCampSV.SponsorListCodeCampYearResult" 
        
         SelectMethod="GetByParams" 
        TypeName="CodeCampSV.SponsorListCodeCampYearManager" UpdateMethod="UpdateSponsorSimple1">
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="TextBoxMin" Name="donationAmtMin" 
                PropertyName="Text" Type="Double" />
            <asp:ControlParameter ControlID="TextBoxMax" Name="donationAmtMax" 
                PropertyName="Text" Type="Double" />
            <asp:ControlParameter ControlID="TextBoxCCYar" Name="codeCampYearId" 
                PropertyName="Text" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
        
        
           Min:
        <asp:TextBox ID="TextBoxMin" Text="3900.0" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; max:
        <asp:TextBox ID="TextBoxMax" Text="3900.0" runat="server"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        CCYear:&nbsp;
        <asp:TextBox ID="TextBoxCCYar" Text="6" runat="server" Width="132px"></asp:TextBox>
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         <asp:Button ID="Button1" runat="server" Text="Refresh" /><br />
        <br />
        <br />
        <br />
        

    </form>
</body>
</html>
