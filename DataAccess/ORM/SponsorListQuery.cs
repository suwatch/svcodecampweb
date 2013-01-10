//  This file is meant as a starting point only.  it should be copied into working source tree only if there is not
//  an existing file with this name in it already.
//  C 3PLogic, Inc.
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace CodeCampSV
{
    public partial class SponsorListQuery
    {
		//  Make sure to make Query Parameters Properties with  {get; set;} and not just public properties
		//  (Otherwise, the [AutoGenAttribute] column can not do it's job to make sure you don't specify parameters
		//   that are not in GetJustBaseTableColumns return)
        // 
        //  All the items in this section are ANDed together.
        //  Below are examples of the types of Queries you can use among others.
        //  public bool? WithCargo { get; set; }
        //  public bool? IsStared { get; set; }
        //  public List<int> PlanIds { get; set; }

        public bool? WithCodeCampYears { get; set; }

        public int? CodeCampYearId { get; set; }

        public bool? WithContacts { get; set; }

        public bool? WithNotes { get; set; }

        public bool? WithJobListings { get; set; }

        public bool? IncludeSponsorLevel { get; set; }

        public double? PlatinumLevel { get; set; }
        public double? GoldLevel { get; set; }
        public double? SilverLevel { get; set; }
        public double? BronzeLevel { get; set; }

        public bool? Visible { get; set; }


    }
}
