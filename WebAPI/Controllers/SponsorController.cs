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
    public class SponsorController : Controller
    {
        public ActionResult Index(string year)
        {
            return IndexBase(year);
        }

        public ActionResult IndexTest(string year)
        {
            return IndexBase(year);
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
            }
            return View(commonViewModel);
        }
    }
}
