<%@ Page Language="C#" MasterPageFile="~/RightNote.master" AutoEventWireup="true" EnableEventValidation="false" ValidateRequest="false" Inherits="Tracks" Title="Silicon Valley Code Camp Tracks" Codebehind="Tracks.aspx.cs" %>

<asp:Content ID="SublinksSpeakers" ContentPlaceHolderID="Sublinks" runat="server">
    <asp:Menu ID="subMenu" runat="server" DataSourceID="SiteMapProgram" SkinID="subMenu">
    </asp:Menu>
    <%--The next line should be here, but seems to work without because it is in master page--%>
    <asp:SiteMapDataSource ID="SiteMapProgram" runat="server" ShowStartingNode="False"
        StartingNodeUrl="~/Program.aspx" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="mainHeading">
        Tracks</div>
    <div class="pad">
        <p>
            Silicon Valley Code Camp has chosen a few subject areas that have been organized
            into tracks. The tracks are designed and scheduled so that if you have a specific
            interest in the track theme you can attend all the sessions associated with that
            track in an order that makes sense with minimal overlap. This does not mean that there
            are not other sessions in the same theme as the track, but those other sessions
            may overlap. It's just the nature of a big open event like this. 
        </p>
        <hr />
    </div>
    
    <p><asp:Label ID="LabelMessage" runat="server" Text=""></asp:Label></p>

    <p><asp:Label ID="LabelInstructionsForNewYear" Visible="false" runat="server" Text="">TO START YEAR WITH NEW TRACK, GO TO PREVIOUS YEAR AND INSERT NEW WITH CODECAMPYEARID SET</asp:Label></p>
    
    
    <!-- Begin container for speaker listings -->
    <div class="speakersContainer">
        <asp:Repeater ID="RepeaterTracks" runat="server" EnableViewState="true" DataSourceID="SqlDataSourceTracks">
            <HeaderTemplate>
                <div id="three-column-containerw">
            </HeaderTemplate>
            <ItemTemplate>
                <div class="pad">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td valign="top">
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <div class="subHeading">
                                               <%-- <asp:Label ID="Label2" runat="server" EnableViewState="false" Font-Bold="true" Text='<%# Eval("Named") %>'></asp:Label>--%>
                                            
                                           <%-- <asp:HyperLink ID="HyperLinkTrack" Text='<%# Eval("Named") %>' NavigateUrl='<%# "~/Sessions.aspx?track=" + Eval("Id") %>'
                                                     runat="server"></asp:HyperLink>--%>
                                                     
                                           <asp:HyperLink ID="HyperLinkTrack" Text='<%# Eval("Named") %>' NavigateUrl='<%# GetNavigateURL((int)Eval("Id")) %>'
                                                     runat="server"></asp:HyperLink>
                                            </div>
                                        </td>
                                        <td valign="middle">
                                            <div class="slashBullet">
                                                <%--<asp:HyperLink ID="HyperLinkSessions" NavigateUrl='<%# "~/Sessions.aspx?track=" + Eval("Id") %>'
                                                    Text="Sessions" runat="server"></asp:HyperLink>--%></div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="speakerDescription">
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("Description") %>' EnableViewState="false"></asp:Label>
                                </div>
                                <div class="slashBullet">
                                    <%--<asp:HyperLink ID="HyperLink3" NavigateUrl='<%# GetUserWebSite((object) Eval("UserWebSite")) %>'
                                        EnableViewState="false" Text='<%# GetUserWebSite((object) Eval("UserWebSite")) %>'
                                        runat="server"></asp:HyperLink>--%>
                                </div>
                            </td>
                            <td valign="middle" align="right">
                                <%--<asp:Image EnableViewState="false" runat="server" ID="Image1" ImageUrl='<%# "~/DisplayImage.ashx?sizex=100&PKID=" + Eval("PKID")  %>'
                                    ImageAlign="middle" BorderStyle="None" CssClass="speakerImage" />--%>
                            </td>
                        </tr>
                    </table>
                </div>
            </ItemTemplate>
            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <asp:SqlDataSource ID="SqlDataSourceTracks" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        SelectCommand="
                    SELECT [Id],
                   [Named],
                   [Description],
                   [OwnerAttendeeId],
                   [CodeCampYearId]
            FROM [Track]
            WHERE (ID IN (SELECT DISTINCT TrackSession.TrackId FROM Track INNER JOIN
             TrackSession ON (Track.Id = TrackSession.TrackId) WHERE visible=1 AND Track.CodeCampYearId =
              @CodeCampYearId) )
            ORDER BY [Named]
        ">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" PropertyName="Text"
                Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:Label ID="LabelCodeCampYearId" runat="server" Text="" Visible="false"></asp:Label>
    
    <div runat="server" id="Trackeditid" visible="false" >
    
    <b>TRACK DESCRIPTION WILL NOT APPEAR ABOVE UNTIL A TRACK IS ASSIGNED TO IT!!!</b>
    
        <br />
        <hr />
        <br />
        
        <asp:DropDownList ID="DropDownListTracks" runat="server" AutoPostBack="True" 
            DataSourceID="SqlDataSourceTrackForDropDown" DataTextField="Named" DataValueField="Id">
        </asp:DropDownList>
    
        <br />
    
        <asp:DetailsView ID="DetailsViewTrack" runat="server" Height="100px" 
            Width="700px" AutoGenerateRows="False" DataKeyNames="Id"  
            DataSourceID="SqlDataSourceTrack"  CellPadding="3" BackColor="White" 
            BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" 
            GridLines="Vertical" onitemcommand="DetailsViewTrack_ItemCommand" 
            onitemcreated="DetailsViewTrack_ItemCreated" 
            onitemdeleted="DetailsViewTrack_ItemDeleted" 
            oniteminserted="DetailsViewTrack_ItemInserted" 
            onitemupdated="DetailsViewTrack_ItemUpdated" AllowPaging="True" 
            oniteminserting="DetailsViewTrack_ItemInserting"      >
            <FooterStyle BackColor="#CCCCCC" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <Fields>
            
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                    ShowInsertButton="True" />
               
            
             <%-- <asp:TemplateField ShowHeader="False">
                     <ItemTemplate>
                         <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" Visible='<%# GetShowEditButton() %>'
                             CommandName="Edit" Text="Edit"></asp:LinkButton>
                         &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" Visible='<%# GetShowInsertButton() %>'
                             CommandName="New" Text="New"></asp:LinkButton>
                         &nbsp;<asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" 
                             CommandName="Delete" Text="Delete" Visible='<%# GetShowDeleteButton() %>'     ></asp:LinkButton>
                     </ItemTemplate>
                </asp:TemplateField>--%>
            
               <%-- <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" 
                    ReadOnly="True" SortExpression="Id" />--%>

                      <asp:BoundField DataField="Id" HeaderText="Id   " 
                    SortExpression="Id" />

                <asp:TemplateField HeaderText="Track Owner   " 
                    SortExpression="OwnerAttendeeId">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# GetUsernameFromAttendeeId( (int) Eval("OwnerAttendeeId")) %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("OwnerAttendeeId") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("OwnerAttendeeId") %>'></asp:TextBox>
                    </InsertItemTemplate>
                </asp:TemplateField>
                    
                 
                    
                 <asp:CheckBoxField DataField="Visible" HeaderText="Show On Web   " 
                    SortExpression="Visible" />
                 <asp:BoundField DataField="Named" HeaderText="Named   " 
                    SortExpression="Named" />



                    
                <asp:BoundField DataField="AlternateURL" HeaderText="AlternateURL   " 
                    SortExpression="AlternateURL" />   
                <asp:TemplateField HeaderText="Description   " SortExpression="Description">
                    <ItemTemplate>
                          <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine"  Width="500" Height="400" Enabled="false"   Text='<%# Bind("Description") %>'></asp:TextBox>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine"  Width="500" Height="400"    Text='<%# Bind("Description") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:TextBox ID="TextBox1"  TextMode="MultiLine" runat="server" Width="500" Height="400" Text='<%# Bind("Description") %>'></asp:TextBox>
                    </InsertItemTemplate>
                </asp:TemplateField>
              
                <asp:TemplateField HeaderText="CodeCampYearId" SortExpression="CodeCampYearId">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("CodeCampYearId") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("CodeCampYearId") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("CodeCampYearId") %>'></asp:TextBox>
                    </InsertItemTemplate>
                </asp:TemplateField>
              
            </Fields>
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="#CCCCCC" />
        </asp:DetailsView>
        
        <asp:SqlDataSource ID="SqlDataSourceTrack" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
            DeleteCommand="DELETE FROM [Track] WHERE [Id] = @Id" 
            InsertCommand="INSERT INTO [Track] ([OwnerAttendeeId], [Named], [Description],[AlternateURL], [Visible], [CodeCampYearId]) VALUES (@OwnerAttendeeId, @Named, @Description, @AlternateURL,@Visible, @CodeCampYearId)" 
            SelectCommand="SELECT [Id], [OwnerAttendeeId], [Named], [Description],[AlternateURL], [Visible], [CodeCampYearId] FROM [Track] WHERE (([CodeCampYearId] = @CodeCampYearId) AND ([Id] = @Id)) ORDER BY [Named]"             
            UpdateCommand="UPDATE [Track] SET [OwnerAttendeeId] = @OwnerAttendeeId, [Named] = @Named,[AlternateURL] = @AlternateURL, [Description] = @Description, [Visible] = @Visible, [CodeCampYearId] = @CodeCampYearId WHERE [Id] = @Id">
            <SelectParameters>
                <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                    PropertyName="Text" Type="Int32" />
                <asp:ControlParameter ControlID="DropDownListTracks" Name="Id" 
                    PropertyName="SelectedValue" Type="Int32" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="Id" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="OwnerAttendeeId" Type="Int32" />
                <asp:Parameter Name="Named" Type="String" />
                <asp:Parameter Name="AlternateURL" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:Parameter Name="Visible" Type="Boolean" />
                <asp:Parameter Name="CodeCampYearId" Type="Int32" />
                <asp:Parameter Name="Id" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="OwnerAttendeeId" Type="Int32" />
                <asp:Parameter Name="Named" Type="String" />
                <asp:Parameter Name="AlternateURL" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:Parameter Name="Visible" Type="Boolean" />
                <asp:Parameter Name="CodeCampYearId" Type="Int32" />
            </InsertParameters>
        </asp:SqlDataSource>
        
          <asp:SqlDataSource ID="SqlDataSourceTrackForDropDown" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
            DeleteCommand="DELETE FROM [Track] WHERE [Id] = @Id" 
            InsertCommand="INSERT INTO [Track] ([OwnerAttendeeId], [Named], [Description],[AlternateURL], [Visible]) VALUES (@OwnerAttendeeId, @Named, @Description, @Visible)" 
            SelectCommand="SELECT [Id], [OwnerAttendeeId], [Named], [Description],[AlternateURL], [Visible] FROM [Track] WHERE (([CodeCampYearId] = @CodeCampYearId))" 
            
            
            UpdateCommand="UPDATE [Track] SET [OwnerAttendeeId] = @OwnerAttendeeId, [Named] = @Named, [Description] = @Description, [Visible] = @Visible WHERE [Id] = @Id">
            <SelectParameters>
                <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                    PropertyName="Text" Type="Int32" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="Id" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="OwnerAttendeeId" Type="Int32" />
                <asp:Parameter Name="Named" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:Parameter Name="Visible" Type="Boolean" />
                <asp:Parameter Name="Id" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="OwnerAttendeeId" Type="Int32" />
                <asp:Parameter Name="Named" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:Parameter Name="Visible" Type="Boolean" />
            </InsertParameters>
        </asp:SqlDataSource>
    
    
    
    </div>
    
    
    
</asp:Content>
