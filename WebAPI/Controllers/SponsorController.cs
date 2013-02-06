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
       
        private ActionResult IndexBase(string year)
        {
            return View(_repositorySponsor.GetDataForYear(year));
        }
    }

   
}
