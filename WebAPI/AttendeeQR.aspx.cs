using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class AttendeeQR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {

    }
    protected void Button1_Click1(object sender, EventArgs e)
    {
        // http://www.nttdocomo.co.jp/english/service/imode/make/content/barcode/function/application/addressbook/index.html


        StringBuilder stringBuilder = new StringBuilder("MECARD:");
        stringBuilder.Append("N:Reagan,Ronald;");
        stringBuilder.Append("TEL:(408) 234-1385;");
        stringBuilder.Append("EMAIL:ronald@whitehouse.gov;");
        stringBuilder.Append("NOTE:Captured At Silicon Valley Code Camp 2010;");
        stringBuilder.Append("ADR:60 Birchwood Lane,,Hartsdale,NY,10530;");
        stringBuilder.Append("EMAIL:ronald@whitehouse.gov;");

        TextBox1.Text = stringBuilder.ToString();
        

    }
}