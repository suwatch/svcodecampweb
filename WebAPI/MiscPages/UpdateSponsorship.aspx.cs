using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class MiscPages_UpdateSponsorship : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TextBoxMin.Text = Utils.MinSponsorLevelPlatinum.ToString(CultureInfo.InvariantCulture);
            TextBoxMax.Text = Utils.MinSponsorLevelPlatinum.ToString(CultureInfo.InvariantCulture);
            TextBoxCCYar.Text = Utils.GetCurrentCodeCampYear().ToString(CultureInfo.InvariantCulture);
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
    }
}