using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CodeCampSV;

public partial class Previous : BaseContentPage
{
    protected class YearContainer
    {
        public int YearValue { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        const int startingYear = 2006;
        int endingYear = Utils.CurrentCodeCampYear+ 2005;

        var years = new List<YearContainer>();
        for (int i=startingYear;i<endingYear;i++)
        {
            years.Add(new YearContainer() {YearValue = i});
        }
        RepeaterId.DataSource = years;
        RepeaterId.DataBind();

        HyperLinkThisYear.NavigateUrl = string.Format("~/Default.aspx?Year={0}", endingYear);
    }

    protected string GetCodeCampYearText(int codeCampYearId)
    {
        return codeCampYearId.ToString(CultureInfo.InvariantCulture) + " CodeCamp";

    }

    protected object GetCodeCampYearUrl(int codeCampYearId)
    {
        return "~/Default.aspx?Year=" + codeCampYearId.ToString(CultureInfo.InvariantCulture);
    }

    protected bool GetCodeCampYearEnabled(int codeCampYear)
    {
        return codeCampYear >= 2008;
    }
}
