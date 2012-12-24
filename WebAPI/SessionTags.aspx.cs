using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class SessionTags : BaseContentPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ButtonAddTag_Click(object sender, EventArgs e)
    {
        if (TextBoxTag.Text.Length > 0)
        {
            SqlDataSource1.InsertParameters["TagName"].DefaultValue = TextBoxTag.Text;
            
        }
        int retVal = SqlDataSource1.Insert();
        GridView1.DataBind();
    }
}
