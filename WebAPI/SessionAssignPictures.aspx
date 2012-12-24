<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="SessionAssignPictures" Title="Untitled Page" Codebehind="SessionAssignPictures.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="parentContent" runat="Server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <br />
    <asp:HyperLink Visible="true" ID="HyperLinkReturn" runat="server">Return to Session</asp:HyperLink><br />
    <br />
    <div runat="server" id="DivAvailablePictureUpload">
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Upload Files (Admin Only, only for Peter for now, no flicker)" /><br />
        <asp:FileUpload ID="FileUploadImages" runat="server" />
        <br />
    </div>
    <div style="float: right; width: 50%;" runat="server" id="DivAvailablePictureList">
        Number Pictures to Show on Available List
        <asp:UpdatePanel ID="panel1"  runat="server">
            <ContentTemplate>
                <asp:DropDownList ID="DropDownListPerPageCount" AutoPostBack="true" runat="server"
                    OnSelectedIndexChanged="DropDownListPerPageCount_SelectedIndexChanged">
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>25</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>100</asp:ListItem>
                    <asp:ListItem>500</asp:ListItem>
                </asp:DropDownList>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <asp:UpdatePanel ID="UpdatePanel2"  runat="server">
            <ContentTemplate>
                <asp:GridView ID="GridViewAvailablePictures" runat="server" PageSize="5" AutoGenerateColumns="False"
                    DataKeyNames="id" DataSourceID="ObjectDataSourceAvailablePictures" OnRowCommand="GridViewAvailablePictures_RowCommand"
                    AllowPaging="True">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                            
                                <table>
                                <tr><td>
                                 <asp:LinkButton ID="LinkAssignButton" runat="server" CommandName="Assign" Text="Assign Below Picture To Session"
                                    Visible="true" CommandArgument='<%# Eval("id") %>'>
                                </asp:LinkButton>
                                </td></tr>
                                 <tr><td>
                                 <asp:Image ID="Image2" runat="server" ImageUrl='<%# "~/DisplayImage.ashx?sizex=200&pictureid=" + Eval("id")  %>' />
                                
                                </td></tr>
                                 <tr><td>
                                <asp:HyperLink ID="HyperLinkFullRes" runat="server" NavigateUrl='<%# "http://peterkellner.net/images/CodeCampSV06/" + (string) UpperCaseName((string) Eval("FileName")) %>'
                                    Text="(Full Resolution Link)"></asp:HyperLink>
                                </td></tr>
                                </table>
                            
                               
                                <%--<asp:ImageButton ID="ImageButton1" ImageUrl='<%# "~/DisplayImage.ashx?sizex=200&pictureid=" + Eval("id")  %>' runat="server"
                                        PostBackUrl='<%# "http://peterkellner.net/images/CodeCampSV06/" + GetUpperName((string) Eval("FileName")) %>'  />
                                 --%>
                                
                                
                                <br />
                                <br />
                                <br />
                                <%--<asp:Label ID="LabelFilename" runat="server" Text='<%# Bind("FileName") %>'></asp:Label>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:ObjectDataSource ID="ObjectDataSourceAvailablePictures" runat="server" TypeName="DataSetPicturesTableAdapters.PicturesTableAdapter"
            DeleteMethod="Delete" InsertMethod="Insert" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetByNotAssignedToSession" UpdateMethod="Update">
            <DeleteParameters>
                <asp:Parameter Name="Original_id" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="AttendeePKID" Type="Object" />
                <asp:Parameter Name="DateCreated" Type="DateTime" />
                <asp:Parameter Name="DateUpdated" Type="DateTime" />
                <asp:Parameter Name="PictureBytes" Type="Object" />
                <asp:Parameter Name="FileName" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:Parameter Name="Original_id" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="AttendeePKID" Type="Object" />
                <asp:Parameter Name="DateCreated" Type="DateTime" />
                <asp:Parameter Name="DateUpdated" Type="DateTime" />
                <asp:Parameter Name="PictureBytes" Type="Object" />
                <asp:Parameter Name="FileName" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
            </InsertParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="SessionID" QueryStringField="sessionid" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    
    <asp:UpdateProgress ID="progress1" runat="server">
            <ProgressTemplate>
                <div class="progress">
                    <img src="Images/pleasewait.gif" />
                    Please Wait...
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    
    <div runat="server" id="DivAssignedPictureList">
        <asp:UpdatePanel ID="UpdatePanel1"  runat="server">
            <ContentTemplate>
                <asp:GridView BorderWidth="0" ID="GridViewPicturesAssigned" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="id" DataSourceID="ObjectDataSourcePicturesAssigned" OnRowCommand="GridViewPicturesAssigned_RowCommand">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="LinkUnAssignButton" runat="server" CommandName="Assign" Text="UnAssign Picture To Session"
                                                Visible='<%# (bool) CheckForAuthenticated() %>' CommandArgument='<%# "unassign^" + Eval("id") %>'>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="LinkButtonMakeDefault" runat="server" CommandName="Assign" Text="Make Default For Session"
                                                Visible='<%# (bool) CheckForAuthenticated() %>' CommandArgument='<%# "makedefault^" + Eval("id") %>'>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%--<asp:ImageButton ID="ImageButton1" ImageUrl='<%# "~/DisplayImage.ashx?sizex=200&pictureid=" + Eval("id")  %>' runat="server"
                                        PostBackUrl='<%# "http://peterkellner.net/images/CodeCampSV06/" + GetUpperName((string) Eval("FileName")) %>'  />--%>
                                            <asp:Image ID="Image1" runat="server" ImageUrl='<%# "~/DisplayImage.ashx?sizex=200&pictureid=" + Eval("id")  %>' />
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                    <td>
                                    <asp:HyperLink ID="HyperLinkFullRes" runat="server" NavigateUrl='<%# "http://peterkellner.net/images/CodeCampSV06/" + (string) UpperCaseName((string) Eval("FileName")) %>'
                                                Text="(Full Resolution Link)"></asp:HyperLink>
                                    </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LabelDescription" runat="server" Text='<%# GetDescriptionFromPictureId((int) Eval("id")) %>'></asp:Label>
                                            <br />
                                            <asp:LinkButton ID="LinkButtonEditDescription" Visible='<%# (bool) CheckForAuthenticated() %>'
                                                Text="Edit Description" CommandName="EditDescription" CommandArgument='<%# "editdescription^" + Eval("id") %>'
                                                runat="server"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <br />
                                    <br />
                                    <hr />
                                </table>
                                <%----%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:ObjectDataSource ID="ObjectDataSourcePicturesAssigned" runat="server" DeleteMethod="Delete"
            InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" SelectMethod="GetByAssignedToSession"
            TypeName="DataSetPicturesTableAdapters.PicturesTableAdapter" UpdateMethod="Update">
            <DeleteParameters>
                <asp:Parameter Name="Original_id" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="AttendeePKID" Type="Object" />
                <asp:Parameter Name="DateCreated" Type="DateTime" />
                <asp:Parameter Name="DateUpdated" Type="DateTime" />
                <asp:Parameter Name="PictureBytes" Type="Object" />
                <asp:Parameter Name="FileName" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:Parameter Name="Original_id" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="SessionID" QueryStringField="sessionid" Type="Int32" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="AttendeePKID" Type="Object" />
                <asp:Parameter Name="DateCreated" Type="DateTime" />
                <asp:Parameter Name="DateUpdated" Type="DateTime" />
                <asp:Parameter Name="PictureBytes" Type="Object" />
                <asp:Parameter Name="FileName" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </div>
    <div style="clear: both">
    </div>
</asp:Content>
