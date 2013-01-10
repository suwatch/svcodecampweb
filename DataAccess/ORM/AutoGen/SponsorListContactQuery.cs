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
    public partial class SponsorListContactQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public int? SponsorListId { get; set; }
        [AutoGenColumn]
        public int? SponsorListContactTypeId { get; set; }
        [AutoGenColumn]
        public string ContactType { get; set; }
        [AutoGenColumn]
        public string EmailAddress { get; set; }
        [AutoGenColumn]
        public string FirstName { get; set; }
        [AutoGenColumn]
        public string LastName { get; set; }
        [AutoGenColumn]
        public string PhoneNumberOffice { get; set; }
        [AutoGenColumn]
        public string PhoneNumberCell { get; set; }
        [AutoGenColumn]
        public string Comment { get; set; }
        [AutoGenColumn]
        public DateTime? DateCreated { get; set; }
        [AutoGenColumn]
        public bool? GeneralCCMailings { get; set; }
        [AutoGenColumn]
        public bool? SponsorCCMailings { get; set; }
    }
}
