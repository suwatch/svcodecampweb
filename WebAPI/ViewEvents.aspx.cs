using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class ViewEvents : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ButtonClearLog_Click(object sender, EventArgs e)
    {
          string connectionName = ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString;
        Utils.ClearLog4NetEntries(connectionName);
    }
}