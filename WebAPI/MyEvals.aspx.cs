using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;
using System.Drawing;

public partial class MyEvals : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LiteralEvalsDoneThisYear.Text = Utils.GetNumberCodeCampEvals().ToString();
            LiteralSessionsEvaluatedThisYear.Text = Utils.GetNumberEvals().ToString();
        }

        if (Context.User.Identity.IsAuthenticated)
        {
            LabelCurrentUser.Text = Context.User.Identity.Name;
            //LabelCurrentUser.Visible = true;

            bool hasDoneEval = Utils.CheckAttendeeDoneEval(Context.User.Identity.Name);
            if (hasDoneEval)
            {
                LabelDoneEval.Text = "(Evaluation Done)";
                LabelDoneEval.ForeColor = Color.Green;
            }
            else
            {
                LabelDoneEval.Text = "(Evaluation Not Done. Please Click Here and Complete it for prize eligibility)";
                LabelDoneEval.ForeColor = Color.Red;
            }

        }

        LabelCodeCampYearId.Text = Utils.GetCurrentCodeCampYear().ToString();
        //LabelCodeCampYearId.Visible = true;
    }
}