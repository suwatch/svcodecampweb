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
            int codeCampYearId;
            CommonViewModel commonViewModel = ControllerUtils.UpdateViewModel
                 (new CommonViewModel(), ControllerUtils.GetCodeCampYearId(year), out codeCampYearId);

            UpdateCommonViewWithPresenters(commonViewModel, codeCampYearId);

            return View(commonViewModel);
        }

        public ActionResult IndexTest(string year)
        {
            int codeCampYearId;
            CommonViewModel commonViewModel = ControllerUtils.UpdateViewModel
                 (new CommonViewModel(), ControllerUtils.GetCodeCampYearId(year),out codeCampYearId);

            UpdateCommonViewWithPresenters(commonViewModel, codeCampYearId);

            return View(commonViewModel);
        }

        private void UpdateCommonViewWithPresenters(CommonViewModel commonViewModel,int codeCampYearId)
        {
            commonViewModel.Speakers =
              AttendeesManager.I.GetSpeakerResults(new AttendeesQuery()
              {
                  CodeCampYearId = codeCampYearId,
                  PresentersOnly = true,
                  IncludeSessions = true
              });
        }

        public ActionResult Detail(string year,string speakername)
        {
            int codeCampYearId;
            CommonViewModel commonViewModel = ControllerUtils.UpdateViewModel
                (new CommonViewModel(), ControllerUtils.GetCodeCampYearId(year), out codeCampYearId);

            commonViewModel.Speakers =
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
            return View(commonViewModel);
        }

       



        //private static int CodeCampYearId(string year)
        //{
        //    var codeCampYears = Utils.GetListCodeCampYear();
        //    var dateDict = codeCampYears.ToDictionary(k => k.CampStartDate.Year.ToString(CultureInfo.InvariantCulture),
        //                                              v => v.Id);
        //    int codeCampYearId = -1;
        //    if (dateDict.ContainsKey(year))
        //    {
        //        codeCampYearId = dateDict[year];
        //    }
        //    return codeCampYearId;
        //}


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
