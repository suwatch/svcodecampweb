using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class RoomAssignments : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LabelCodeCampYearId.Text = Utils.CurrentCodeCampYear.ToString();

        //if (DropDownListUsedRooms.SelectedValue.Equals("0"))
        //{
        //    DivAllRooms.Visible = true;
        //    DivOneRoom.Visible = false;
        //}
        //else
        //{
        //    DivAllRooms.Visible = false;
        //    DivOneRoom.Visible = true;
        //}
    }

    protected string GetWillAttendCount(int sessionId)
    {
        string retStr = string.Empty;
        if (Utils.CheckUserIsAdmin())
        {
            SessionAttendeeODS saODS = new SessionAttendeeODS();
            int cnt = saODS.GetCountBySessionIdAndInterest(sessionId, 3);
            retStr = string.Format(" Attend:{0}", cnt);
        }
        return retStr;
    }
    protected string GetNiceDayFormat(DateTime inDate)
    {
        string str1 = inDate.ToString().Replace("10/27/2007", "11/8/2008");
        string str2 = str1.Replace("10/28/2007", "11/9/2008");
        return str2;
        //string str = string.Empty;
        //if (inDate.Hour < 12 )
        //{
        //    str = inDate.Hour + ":" + inDate.Minute + " AM";
        //}
        //else
        //{
        //    str = (inDate.Hour-12).ToString() + ":" + inDate.Minute + " PM";
        //}
        //// this is VERY BAD $$$ need to fix.  this assumes code camp was 10/27/2007 and 28
        //if (inDate.Day == 27)
        //{
        //    return "Saturday " + str;
        //}
        //else
        //{
        //    return "Sunday " + str;
        //}
    }
}
