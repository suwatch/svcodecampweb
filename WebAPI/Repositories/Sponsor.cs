using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Code;
using WebAPI.ViewModels;

namespace WebAPI.Repositories
{
    public class Sponsor : IRepositorySponsor
    {
        public CommonViewModel GetDataForYear(string year)
        {
            return ControllerUtils.UpdateViewModel
                (new CommonViewModel(), ControllerUtils.GetCodeCampYearId(year));
        }
    }

    public interface IRepositorySponsor
    {
        CommonViewModel GetDataForYear(string year);
    }
}

