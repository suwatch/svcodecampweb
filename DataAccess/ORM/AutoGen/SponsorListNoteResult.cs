//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CodeCampSV
{
    public partial class SponsorListNoteResult : ResultBase
    {
        [DataMember] public int SponsorListId { get; set; }
        [DataMember] public DateTime TimeStampOfNote { get; set; }
        [DataMember] public string NoteAuthor { get; set; }
        [DataMember] public string Note { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
