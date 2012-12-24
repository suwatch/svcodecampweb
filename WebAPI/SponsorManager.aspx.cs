using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class SponsorManager : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!Utils.CheckUserIsSponsorManagerOrAdmin() )
            {
                Response.Redirect("~/default.aspx");
            }
        }

    }

    protected string GetHyperLink(int id)
    {
        string str = String.Format("~/SponsorInformation.aspx?sponsorid={0}", id);
        return str;
    }
}