using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeCampSV;
using WebAPI.Code;
using WebAPI.DependencyResolution;
using WebAPI.Repositories;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    public class SponsorController : Controller
    {
        private readonly IRepositorySponsor _repositorySponsor;

        public SponsorController(IRepositorySponsor repository)
        {
            _repositorySponsor = repository;
        }

        public ActionResult Index(string year)
        {
            return IndexBase(year);
        }

        public ActionResult IndexTest(string year)
        {
            return IndexBase(year);
        }

        public ActionResult IndexTest1(string year)
        {
            return IndexBase(year);
        }
       
        private ActionResult IndexBase(string year)
        {
            var data = _repositorySponsor.GetDataForYear(year);

            for (int index = 0; index < data.Sponsors.Count; index++)
            {
                var rec = data.Sponsors[index];
                rec.ShowFeatured = index%2 == 0;
                rec.ShowQuestionMark = index%3 == 0;
            }

           
            data.Sponsors = _repositorySponsor.GetDataForYear(year).Sponsors.
                OrderBy(sp => sp.SponsorSupportLevelOrder).
                ThenBy(a=>a.SponsorName).
                ToList();

            // move community sponsors to end
            foreach (var rec in data.Sponsors)
            {
                if (rec.SponsorSupportLevelOrder == 0)
                {
                    rec.SponsorSupportLevelOrder = 99; // move community to bottom
                }
            }

            return View(data);
        }
    }

   
}
