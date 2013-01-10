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
    public partial class TrackQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public int? CodeCampYearId { get; set; }
        [AutoGenColumn]
        public int? OwnerAttendeeId { get; set; }
        [AutoGenColumn]
        public string Named { get; set; }
        [AutoGenColumn]
        public string Description { get; set; }
        [AutoGenColumn]
        public bool? Visible { get; set; }
        [AutoGenColumn]
        public string AlternateURL { get; set; }
        [AutoGenColumn]
        public System.Data.Linq.Binary TrackImage { get; set; }
        [AutoGenColumn]
        public DateTime? CreationDate { get; set; }
        [AutoGenColumn]
        public DateTime? ModifiedDate { get; set; }
    }
}
