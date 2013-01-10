//  This is the Manager class used for data operations.  It is meant to have another Partial
//  class associated with it.
//  C 3PLogic, Inc.
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

using CodeCampSV;


namespace CodeCampSV
{
    //  Here are the 2 methods that needs to be auto genearted. 
    //  First is a one to one maping to the database columns. 
    //  Since we auto generate the results class too, we can guarantee the columns are all there
    [DataObject(true)]
    public partial class CodeCampEvalsManager : ManagerBase<CodeCampEvalsManager, CodeCampEvalsResult, CodeCampEvals, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(CodeCampEvals record, CodeCampEvalsResult result)
        {
            record.AttendeePKID = result.AttendeePKID;
            record.CodeCampYearId = result.CodeCampYearId;
            record.MetExpectations = result.MetExpectations;
            record.PlanToAttendAgain = result.PlanToAttendAgain;
            record.EnjoyedFreeFood = result.EnjoyedFreeFood;
            record.SessionsVariedEnough = result.SessionsVariedEnough;
            record.EnoughSessionsAtMyLevel = result.EnoughSessionsAtMyLevel;
            record.FoothillGoodVenue = result.FoothillGoodVenue;
            record.WishToldMoreFriends = result.WishToldMoreFriends;
            record.EventWellPlanned = result.EventWellPlanned;
            record.WirelessAccessImportant = result.WirelessAccessImportant;
            record.WiredAccessImportant = result.WiredAccessImportant;
            record.LikeReceivingUpdateByEmail = result.LikeReceivingUpdateByEmail;
            record.LikeReceivingUpdateByByRSSFeed = result.LikeReceivingUpdateByByRSSFeed;
            record.RatherNoSponsorAndNoFreeFood = result.RatherNoSponsorAndNoFreeFood;
            record.AttendedVistaFairOnly = result.AttendedVistaFairOnly;
            record.AttendedVistaFairAndCC = result.AttendedVistaFairAndCC;
            record.AttendedCCOnly = result.AttendedCCOnly;
            record.BestPartOfEvent = result.BestPartOfEvent;
            record.WhatWouldYouChange = result.WhatWouldYouChange;
            record.NotSatisfiedWhy = result.NotSatisfiedWhy;
            record.WhatFoothillClassesToAdd = result.WhatFoothillClassesToAdd;
            record.InteresteInLongTermPlanning = result.InteresteInLongTermPlanning;
            record.InteresteInWebBackEnd = result.InteresteInWebBackEnd;
            record.InterestedInWebFrontEnd = result.InterestedInWebFrontEnd;
            record.InteresteInLongSessionReviewPanel = result.InteresteInLongSessionReviewPanel;
            record.InteresteInContributorSolicitation = result.InteresteInContributorSolicitation;
            record.InteresteInBeingContributor = result.InteresteInBeingContributor;
            record.InteresteInBeforeEvent = result.InteresteInBeforeEvent;
            record.InteresteInDayOfEvent = result.InteresteInDayOfEvent;
            record.InteresteInEventTearDown = result.InteresteInEventTearDown;
            record.InteresteInAfterEvent = result.InteresteInAfterEvent;
            record.ForVolunteeringBestWayToContactEmail = result.ForVolunteeringBestWayToContactEmail;
            record.ForVolunteeringBestWayToContactPhone = result.ForVolunteeringBestWayToContactPhone;
            record.DateSubmitted = result.DateSubmitted;
            record.DateUpdated = result.DateUpdated;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override CodeCampEvals GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.CodeCampEvals where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<CodeCampEvalsResult> GetBaseResultIQueryable(IQueryable<CodeCampEvals> baseQuery)
        {
      IQueryable<CodeCampEvalsResult> results = (from myData in baseQuery orderby myData.Id select new CodeCampEvalsResult { Id= myData.Id,
            AttendeePKID = myData.AttendeePKID,
            CodeCampYearId = myData.CodeCampYearId,
            MetExpectations = myData.MetExpectations,
            PlanToAttendAgain = myData.PlanToAttendAgain,
            EnjoyedFreeFood = myData.EnjoyedFreeFood,
            SessionsVariedEnough = myData.SessionsVariedEnough,
            EnoughSessionsAtMyLevel = myData.EnoughSessionsAtMyLevel,
            FoothillGoodVenue = myData.FoothillGoodVenue,
            WishToldMoreFriends = myData.WishToldMoreFriends,
            EventWellPlanned = myData.EventWellPlanned,
            WirelessAccessImportant = myData.WirelessAccessImportant,
            WiredAccessImportant = myData.WiredAccessImportant,
            LikeReceivingUpdateByEmail = myData.LikeReceivingUpdateByEmail,
            LikeReceivingUpdateByByRSSFeed = myData.LikeReceivingUpdateByByRSSFeed,
            RatherNoSponsorAndNoFreeFood = myData.RatherNoSponsorAndNoFreeFood,
            AttendedVistaFairOnly = myData.AttendedVistaFairOnly,
            AttendedVistaFairAndCC = myData.AttendedVistaFairAndCC,
            AttendedCCOnly = myData.AttendedCCOnly,
            BestPartOfEvent = myData.BestPartOfEvent,
            WhatWouldYouChange = myData.WhatWouldYouChange,
            NotSatisfiedWhy = myData.NotSatisfiedWhy,
            WhatFoothillClassesToAdd = myData.WhatFoothillClassesToAdd,
            InteresteInLongTermPlanning = myData.InteresteInLongTermPlanning,
            InteresteInWebBackEnd = myData.InteresteInWebBackEnd,
            InterestedInWebFrontEnd = myData.InterestedInWebFrontEnd,
            InteresteInLongSessionReviewPanel = myData.InteresteInLongSessionReviewPanel,
            InteresteInContributorSolicitation = myData.InteresteInContributorSolicitation,
            InteresteInBeingContributor = myData.InteresteInBeingContributor,
            InteresteInBeforeEvent = myData.InteresteInBeforeEvent,
            InteresteInDayOfEvent = myData.InteresteInDayOfEvent,
            InteresteInEventTearDown = myData.InteresteInEventTearDown,
            InteresteInAfterEvent = myData.InteresteInAfterEvent,
            ForVolunteeringBestWayToContactEmail = myData.ForVolunteeringBestWayToContactEmail,
            ForVolunteeringBestWayToContactPhone = myData.ForVolunteeringBestWayToContactPhone,
            DateSubmitted = myData.DateSubmitted == null ? null :  (DateTime?) new DateTime(myData.DateSubmitted.Value.Ticks,DateTimeKind.Utc),
            DateUpdated = myData.DateUpdated == null ? null :  (DateTime?) new DateTime(myData.DateUpdated.Value.Ticks,DateTimeKind.Utc)
      });
		    return results;
        }
        
