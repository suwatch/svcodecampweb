﻿<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultNoColumns.master" AutoEventWireup="true" Inherits="SponsorPayPal" Codebehind="SponsorPayPal.aspx.cs" %>


<asp:Content ID="Content2" ContentPlaceHolderID="blankContent" Runat="Server">

<script type="text/javascript">
function GetPP(a){
var mybuttons = new Array();
mybuttons[0] = "-----BEGIN PKCS7-----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-----END PKCS7-----";
mybuttons[1] = "-----BEGIN PKCS7-----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-----END PKCS7-----";
mybuttons[2] = "-----BEGIN PKCS7-----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-----END PKCS7-----";
mybuttons[3] = "-----BEGIN PKCS7-----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-----END PKCS7-----";
mybuttons[4] = "-----BEGIN PKCS7-----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-----END PKCS7-----";
mybuttons[5] = "-----BEGIN PKCS7-----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-----END PKCS7-----";
mybuttons[6] = "-----BEGIN PKCS7-----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-----END PKCS7-----";
mybuttons[7] = "-----BEGIN PKCS7-----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-----END PKCS7-----";
mybuttons[8] = "-----BEGIN PKCS7-----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-----END PKCS7-----";
mybuttons[9] = "-----BEGIN PKCS7-----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-----END PKCS7-----";
mybuttons[10] = "-----BEGIN PKCS7-----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-----END PKCS7-----";
mybuttons[11] = "-----BEGIN PKCS7-----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-----END PKCS7-----";
return mybuttons[a];
}
function assignPP(a){
var b = a;
try{
myElement = document.createElement('<input name="encrypted" />');
}catch(err){
myElement = document.createElement('input');
}
myElement.setAttribute('name','encrypted');
myElement.setAttribute('type','hidden');
myElement.setAttribute('value',GetPP(b));
document.getElementById('sponsorships').appendChild(myElement);
document.getElementById('aspnetForm').action = 'https://www.paypal.com/cgi-bin/webscr';
}
    </script>

    <div id="sponsorships" style="text-align: center;margin-left: 20px; margin-right: 20px;">
        <h2>
            
            Clicking the sponsorship buttons below will take you to our PayPal sponsorship pages</h2>
         <p><span style="font-size: medium;">Code Camp has a partnership with
             <a href="http://www.baadd.org/">BAADD</a> (Bay Area Database Developers) where 
             they take donations on Code Camp&#39;s behalf.&nbsp; This is a huge help to us 
             because they have a non-profit status which helps encourage donations.&nbsp; 
             After you enter all your information into PayPal to make a donation (based on 
             the amount of the button below), you will see that you are donating to BAADD.&nbsp; 
             Your reference number that you will see on your credit card statement show SVCC 
             which means it is directed towards Silicon Valley Code Camp.&nbsp; Once you 
             complete your donation,&nbsp; you will have a chance in PayPal to print out your 
             receipt which you can use for tax purposes.&nbsp; Also, if you sponsorship 
             amount is over $100, please contact your bank or credit card company and let 
             them know.&nbsp; Otherwise, PayPal seems to bounce these donations.</span></p>
    <p><span style="font-size: medium;">Please forward a copy of your paypal receipt to
        <a href="mailto:info@siliconvalley-codecamp.com">info@siliconvalley-codecamp.com</a> 
        along with the company name, and URL you want on your link from our home page.</p>
    <p><b>Thanks for your support!</b></span></p>
            
        
        <input type="hidden" name="cmd" value="_s-xclick" />
        <input id="submit1" runat="server" visible="false" type="image" src="~/Images/PayPalStuff/SVCC00010.png" name="submit" alt="10 dollar sponsor"
            onclick="assignPP(0);" />
        <input id="submit2" runat="server" visible="false" type="image" src="~/Images/PayPalStuff/SVCC00025.png" name="submit" alt="25 dollar sponsor"
            onclick="assignPP(1);" />
        <input id="submit3" runat="server" visible="false" type="image" src="~/Images/PayPalStuff/SVCC00050.png" name="submit" alt="50 dollar sponsor"
            onclick="assignPP(2);" />
        <input id="submit4" runat="server" visible="false" type="image" src="~/Images/PayPalStuff/SVCC00100.png" name="submit" alt="100 dollar sponsor"
            onclick="assignPP(3);" />
        <br />
        <input id="submit5" runat="server" type="image" src="~/Images/PayPalStuff/SVCC00250.png" name="submit" alt="250 dollar sponsor"
            onclick="assignPP(4);" />
        <input id="submit6" runat="server" type="image" src="~/Images/PayPalStuff/SVCC00500.png" name="submit" alt="500 dollar sponsor"
            onclick="assignPP(5);" />
        <input id="submit7" runat="server" type="image" src="~/Images/PayPalStuff/SVCC01000.png" name="submit" alt="1000 dollar sponsor"
            onclick="assignPP(6);" />
        <br />
        <input id="submit8" runat="server" type="image" src="~/Images/PayPalStuff/SVCC01500.png" name="submit" alt="1500 dollar sponsor"
           onclick="assignPP(7);" />
        <br />
        <input id="submit9" runat="server" type="image" src="~/Images/PayPalStuff/SVCC02500.png" name="submit" alt="2500 dollar sponsor"
            onclick="assignPP(8);" />
        <input id="submit10" runat="server" type="image" src="~/Images/PayPalStuff/SVCC04000.png" name="submit" alt="4000 dollar sponsor"
           onclick="assignPP(9);" />
        <input id="submit11" runat="server" type="image" src="~/Images/PayPalStuff/SVCC05000.png" name="submit" alt="5000 dollar sponsor"
          onclick="assignPP(10);" />
        <input id="submit12" runat="server" type="image" src="~/Images/PayPalStuff/SVCC10000.png" name="submit" alt="10000 dollar sponsor"
          onclick="assignPP(11);" />
        <br />
        <br />
    </div>


</asp:Content>

