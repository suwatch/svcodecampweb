using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CodeCampSV;

public partial class StatsRep : BaseContentPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int numberEvals = CodeCampSV.Utils.GetNumberEvals();
        LabelEvals.Text = numberEvals.ToString();

        int numberCCEvals = CodeCampSV.Utils.GetNumberCodeCampEvals();
        LabelCodeCampEvals.Text = numberCCEvals.ToString();

        //int picturesAssigned = CodeCampSV.Utils.GetNumberPicturesAssigned();
        //LabelPicturesAssigned.Text = picturesAssigned.ToString();

        var numberRegistered = Utils.GetNumberRegistered();
        int numberRegisteredWithViewPermission = Utils.GetNumberRegisteredWithViewPermission();
        int numberPresentations = Utils.GetNumberSessions();
        var str = string.Format("Registered: {0} Presentations: {1} Number Registered With Permission: {2}", 
            numberRegistered, numberPresentations, numberRegisteredWithViewPermission);
        Label1.Text = str;

        int numberAttendingSaturday = Utils.GetNumberAttendeesByParam("SaturdayClasses");
        LabelSaturdayClasses.Text = numberAttendingSaturday.ToString();

        int numberAttendingSunday = Utils.GetNumberAttendeesByParam("SundayClasses");
        LabelSundayClasses.Text = numberAttendingSunday.ToString();

       
        int numberSessionsInterest = CodeCampSV.Utils.GetNumberAttendeesByParam("SessionsInterest");
        LabelSessionsInterest.Text = numberSessionsInterest.ToString();

         int numberSessionsPlanAttend = CodeCampSV.Utils.GetNumberAttendeesByParam("SessionsPlanAttend");
        LabelSessionsPlanAttend.Text = numberSessionsPlanAttend.ToString();



        int numberAttendingBothDays = Utils.GetNumberAttendeesAttendingBothDays();
        LabelAttendingBothDays.Text = numberAttendingBothDays.ToString();

      

        int loginsLastDays = Utils.RetrieveAttendeeActivityLastDays(7);
        LabelLast7DaysActivity.Text = loginsLastDays.ToString();

        LabelSendSpeakerInterested.Text = Utils.GetNumberSpeakerCanEmailInterested().ToString();
        LabelSendSpeakerPlanToAttend.Text = Utils.GetNumberSpeakerCanEmailPlanToAttend().ToString();

        //int saturdayDinner = Utils.RetrieveSaturdayDinnerCount();
        //LabelBarbequeAttend.Text = saturdayDinner.ToString();



        if (CodeCampSV.Utils.CheckUserIsAdmin())
        {
            GridView3.Visible = true;
        }
        else
        {
            GridView3.Visible = false;
        }


    }
    protected void ButtonTemp_Click(object sender, EventArgs e)
    {
        //Dictionary<int,int>  emailFromSpeakerPlanToAttendDict;
        //Dictionary<int, int> emailFromSpeakerInterestedDict;
        //Utils.GetDictionariesOfAllowEmailFromSpeaker(Utils.GetCurrentCodeCampYear(), out emailFromSpeakerInterestedDict,
        //                                             out emailFromSpeakerPlanToAttendDict);



    }
}
