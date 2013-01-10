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
    public partial class LectureRoomsQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public string Number { get; set; }
        [AutoGenColumn]
        public string Description { get; set; }
        [AutoGenColumn]
        public string Style { get; set; }
        [AutoGenColumn]
        public int? Capacity { get; set; }
        [AutoGenColumn]
        public bool? Projector { get; set; }
        [AutoGenColumn]
        public bool? Screen { get; set; }
        [AutoGenColumn]
        public System.Data.Linq.Binary Picture { get; set; }
        [AutoGenColumn]
        public bool? Available { get; set; }
    }
}
