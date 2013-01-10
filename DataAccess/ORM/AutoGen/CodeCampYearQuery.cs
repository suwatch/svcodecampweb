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
    public partial class CodeCampYearQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public string Name { get; set; }
        [AutoGenColumn]
        public DateTime? CampStartDate { get; set; }
        [AutoGenColumn]
        public DateTime? CampEndDate { get; set; }
        [AutoGenColumn]
        public bool? ReadOnly { get; set; }
        [AutoGenColumn]
        public string CodeCampDateString { get; set; }
        [AutoGenColumn]
        public string CodeCampSaturdayString { get; set; }
        [AutoGenColumn]
        public string CodeCampSundayString { get; set; }
    }
}
