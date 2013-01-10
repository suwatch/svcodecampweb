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
    public partial class SessionsJobListingQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public int? SessionId { get; set; }
        [AutoGenColumn]
        public int? JobListingId { get; set; }
        [AutoGenColumn]
        public bool? ShowImageOnSession { get; set; }
        [AutoGenColumn]
        public bool? ShowTextOnSession { get; set; }
        [AutoGenColumn]
        public DateTime? DateCreated { get; set; }
        [AutoGenColumn]
        public DateTime? DateUpdate { get; set; }
    }
}
