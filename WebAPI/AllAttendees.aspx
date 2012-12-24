<%@ Page Language="C#" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" Inherits="AdminOnly_AllAttendees" Codebehind="AllAttendees.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>All Attendees</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
        DeleteCommand="DELETE FROM [Attendees] WHERE [PKID] = @PKID" 
        InsertCommand="INSERT INTO [Attendees] ([Username], [Email], [IsApproved], [IsLockedOut], [UserLocation], [UserFirstName], [UserLastName], [UserZipCode], [UserShareInfo], [VistaSlotsId], [VistaOnly], [SaturdayClasses], [SundayClasses], [DesktopOrLaptopForVista], [PKID]) VALUES (@Username, @Email, @IsApproved, @IsLockedOut, @UserLocation, @UserFirstName, @UserLastName, @UserZipCode, @UserShareInfo, @VistaSlotsId, @VistaOnly, @SaturdayClasses, @SundayClasses, @DesktopOrLaptopForVista, @PKID)" 
        SelectCommand="SELECT [Username], [Email], [IsApproved], [IsLockedOut], [UserLocation], [UserFirstName], [UserLastName], [UserZipCode], [UserShareInfo], [VistaSlotsId], [VistaOnly], [SaturdayClasses], [SundayClasses], [DesktopOrLaptopForVista], [PKID], [CreationDate] FROM [Attendees] ORDER BY [UserLastName], [UserFirstName]" 
        UpdateCommand="UPDATE [Attendees] SET [Username] = @Username, [Email] = @Email, [IsApproved] = @IsApproved, [IsLockedOut] = @IsLockedOut, [UserLocation] = @UserLocation, [UserFirstName] = @UserFirstName, [UserLastName] = @UserLastName, [UserZipCode] = @UserZipCode, [UserShareInfo] = @UserShareInfo, [VistaSlotsId] = @VistaSlotsId, [VistaOnly] = @VistaOnly, [SaturdayClasses] = @SaturdayClasses, [SundayClasses] = @SundayClasses, [DesktopOrLaptopForVista] = @DesktopOrLaptopForVista WHERE [PKID] = @PKID">
        <DeleteParameters>
            <asp:Parameter Name="PKID" Type="Object" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Username" Type="String" />
            <asp:Parameter Name="Email" Type="String" />
            <asp:Parameter Name="IsApproved" Type="Boolean" />
            <asp:Parameter Name="IsLockedOut" Type="Boolean" />
            <asp:Parameter Name="UserLocation" Type="String" />
            <asp:Parameter Name="UserFirstName" Type="String" />
            <asp:Parameter Name="UserLastName" Type="String" />
            <asp:Parameter Name="UserZipCode" Type="String" />
            <asp:Parameter Name="UserShareInfo" Type="Boolean" />
            <asp:Parameter Name="VistaSlotsId" Type="Int32" />
            <asp:Parameter Name="VistaOnly" Type="Boolean" />
            <asp:Parameter Name="SaturdayClasses" Type="Boolean" />
            <asp:Parameter Name="SundayClasses" Type="Boolean" />
            <asp:Parameter Name="DesktopOrLaptopForVista" Type="String" />
            <asp:Parameter Name="PKID" Type="Object" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="Username" Type="String" />
            <asp:Parameter Name="Email" Type="String" />
            <asp:Parameter Name="IsApproved" Type="Boolean" />
            <asp:Parameter Name="IsLockedOut" Type="Boolean" />
            <asp:Parameter Name="UserLocation" Type="String" />
            <asp:Parameter Name="UserFirstName" Type="String" />
            <asp:Parameter Name="UserLastName" Type="String" />
            <asp:Parameter Name="UserZipCode" Type="String" />
            <asp:Parameter Name="UserShareInfo" Type="Boolean" />
            <asp:Parameter Name="VistaSlotsId" Type="Int32" />
            <asp:Parameter Name="VistaOnly" Type="Boolean" />
            <asp:Parameter Name="SaturdayClasses" Type="Boolean" />
            <asp:Parameter Name="SundayClasses" Type="Boolean" />
            <asp:Parameter Name="DesktopOrLaptopForVista" Type="String" />
            <asp:Parameter Name="PKID" Type="Object" />
        </InsertParameters>
    </asp:SqlDataSource>
        <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
            DataKeyNames="PKID" DataSourceID="SqlDataSource1">
            <Columns>
                <asp:CommandField ShowEditButton="True" />
                <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
                <asp:BoundField DataField="CreationDate" HtmlEncode="false" DataFormatString="{0:M-dd-yyyy}" HeaderText="CreationDate" SortExpression="CreationDate" />
                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                <asp:CheckBoxField DataField="IsApproved" HeaderText="IsApproved" SortExpression="IsApproved" />
                <asp:CheckBoxField DataField="IsLockedOut" HeaderText="IsLockedOut" SortExpression="IsLockedOut" />
                <asp:BoundField DataField="UserLocation" HeaderText="UserLocation" SortExpression="UserLocation" />
                <asp:BoundField DataField="UserFirstName" HeaderText="UserFirstName" SortExpression="UserFirstName" />
                <asp:BoundField DataField="UserLastName" HeaderText="UserLastName" SortExpression="UserLastName" />
                <asp:BoundField DataField="UserZipCode" HeaderText="UserZipCode" SortExpression="UserZipCode" />
                <asp:CheckBoxField DataField="UserShareInfo" HeaderText="UserShareInfo" SortExpression="UserShareInfo" />
                <asp:BoundField DataField="VistaSlotsId" HeaderText="VistaSlotsId" SortExpression="VistaSlotsId" />
                <asp:CheckBoxField DataField="VistaOnly" HeaderText="VistaOnly" SortExpression="VistaOnly" />
                <asp:CheckBoxField DataField="SaturdayClasses" HeaderText="SaturdayClasses" SortExpression="SaturdayClasses" />
                <asp:CheckBoxField DataField="SundayClasses" HeaderText="SundayClasses" SortExpression="SundayClasses" />
                <asp:BoundField DataField="DesktopOrLaptopForVista" HeaderText="DesktopOrLaptopForVista"
                    SortExpression="DesktopOrLaptopForVista" />
                <asp:BoundField DataField="PKID" Visible="false" HeaderText="PKID" ReadOnly="True" SortExpression="PKID" />
            </Columns>
        </asp:GridView>

    </div>
    </form>
</body>
</html>
