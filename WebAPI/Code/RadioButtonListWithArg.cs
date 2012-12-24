using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

namespace CodeCampSV
{
    /// <summary>
    /// Summary description for RadioButtonListWithArg
    /// </summary>
    /// 
    [ToolboxData("<{0}:RadioButtonListWithArg runat=server></{0}:RadioButtonListWithArg>")]
    public class RadioButtonListWithArg : RadioButtonList
    {
        [Bindable(true),
             Category("Behavior"),
             DefaultValue("")]
        public string CommandArg
        {
            get
            {
                // look for hover color in ViewState
                object o = ViewState["CommandArg"];
                if (o == null)
                    return String.Empty;
                else
                    return (string)o;
            }
            set
            {
                ViewState["CommandArg"] = value;
            }
        }


        //public RadioButtonListWithArg()
        //{
        //}
    }
}