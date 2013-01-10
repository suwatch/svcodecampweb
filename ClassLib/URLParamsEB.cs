using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Compilation;
using System.CodeDom;



/// <summary>
/// Summary description for URLParamsEB
/// </summary>
public class URLParamsEB : ExpressionBuilder
{
    public override System.CodeDom.CodeExpression GetCodeExpression(
        BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
    {
        CodeTypeReferenceExpression targetClass =
            new CodeTypeReferenceExpression(typeof(URLParamsEB));
        string targetMethod = "GetURLParam";
        CodeExpression methodParameter =
            new CodePrimitiveExpression(entry.Expression.Trim());
        return new CodeMethodInvokeExpression(
            targetClass, targetMethod, methodParameter);
    }

    public static object GetURLParam(string param)
    {
        string returnString = string.Empty;

        if (param.ToLower().Equals("rsspage"))
        {
            returnString = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
            HttpContext.Current.Request.ApplicationPath +
            "/Rss.aspx";
        }
        else if (param.ToLower().Equals("atompage"))
        {
            returnString = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
            HttpContext.Current.Request.ApplicationPath +
            "/Atom.aspx";
        }

        return returnString;
    }

    
}
