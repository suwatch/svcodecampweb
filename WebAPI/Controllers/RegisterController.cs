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
            string year =
                Utils.ConvertCodeCampYearToActualYear(
                    Utils.GetCurrentCodeCampYear().ToString(CultureInfo.InvariantCulture));
            var viewModel = GetViewModel(year);

            viewModel.LoggedInUsername = User.Identity.IsAuthenticated ? User.Identity.Name : "NONE";

            return View(viewModel);
        }

        public ActionResult IndexTest()
        {
            string year = Utils.ConvertCodeCampYearToActualYear
                (Utils.GetCurrentCodeCampYear().ToString(CultureInfo.InvariantCulture));
            var viewModel = GetViewModel(year);
            return View(viewModel);
        }

        public ActionResult Unsubscribe()
        {
            string year = Utils.ConvertCodeCampYearToActualYear
                (Utils.GetCurrentCodeCampYear().ToString(CultureInfo.InvariantCulture));
            var viewModel = GetViewModel(year);
            return View(viewModel);
        }




        private CommonViewModel GetViewModel(string year)
        {
            return ControllerUtils.UpdateViewModel
                (new CommonViewModel(), ControllerUtils.GetCodeCampYearId(year));
        }

      

    }
}
