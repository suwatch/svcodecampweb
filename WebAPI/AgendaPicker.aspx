<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" Inherits="AgendaPicker"
    Title="Agenda Picker" Codebehind="AgendaPicker.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" runat="Server">
    <asp:HyperLink ID="HyperLinkAgenda" NavigateUrl="~/AgendaUpdate.aspx" runat="server">Back to Agenda</asp:HyperLink>
    <br />
   

    <asp:SqlDataSource ID="SqlDataSourceSessionTimes" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        
        SelectCommand="SELECT [id], [Description] FROM [SessionTimes] WHERE id &lt;&gt; 10 AND CodeCampYearId = @CodeCampYearId ORDER BY [StartTime]">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>

  
    <br />
    <asp:DropDownList ID="DropDownListSessionTimes" runat="server" DataSourceID="SqlDataSourceSessionTimes"
        DataTextField="Description" DataValueField="id" AutoPostBack="True" OnSelectedIndexChanged="DropDownListSessionTimes_SelectedIndexChanged">
    </asp:DropDownList>


    <strong>Pick A Session By Pressing Select next to the Title</strong>
    <br />
    <strong>For the Following Room and Time</strong> (DANGER, EASY TO CLEAR ROOM 
    ASSIGNMENTS, ovewrites rooms you don&#39;t see here)<br />
    Room
    <asp:Label ID="LabelRoom" runat="server" Text="Label"></asp:Label><br />
    <asp:Label ID="LabelTime" runat="server" Text="Label"></asp:Label><br />
    <br />
    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"   
        DataKeyNames="id" DataSourceID="SqlDataSourceSessions" 
        OnRowCommand="GridView1_RowCommand">
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                
                    <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("id") %>' runat="server"
                        CausesValidation="False" CommandName="Select" Text="Select"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True"
                SortExpression="id" Visible="False" />
            <asp:BoundField DataField="title" HeaderText="title" SortExpression="title" />
            <asp:BoundField DataField="description" HeaderText="description" SortExpression="description"
                Visible="False" />
            <asp:TemplateField HeaderText="SessionTimesId" SortExpression="SessionTimesId">
                <ItemTemplate>
                    <asp:Label ID="LabelSessionTime" runat="server" 
                    Text='<%# (string) GetSessiontimesFromDictionary((int) Eval("SessionTimesId")) %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="LectureRoomsId" SortExpression="LectureRoomsId">
                <ItemTemplate>
                    <asp:Label ID="LabelLectureRoom" runat="server" 
                      Text='<%# (string) GetRoomFromDictionary((int) Eval("LectureRoomsId")) %>'    
                    >
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="LabelPresenter" runat="server" Text='<%# (string) GetPresenterFromSessionId((int) Eval("id")) %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="LabelPresenter1" runat="server" Text='<%# (string) GetInterestFromSessionId((int) Eval("id")) %>'>
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSourceSessions" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        
        SelectCommand="SELECT id, title, description, SessionTimesId, LectureRoomsId FROM Sessions WHERE (CodeCampYearId = @CodeCampYearId) ORDER BY title">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <asp:Label ID="LabelCodeCampYearId" runat="server" Text="Label"></asp:Label>
</asp:Content>
