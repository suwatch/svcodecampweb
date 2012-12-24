<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true" Inherits="TrackViewer" Codebehind="TrackViewer.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="blankSublinks" Runat="Server">
</asp:Content> 
<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" Runat="Server">

<br />
<br />


    &nbsp;&nbsp;


    <asp:DropDownList ID="DropDownListTracks" runat="server" 
        DataSourceID="SqlDataSourceTracks" DataTextField="Named" 
        DataValueField="Id" AutoPostBack="True" 
        >
    </asp:DropDownList>
    <asp:SqlDataSource ID="SqlDataSourceTracks" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
        SelectCommand="SELECT [Named], [Id] FROM [Track] WHERE ([CodeCampYearId] = @CodeCampYearId) ORDER BY [Named]">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                PropertyName="Text" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>

    <br />
<br />



    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="SELECT 
  dbo.SessionsOverview.Title,
  dbo.SessionsOverview.Number,
  dbo.SessionsOverview.UserFirstName,
  dbo.SessionsOverview.UserLastName,
  dbo.SessionsOverview.StartTime
FROM
  dbo.Track
  INNER JOIN dbo.TrackSession ON (dbo.Track.Id = dbo.TrackSession.TrackId)
  INNER JOIN dbo.SessionsOverview ON (dbo.TrackSession.SessionId = dbo.SessionsOverview.SessionId)
  INNER JOIN dbo.SessionTimes ON (dbo.SessionsOverview.SessionTimesId = dbo.SessionTimes.Id)
WHERE
  dbo.SessionsOverview.CodeCampYearId = @CodeCampYearId AND dbo.Track.Id = @TrackId
ORDER BY
  dbo.SessionsOverview.StartTime">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                PropertyName="Text" />
            <asp:ControlParameter ControlID="DropDownListTracks" Name="TrackId" 
                PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1">

    <ItemTemplate>
    <li>

    <%# Eval("StartTime","{0:t}") %>  &nbsp;&nbsp; <u><%# Eval("Title") %></u> &nbsp; &nbsp;&nbsp; <%# Eval("UserFirstName") %>  <%# Eval("UserLastName") %>  &nbsp;&nbsp;<i>Room: <%# Eval("Number") %></i>


    </li>
    </ItemTemplate>



    <HeaderTemplate><ul></HeaderTemplate>
    <FooterTemplate></ul></FooterTemplate>


    </asp:Repeater>
 

  <asp:Label ID="LabelCodeCampYearId" Visible="false" runat="server" Text=""></asp:Label>
</asp:Content>

