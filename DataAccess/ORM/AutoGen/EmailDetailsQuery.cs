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
    public partial class EmailDetailsQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public int? AttendeesId { get; set; }
        [AutoGenColumn]
        public int? EmailDetailsTopicId { get; set; }
        [AutoGenColumn]
        public Guid? EmailDetailsGuid { get; set; }
        [AutoGenColumn]
        public int? EmailReadCount { get; set; }
        [AutoGenColumn]
        public DateTime? EmailReadDate { get; set; }
        [AutoGenColumn]
        public string MessageUniqueId { get; set; }
        [AutoGenColumn]
        public string EmailSendStatus { get; set; }
        [AutoGenColumn]
        public DateTime? EmailSendStartTime { get; set; }
        [AutoGenColumn]
        public DateTime? EmailSendFinishTime { get; set; }
        [AutoGenColumn]
        public string EmailSendLogMessage { get; set; }
        [AutoGenColumn]
        public string Subject { get; set; }
        [AutoGenColumn]
        public string BodyText { get; set; }
        [AutoGenColumn]
        public DateTime? SentDateTime { get; set; }
        [AutoGenColumn]
        public string EmailFrom { get; set; }
        [AutoGenColumn]
        public string EmailTo { get; set; }
    }
}
