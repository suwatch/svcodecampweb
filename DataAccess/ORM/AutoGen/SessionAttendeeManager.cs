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
    public partial class SessionAttendeeManager : ManagerBase<SessionAttendeeManager, SessionAttendeeResult, SessionAttendee, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(SessionAttendee record, SessionAttendeeResult result)
        {
            record.Sessions_id = result.Sessions_id;
            record.Attendees_username = result.Attendees_username;
            record.Interestlevel = result.Interestlevel;
            record.LastUpdatedDate = result.LastUpdatedDate;
            record.UpdateByProgram = result.UpdateByProgram;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override SessionAttendee GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.SessionAttendee where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<SessionAttendeeResult> GetBaseResultIQueryable(IQueryable<SessionAttendee> baseQuery)
        {
      IQueryable<SessionAttendeeResult> results = (from myData in baseQuery orderby myData.Id select new SessionAttendeeResult { Id= myData.Id,
            Sessions_id = myData.Sessions_id,
            Attendees_username = myData.Attendees_username,
            Interestlevel = myData.Interestlevel,
            LastUpdatedDate = myData.LastUpdatedDate == null ? null :  (DateTime?) new DateTime(myData.LastUpdatedDate.Value.Ticks,DateTimeKind.Utc),
            UpdateByProgram = myData.UpdateByProgram
      });
		    return results;
        }
        
        public List<SessionAttendeeResult> GetJustBaseTableColumns(SessionAttendeeQuery query)
        {
            foreach (var info in typeof (SessionAttendeeQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: SessionAttendee QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<SessionAttendee> baseQuery = from myData in meta.SessionAttendee select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<SessionAttendeeResult> results = (from myData in baseQuery orderby myData.Id select new SessionAttendeeResult { Id= myData.Id,
                        Sessions_id = myData.Sessions_id,
                        Attendees_username = myData.Attendees_username,
                        Interestlevel = myData.Interestlevel,
                        LastUpdatedDate = myData.LastUpdatedDate == null ? null :  (DateTime?) new DateTime(myData.LastUpdatedDate.Value.Ticks,DateTimeKind.Utc),
                        UpdateByProgram = myData.UpdateByProgram
            });
            
            List<SessionAttendeeResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<SessionAttendee> BaseQueryAutoGen(IQueryable<SessionAttendee> baseQuery, SessionAttendeeQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.Sessions_id != null) baseQuery = baseQuery.Where(a => a.Sessions_id == query.Sessions_id);
            if (query.Interestlevel != null) baseQuery = baseQuery.Where(a => a.Interestlevel == query.Interestlevel);
            if (query.LastUpdatedDate != null) baseQuery = baseQuery.Where(a => a.LastUpdatedDate.Value.CompareTo(query.LastUpdatedDate.Value) == 0);
            if (query.UpdateByProgram != null) baseQuery = baseQuery.Where(a => a.UpdateByProgram.ToLower().Equals(query.UpdateByProgram.ToLower()));

            return baseQuery;
        }
        
    }
}
