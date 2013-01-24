using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeCampSV;
using WebAPI.ViewModels;

namespace WebAPI.Controllers.SpeakerController
{
    public class SpeakerController : Controller
    {
        public ActionResult Index(string year)
        {
            var viewModel = GetSessionViewModel(year);

            return View(viewModel);
        }

        public ActionResult IndexTest(string year)
        {
            var viewModel = GetSessionViewModel(year);

            return View(viewModel);
        }

        private CommonViewModel GetSessionViewModel(string year)
        {
            var codeCampYearId = CodeCampYearId(year);
            if (codeCampYearId < 0)
            {
                throw new HttpException(404, "NotFound");
            }

            //List<SessionsResult> sessions = SessionsManager.I.Get(new SessionsQuery
            //                                                          {
            //                                                              CodeCampYearId = codeCampYearId,
            //                                                              WithInterestOrPlanToAttend = true,
            //                                                              WithLectureRoom = true,
            //                                                              WithSpeakers = true,
            //                                                              WithTags = true


            //                                                              //Attendeesid = 1164 // nima
            //                                                          });

            List<AttendeesResult> speakers = AttendeesManager.I.Get(new AttendeesQuery()
                                                                  {
                                                                      CodeCampYearId = codeCampYearId,
                                                                      PresentersOnly = true,
                                                                      IncludeSessions = true
                                                                  });

            List<SponsorListResult> sponsors =
                SponsorListManager.I.Get(new SponsorListQuery
                                             {
                                                 CodeCampYearId = codeCampYearId,
                                                 IncludeSponsorLevel = true,
                                                 PlatinumLevel = Utils.MinSponsorLevelPlatinum,
                                                 GoldLevel = Utils.MinSponsorLevelGold,
                                                 SilverLevel = Utils.MinSponsorLevelSilver,
                                                 BronzeLevel = Utils.MinSponsorLevelBronze
                                             });


            foreach (var rec in sponsors)
            {
                UpdateSponsorPictureUrl(rec);
            }

           
           

            var viewModel = new CommonViewModel()
                                {
                                    Speakers = speakers,
                                    Sponsors = sponsors
                                };
            return viewModel;
        }


        public ActionResult Detail(string year, string session)
        {
            SessionsResult sessionResult;

            var codeCampYearId = CodeCampYearId(year);

            if (codeCampYearId < 0)
            {
                throw new HttpException(404, "NotFound");
            }

            List<SessionsResult> sessions = SessionsManager.I.Get(new SessionsQuery()
                                                                      {
                                                                          CodeCampYearId = codeCampYearId
                                                                      });

            var sessionSlugsDict = new Dictionary<string, int>();
            foreach (SessionsResult result in sessions)
            {

                if (!sessionSlugsDict.ContainsKey(result.SessionSlug))
                {
                    sessionSlugsDict.Add(result.SessionSlug, result.Id);
                }
            }

            if (sessionSlugsDict.ContainsKey(session))
            {
                sessionResult = sessions.FirstOrDefault(a => a.Id == sessionSlugsDict[session]);
                if (sessionResult != null && Request.Url != null)
                {
                    UpdateSpeakerPictureUrl(sessionResult);
                }
            }
            else
            {
                throw new HttpException(404, "NotFound");
            }

            return View(sessionResult);
        }

        private void UpdateSpeakerPictureUrl(SessionsResult rec)
        {
            rec.SpeakerPictureUrl =
                String.Format(
                    "{0}://{1}/attendeeimage/{2}.jpg",
                    // adding stuff like ?format=gif&w=160&h=160&scale=both&mode=pad&bgcolor=white is for the client
                    Request.IsSecureConnection ? "https" : "http",
                    Request.Url.Authority, rec.Attendeesid);

            rec.SessionUrl =
                String.Format(
                    "{0}://{1}/Session/{2}/{3}",
                    Request.IsSecureConnection ? "https" : "http",
                    Request.Url.Authority,
                    Utils.ConvertCodeCampYearToActualYear(
                        rec.CodeCampYearId.ToString(CultureInfo.InvariantCulture)),
                    rec.SessionSlug);


        }

        private void UpdateSponsorPictureUrl(SponsorListResult rec)
        {
            rec.ImageURL =
                String.Format(
                    "{0}://{1}/sponsorimage/{2}.jpg",
                    // adding stuff like ?format=gif&w=160&h=160&scale=both&mode=pad&bgcolor=white is for the client
                    Request.IsSecureConnection ? "https" : "http",
                    Request.Url.Authority, rec.Id);
        }

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
