<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" Inherits="SponsorInformation" Codebehind="SponsorInformation.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="blankSublinks" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" runat="Server">
    
    <br/>
    <a href="SponsorManager.aspx">Sponsor Manager</a>
    <br/>
  
    <tr>
        <td>
            <asp:Label ID="LabelUnassigned" runat="server" Visible="false" Text="No Sponsor Associated With Your Login Name Email"
                ForeColor="Orange"></asp:Label>
            <div runat="server" id="PageInfoId">
                <table cellpadding="5" cellspacing="10" border="0">
                    <tr>
                      
                        <td colspan="2" style="font-size: x-large">
                            <asp:HyperLink ID="HyperLinkEdit" runat="server">Edit</asp:HyperLink>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Label ID="LabelSponsorName" runat="server"   Text="" Font-Size="X-Large" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    
                     <tr>
                        <td style="width: 600px; height: 27px;">
                            Platinum Table? (If you are a platinum sponsor, will you be need a table in the sponsor area)
                        </td>
                        <td style="height: 27px">
                            <asp:CheckBox ID="CheckBoxIDPlatinumTable"  Enabled="false" runat="server"/>
                        </td>
                    </tr>
                    
                     <tr>
                        <td style="width: 600px; height: 27px;">
                            Platinum Flier For Giveaway bag? (If you are a platinum sponsor, will you be providing an single sheet flier for attendee bag)
                        </td>
                        <td style="height: 27px">
                            <asp:CheckBox ID="CheckBoxIdPlatinumFlier"  Enabled="false" runat="server"/>
                        </td>
                    </tr>
                    
                     <tr>
                        <td style="width: 600px; height: 27px;">
                            Code Camp Has Received Your Flier For the attendee bag
                        </td>
                        <td style="height: 27px">
                            <asp:CheckBox ID="CheckBoxIdPlatinumFlierReceived" Enabled="false" runat="server"/>
                        </td>
                    </tr>
                    

                    <tr>
                        <td style="width: 600px; height: 27px;">
                            Are You Shipping Anything To Code Camp?
                        </td>
                        <td style="height: 27px">
                            <asp:CheckBox ID="CheckBoxIdShippingItemstoCodeCamp" Enabled="false"  runat="server"/>
                            <br/>
                            What:  &nbsp; <asp:Label ID="LabelShippingWhatToCodeCamp" runat="server">stuff</asp:Label>
                            

                        </td>
                    </tr>

                    <tr>
                        <td style="width: 600px; height: 27px;">
                            Company Address Line 1
                        </td>
                        <td style="height: 27px">
                            <asp:Label ID="LabelAddressLine1" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 600px">
                            Company Address Line 2
                        </td>
                        <td>
                            <asp:Label ID="LabelAddressLine2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 600px">
                            Company City
                        </td>
                        <td>
                            <asp:Label ID="LabelCity" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 600px">
                            Company State
                        </td>
                        <td>
                            <asp:Label ID="LabelState" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 600px">
                            Company Zip
                        </td>
                        <td>
                            <asp:Label ID="LabelZip" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 600px; height: 27px;">
                            Company Phone Number
                        </td>
                        <td style="height: 27px">
                            <asp:Label ID="LabelPhone" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Full Company Description (may be used on our web site)
                        </td>
                        <td>
                            <asp:Label ID="LabelLongDescription" runat="server" Text="This is the full description"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Short Company Description (may be used on our web site)
                        </td>
                        <td>
                            <asp:Label ID="LabelShortDescription" runat="server" Text="This is the short description"></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="2">
                           Note From Code Camp Staff:&nbsp;
                           <asp:Label runat="server" BackColor="Cyan" ID="LabelNoteFromCodeCamp"></asp:Label>

                        </td>
                        

                    </tr>
                </table>
                <br />
                &nbsp;Please Update Contact Information Below For All People associated with sponsor. Please Add and Delete as necessary.  Also, please
                make one person of type "Primary" for category.  To add additional people, fill in email address on bottom line of grid and then press
                the <u>Insert</u> link.  Then, use <u>Edit</u> link at top to fill in the rest of the information (first name,last name, etc.).
                &nbsp;<asp:GridView ID="GridViewSponsorListContact" runat="server" AutoGenerateColumns="False"
                    ShowFooter="true" DataKeyNames="Id,SponsorListId,DateCreated,Comment,ContactType"
                    DataSourceID="ObjectDataSourceSponsorListContact" ForeColor="#333333" CellPadding="5"
                    CellSpacing="10" GridLines="None" OnRowCommand="GridViewSponsorListContact_RowCommand">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                                    Text="Update"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                    Text="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
                                &nbsp;<asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                    Text="Edit"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandName="Delete"
                                    Text="Delete"></asp:LinkButton>
                            </ItemTemplate>
                            <FooterTemplate>
                                <%-- <asp:Button ID="Button1" runat="server" Text="Button" />--%>
                                &nbsp;<asp:LinkButton ID="lbInsert" runat="server" CommandName="Insert" Text="Insert"
                                    OnClick="lbInsert_Click">
                                </asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SponsorListId" HeaderText="SponsorListId" SortExpression="SponsorListId"
                            Visible="False" />
                        <asp:TemplateField HeaderText="Email" SortExpression="EmailAddress">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("EmailAddress") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("EmailAddress") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="EmailAddress" runat="Server"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Category">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DropDownListSponsorListContactType" DataSourceID="ObjectDataSourceSponsorListContactType"
                                    SelectedValue='<%# Bind("SponsorListContactTypeId") %>' Enabled="true" AppendDataBoundItems="True"
                                    runat="server" DataTextField="Description" DataValueField="Id">
                                    <asp:ListItem Text="-none selected-" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:DropDownList ID="DropDownListSponsorListContactType" DataSourceID="ObjectDataSourceSponsorListContactType"
                                    SelectedValue='<%# Eval("SponsorListContactTypeId") %>' Enabled="false" AppendDataBoundItems="True"
                                    runat="server" DataTextField="Description" DataValueField="Id">
                                    <asp:ListItem Text="-none selected-" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="Label2" runat="server" Text="(After Inserting Email, Used Edit)"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ContactType" HeaderText="ContactType" SortExpression="ContactType"
                            Visible="False" />
                        <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
                        <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" />
                        <asp:BoundField DataField="PhoneNumberOffice" HeaderText="Office Phone" SortExpression="PhoneNumberOffice" />
                        <asp:BoundField DataField="PhoneNumberCell" HeaderText="Cell Phone" SortExpression="PhoneNumberCell" />
                        <asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment"
                            Visible="False" />
                        <asp:CheckBoxField DataField="GeneralCCMailings" HeaderText="Receive Attendee Code Camp Mailings"
                            SortExpression="GeneralCCMailings">
                            <ControlStyle Width="80px" />
                            <FooterStyle Width="80px" />
                            <HeaderStyle Width="80px" />
                            <ItemStyle Width="80px" />
                        </asp:CheckBoxField>
                        <asp:CheckBoxField DataField="SponsorCCMailings" HeaderText="Receive Mailings Designated For Sponsors"
                            SortExpression="SponsorCCMailings">
                            <ControlStyle Width="80px" />
                            <FooterStyle Width="80px" />
                            <HeaderStyle Width="80px" />
                            <ItemStyle Width="80px" />
                        </asp:CheckBoxField>
                        <asp:BoundField DataField="UsernameAssociated" HeaderText="UsernameAssociated" SortExpression="UsernameAssociated"
                            Visible="False" />
                        <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" SortExpression="DateCreated"
                            Visible="False" />
                        <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" SortExpression="Id"
                            Visible="False" />
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle Font-Bold="True" ForeColor="Black" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSourceSponsorListContact" runat="server" DataObjectTypeName="CodeCampSV.SponsorListContactResult"
                    DeleteMethod="Delete" InsertMethod="Insert" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetBySponsorListId" TypeName="CodeCampSV.SponsorListContactManager"
                    UpdateMethod="Update">
                    <DeleteParameters>
                        <asp:Parameter Name="id" Type="Int32" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="LabelSponsorListId" Name="sponsorListId" PropertyName="Text"
                            Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="ObjectDataSourceSponsorListContactType" runat="server"
                    DataObjectTypeName="CodeCampSV.SponsorListContactTypeResult" DeleteMethod="Delete"
                    InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" SelectMethod="GetAll"
                    TypeName="CodeCampSV.SponsorListContactTypeManager">
                    <DeleteParameters>
                        <asp:Parameter Name="id" Type="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
                <asp:Label ID="LabelSponsorListId" runat="server" Visible="false"></asp:Label>
            </div>
            <br />
        </td>
    </tr>
    </td> </tr>
</asp:Content>
