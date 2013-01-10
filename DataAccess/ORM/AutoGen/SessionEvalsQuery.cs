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
    public partial class SessionEvalsQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public int? SessionId { get; set; }
        [AutoGenColumn]
        public Guid? PKID { get; set; }
        [AutoGenColumn]
        public DateTime? CreateDate { get; set; }
        [AutoGenColumn]
        public DateTime? UpdateDate { get; set; }
        [AutoGenColumn]
        public int? CourseAsWhole { get; set; }
        [AutoGenColumn]
        public int? CourseContent { get; set; }
        [AutoGenColumn]
        public int? InstructorEff { get; set; }
        [AutoGenColumn]
        public int? InstructorAbilityExplain { get; set; }
        [AutoGenColumn]
        public int? InstructorEffective { get; set; }
        [AutoGenColumn]
        public int? InstructorKnowledge { get; set; }
        [AutoGenColumn]
        public int? QualityOfFacility { get; set; }
        [AutoGenColumn]
        public int? OverallCodeCamp { get; set; }
        [AutoGenColumn]
        public int? ContentLevel { get; set; }
        [AutoGenColumn]
        public string Favorite { get; set; }
        [AutoGenColumn]
        public string Improved { get; set; }
        [AutoGenColumn]
        public string GeneralComments { get; set; }
        [AutoGenColumn]
        public bool? DiscloseEval { get; set; }
    }
}
