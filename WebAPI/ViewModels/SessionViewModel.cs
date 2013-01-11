using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeCampSV;

namespace WebAPI.ViewModels
{
    public class SessionViewModel
    {
        public string DaysUntilCodeCampString { get; set; }
        public List<SessionsResult> Sessions { get; set; }
        public List<SponsorListResult> Sponsors { get; set; }

        public List<SessionTimesResult> SessionsByTime { get; set; }

    }
}