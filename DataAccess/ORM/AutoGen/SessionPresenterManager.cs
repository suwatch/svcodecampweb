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
    public partial class SessionPresenterManager : ManagerBase<SessionPresenterManager, SessionPresenterResult, SessionPresenter, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(SessionPresenter record, SessionPresenterResult result)
        {
            record.AttendeeId = result.AttendeeId;
            record.SessionId = result.SessionId;
            record.DoNotShow = result.DoNotShow;
            record.Primary = result.Primary;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override SessionPresenter GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.SessionPresenter where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<SessionPresenterResult> GetBaseResultIQueryable(IQueryable<SessionPresenter> baseQuery)
        {
      IQueryable<SessionPresenterResult> results = (from myData in baseQuery orderby myData.Id select new SessionPresenterResult { Id= myData.Id,
            AttendeeId = myData.AttendeeId,
            SessionId = myData.SessionId,
            DoNotShow = myData.DoNotShow,
            Primary = myData.Primary
      });
		    return results;
        }
        
        public List<SessionPresenterResult> GetJustBaseTableColumns(SessionPresenterQuery query)
        {
            foreach (var info in typeof (SessionPresenterQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: SessionPresenter QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<SessionPresenter> baseQuery = from myData in meta.SessionPresenter select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<SessionPresenterResult> results = (from myData in baseQuery orderby myData.Id select new SessionPresenterResult { Id= myData.Id,
                        AttendeeId = myData.AttendeeId,
                        SessionId = myData.SessionId,
                        DoNotShow = myData.DoNotShow,
                        Primary = myData.Primary
            });
            
            List<SessionPresenterResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<SessionPresenter> BaseQueryAutoGen(IQueryable<SessionPresenter> baseQuery, SessionPresenterQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.AttendeeId != null) baseQuery = baseQuery.Where(a => a.AttendeeId == query.AttendeeId);
            if (query.SessionId != null) baseQuery = baseQuery.Where(a => a.SessionId == query.SessionId);
            if (query.DoNotShow != null) baseQuery = baseQuery.Where(a => a.DoNotShow == query.DoNotShow);
            if (query.Primary != null) baseQuery = baseQuery.Where(a => a.Primary == query.Primary);

            return baseQuery;
        }
        
    }
}
