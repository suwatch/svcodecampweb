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
    public partial class EmailDetailsTopicQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public DateTime? CreateDate { get; set; }
        [AutoGenColumn]
        public string Title { get; set; }
        [AutoGenColumn]
        public DateTime? FirstRunDate { get; set; }
        [AutoGenColumn]
        public string Notes { get; set; }
        [AutoGenColumn]
        public string EmailSubject { get; set; }
        [AutoGenColumn]
        public string EmailMime { get; set; }
    }
}