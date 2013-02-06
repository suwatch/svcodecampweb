using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using CodeCampSV;
using WebAPI.Code;
using WebAPI.Repositories;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    public class SessionController : Controller
    {
         private readonly IRepositorySession _repositorySession;

        public SessionController(IRepositorySession repository)
        {
            _repositorySession = repository;
        }

        public SessionController()
        {
        }

        public ActionResult Index(string year)
        {
            return IndexReturn(year);
        }
       
        private ActionResult IndexReturn(string year)
        {
            return View(_repositorySession.GetDataForYear(year));
        }

        public ActionResult Detail(string year, string session)
        {
            return View(_repositorySession.Detail(year, session));
        }

        public ActionResult IndexTest(string year)
        {
            return IndexReturn(year);
        }

        public ActionResult IndexTest1(string year)
        {
            return IndexReturn(year);
        }

        public ActionResult IndexTest2(string year)
        {
            return IndexReturn(year);
        }


    }
}
