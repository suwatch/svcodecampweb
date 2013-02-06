using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeCampSV;
using WebAPI.Code;
using WebAPI.ViewModels;

namespace WebAPI.Repositories
{
    public class SessionTest : IRepositorySession
    {
        public CommonViewModel GetDataForYear(string year)
        {
            return ControllerUtils.CommonViewModelTestData();
        }

        public CommonViewModel Detail(string year, string session)
        {
           var commonViewModel = ControllerUtils.CommonViewModelTestData();
            return ControllerUtils.GetCommonViewModelOneSession(session, commonViewModel);
        }
    }

  
}

