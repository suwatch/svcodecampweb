//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CodeCampSV
{
    public partial class ZIPCODEWORLDGOLDResult : ResultBase
    {
        [DataMember] public string ZIP_CODE { get; set; }
        [DataMember] public string CITY { get; set; }
        [DataMember] public string STATE { get; set; }
        [DataMember] public string AREA_CODE { get; set; }
        [DataMember] public string CITY_ALIAS_NAME { get; set; }
        [DataMember] public string CITY_ALIAS_ABBR { get; set; }
        [DataMember] public string CITY_TYPE { get; set; }
        [DataMember] public string COUNTY_NAME { get; set; }
        [DataMember] public string STATE_FIPS { get; set; }
        [DataMember] public string COUNTY_FIPS { get; set; }
        [DataMember] public string TIME_ZONE { get; set; }
        [DataMember] public string DAY_LIGHT_SAVING { get; set; }
        [DataMember] public string LATITUDE { get; set; }
        [DataMember] public string LONGITUDE { get; set; }
        [DataMember] public string ELEVATION { get; set; }
        [DataMember] public string MSA2000 { get; set; }
        [DataMember] public string PMSA { get; set; }
        [DataMember] public string CBSA { get; set; }
        [DataMember] public string CBSA_DIV { get; set; }
        [DataMember] public string CBSA_TITLE { get; set; }
        [DataMember] public string PERSONS_PER_HOUSEHOLD { get; set; }
        [DataMember] public string ZIPCODE_POPULATION { get; set; }
        [DataMember] public string COUNTIES_AREA { get; set; }
        [DataMember] public string HOUSEHOLDS_PER_ZIPCODE { get; set; }
        [DataMember] public string WHITE_POPULATION { get; set; }
        [DataMember] public string BLACK_POPULATION { get; set; }
        [DataMember] public string HISPANIC_POPULATION { get; set; }
        [DataMember] public string INCOME_PER_HOUSEHOLD { get; set; }
        [DataMember] public string AVERAGE_HOUSE_VALUE { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
