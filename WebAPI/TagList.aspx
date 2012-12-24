<%@ Page Title="" Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true" Inherits="TagList" Codebehind="TagList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Sublinks" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

<div class="mainHeading">Tag List Setup For Analysis</div>
    <br/>
     
 <asp:HyperLink ID="HyperLinkTagGroupGraph" NavigateUrl="TagGroupGraph.aspx" runat="server">Perform Analysis and Display Graph</asp:HyperLink>
 <br/><br/>

    <asp:GridView ID="GridViewTagList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
        DataKeyNames="Id,AttendeesId" DataSourceID="SqlDataSourceTagList" BackColor="White"
        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" EmptyDataText="Press Create Button"
        GridLines="Vertical" 
        onselectedindexchanged="GridViewTagListSelectedIndexChanged" 
        onrowupdating="GridViewTagListRowUpdating" onrowcommand="GridViewTagList_RowCommand" 
        >
        <AlternatingRowStyle BackColor="#DCDCDC" />
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <EditItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                        CommandName="Update" Text="Update"></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                        CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                        CommandName="Edit" Text="Edit"></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                        CommandName="Select" Text="Select"></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" 
                        CommandName="Delete" Text="Delete" CommandArgument='<%# Eval("Id") %>'   ></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;">
            </asp:TemplateField>
            <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True"
                SortExpression="Id" Visible="False" />
            <asp:BoundField DataField="AttendeesId" HeaderText="AttendeesId" SortExpression="AttendeesId"
                Visible="False" />
            <asp:BoundField DataField="TagListName" HeaderText="Tag List Name" 
                SortExpression="TagListName" />
        </Columns>
        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#0000A9" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#000065" />
        <SelectedRowStyle BackColor="Yellow" />
    </asp:GridView>
    <asp:Label ID="LabelError1" runat="server" Text=""></asp:Label>
    <br />
    <asp:Button ID="ButtonCreateNewTagListName" runat="server" Text=" Create Tag List From TextBox On Right"
        Width="290px" OnClick="ButtonCreateNewTagListName_Click" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <br />
    <hr />
        <asp:Label ID="Label2" runat="server"  Text="Tag Group Selected"    Font-Bold="True" Font-Size="Larger"></asp:Label>
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="LabelTagGroupName" runat="server"  Text="(None, select from list above)"    Font-Bold="True" Font-Size="Larger"></asp:Label>
        
  
    <table style="margin: 25 25 25 25; border-spacing: 25">
        <tr>
            <td style="vertical-align: top">
                <asp:GridView ID="GridViewNotSelected" runat="server"  AutoGenerateColumns="False"
                    DataSourceID="SqlDataSourceTagsNotSelected" OnRowCommand="GridViewNotSelected_RowCommand">
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id") %>'
                                    CommandName="Select" Text="Select"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;">
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="Label1" Text='<%# Eval("TagName") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td style="vertical-align: top">
                <asp:GridView ID="GridViewSelected" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    DataSourceID="SqlDataSourceTagsSelected" OnRowCommand="GridViewSelectedRowCommand">
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id") %>'
                                    CommandName="Select" Text="Remove"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;">
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="Label1" Text='<%# Eval("TagName") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <asp:SqlDataSource ID="SqlDataSourceTagsSelected" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        SelectCommand="SELECT Id,TagName FROM Tags WHERE TagName <> '' AND TagName IS NOT NULL AND ID IN (
                            SELECT dbo.Tags.Id
                            FROM dbo.AttendeesTagListDetail
                                 INNER JOIN dbo.AttendeesTagList ON (
                                 dbo.AttendeesTagListDetail.AttendeesTagListId = dbo.AttendeesTagList.Id)
                                 INNER JOIN dbo.Tags ON (dbo.AttendeesTagListDetail.TagsId = dbo.Tags.Id)
                            WHERE dbo.AttendeesTagList.AttendeesId = @AttendeesId AND
                                  dbo.AttendeesTagList.Id = @AttendeesTagListId) ORDER BY LTRIM(TagName)">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelAttendeesId" Name="AttendeesId" PropertyName="Text" />
            <asp:ControlParameter ControlID="GridViewTagList" Name="AttendeesTagListId" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceTagsNotSelected" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        SelectCommand="
        SELECT Id,
            TagName
            FROM Tags
            WHERE Id IN (
                          SELECT DISTINCT tagid
                          FROM dbo.SessionTags
                               INNER JOIN dbo.Sessions ON (dbo.SessionTags.SessionId =
                               dbo.Sessions.Id)
                          WHERE dbo.Sessions.CodeCampYearId <> @CodeCampYearId + 10000
                  ) AND
                  TagName <> '' AND
                  TagName IS NOT NULL AND
                  ID NOT IN (
                              SELECT dbo.Tags.Id
                              FROM dbo.AttendeesTagListDetail
                                   INNER JOIN dbo.AttendeesTagList ON (
                                   dbo.AttendeesTagListDetail.AttendeesTagListId =
                                   dbo.AttendeesTagList.Id)
                                   INNER JOIN dbo.Tags ON (dbo.AttendeesTagListDetail.TagsId
                                   = dbo.Tags.Id)
                              WHERE dbo.AttendeesTagList.AttendeesId = @AttendeesId AND
                                    dbo.AttendeesTagList.Id = @AttendeesTagListId
                  )
            ORDER BY LTRIM(TagName)
        ">
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelCodeCampYearId" Name="CodeCampYearId" 
                PropertyName="Text" />
            <asp:ControlParameter ControlID="LabelAttendeesId" Name="AttendeesId" PropertyName="Text" />
            <asp:ControlParameter ControlID="GridViewTagList" Name="AttendeesTagListId" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceTagList" runat="server" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>"
        DeleteCommand="DELETE FROM [AttendeesTagList] WHERE [Id] = @Id" 
        InsertCommand="INSERT INTO [AttendeesTagList] ([AttendeesId], [TagListName]) VALUES (@AttendeesId, @TagListName)"
        SelectCommand="SELECT [Id], [AttendeesId], [TagListName] FROM [AttendeesTagList] WHERE ([AttendeesId] = @AttendeesId) ORDER BY [TagListName]"
        
        UpdateCommand="UPDATE [AttendeesTagList] SET [AttendeesId] = @AttendeesId, [TagListName] = @TagListName WHERE [Id] = @Id">
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="AttendeesId" Type="Int32" />
            <asp:Parameter Name="TagListName" Type="String" />
        </InsertParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="LabelAttendeesId" Name="AttendeesId" 
                PropertyName="Text" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="AttendeesId" Type="Int32" />
            <asp:Parameter Name="TagListName" Type="String" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:Label ID="LabelAttendeesId" Visible="false" runat="server" Text="Label"></asp:Label>
     <asp:Label ID="LabelCodeCampYearId" Visible="false" runat="server" Text="Label"></asp:Label>
</asp:Content>
