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
    public partial class Log4NetAllQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public DateTime? Date { get; set; }
        [AutoGenColumn]
        public string Thread { get; set; }
        [AutoGenColumn]
        public string Level { get; set; }
        [AutoGenColumn]
        public string Logger { get; set; }
        [AutoGenColumn]
        public string Message { get; set; }
        [AutoGenColumn]
        public string ExceptionMessage { get; set; }
        [AutoGenColumn]
        public string ExceptionStackTrace { get; set; }
        [AutoGenColumn]
        public string UserName { get; set; }
        [AutoGenColumn]
        public int? EllapsedTime { get; set; }
        [AutoGenColumn]
        public string MessageLine1 { get; set; }
        [AutoGenColumn]
        public string MessageLine2 { get; set; }
    }
}
