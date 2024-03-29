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
    public partial class SessionAttendeeQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public int? Sessions_id { get; set; }
        [AutoGenColumn]
        public Guid? Attendees_username { get; set; }
        [AutoGenColumn]
        public int? Interestlevel { get; set; }
        [AutoGenColumn]
        public DateTime? LastUpdatedDate { get; set; }
        [AutoGenColumn]
        public string UpdateByProgram { get; set; }
    }
}
