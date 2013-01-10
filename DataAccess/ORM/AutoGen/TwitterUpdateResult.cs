//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace CodeCampSV
{
    public partial class TwitterUpdateResult : ResultBase
    {
        [DataMember] public string Title { get; set; }
        [DataMember] public DateTime? Published { get; set; }
        [DataMember] public string AuthorImageUrl { get; set; }
        [DataMember] public string AuthorUrl { get; set; }
        [DataMember] public string AuthorName { get; set; }
        [DataMember] public string AuthorHandle { get; set; }
        [DataMember] public string ContentTweet { get; set; }
        [DataMember] public string AlternateTweet { get; set; }
        [DataMember] public string CodeCampSpeakerUrl { get; set; }
        [DataMember] public string CodeCampSessionsUrl { get; set; }
        [DataMember] public bool? TweetNotRelevant { get; set; }
        [DataMember] public DateTime? TweetInserted { get; set; }
        [DataMember] public DateTime? TweetUpdate { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
