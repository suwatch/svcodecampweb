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
    public partial class SponsorListCodeCampYearQuery : QueryBase
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
        public int? SponsorListId { get; set; }
        [AutoGenColumn]
        public Double? DonationAmount { get; set; }
        [AutoGenColumn]
        public int? SortIndex { get; set; }
        [AutoGenColumn]
        public bool? Visible { get; set; }
        [AutoGenColumn]
        public string Status { get; set; }
        [AutoGenColumn]
        public DateTime? NextActionDate { get; set; }
        [AutoGenColumn]
        public string WhoOwns { get; set; }
        [AutoGenColumn]
        public bool? TableRequired { get; set; }
        [AutoGenColumn]
        public bool? AttendeeBagItem { get; set; }
        [AutoGenColumn]
        public bool? ItemSentToUPS { get; set; }
        [AutoGenColumn]
        public bool? ItemSentToFoothill { get; set; }
        [AutoGenColumn]
        public string Comments { get; set; }
        [AutoGenColumn]
        public string ItemsShippingDescription { get; set; }
        [AutoGenColumn]
        public string NoteFromCodeCamp { get; set; }
    }
}
