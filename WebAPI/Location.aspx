<%@ Page Language="C#" MasterPageFile="~/RightRegister.master" AutoEventWireup="true" Inherits="Location" Title="Venue CodeCamp SFBA" Codebehind="Location.aspx.cs" %>
    
    
    <asp:Content ID="SublinksAbout" ContentPlaceHolderID="Sublinks" runat="server">
    <asp:Menu ID="aboutSubMenu" runat="server" DataSourceID="SiteMapAbout" SkinID="subMenu">
    </asp:Menu>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

<div class="mainHeading">Location</div>

<div class="pad">

    <table cellpadding="10" cellspacing="10">
        <tr>
            <td colspan="2">
            
                <asp:HyperLink ID="HyperLink1" NavigateUrl="http://codecamp.pbwiki.com/CodeCampSiliconValleyTravel09"  runat="server">Directions to CodeCamp at Foothill College</asp:HyperLink>
                <br />
                12345 El Monte Road<br />
                Los Altos Hills, CA 94022<br />
                <br />
                <a href="misc/svcc-2012-map.png">Campus Map With Buildings</a><br />
               
            </td>
        </tr>
        <tr>
            <td>
                <img src="images/foothillstaff2007a.jpg"   alt="Foothill Staff"/></td>
            <td>
                <p>
                    <b>Foothill Staff <span><a href="http://www.foothill.edu/ctis/haight.php">Elaine 
                    Haight </a>(Faculty), Judy Miner (President of Foothill College),
                    <a href="http://krypton.fhda.edu/~mmurphy/">Mike Murphy</a> 
                    (Faculty) and <a href="http://www.foothill.edu/ctis/woods.php">Tim Woods</a> (CTIS Devision 
                    Dean).</span><br />
                   </b><br /></p>
            </td>
        </tr>
        <tr>
            <td>
                <img src="images/IMGP1065.jpg" alt="Foothill Picture"   /></td>
            <td>
                <img src="images/IMGP1066.jpg" alt="Foothill Picture" /></td>
        </tr>
        <tr>
            <td>
                <img src="images/IMGP1071.jpg" alt="Foothill Picture" /></td>
            <td>
                <img src="images/IMGP1073.jpg" alt="Foothill Picture" /></td>
        </tr>
        <tr>
            <td>
                <img src="images/IMGP1069.jpg" alt="Foothill Picture" />
            </td>
            <td>
                <img src="images/IMGP1075.jpg" alt="Foothill Picture" /></td>
        </tr>
        <tr>
            <td>
            
            Computers Technology and<br />
            Information Systems<br />
            Foothill College<br />
            12345 El Monte Road<br />
            Los Altos Hills, CA 94022<br />

            
            </td>
            <td>
                <img src="images/IMGP1079.jpg" alt="Foothill Picture" /></td>
            
        </tr>
    </table>
    
    </div>
    
</asp:Content>
