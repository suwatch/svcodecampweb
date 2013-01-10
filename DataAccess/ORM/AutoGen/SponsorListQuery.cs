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
    public partial class SponsorListQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public string SponsorName { get; set; }
        [AutoGenColumn]
        public DateTime? NextActionDate { get; set; }
        [AutoGenColumn]
        public string ImageURL { get; set; }
        [AutoGenColumn]
        public string NavigateURL { get; set; }
        [AutoGenColumn]
        public string HoverOverText { get; set; }
        [AutoGenColumn]
        public string UnderLogoText { get; set; }
        [AutoGenColumn]
        public string Comment { get; set; }
        [AutoGenColumn]
        public string CompanyDescriptionShort { get; set; }
        [AutoGenColumn]
        public string CompanyDescriptionLong { get; set; }
        [AutoGenColumn]
        public string CompanyAddressLine1 { get; set; }
        [AutoGenColumn]
        public string CompanyAddressLine2 { get; set; }
        [AutoGenColumn]
        public string CompanyCity { get; set; }
        [AutoGenColumn]
        public string CompanyState { get; set; }
        [AutoGenColumn]
        public string CompanyZip { get; set; }
        [AutoGenColumn]
        public string CompanyPhoneNumber { get; set; }
        [AutoGenColumn]
        public System.Data.Linq.Binary CompanyImage { get; set; }
        [AutoGenColumn]
        public DateTime? CreationDate { get; set; }
        [AutoGenColumn]
        public DateTime? ModifiedDate { get; set; }
        [AutoGenColumn]
        public string CompanyImageType { get; set; }
    }
}
