using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Code;
using WebAPI.ViewModels;

namespace WebAPI.Repositories
{
    public class SponsorTest : IRepositorySponsor
    {
        public CommonViewModel GetDataForYear(string year)
        {
           return ControllerUtils.CommonViewModelTestData();
        }
    }

   
}

