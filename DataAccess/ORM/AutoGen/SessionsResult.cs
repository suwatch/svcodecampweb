//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CodeCampSV
{
    public partial class SessionsResult : ResultBase
    {
        [DataMember] public int CodeCampYearId { get; set; }
        [DataMember] public int? SessionLevel_id { get; set; }
        [DataMember] public int? SponsorId { get; set; }
        [DataMember] public string Username { get; set; }
        [DataMember] public string Title { get; set; }
        [DataMember] public string Description { get; set; }
        [DataMember] public bool? Approved { get; set; }
        [DataMember] public DateTime? Createdate { get; set; }
        [DataMember] public DateTime? Updatedate { get; set; }
        [DataMember] public string AdminComments { get; set; }
        [DataMember] public bool DoNotShowPrimarySpeaker { get; set; }
        [DataMember] public bool? InterentAccessRequired { get; set; }
        [DataMember] public int? LectureRoomsId { get; set; }
        [DataMember] public int? SessionTimesId { get; set; }
        [DataMember] public string TweetLine { get; set; }
        [DataMember] public DateTime? TweetLineTweetedDate { get; set; }
        [DataMember] public bool? TweetLineTweeted { get; set; }
        [DataMember] public string SessionsMaterialUrl { get; set; }
        [DataMember] public bool? SessionsMaterialQueueToSend { get; set; }
        [DataMember] public DateTime? SessionMaterialUrlDateSent { get; set; }
        [DataMember] public string SessionMaterialUrlMessage { get; set; }
        [DataMember] public string TwitterHashTags { get; set; }
        [DataMember] public string TweetLinePreCamp { get; set; }
        [DataMember] public DateTime? TweetLineTweetedDatePreCamp { get; set; }
        [DataMember] public bool? TweetLineTweetedPreCamp { get; set; }
        [DataMember] public string ShortUrl { get; set; }
        [DataMember] public string BoxFolderIdString { get; set; }
        [DataMember] public string BoxFolderEmailInAddress { get; set; }
        [DataMember] public string BoxFolderPublicUrl { get; set; }
        [DataMember] public string OptInTechJobKeyWords { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
