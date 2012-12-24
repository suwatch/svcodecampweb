<%@ Page Language="C#" AutoEventWireup="true" Inherits="SponsorListEmail" Codebehind="SponsorListEmail.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <br/>
        Dollar Start Amount:  
        <asp:TextBox ID="TextBoxDollarStart" runat="server"></asp:TextBox>
        <br/>
         Dollar Start Amount:  
        <asp:TextBox ID="TextBoxDollarEndAmount" runat="server"></asp:TextBox>

          <br/>
         Code Camp Year Id:  
        <asp:TextBox ID="TextBoxCodeCampYearId" runat="server"></asp:TextBox>




     <br /><br />
        <asp:CheckBox ID="CheckBoxCurrentYear" runat="server" Checked="True" 
            Text="Checked Means This Year, UnChecked Means Other Years and Not This Year" />
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="Refresh" 
            onclick="Button1_Click" />
        
        <br />
        <br />
        <asp:Button ID="ButtonCopySelected" runat="server" 
            onclick="ButtonCopySelected_Click" Text="Copy Selected Into TextBox" />
        <br />
        <br />



           <br />
         EXCLUDE CHECKED NAMES (BACKWARDS, I KNOW)
        <br />
        <asp:CheckBoxList ID="CheckBoxList1" runat="server" 
            DataSourceID="SqlDataSource1" DataTextField="INFO" 
            DataValueField="EmailAddress" >
        </asp:CheckBoxList>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
            SelectCommand="SELECT dbo.SponsorListContact.EmailAddress,  CAST(dbo.SponsorList.SponsorName as varchar(10)) + ':' + CAST(dbo.SponsorListContact.EmailAddress as varchar(40)) as INFO
                                FROM dbo.SponsorList
                                     INNER JOIN dbo.SponsorListCodeCampYear ON (dbo.SponsorList.Id =
                                     dbo.SponsorListCodeCampYear.SponsorListId)
                                     INNER JOIN dbo.SponsorListContact ON (dbo.SponsorList.Id =
                                     dbo.SponsorListContact.SponsorListId)
                                WHERE dbo.SponsorListCodeCampYear.CodeCampYearId = @CodeCampYearId AND
                                      dbo.SponsorListCodeCampYear.DonationAmount  &gt;= @AmountStart AND
                                 dbo.SponsorListCodeCampYear.DonationAmount  &lt;= @AmountStop
                                ORDER BY dbo.SponsorList.SponsorName
                                ">
            <SelectParameters>
                <asp:ControlParameter ControlID="TextBoxCodeCampYearId" Name="CodeCampYearId" 
                    PropertyName="Text" />
                <asp:ControlParameter ControlID="TextBoxDollarStart" Name="AmountStart" 
                    PropertyName="Text" />
                <asp:ControlParameter ControlID="TextBoxDollarEndAmount" Name="AmountStop" 
                    PropertyName="Text" />
            </SelectParameters>
        </asp:SqlDataSource>


    
        <br />
        <asp:TextBox ID="TextBox1" runat="server" Height="305px" TextMode="MultiLine" 
            Width="949px"></asp:TextBox>
        <br />
        <br />


    
    </div>
    </form>
</body>
</html>
