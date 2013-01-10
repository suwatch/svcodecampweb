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
    public partial class SessionTimesManager : ManagerBase<SessionTimesManager, SessionTimesResult, SessionTimes, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(SessionTimes record, SessionTimesResult result)
        {
            record.StartTime = result.StartTime;
            record.StartTimeFriendly = result.StartTimeFriendly;
            record.EndTime = result.EndTime;
            record.EndTimeFriendly = result.EndTimeFriendly;
            record.SessionMinutes = result.SessionMinutes;
            record.Description = result.Description;
            record.TitleBeforeOnAgenda = result.TitleBeforeOnAgenda;
            record.TitleAfterOnAgenda = result.TitleAfterOnAgenda;
            record.CodeCampYearId = result.CodeCampYearId;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override SessionTimes GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.SessionTimes where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<SessionTimesResult> GetBaseResultIQueryable(IQueryable<SessionTimes> baseQuery)
        {
      IQueryable<SessionTimesResult> results = (from myData in baseQuery orderby myData.Id select new SessionTimesResult { Id= myData.Id,
            StartTime = myData.StartTime == null ? null :  (DateTime?) new DateTime(myData.StartTime.Value.Ticks,DateTimeKind.Utc),
            StartTimeFriendly = myData.StartTimeFriendly,
            EndTime = myData.EndTime == null ? null :  (DateTime?) new DateTime(myData.EndTime.Value.Ticks,DateTimeKind.Utc),
            EndTimeFriendly = myData.EndTimeFriendly,
            SessionMinutes = myData.SessionMinutes,
            Description = myData.Description,
            TitleBeforeOnAgenda = myData.TitleBeforeOnAgenda,
            TitleAfterOnAgenda = myData.TitleAfterOnAgenda,
            CodeCampYearId = myData.CodeCampYearId
      });
		    return results;
        }
        
        public List<SessionTimesResult> GetJustBaseTableColumns(SessionTimesQuery query)
        {
            foreach (var info in typeof (SessionTimesQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: SessionTimes QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<SessionTimes> baseQuery = from myData in meta.SessionTimes select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<SessionTimesResult> results = (from myData in baseQuery orderby myData.Id select new SessionTimesResult { Id= myData.Id,
                        StartTime = myData.StartTime == null ? null :  (DateTime?) new DateTime(myData.StartTime.Value.Ticks,DateTimeKind.Utc),
                        StartTimeFriendly = myData.StartTimeFriendly,
                        EndTime = myData.EndTime == null ? null :  (DateTime?) new DateTime(myData.EndTime.Value.Ticks,DateTimeKind.Utc),
                        EndTimeFriendly = myData.EndTimeFriendly,
                        SessionMinutes = myData.SessionMinutes,
                        Description = myData.Description,
                        TitleBeforeOnAgenda = myData.TitleBeforeOnAgenda,
                        TitleAfterOnAgenda = myData.TitleAfterOnAgenda,
                        CodeCampYearId = myData.CodeCampYearId
            });
            
            List<SessionTimesResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<SessionTimes> BaseQueryAutoGen(IQueryable<SessionTimes> baseQuery, SessionTimesQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.StartTime != null) baseQuery = baseQuery.Where(a => a.StartTime.Value.CompareTo(query.StartTime.Value) == 0);
            if (query.StartTimeFriendly != null) baseQuery = baseQuery.Where(a => a.StartTimeFriendly.ToLower().Equals(query.StartTimeFriendly.ToLower()));
            if (query.EndTime != null) baseQuery = baseQuery.Where(a => a.EndTime.Value.CompareTo(query.EndTime.Value) == 0);
            if (query.EndTimeFriendly != null) baseQuery = baseQuery.Where(a => a.EndTimeFriendly.ToLower().Equals(query.EndTimeFriendly.ToLower()));
            if (query.SessionMinutes != null) baseQuery = baseQuery.Where(a => a.SessionMinutes == query.SessionMinutes);
            if (query.Description != null) baseQuery = baseQuery.Where(a => a.Description.ToLower().Equals(query.Description.ToLower()));
            if (query.TitleBeforeOnAgenda != null) baseQuery = baseQuery.Where(a => a.TitleBeforeOnAgenda.ToLower().Equals(query.TitleBeforeOnAgenda.ToLower()));
            if (query.TitleAfterOnAgenda != null) baseQuery = baseQuery.Where(a => a.TitleAfterOnAgenda.ToLower().Equals(query.TitleAfterOnAgenda.ToLower()));
            if (query.CodeCampYearId != null) baseQuery = baseQuery.Where(a => a.CodeCampYearId == query.CodeCampYearId);

            return baseQuery;
        }
        
    }
}