        public List<CodeCampEvalsResult> GetJustBaseTableColumns(CodeCampEvalsQuery query)
        {
            foreach (var info in typeof (CodeCampEvalsQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: CodeCampEvals QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<CodeCampEvals> baseQuery = from myData in meta.CodeCampEvals select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<CodeCampEvalsResult> results = (from myData in baseQuery orderby myData.Id select new CodeCampEvalsResult { Id= myData.Id,
                        AttendeePKID = myData.AttendeePKID,
                        CodeCampYearId = myData.CodeCampYearId,
                        MetExpectations = myData.MetExpectations,
                        PlanToAttendAgain = myData.PlanToAttendAgain,
                        EnjoyedFreeFood = myData.EnjoyedFreeFood,
                        SessionsVariedEnough = myData.SessionsVariedEnough,
                        EnoughSessionsAtMyLevel = myData.EnoughSessionsAtMyLevel,
                        FoothillGoodVenue = myData.FoothillGoodVenue,
                        WishToldMoreFriends = myData.WishToldMoreFriends,
                        EventWellPlanned = myData.EventWellPlanned,
                        WirelessAccessImportant = myData.WirelessAccessImportant,
                        WiredAccessImportant = myData.WiredAccessImportant,
                        LikeReceivingUpdateByEmail = myData.LikeReceivingUpdateByEmail,
                        LikeReceivingUpdateByByRSSFeed = myData.LikeReceivingUpdateByByRSSFeed,
                        RatherNoSponsorAndNoFreeFood = myData.RatherNoSponsorAndNoFreeFood,
                        AttendedVistaFairOnly = myData.AttendedVistaFairOnly,
                        AttendedVistaFairAndCC = myData.AttendedVistaFairAndCC,
                        AttendedCCOnly = myData.AttendedCCOnly,
                        BestPartOfEvent = myData.BestPartOfEvent,
                        WhatWouldYouChange = myData.WhatWouldYouChange,
                        NotSatisfiedWhy = myData.NotSatisfiedWhy,
                        WhatFoothillClassesToAdd = myData.WhatFoothillClassesToAdd,
                        InteresteInLongTermPlanning = myData.InteresteInLongTermPlanning,
                        InteresteInWebBackEnd = myData.InteresteInWebBackEnd,
                        InterestedInWebFrontEnd = myData.InterestedInWebFrontEnd,
                        InteresteInLongSessionReviewPanel = myData.InteresteInLongSessionReviewPanel,
                        InteresteInContributorSolicitation = myData.InteresteInContributorSolicitation,
                        InteresteInBeingContributor = myData.InteresteInBeingContributor,
                        InteresteInBeforeEvent = myData.InteresteInBeforeEvent,
                        InteresteInDayOfEvent = myData.InteresteInDayOfEvent,
                        InteresteInEventTearDown = myData.InteresteInEventTearDown,
                        InteresteInAfterEvent = myData.InteresteInAfterEvent,
                        ForVolunteeringBestWayToContactEmail = myData.ForVolunteeringBestWayToContactEmail,
                        ForVolunteeringBestWayToContactPhone = myData.ForVolunteeringBestWayToContactPhone,
                        DateSubmitted = myData.DateSubmitted == null ? null :  (DateTime?) new DateTime(myData.DateSubmitted.Value.Ticks,DateTimeKind.Utc),
                        DateUpdated = myData.DateUpdated == null ? null :  (DateTime?) new DateTime(myData.DateUpdated.Value.Ticks,DateTimeKind.Utc)
            });
            
            List<CodeCampEvalsResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<CodeCampEvals> BaseQueryAutoGen(IQueryable<CodeCampEvals> baseQuery, CodeCampEvalsQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.CodeCampYearId != null) baseQuery = baseQuery.Where(a => a.CodeCampYearId == query.CodeCampYearId);
            if (query.MetExpectations != null) baseQuery = baseQuery.Where(a => a.MetExpectations == query.MetExpectations);
            if (query.PlanToAttendAgain != null) baseQuery = baseQuery.Where(a => a.PlanToAttendAgain == query.PlanToAttendAgain);
            if (query.EnjoyedFreeFood != null) baseQuery = baseQuery.Where(a => a.EnjoyedFreeFood == query.EnjoyedFreeFood);
            if (query.SessionsVariedEnough != null) baseQuery = baseQuery.Where(a => a.SessionsVariedEnough == query.SessionsVariedEnough);
            if (query.EnoughSessionsAtMyLevel != null) baseQuery = baseQuery.Where(a => a.EnoughSessionsAtMyLevel == query.EnoughSessionsAtMyLevel);
            if (query.FoothillGoodVenue != null) baseQuery = baseQuery.Where(a => a.FoothillGoodVenue == query.FoothillGoodVenue);
            if (query.WishToldMoreFriends != null) baseQuery = baseQuery.Where(a => a.WishToldMoreFriends == query.WishToldMoreFriends);
            if (query.EventWellPlanned != null) baseQuery = baseQuery.Where(a => a.EventWellPlanned == query.EventWellPlanned);
            if (query.WirelessAccessImportant != null) baseQuery = baseQuery.Where(a => a.WirelessAccessImportant == query.WirelessAccessImportant);
            if (query.WiredAccessImportant != null) baseQuery = baseQuery.Where(a => a.WiredAccessImportant == query.WiredAccessImportant);
            if (query.LikeReceivingUpdateByEmail != null) baseQuery = baseQuery.Where(a => a.LikeReceivingUpdateByEmail == query.LikeReceivingUpdateByEmail);
            if (query.LikeReceivingUpdateByByRSSFeed != null) baseQuery = baseQuery.Where(a => a.LikeReceivingUpdateByByRSSFeed == query.LikeReceivingUpdateByByRSSFeed);
            if (query.RatherNoSponsorAndNoFreeFood != null) baseQuery = baseQuery.Where(a => a.RatherNoSponsorAndNoFreeFood == query.RatherNoSponsorAndNoFreeFood);
            if (query.AttendedVistaFairOnly != null) baseQuery = baseQuery.Where(a => a.AttendedVistaFairOnly == query.AttendedVistaFairOnly);
            if (query.AttendedVistaFairAndCC != null) baseQuery = baseQuery.Where(a => a.AttendedVistaFairAndCC == query.AttendedVistaFairAndCC);
            if (query.AttendedCCOnly != null) baseQuery = baseQuery.Where(a => a.AttendedCCOnly == query.AttendedCCOnly);
            if (query.BestPartOfEvent != null) baseQuery = baseQuery.Where(a => a.BestPartOfEvent.ToLower().Equals(query.BestPartOfEvent.ToLower()));
            if (query.WhatWouldYouChange != null) baseQuery = baseQuery.Where(a => a.WhatWouldYouChange.ToLower().Equals(query.WhatWouldYouChange.ToLower()));
            if (query.NotSatisfiedWhy != null) baseQuery = baseQuery.Where(a => a.NotSatisfiedWhy.ToLower().Equals(query.NotSatisfiedWhy.ToLower()));
            if (query.WhatFoothillClassesToAdd != null) baseQuery = baseQuery.Where(a => a.WhatFoothillClassesToAdd.ToLower().Equals(query.WhatFoothillClassesToAdd.ToLower()));
            if (query.InteresteInLongTermPlanning != null) baseQuery = baseQuery.Where(a => a.InteresteInLongTermPlanning == query.InteresteInLongTermPlanning);
            if (query.InteresteInWebBackEnd != null) baseQuery = baseQuery.Where(a => a.InteresteInWebBackEnd == query.InteresteInWebBackEnd);
            if (query.InterestedInWebFrontEnd != null) baseQuery = baseQuery.Where(a => a.InterestedInWebFrontEnd == query.InterestedInWebFrontEnd);
            if (query.InteresteInLongSessionReviewPanel != null) baseQuery = baseQuery.Where(a => a.InteresteInLongSessionReviewPanel == query.InteresteInLongSessionReviewPanel);
            if (query.InteresteInContributorSolicitation != null) baseQuery = baseQuery.Where(a => a.InteresteInContributorSolicitation == query.InteresteInContributorSolicitation);
            if (query.InteresteInBeingContributor != null) baseQuery = baseQuery.Where(a => a.InteresteInBeingContributor == query.InteresteInBeingContributor);
            if (query.InteresteInBeforeEvent != null) baseQuery = baseQuery.Where(a => a.InteresteInBeforeEvent == query.InteresteInBeforeEvent);
            if (query.InteresteInDayOfEvent != null) baseQuery = baseQuery.Where(a => a.InteresteInDayOfEvent == query.InteresteInDayOfEvent);
            if (query.InteresteInEventTearDown != null) baseQuery = baseQuery.Where(a => a.InteresteInEventTearDown == query.InteresteInEventTearDown);
            if (query.InteresteInAfterEvent != null) baseQuery = baseQuery.Where(a => a.InteresteInAfterEvent == query.InteresteInAfterEvent);
            if (query.ForVolunteeringBestWayToContactEmail != null) baseQuery = baseQuery.Where(a => a.ForVolunteeringBestWayToContactEmail.ToLower().Equals(query.ForVolunteeringBestWayToContactEmail.ToLower()));
            if (query.ForVolunteeringBestWayToContactPhone != null) baseQuery = baseQuery.Where(a => a.ForVolunteeringBestWayToContactPhone.ToLower().Equals(query.ForVolunteeringBestWayToContactPhone.ToLower()));
            if (query.DateSubmitted != null) baseQuery = baseQuery.Where(a => a.DateSubmitted.Value.CompareTo(query.DateSubmitted.Value) == 0);
            if (query.DateUpdated != null) baseQuery = baseQuery.Where(a => a.DateUpdated.Value.CompareTo(query.DateUpdated.Value) == 0);

            return baseQuery;
        }
        
    }
}
