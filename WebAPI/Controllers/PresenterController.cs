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
            return IndexBase(year);
        }

       

        public ActionResult IndexTest(string year)
        {
            return IndexBase(year);
        }

        public ActionResult Detail(string year, string speakername)
        {
            CommonViewModel commonViewModel;
            if (ControllerUtils.IsTestMode)
            {
                commonViewModel = ControllerUtils.CommonViewModelTestData();

                var parts = speakername.Split(new[] { '-' }).ToList();
                if (parts.Count == 3)
                {
                    int attendeeId;
                    if (Int32.TryParse(parts[2], out attendeeId))
                    {
                        commonViewModel.Speakers =
                            commonViewModel.Speakers.Where(a => a.AttendeeId == attendeeId).ToList();
                    }
                }
            }
            else
            {
                int codeCampYearId;
                commonViewModel = ControllerUtils.UpdateViewModel
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
            }
            return View(commonViewModel);
        }

        private ActionResult IndexBase(string year)
        {
            CommonViewModel commonViewModel;
            if (ControllerUtils.IsTestMode)
            {
                commonViewModel = ControllerUtils.CommonViewModelTestData();
            }
            else
            {
                int codeCampYearId;
                commonViewModel = ControllerUtils.UpdateViewModel
                    (new CommonViewModel(), ControllerUtils.GetCodeCampYearId(year), out codeCampYearId);

                UpdateCommonViewWithPresenters(commonViewModel, codeCampYearId);
            }
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


    }
}
