using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Configuration;

/// <summary>
/// Summary description for BasePage
/// (SEE BASECONTENTPAGE IN APP_CODE OF WEB SITE)
/// </summary>
public class BasePageXXX : Page
{
    // http://sandarenu.blogspot.com/2007/02/determining-viewstate-size.html
    protected override void SavePageStateToPersistenceMedium(object state)
    {
        base.SavePageStateToPersistenceMedium(state); // $$$ need to verify if this is first or last.

        if (ConfigurationManager.AppSettings["ShowViewStateWarningThreshHold"] != null)
        {
            int viewStateThreshHold = Convert.ToInt32(ConfigurationManager.AppSettings["ShowViewStateWarningThreshHold"]);
            if (viewStateThreshHold != 0)
            {
                base.SavePageStateToPersistenceMedium(state);
                var format = new LosFormatter();
                var writer = new StringWriter();
                format.Serialize(writer, state);
                int viewStateSize = writer.ToString().Length;
                if (viewStateSize >= viewStateThreshHold)
                {
                    string pageName = GetCurrentPageName();
                    Alert.Show(string.Format("ViewStateSize for: {0} = {1}", pageName, viewStateSize.ToString()));
                    //HttpContext.Current.Trace.Warn(pageName + ": The ViewState Size is:", viewStateSize.ToString());
                }
            }
        }

        
    }

    private string GetCurrentPageName()
    {
        string path = HttpContext.Current.Request.Url.AbsolutePath;
        if (!String.IsNullOrEmpty(path))
        {
            var oInfo = new FileInfo(path);
            string name = oInfo.Name;
            return name;
        }

        return "";
    }
}