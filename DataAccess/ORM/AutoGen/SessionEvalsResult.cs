//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace CodeCampSV
{
    public partial class SessionEvalsResult : ResultBase
    {
        [DataMember] public int SessionId { get; set; }
        [DataMember] public Guid PKID { get; set; }
        [DataMember] public DateTime? CreateDate { get; set; }
        [DataMember] public DateTime? UpdateDate { get; set; }
        [DataMember] public int? CourseAsWhole { get; set; }
        [DataMember] public int? CourseContent { get; set; }
        [DataMember] public int? InstructorEff { get; set; }
        [DataMember] public int? InstructorAbilityExplain { get; set; }
        [DataMember] public int? InstructorEffective { get; set; }
        [DataMember] public int? InstructorKnowledge { get; set; }
        [DataMember] public int? QualityOfFacility { get; set; }
        [DataMember] public int? OverallCodeCamp { get; set; }
        [DataMember] public int? ContentLevel { get; set; }
        [DataMember] public string Favorite { get; set; }
        [DataMember] public string Improved { get; set; }
        [DataMember] public string GeneralComments { get; set; }
        [DataMember] public bool? DiscloseEval { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
