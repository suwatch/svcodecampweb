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
    public partial class Log4NetAllManager : ManagerBase<Log4NetAllManager, Log4NetAllResult, Log4NetAll, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(Log4NetAll record, Log4NetAllResult result)
        {
            record.Date = result.Date;
            record.Thread = result.Thread;
            record.Level = result.Level;
            record.Logger = result.Logger;
            record.Message = result.Message;
            record.ExceptionMessage = result.ExceptionMessage;
            record.ExceptionStackTrace = result.ExceptionStackTrace;
            record.UserName = result.UserName;
            record.EllapsedTime = result.EllapsedTime;
            record.MessageLine1 = result.MessageLine1;
            record.MessageLine2 = result.MessageLine2;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override Log4NetAll GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.Log4NetAll where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<Log4NetAllResult> GetBaseResultIQueryable(IQueryable<Log4NetAll> baseQuery)
        {
      IQueryable<Log4NetAllResult> results = (from myData in baseQuery orderby myData.Id select new Log4NetAllResult { Id= myData.Id,
            Date = new DateTime(myData.Date.Ticks,DateTimeKind.Utc),
            Thread = myData.Thread,
            Level = myData.Level,
            Logger = myData.Logger,
            Message = myData.Message,
            ExceptionMessage = myData.ExceptionMessage,
            ExceptionStackTrace = myData.ExceptionStackTrace,
            UserName = myData.UserName,
            EllapsedTime = myData.EllapsedTime,
            MessageLine1 = myData.MessageLine1,
            MessageLine2 = myData.MessageLine2
      });
		    return results;
        }
        
        public List<Log4NetAllResult> GetJustBaseTableColumns(Log4NetAllQuery query)
        {
            foreach (var info in typeof (Log4NetAllQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: Log4NetAll QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<Log4NetAll> baseQuery = from myData in meta.Log4NetAll select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<Log4NetAllResult> results = (from myData in baseQuery orderby myData.Id select new Log4NetAllResult { Id= myData.Id,
                        Date = new DateTime(myData.Date.Ticks,DateTimeKind.Utc),
                        Thread = myData.Thread,
                        Level = myData.Level,
                        Logger = myData.Logger,
                        Message = myData.Message,
                        ExceptionMessage = myData.ExceptionMessage,
                        ExceptionStackTrace = myData.ExceptionStackTrace,
                        UserName = myData.UserName,
                        EllapsedTime = myData.EllapsedTime,
                        MessageLine1 = myData.MessageLine1,
                        MessageLine2 = myData.MessageLine2
            });
            
            List<Log4NetAllResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<Log4NetAll> BaseQueryAutoGen(IQueryable<Log4NetAll> baseQuery, Log4NetAllQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.Date != null) baseQuery = baseQuery.Where(a => a.Date.CompareTo(query.Date) == 0);
            if (query.Thread != null) baseQuery = baseQuery.Where(a => a.Thread.ToLower().Equals(query.Thread.ToLower()));
            if (query.Level != null) baseQuery = baseQuery.Where(a => a.Level.ToLower().Equals(query.Level.ToLower()));
            if (query.Logger != null) baseQuery = baseQuery.Where(a => a.Logger.ToLower().Equals(query.Logger.ToLower()));
            if (query.Message != null) baseQuery = baseQuery.Where(a => a.Message.ToLower().Equals(query.Message.ToLower()));
            if (query.ExceptionMessage != null) baseQuery = baseQuery.Where(a => a.ExceptionMessage.ToLower().Equals(query.ExceptionMessage.ToLower()));
            if (query.ExceptionStackTrace != null) baseQuery = baseQuery.Where(a => a.ExceptionStackTrace.ToLower().Equals(query.ExceptionStackTrace.ToLower()));
            if (query.UserName != null) baseQuery = baseQuery.Where(a => a.UserName.ToLower().Equals(query.UserName.ToLower()));
            if (query.EllapsedTime != null) baseQuery = baseQuery.Where(a => a.EllapsedTime == query.EllapsedTime);
            if (query.MessageLine1 != null) baseQuery = baseQuery.Where(a => a.MessageLine1.ToLower().Equals(query.MessageLine1.ToLower()));
            if (query.MessageLine2 != null) baseQuery = baseQuery.Where(a => a.MessageLine2.ToLower().Equals(query.MessageLine2.ToLower()));

            return baseQuery;
        }
        
    }
}
