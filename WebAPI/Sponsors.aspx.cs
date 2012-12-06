using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAPI
{
    public partial class Sponsors : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["SponsorPageRaffleURL"] != null)
            {
                HyperLinkRaffle.Text = ConfigurationManager.AppSettings["SponsorPageRaffleURL"];
                HyperLinkRaffle.Target = ConfigurationManager.AppSettings["SponsorPageRaffleURL"];
            }
        }
    }
}