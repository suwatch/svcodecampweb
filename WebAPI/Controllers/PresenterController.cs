using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeCampSV;
using WebAPI.Code;
using WebAPI.Repositories;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    public class PresenterController : Controller
    {
        private readonly IRepositoryPresenter _repositoryPresenter;

        public PresenterController(IRepositoryPresenter repository)
        {
            _repositoryPresenter = repository;
        }

        public PresenterController()
        {
        }

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
            return View(_repositoryPresenter.Detail(year, speakername));
        }

        private ActionResult IndexBase(string year)
        {
            return View(_repositoryPresenter.GetDataForYear(year));
        }

      
    }
}
