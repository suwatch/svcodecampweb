using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeCampSV;
using WebAPI.Code;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    public class SessionController : Controller
    {
        public ActionResult Index(string year)
        {
            var viewModel = GetViewModel(year);

            return View(viewModel);
        }

        public ActionResult IndexTest(string year)
        {
            var viewModel = GetViewModel(year);

            return View(viewModel);
        }

        private CommonViewModel GetViewModel(string year)
        {
            var codeCampYearId = CodeCampYearId(year);
            if (codeCampYearId < 0)
            {
                throw new HttpException(404, "NotFound");
            }

            var sponsors = ControllerUtils.AllSponsors(codeCampYearId);

            var jobs = ControllerUtils.JobsTop();

            List<RSSItem> feedItems = ControllerUtils.FeedItems();

            List<SessionsResult> sessions = SessionsManager.I.Get(new SessionsQuery
                                                                      {
                                                                          CodeCampYearId = codeCampYearId,
                                                                          WithInterestOrPlanToAttend = true,
                                                                          WithLectureRoom = true,
                                                                          WithSpeakers = true,
                                                                          WithTags = true,
                                                                          //Attendeesid = 1164 // nima
                                                                      });

            List<SessionTimesResult> sessionTimeResults =
                SessionTimesManager.I.Get(new SessionTimesQuery
                                              {
                                                  CodeCampYearId = codeCampYearId
                                              });

            List<TagsResult> tagsResults =
                TagsManager.I.Get(new TagsQuery
                                      {
                                          CodeCampYearId = codeCampYearId
                                      });


            var viewModel = new CommonViewModel
                                {
                                    Sessions = sessions.OrderBy(a => a.SessionSlug).ToList(),
                                    SessionsByTime = ControllerUtils.SessionTimesResultsWithSessionInfo(codeCampYearId, sessions),
                                    Sponsors = sponsors,
                                    SessionTimeResults = sessionTimeResults,
                                    TagsResults = tagsResults,
                                    JobListings = jobs,
                                    FeedItems = feedItems
                                };
            return viewModel;
        }


       


        public ActionResult Detail(string year, string session)
        {
          
            var codeCampYearId = CodeCampYearId(year);

            if (codeCampYearId < 0)
            {
                throw new HttpException(404, "NotFound");
            }

            List<SessionsResult> sessionsTemp = SessionsManager.I.Get(new SessionsQuery()
                                                      {
                                                          CodeCampYearId = codeCampYearId
                                                      });

            var sessionSlugsDict = new Dictionary<string, int>();
            foreach (SessionsResult result in sessionsTemp)
            {

                if (!sessionSlugsDict.ContainsKey(result.SessionSlug))
                {
                    sessionSlugsDict.Add(result.SessionSlug, result.Id);
                }
            }

            var sessions = new List<SessionsResult>();
            if (sessionSlugsDict.ContainsKey(session))
            {
                SessionsResult sr = sessionsTemp.FirstOrDefault(a => a.Id == sessionSlugsDict[session]);
                if (sr != null)
                {
                    sessions = SessionsManager.I.Get(new SessionsQuery
                                                         {
                                                             Id = sr.Id,
                                                             WithInterestOrPlanToAttend = true,
                                                             WithLectureRoom = true,
                                                             WithSpeakers = true,
                                                             WithTags = true,

                                                             //Attendeesid = 1164 // nima
                                                         });
                }






                //if (sessionResult != null && Request.Url != null)
                //{
                //    UpdateSpeakerPictureUrl(sessionResult);
                //}
            }
            else
            {
                throw new HttpException(404, "NotFound");
            }

           

            var viewModel = new CommonViewModel()
            {
                Sessions = sessions,
                Sponsors = ControllerUtils.AllSponsors(codeCampYearId)
            };
            return View(viewModel);
        }

        //private void UpdateSpeakerPictureUrl(SessionsResult rec)
        //{
        //    rec.SpeakerPictureUrl =
        //        String.Format(
        //            "{0}://{1}/attendeeimage/{2}.jpg", // adding stuff like ?format=gif&w=160&h=160&scale=both&mode=pad&bgcolor=white is for the client
        //            Request.IsSecureConnection ? "https" : "http",
        //            Request.Url.Authority, rec.Attendeesid);

        //    rec.SessionUrl =
        //        String.Format(
        //            "{0}://{1}/Session/{2}/{3}",
        //            Request.IsSecureConnection ? "https" : "http",
        //            Request.Url.Authority,
        //            Utils.ConvertCodeCampYearToActualYear(
        //                rec.CodeCampYearId.ToString(CultureInfo.InvariantCulture)),
        //            rec.SessionSlug);


        //}

        //private void UpdateSponsorPictureUrl(SponsorListResult rec)
        //{
        //    rec.ImageURL =
        //       String.Format(
        //           "{0}://{1}/sponsorimage/{2}.jpg", // adding stuff like ?format=gif&w=160&h=160&scale=both&mode=pad&bgcolor=white is for the client
        //           Request.IsSecureConnection ? "https" : "http",
        //           Request.Url.Authority, rec.Id);
        //}

        private static int CodeCampYearId(string year)
        {
            var codeCampYears = Utils.GetListCodeCampYear();
            var dateDict = codeCampYears.ToDictionary(k => k.CampStartDate.Year.ToString(CultureInfo.InvariantCulture),
                                                      v => v.Id);
            int codeCampYearId = -1;
            if (dateDict.ContainsKey(year))
            {
                codeCampYearId = dateDict[year];
            }
            return codeCampYearId;
        }


        ////
        //// GET: /Session/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        ////
        //// GET: /Session/Details/5

        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        ////
        //// GET: /Session/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////
        //// POST: /Session/Create

        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        ////
        //// GET: /Session/Edit/5

        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /Session/Edit/5

        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        ////
        //// GET: /Session/Delete/5

        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /Session/Delete/5

        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
