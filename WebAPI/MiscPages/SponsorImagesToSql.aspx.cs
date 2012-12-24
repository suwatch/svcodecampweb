using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class MiscPages_SponsorImagesToSql : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        var sponsors = CodeCampSV.SponsorListManager.I.GetAllCurrentYear7();
         int good = 0;
            int bad = 0;
        foreach (var sponsor in sponsors)
        {
            string fileName = Context.Server.MapPath(sponsor.ImageURL);
           
            if (sponsor.SponsorName == "Intuit")
            {
                
            }

            try
            {
                Byte[] bytes;
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    var r = new BinaryReader(fs);
                    bytes = r.ReadBytes((int) fs.Length);
                }
                Utils.UpdateSponsorImage(sponsor.Id, bytes,fileName);
                good++;
            }
            catch (Exception)
            {

                bad++;
            }


        }

        Label1.Text = "good: " + good.ToString() + "  bad: " + bad.ToString();


    }

    protected string GetImageUrl(int id)
    {
        string str = "/sponsorimage/" + id.ToString(CultureInfo.InvariantCulture) + ".jpg?width=50&height=50";
        return str;
    }
}