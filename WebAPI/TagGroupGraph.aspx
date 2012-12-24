<%@ Page Title="" Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true" Inherits="TagGroupGraph" Codebehind="TagGroupGraph.aspx.cs" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<div class="mainHeading">Percentage of Interested Registered Users</div>
   <asp:Label ID="LabelAttendeesId" Visible="false" runat="server" Text="Label"></asp:Label>
   <asp:Label ID="LabelCodeCampYearId" Visible="false" runat="server" Text="Label"></asp:Label>

    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
    <br/>
<p>
Values shown are the percentage of interested registered users that are members of each Tag List.  The total population of interested registered users in 2010 is 2,370.  Of all the registered users in 2010, interested registered users either selected Interested and/or Will Attend for one or more sessions.
</p>
<br/>
<p>
A value of 62 is 62% of 2,370 interested registered users that expressed Interested and/or Will Attend for one or more of the sessions in that Tag List.
</p>


<br />
<br />
    <asp:Label ID="LabelNotAvailable" runat="server" Visible="False" 
        Text="This Feature is available for Platinum Sponsors. If you feel you should not be getting this error, please email info@siliconvalley-codecamp.com" 
        Font-Bold="True" Font-Size="Larger"></asp:Label>
<asp:Chart ID="Chart1" runat="server" DataSourceID="ObjectDataSource1" 
        Width="750px">
    <series>
        <asp:Series ChartType="StackedBar" Name="Series1" 
            XValueMember="TagGroupName" YValueMembers="PercentInterest" 
            IsValueShownAsLabel="True">
        </asp:Series>
    </series>
    <chartareas>
        <asp:ChartArea Name="ChartArea1">
        </asp:ChartArea>
    </chartareas>
</asp:Chart>
  <br/>
  <br/>
 <asp:HyperLink ID="HyperLinkTagGroupSetup" NavigateUrl="TagList.aspx" runat="server">Setup Tag Lists</asp:HyperLink>
 <br/>

<br />

<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
    SelectMethod="GetAll" TypeName="CodeCampSV.ResultSessionInterestTagsClass">
    <SelectParameters>
        <asp:ControlParameter ControlID="LabelCodeCampYearId" DefaultValue="5" 
            Name="codeCampYearId" PropertyName="Text" Type="Int32" />
        <asp:ControlParameter ControlID="LabelAttendeesId" DefaultValue="" 
            Name="attendeesId" PropertyName="Text" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
<br />

</asp:Content>

