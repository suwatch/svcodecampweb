<%@ Page Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true"
    ValidateRequest="false" MaintainScrollPositionOnPostback="true"
    Inherits="SessionsEdit" Title="Session Edit" Codebehind="SessionsEdit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="blankContent" runat="Server">
    <tr>
        <td>
            <br />
            <asp:Button ID="ButtonUpdate1" runat="server" Text="Update Session Information" OnClick="ButtonUpdate1_Click" />
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            <asp:HyperLink ID="HyperLinkReturn" runat="server" NavigateUrl="~/Sessions.aspx">Return To Sessions Listing</asp:HyperLink><br />
            <br />
            <div class="session">
                <table cellspacing="15" style="width: 1000px">
                    <tr>
                        <td>
                            Session Title
                        </td>
                        <td>
                            <div class="title">
                                <asp:TextBox ID="TextBoxTitle" Width="400" runat="server"></asp:TextBox><br/>
                                &nbsp;&nbsp;&nbsp; (No HTML please)&nbsp;&nbsp;&nbsp;&nbsp;
                            </div>
                        </td>
                    </tr>
                    
                    <tr>
                          <td>
                              <asp:Label ID="LabelMaterialUrl"  Visible="false"   runat="server">URL Pointing To Slides and Demo Code</asp:Label>
                            
                        </td>
                    <td>
                            <div class="title">
                                <asp:TextBox ID="TextBoxMaterialUrl"   Visible="false"    Width="400" runat="server"></asp:TextBox>
                               
                            </div>
                        </td>
                        

                    </tr>
                         <td>
                            Slides and Code Upload Email Address (Attach Your Slides and code to this email and they will be automatically posted to your session)
                        </td>
                        <td>
                             <a runat="server" ID="UploadToBoxMailToHrefId" 
                                 href="mailto:JobAds@SiliconValley-CodeCamp.com?subject=I'm Interested In Placing a Job Ad on Silicon Valley Code Camp Site">
                                            Email Me Job Ad Pricing</a> &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="ButtonCreateMailInId" runat="server" Text="Create My Unique Email Address For Uploading Slides and Codes" OnClick="ButtonCreateMailInId_Click" />
                            <asp:Button ID="ButtonDeleteSlidesFolderId" runat="server" Text="Clear All My Slides and Code (no recovery, must create again)" OnClick="ButtonDeleteSlidesFolderId_Click" />


                        </td>
                    
                    
                     <tr>
                        <td>
                            Session Hash Tags for Twitter (ex:   #sqlserver #vsstudio)  (no ,:; please, just spaces and #'s)
                        </td>
                        <td>
                            <div class="title">
                                <asp:TextBox ID="TextBoxHashTags" Width="400" runat="server"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </div>
                        </td>
                    </tr>
                    
                    
                    <tr runat="server" id="SpeakerHandle1Tr">
                        <td>
                           Twitter Handle For Speaker 1: <asp:Label runat="server" ID="SpeakerName1"></asp:Label>  (ex: @MrDatabase) 
                             <asp:Label runat="server" ID="SpeakerHandle1Id" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <div class="title">
                                <asp:TextBox ID="TextBoxSpeakerHandle1" Width="400" runat="server"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    
                     <tr runat="server" id="SpeakerHandle2Tr">
                        <td>
                           Twitter Handle For Speaker 2: <asp:Label runat="server" ID="SpeakerName2"></asp:Label>  (ex: @MrCloud)  
                            <asp:Label runat="server" ID="SpeakerHandle2Id" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <div class="title">
                                <asp:TextBox ID="TextBoxSpeakerHandle2" Width="400" runat="server"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    
                     <tr runat="server" id="SpeakerHandle3Tr">
                        <td>
                           Twitter Handle For Speaker 3:<asp:Label runat="server" ID="SpeakerName3"></asp:Label>  (ex: @MsMVC)
                             <asp:Label runat="server" ID="SpeakerHandle3Id" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <div class="title">
                                <asp:TextBox ID="TextBoxSpeakerHandle3" Width="400" runat="server"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    
                    <%-- <tr>
                        <td>
                            URL Pointing To Slides and Demo Code
                        </td>
                        <td>
                            <div class="title">
                                <asp:TextBox ID="TextBoxSlides" Width="400" runat="server"></asp:TextBox><br/>
                                &nbsp;&nbsp;&nbsp; (ex: http://www.myslides.com/myslides.html)&nbsp;&nbsp;&nbsp;&nbsp;
                            </div>
                        </td>
                    </tr>--%>
                     <tr>
                        <td>
                            
                        </td>
                        <td>
                            <asp:CheckBox ID="CheckBoxQueueEmailNotification" Visible="false" Text="  Queue Email Notification"  runat="server" />
                        </td>
                    </tr>
                     <tr>
                        <td colspan="2">
                            <asp:Label ID="LabelEmailNotification" Visible="false" runat="server" Text="xx"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Session Level
                        </td>
                        <td>
                            <div class="level">
                                <asp:DropDownList ID="DropDownListLevels" runat="server" DataSourceID="SqlDataSourceLevel"
                                    DataTextField="description" DataValueField="id">
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Session Presenter
                        </td>
                        <td>
                            <div>
                                Primary:&nbsp;<asp:Label ID="LabelPresenter" runat="server" Text="Presenter"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Secondary:
                                <asp:Label ID="LabelPresenterAdditional" runat="server"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Additional Speaker Username
                        </td>
                        <td>
                            <div>
                                <asp:TextBox ID="TextBoxAdditionalSpeakerLoginName" runat="server" Width="229px"></asp:TextBox>
                                &nbsp;
                                <asp:CheckBox ID="CheckBoxRemoveAttendee" runat="server" Text="Remove Speaker" />
                            </div>
                        </td>
                    </tr>
                </table>
                <div runat="server" id="DivRemovePrimarySpeakerFromSessionDisplay">
                <br />
                    <table border="1">
                        <tr>
                            <td>
                                <p>
                                    Do Not Show Primary Speaker On Session. Must Have Role: admin or RemovePrimarySpeaker
                                    to see this</p>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBoxDoNotShowPrimarySpeaker" runat="server" />
                            </td>
                        </tr>
                    </table>
                      <br />
                </div>

                 <div runat="server" id="DivAssignedSponsor">
                <br />
                    <table border="1">
                        <tr>
                            <td>
                             <p>  Choose Sponsor to associate with Session (for job ads if running)</p>
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownListSponsors" AppendDataBoundItems="True"  DataTextField="SponsorName"
                                    DataValueField="Id"
                                    runat="server" DataSourceID="SqlDataSourceSponsors">
                                   <asp:ListItem Value="0">--No Sponsor Selected</asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSourceSponsors" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" 
                                    SelectCommand="SELECT [Id], [SponsorName] FROM [SponsorList] ORDER BY [SponsorName]">
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                    </table>
                      <br />
                </div>


                <%--        <div class="ClassRoom">
            Class Room
        </div>
       <<div style="float: right;">
            <asp:Image runat="server" Visible="false" ID="ImageUser" ImageUrl="~/DisplayImage.ashx?roomid=19" />
        </div>--%>
                <div>
                    <br />
                    <br />
                   <%-- (HTML Accepted: &lt;b&gt; &lt;/b&gt; &lt;i&gt; &lt;/i&gt; &lt;p&gt; &lt;/p&gt; &lt;u&gt;
                    &lt;/u&gt; &nbsp;&nbsp;&lt;&nbsp;br/&gt;&nbsp;&nbsp;&lt;ul&gt;&lt;li&gt;&lt;/li&gt;&lt;/ul&gt;)--%>
                     Session Description (no HTML please, no special characters. Both cause issues on mobile)
                     <br />
                    <asp:TextBox ID="TextBoxSummary" Width="700" Height="300" TextMode="MultiLine" runat="server"></asp:TextBox>
                    <br />
                </div>
                <br />
                <div>
                    <asp:Image ID="Image1" ImageUrl="~/app_themes/default/tag2.png" runat="server" />
                    <asp:CheckBoxList ID="CheckBoxListTags" RepeatColumns="4" RepeatLayout="Table" RepeatDirection="Vertical"
                        runat="server" DataSourceID="SqlDataSourceTags" DataTextField="TagName" DataValueField="id">
                    </asp:CheckBoxList>
                </div>
                <div id="newtag" runat="server" visible="true" style="margin: 7px 0px 7px 0px;">
                    New Categories for your topic separated by ".,;:" if multiple:&nbsp;&nbsp;
                    <asp:TextBox ID="TextBoxAddCategories" runat="server" Width="300px"></asp:TextBox>
                </div>
            </div>
            <asp:Button ID="ButtonUpdate2" runat="server" Text="Update Session Information" OnClick="ButtonUpdate2_Click" />
            <br />
            <asp:SqlDataSource ID="SqlDataSourceTags" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
                SelectCommand="SELECT [id], [TagName] FROM [Tags] WHERE [TagName] <> '' ORDER BY UPPER(LTRIM(TagName))">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceLevel" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
                SelectCommand="SELECT [id], [description] FROM [SessionLevels] ORDER BY [id]">
            </asp:SqlDataSource>
        </td>
    </tr>
</asp:Content>
