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
    public partial class SponsorRequestQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public string ContactEmail { get; set; }
        [AutoGenColumn]
        public string Company { get; set; }
        [AutoGenColumn]
        public string PhoneNumber { get; set; }
        [AutoGenColumn]
        public bool? EmailMe { get; set; }
        [AutoGenColumn]
        public bool? ContactMeByPhone { get; set; }
        [AutoGenColumn]
        public bool? AlsoAttending { get; set; }
        [AutoGenColumn]
        public bool? PastSponsor { get; set; }
        [AutoGenColumn]
        public string SponsorSpecialNotes { get; set; }
        [AutoGenColumn]
        public string SvccNotes { get; set; }
        [AutoGenColumn]
        public bool? SvccRespondedTo { get; set; }
        [AutoGenColumn]
        public bool? SvccEnteredInSystem { get; set; }
        [AutoGenColumn]
        public DateTime? CreateDate { get; set; }
    }
}
