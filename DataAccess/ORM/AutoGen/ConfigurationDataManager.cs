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
    public partial class ConfigurationDataManager : ManagerBase<ConfigurationDataManager, ConfigurationDataResult, ConfigurationData, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(ConfigurationData record, ConfigurationDataResult result)
        {
            record.Keyname = result.Keyname;
            record.Value = result.Value;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override ConfigurationData GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.ConfigurationData where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<ConfigurationDataResult> GetBaseResultIQueryable(IQueryable<ConfigurationData> baseQuery)
        {
      IQueryable<ConfigurationDataResult> results = (from myData in baseQuery orderby myData.Id select new ConfigurationDataResult { Id= myData.Id,
            Keyname = myData.Keyname,
            Value = myData.Value
      });
		    return results;
        }
        
        public List<ConfigurationDataResult> GetJustBaseTableColumns(ConfigurationDataQuery query)
        {
            foreach (var info in typeof (ConfigurationDataQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: ConfigurationData QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<ConfigurationData> baseQuery = from myData in meta.ConfigurationData select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<ConfigurationDataResult> results = (from myData in baseQuery orderby myData.Id select new ConfigurationDataResult { Id= myData.Id,
                        Keyname = myData.Keyname,
                        Value = myData.Value
            });
            
            List<ConfigurationDataResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<ConfigurationData> BaseQueryAutoGen(IQueryable<ConfigurationData> baseQuery, ConfigurationDataQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.Keyname != null) baseQuery = baseQuery.Where(a => a.Keyname.ToLower().Equals(query.Keyname.ToLower()));
            if (query.Value != null) baseQuery = baseQuery.Where(a => a.Value.ToLower().Equals(query.Value.ToLower()));

            return baseQuery;
        }
        
    }
}
