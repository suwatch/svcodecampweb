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
    public partial class ReferringUrlManager : ManagerBase<ReferringUrlManager, ReferringUrlResult, ReferringUrl, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(ReferringUrl record, ReferringUrlResult result)
        {
            record.RequestUrl = result.RequestUrl;
            record.ReferringUrlName = result.ReferringUrlName;
            record.ReferringIpAddress = result.ReferringIpAddress;
            record.RequestDate = result.RequestDate;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override ReferringUrl GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.ReferringUrl where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<ReferringUrlResult> GetBaseResultIQueryable(IQueryable<ReferringUrl> baseQuery)
        {
      IQueryable<ReferringUrlResult> results = (from myData in baseQuery orderby myData.Id select new ReferringUrlResult { Id= myData.Id,
            RequestUrl = myData.RequestUrl,
            ReferringUrlName = myData.ReferringUrlName,
            ReferringIpAddress = myData.ReferringIpAddress,
            RequestDate = new DateTime(myData.RequestDate.Ticks,DateTimeKind.Utc)
      });
		    return results;
        }
        
        public List<ReferringUrlResult> GetJustBaseTableColumns(ReferringUrlQuery query)
        {
            foreach (var info in typeof (ReferringUrlQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: ReferringUrl QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<ReferringUrl> baseQuery = from myData in meta.ReferringUrl select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<ReferringUrlResult> results = (from myData in baseQuery orderby myData.Id select new ReferringUrlResult { Id= myData.Id,
                        RequestUrl = myData.RequestUrl,
                        ReferringUrlName = myData.ReferringUrlName,
                        ReferringIpAddress = myData.ReferringIpAddress,
                        RequestDate = new DateTime(myData.RequestDate.Ticks,DateTimeKind.Utc)
            });
            
            List<ReferringUrlResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<ReferringUrl> BaseQueryAutoGen(IQueryable<ReferringUrl> baseQuery, ReferringUrlQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.RequestUrl != null) baseQuery = baseQuery.Where(a => a.RequestUrl.ToLower().Equals(query.RequestUrl.ToLower()));
            if (query.ReferringUrlName != null) baseQuery = baseQuery.Where(a => a.ReferringUrlName.ToLower().Equals(query.ReferringUrlName.ToLower()));
            if (query.ReferringIpAddress != null) baseQuery = baseQuery.Where(a => a.ReferringIpAddress.ToLower().Equals(query.ReferringIpAddress.ToLower()));
            if (query.RequestDate != null) baseQuery = baseQuery.Where(a => a.RequestDate.CompareTo(query.RequestDate) == 0);

            return baseQuery;
        }
        
    }
}
