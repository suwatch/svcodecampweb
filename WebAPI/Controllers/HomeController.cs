using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeCampSV;
using WebAPI.Code;
using WebAPI.ViewModels;
using System.Web.Security;

namespace WebAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string year =
               Utils.ConvertCodeCampYearToActualYear(
                   Utils.GetCurrentCodeCampYear().ToString(CultureInfo.InvariantCulture));
            var viewModel = GetViewModel(year);
            return View(viewModel);
        }

        public ActionResult Login()
        {
            string year =
               Utils.ConvertCodeCampYearToActualYear(
                   Utils.GetCurrentCodeCampYear().ToString(CultureInfo.InvariantCulture));
            var viewModel = GetViewModel(year);
            return View(viewModel);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            string year =
               Utils.ConvertCodeCampYearToActualYear(
                   Utils.GetCurrentCodeCampYear().ToString(CultureInfo.InvariantCulture));
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
