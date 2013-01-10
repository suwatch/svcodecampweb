//   Regenerated Code
//   C 3PLogic, Inc.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlTypes;
using System.ComponentModel;

namespace CodeCampSV
{
    [Serializable]
    public partial class SponsorListJobListingQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public int? SponsorListId { get; set; }
        [AutoGenColumn]
        public string JobName { get; set; }
        [AutoGenColumn]
        public string JobLocation { get; set; }
        [AutoGenColumn]
        public string JobURL { get; set; }
        [AutoGenColumn]
        public string JobBrief { get; set; }
        [AutoGenColumn]
        public string JobTagline { get; set; }
        [AutoGenColumn]
        public string JobButtonToolTip { get; set; }
        [AutoGenColumn]
        public DateTime? EnteredDate { get; set; }
        [AutoGenColumn]
        public string JobCompanyName { get; set; }
        [AutoGenColumn]
        public DateTime? StartRunDate { get; set; }
        [AutoGenColumn]
        public DateTime? EndRunDate { get; set; }
        [AutoGenColumn]
        public bool? HideListing { get; set; }
        [AutoGenColumn]
        public string Notes { get; set; }
    }
}
