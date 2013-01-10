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
    public partial class ZIPCODEWORLDGOLDQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public string ZIP_CODE { get; set; }
        [AutoGenColumn]
        public string CITY { get; set; }
        [AutoGenColumn]
        public string STATE { get; set; }
        [AutoGenColumn]
        public string AREA_CODE { get; set; }
        [AutoGenColumn]
        public string CITY_ALIAS_NAME { get; set; }
        [AutoGenColumn]
        public string CITY_ALIAS_ABBR { get; set; }
        [AutoGenColumn]
        public string CITY_TYPE { get; set; }
        [AutoGenColumn]
        public string COUNTY_NAME { get; set; }
        [AutoGenColumn]
        public string STATE_FIPS { get; set; }
        [AutoGenColumn]
        public string COUNTY_FIPS { get; set; }
        [AutoGenColumn]
        public string TIME_ZONE { get; set; }
        [AutoGenColumn]
        public string DAY_LIGHT_SAVING { get; set; }
        [AutoGenColumn]
        public string LATITUDE { get; set; }
        [AutoGenColumn]
        public string LONGITUDE { get; set; }
        [AutoGenColumn]
        public string ELEVATION { get; set; }
        [AutoGenColumn]
        public string MSA2000 { get; set; }
        [AutoGenColumn]
        public string PMSA { get; set; }
        [AutoGenColumn]
        public string CBSA { get; set; }
        [AutoGenColumn]
        public string CBSA_DIV { get; set; }
        [AutoGenColumn]
        public string CBSA_TITLE { get; set; }
        [AutoGenColumn]
        public string PERSONS_PER_HOUSEHOLD { get; set; }
        [AutoGenColumn]
        public string ZIPCODE_POPULATION { get; set; }
        [AutoGenColumn]
        public string COUNTIES_AREA { get; set; }
        [AutoGenColumn]
        public string HOUSEHOLDS_PER_ZIPCODE { get; set; }
        [AutoGenColumn]
        public string WHITE_POPULATION { get; set; }
        [AutoGenColumn]
        public string BLACK_POPULATION { get; set; }
        [AutoGenColumn]
        public string HISPANIC_POPULATION { get; set; }
        [AutoGenColumn]
        public string INCOME_PER_HOUSEHOLD { get; set; }
        [AutoGenColumn]
        public string AVERAGE_HOUSE_VALUE { get; set; }
    }
}
