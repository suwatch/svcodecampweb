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
    public partial class AttendeeVideoManager : ManagerBase<AttendeeVideoManager, AttendeeVideoResult, AttendeeVideo, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(AttendeeVideo record, AttendeeVideoResult result)
        {
            record.AttendeeId = result.AttendeeId;
            record.VideoId = result.VideoId;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override AttendeeVideo GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.AttendeeVideo where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<AttendeeVideoResult> GetBaseResultIQueryable(IQueryable<AttendeeVideo> baseQuery)
        {
      IQueryable<AttendeeVideoResult> results = (from myData in baseQuery orderby myData.Id select new AttendeeVideoResult { Id= myData.Id,
            AttendeeId = myData.AttendeeId,
            VideoId = myData.VideoId
      });
		    return results;
        }
        
        public List<AttendeeVideoResult> GetJustBaseTableColumns(AttendeeVideoQuery query)
        {
            foreach (var info in typeof (AttendeeVideoQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: AttendeeVideo QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<AttendeeVideo> baseQuery = from myData in meta.AttendeeVideo select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<AttendeeVideoResult> results = (from myData in baseQuery orderby myData.Id select new AttendeeVideoResult { Id= myData.Id,
                        AttendeeId = myData.AttendeeId,
                        VideoId = myData.VideoId
            });
            
            List<AttendeeVideoResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<AttendeeVideo> BaseQueryAutoGen(IQueryable<AttendeeVideo> baseQuery, AttendeeVideoQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.AttendeeId != null) baseQuery = baseQuery.Where(a => a.AttendeeId == query.AttendeeId);
            if (query.VideoId != null) baseQuery = baseQuery.Where(a => a.VideoId == query.VideoId);

            return baseQuery;
        }
        
    }
}
