using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class VolunteerForJob : System.Web.UI.Page
{
    private List<VolunteerJobResult> _volunteerJobResultList;

    private Dictionary<int,int> _attendeeVolunteerCntByJobDict;


    protected void Page_PreRender(object sender,EventArgs e)
    {
       
            UpdateTopCounts();
        

    }

    protected void Page_Load(object sender, EventArgs e)
    {



        // these need to happen on every request, not just on first load
        InitListAndDictionary();

        if (Utils.CheckUserIsVolunteerCoordinator() || Utils.CheckUserIsAdmin())
        {
            LabelCodeCampYearId.Text = Utils.GetCurrentCodeCampYear().ToString();
        }
        else
        {
            LabelCodeCampYearId.Text = Utils.CurrentCodeCampYear.ToString();
        }

        // Verify person has checked volunteer box and has phone number
        bool showVolunteerJobs = false;

        if (Context.User.Identity.IsAuthenticated)
        {
         

            int attendeeId = Utils.GetAttendeesIdFromUsername(Context.User.Identity.Name);
            LabelMyAttendeeId.Text = attendeeId.ToString();
            int currentCodeCampYearId = Utils.GetCurrentCodeCampYear();
           
            var attendeesCodeCampYearResultRec =
                AttendeesCodeCampYearManager.I.Get(new AttendeesCodeCampYearQuery
                {
                    AttendeesId = attendeeId,
                    CodeCampYearId =
                        currentCodeCampYearId
                }).FirstOrDefault();

            if (attendeesCodeCampYearResultRec != null && attendeesCodeCampYearResultRec.Volunteer != null && attendeesCodeCampYearResultRec.Volunteer.Value)
            {
                showVolunteerJobs = true;
            }

        }
        // no else necessary because you should not get here if you are not authenticated
       
       

        const string problemMessage = "To Volunteer, you must go to the Registration page (bottom) and check that you will volunteer as well as put in your phone number.";
        if (showVolunteerJobs || Utils.CheckUserIsAdmin() || Utils.CheckUserIsVolunteerCoordinator())
        {
            GridView1.Visible = true;
            GridViewMyJobs.Visible = true;
            LabelProblemText.Text = string.Empty;
            Div1.Visible = true;
            Div2.Visible = true;
            
        }
        else
        {
            // turn everything off!
            GridView1.Visible = false;
            GridViewMyJobs.Visible = false;
            LabelProblemText.Text = problemMessage;
            Div1.Visible = false;
            Div2.Visible = false;
        }





    }

    private void UpdateTopCounts()
    {
        int numberVolunteered = Utils.GetNumberVolunteeredThisYear();
        int numberNeeded = Utils.GetNumberVolunteersNeededYear();

        VolunteerCountsId.Text = String.Format("We Still Need {0} More Volunteers!",
                                                numberNeeded - numberVolunteered);
    } 
    private void InitListAndDictionary()
    {
        int currentCodeCampYear = Utils.GetCurrentCodeCampYear();
        _volunteerJobResultList = VolunteerJobManager.I.Get(new VolunteerJobQuery()
                                                                {
                                                                    CodeCampYearId = currentCodeCampYear
                                                                });

        _attendeeVolunteerCntByJobDict = Utils.GetVolunteersNeededCounts(currentCodeCampYear);
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        

        // only showing if auth user.
        int volunteerJobId = Convert.ToInt32(e.CommandArgument.ToString());
        int attendeeId = Convert.ToInt32(LabelMyAttendeeId.Text);

        // make sure person has not volunteered for this job already
        var rec = AttendeeVolunteerManager.I.Get(new AttendeeVolunteerQuery
                                                     {
                                                         AttendeeId = Convert.ToInt32(LabelMyAttendeeId.Text),
                                                         VolunteerJobId = volunteerJobId
                                                     }).FirstOrDefault();

        if (rec == null)
        {
            AttendeeVolunteerManager.I.Insert(new AttendeeVolunteerResult
                                                  {
                                                      AttendeeId = Convert.ToInt32(LabelMyAttendeeId.Text),
                                                      VolunteerJobId = volunteerJobId,
                                                      CreatedDate = DateTime.Now,
                                                      LastUpdated = DateTime.Now,
                                                      Notes = string.Empty
                                                  });
        }

        InitListAndDictionary();
        GridViewMyJobs.DataBind();
        GridView1.DataBind();




    }
    protected void GridViewMyJobs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      

        int attendeeVolunteerId = Convert.ToInt32(e.CommandArgument.ToString());
        AttendeeVolunteerManager.I.Delete(attendeeVolunteerId);

        InitListAndDictionary();
        GridViewMyJobs.DataBind();
        GridView1.DataBind();
    }

    protected string GetNumberAlreadyVolunteeredByJob(int jobId)
    {
        int recs = 0;
        if (_attendeeVolunteerCntByJobDict.ContainsKey(jobId))
        {
            recs = _attendeeVolunteerCntByJobDict[jobId];
        }

        //int recs = AttendeeVolunteerManager.I.Get(new AttendeeVolunteerQuery
        //{
        //    VolunteerJobId = jobId
        //}).Count;

        return recs.ToString(CultureInfo.InvariantCulture);

        //return String.Format("(Volunteers Assigned: {0})", recs);
    }

    protected string GetTextForVolunteerForThisButton(int jobId)
    {
        VolunteerJobResult volunteerJobResult = _volunteerJobResultList.Where(a => a.Id == jobId).FirstOrDefault();
            //VolunteerJobManager.I.Get(new VolunteerJobQuery
            //    {
            //        Id = jobId
            //    }).FirstOrDefault();

        int signedUpCount = 0;
        if (_attendeeVolunteerCntByJobDict.ContainsKey(jobId))
        {
            signedUpCount = _attendeeVolunteerCntByJobDict[jobId];
        }

        //(AttendeeVolunteerManager.I.Get(new AttendeeVolunteerQuery()
            //{
            //    VolunteerJobId = jobId
            //})).Count;

        string message = string.Empty;
        if (volunteerJobResult != null)
        {
            message = signedUpCount < volunteerJobResult.NumberNeeded ? "Volunteer For Job" : "---Job Full";
        }
        return message;
    }

    protected bool GetEnabledForVolunteerForThisButton(int jobId)
    {
        var volunteerJobResult = _volunteerJobResultList.Where(a => a.Id == jobId).FirstOrDefault();
          //VolunteerJobManager.I.Get(new VolunteerJobQuery
          //{
          //    Id = jobId
          //}).FirstOrDefault();

        int signedUpCount = 0;
        if (_attendeeVolunteerCntByJobDict.ContainsKey(jobId))
        {
            signedUpCount = _attendeeVolunteerCntByJobDict[jobId];
        }
            //(AttendeeVolunteerManager.I.Get(new AttendeeVolunteerQuery()
            //    {
            //        VolunteerJobId = jobId
            //    })).Count;

        return volunteerJobResult != null && signedUpCount < volunteerJobResult.NumberNeeded;

    }
}