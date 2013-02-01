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
    public class RegisterController : Controller
    {
        public ActionResult Index()
        {
            string year = Utils.ConvertCodeCampYearToActualYear(Utils.GetCurrentCodeCampYear().ToString());

            var viewModel = GetViewModel(year);

            return View(viewModel);
        }

        public ActionResult IndexTest(string year)
        {
            return View();
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

    }
}
