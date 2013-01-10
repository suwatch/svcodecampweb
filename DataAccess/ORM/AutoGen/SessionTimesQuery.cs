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
    public partial class SessionTimesQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public DateTime? StartTime { get; set; }
        [AutoGenColumn]
        public string StartTimeFriendly { get; set; }
        [AutoGenColumn]
        public DateTime? EndTime { get; set; }
        [AutoGenColumn]
        public string EndTimeFriendly { get; set; }
        [AutoGenColumn]
        public int? SessionMinutes { get; set; }
        [AutoGenColumn]
        public string Description { get; set; }
        [AutoGenColumn]
        public string TitleBeforeOnAgenda { get; set; }
        [AutoGenColumn]
        public string TitleAfterOnAgenda { get; set; }
        [AutoGenColumn]
        public int? CodeCampYearId { get; set; }
    }
}
