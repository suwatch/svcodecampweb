using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

namespace WebAPI
{
    public partial class RightRegister : System.Web.UI.MasterPage
    {
        public bool ShowSponsorPage { set; get; }

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime codeCampDateTime = Utils.GetCurrentCodeCampYearStartDate();
            //int codeCampYear = Utils.GetCurrentCodeCampYear();



            //TimeSpan ts = codeCampDateTime.Subtract(new DateTime(2008, 11, 10));
            TimeSpan ts = codeCampDateTime.Subtract(DateTime.Now);
            if (ts.Days > 1)
            {
                RegisterNowID.Text = String.Format("Register Now!  {0} Days Left", ts.Days);
            }
            if (ts.Days == 1)
            {
                RegisterNowID.Text = String.Format("Register Now!  {0} Day Left", ts.Days);
            }
            else if (ts.Days == 0 || ts.Days == -1)
            {
                RegisterNowID.Text = "Code Camp Is Now!";
            }
            else if (ts.Days < -2)
            {
                RegisterNowID.Text = String.Format("This Code Camp Happened {0} days ago", ts.Days * -1);
                RegisterNowID.NavigateUrl = string.Empty;
            }

        }
    }
}