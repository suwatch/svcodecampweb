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
    public partial class ReferringUrlGroupQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public string ReferringUrlName { get; set; }
        [AutoGenColumn]
        public int? AttendeesId { get; set; }
        [AutoGenColumn]
        public string ArticleName { get; set; }
        [AutoGenColumn]
        public string UserGroup { get; set; }
        [AutoGenColumn]
        public int? DeletedCount { get; set; }
        [AutoGenColumn]
        public bool? Visible { get; set; }
    }
}
