<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="RegistrationStatus" Title="Registration Status CodeCamp SFBA" Codebehind="RegistrationStatus.aspx.cs" %>




<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  
    <br />
    <p>The following people have given their permission to have there names shown as
       attendees of Codecamp.  If you are on this list and would like to be removed,
       log into your account, choose update profile on the upper right and change your
       status to "Post Info To Site" to unchecked.</p>
    
    <br/>

    <asp:GridView  ID="GridViewAttendees" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceAttendees" AllowPaging="True" AllowSorting="True" PageSize="50">
        <Columns>
            <asp:BoundField DataField="UserFirstName" HeaderText="First Name" SortExpression="UserFirstName" />
            <asp:BoundField DataField="UserLastName" HeaderText="Last Name" SortExpression="UserLastName" />
            <asp:TemplateField HeaderText="Website" Visible="False" SortExpression="UserWebsite">
                <ItemTemplate>
                    <asp:Label  ID="Label1" runat="server" Text='<%# Bind("UserWebsite") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="UserZipCode" HeaderText="Zip Code" SortExpression="UserZipCode" />
            <asp:BoundField DataField="CreationDate" DataFormatString="{0:M-dd-yyy}" HtmlEncode="False" HeaderText="Registered Date" SortExpression="CreationDate" />
            <asp:BoundField DataField="UserBio" HeaderText="Bio" SortExpression="UserBio" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSourceAttendees"  runat="server" EnableCaching="True" CacheKeyDependency="10" ConnectionString="<%$ ConnectionStrings:CodeCampSV06 %>" SelectCommand="SELECT [CreationDate], [UserWebsite], [UserFirstName], [UserLastName], [UserZipCode],[UserBio] FROM [Attendees] WHERE ([UserShareInfo] = @UserShareInfo) ORDER BY [UserLastName], [UserFirstName]">
        <SelectParameters>
            <asp:Parameter DefaultValue="true" Name="UserShareInfo" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>

  
<asp:Repeater ID="Repeater1" Visible="false" runat="server" DataSourceID="ObjectDataSource2" >
<HeaderTemplate>
            <div id="three-column-containerw">
        </HeaderTemplate>
        <ItemTemplate>
            <div id="three-column-leftw">
                <asp:Image runat="server" ID="ImageUser" ImageUrl='<%# "~/DisplayImage.ashx?PKID=" + Eval("PKID")  %>' />
            </div>
             <div id="three-column-rightw">
                
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("CreationDate") %>'></asp:Label>
            </div>
            <div id="three-column-middlew">
               <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Userfirstname") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Userlastname") %>'></asp:Label>
                            </td>
                        </tr>
                    </table>
                    
            </div>
            <div class="clear"></div>
            <hr />
           
        </ItemTemplate>
        <FooterTemplate>
            </div>
            
        </FooterTemplate>
</asp:Repeater>


   


<asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SortParameterName="SortData"
        SelectMethod="GetByShowOnWeb" TypeName="CodeCampSV.AttendeesODS">
        <SelectParameters>
            <asp:Parameter Name="sortData" Type="String" />
            <asp:Parameter DefaultValue="true" Name="searchusershareinfo" Type="Boolean" />
        </SelectParameters>
</asp:ObjectDataSource>
    
   
  
  
  
</asp:Content>
