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
    public partial class SessionsJobListingManager : ManagerBase<SessionsJobListingManager, SessionsJobListingResult, SessionsJobListing, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(SessionsJobListing record, SessionsJobListingResult result)
        {
            record.SessionId = result.SessionId;
            record.JobListingId = result.JobListingId;
            record.ShowImageOnSession = result.ShowImageOnSession;
            record.ShowTextOnSession = result.ShowTextOnSession;
            record.DateCreated = result.DateCreated;
            record.DateUpdate = result.DateUpdate;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override SessionsJobListing GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.SessionsJobListing where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<SessionsJobListingResult> GetBaseResultIQueryable(IQueryable<SessionsJobListing> baseQuery)
        {
      IQueryable<SessionsJobListingResult> results = (from myData in baseQuery orderby myData.Id select new SessionsJobListingResult { Id= myData.Id,
            SessionId = myData.SessionId,
            JobListingId = myData.JobListingId,
            ShowImageOnSession = myData.ShowImageOnSession,
            ShowTextOnSession = myData.ShowTextOnSession,
            DateCreated = new DateTime(myData.DateCreated.Ticks,DateTimeKind.Utc),
            DateUpdate = new DateTime(myData.DateUpdate.Ticks,DateTimeKind.Utc)
      });
		    return results;
        }
        
        public List<SessionsJobListingResult> GetJustBaseTableColumns(SessionsJobListingQuery query)
        {
            foreach (var info in typeof (SessionsJobListingQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: SessionsJobListing QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<SessionsJobListing> baseQuery = from myData in meta.SessionsJobListing select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<SessionsJobListingResult> results = (from myData in baseQuery orderby myData.Id select new SessionsJobListingResult { Id= myData.Id,
                        SessionId = myData.SessionId,
                        JobListingId = myData.JobListingId,
                        ShowImageOnSession = myData.ShowImageOnSession,
                        ShowTextOnSession = myData.ShowTextOnSession,
                        DateCreated = new DateTime(myData.DateCreated.Ticks,DateTimeKind.Utc),
                        DateUpdate = new DateTime(myData.DateUpdate.Ticks,DateTimeKind.Utc)
            });
            
            List<SessionsJobListingResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<SessionsJobListing> BaseQueryAutoGen(IQueryable<SessionsJobListing> baseQuery, SessionsJobListingQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.SessionId != null) baseQuery = baseQuery.Where(a => a.SessionId == query.SessionId);
            if (query.JobListingId != null) baseQuery = baseQuery.Where(a => a.JobListingId == query.JobListingId);
            if (query.ShowImageOnSession != null) baseQuery = baseQuery.Where(a => a.ShowImageOnSession == query.ShowImageOnSession);
            if (query.ShowTextOnSession != null) baseQuery = baseQuery.Where(a => a.ShowTextOnSession == query.ShowTextOnSession);
            if (query.DateCreated != null) baseQuery = baseQuery.Where(a => a.DateCreated.CompareTo(query.DateCreated) == 0);
            if (query.DateUpdate != null) baseQuery = baseQuery.Where(a => a.DateUpdate.CompareTo(query.DateUpdate) == 0);

            return baseQuery;
        }
        
    }
}
