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
    public class PresenterController : Controller
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

        public ActionResult Detail(string year,string speakername)
        {
            var viewModel = GetSpeakerDetail(year,speakername);

            // always need to populate common things
            viewModel.Sponsors = ControllerUtils.AllSponsors(Utils.GetCurrentCodeCampYear());

            return View(viewModel);
        }

        private CommonViewModel GetSpeakerDetail(string year,string speakername)
        {
            //var codeCampYearId = Utils.GetCurrentCodeCampYear();

            var commonViewModel = new CommonViewModel();
            List<SpeakerResult> speakers =
                AttendeesManager.I.GetSpeakerResults(new AttendeesQuery
                                                         {
                                                             PresentersOnly = true,
                                                             IncludeSessions = true,
                                                             SpeakerNameWithId = speakername,
                                                             CodeCampYearId =
                                                                 Utils
                                                                 .ConvertCodeCampYearToCodeCampYearId
                                                                 (year)
                                                         });
            commonViewModel.Speakers = speakers;
            return commonViewModel;
        }

        private CommonViewModel GetViewModel(string year)
        {
            var codeCampYearId = CodeCampYearId(year);
            if (codeCampYearId < 0)
            {
                throw new HttpException(404, "NotFound");
            }

            List<SpeakerResult> speakers = AttendeesManager.I.GetSpeakerResults(new AttendeesQuery()
                                                                  {
                                                                      CodeCampYearId = codeCampYearId,
                                                                      PresentersOnly = true,
                                                                      IncludeSessions = true
                                                                  });



          


            var viewModel = new CommonViewModel()
                                {
                                    Speakers = speakers,
                                    Sponsors = ControllerUtils.AllSponsors(codeCampYearId)
                                };
            return viewModel;
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
