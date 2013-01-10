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

public class ExpressionBuilderIdentity : ExpressionBuilder
{
    public override System.CodeDom.CodeExpression GetCodeExpression(
        BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
    {
        CodeTypeReferenceExpression targetClass =
            new CodeTypeReferenceExpression(typeof(ExpressionBuilderIdentity));
        string targetMethod = "GetIdentity";
        CodeExpression methodParameter =
            new CodePrimitiveExpression(entry.Expression.Trim());
        return new CodeMethodInvokeExpression(
            targetClass, targetMethod, methodParameter);
    }

    public static object GetIdentity(string param)
    {
        string returnString = string.Empty;

        if (param.ToLower().Equals("name"))
        {
            returnString = HttpContext.Current.User.Identity.Name;
        }

        return returnString;
    }
}
