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
    public partial class VistaSlotsManager : ManagerBase<VistaSlotsManager, VistaSlotsResult, VistaSlots, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(VistaSlots record, VistaSlotsResult result)
        {
            record.Description = result.Description;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override VistaSlots GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.VistaSlots where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<VistaSlotsResult> GetBaseResultIQueryable(IQueryable<VistaSlots> baseQuery)
        {
      IQueryable<VistaSlotsResult> results = (from myData in baseQuery orderby myData.Id select new VistaSlotsResult { Id= myData.Id,
            Description = myData.Description
      });
		    return results;
        }
        
        public List<VistaSlotsResult> GetJustBaseTableColumns(VistaSlotsQuery query)
        {
            foreach (var info in typeof (VistaSlotsQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: VistaSlots QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<VistaSlots> baseQuery = from myData in meta.VistaSlots select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<VistaSlotsResult> results = (from myData in baseQuery orderby myData.Id select new VistaSlotsResult { Id= myData.Id,
                        Description = myData.Description
            });
            
            List<VistaSlotsResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<VistaSlots> BaseQueryAutoGen(IQueryable<VistaSlots> baseQuery, VistaSlotsQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.Description != null) baseQuery = baseQuery.Where(a => a.Description.ToLower().Equals(query.Description.ToLower()));

            return baseQuery;
        }
        
    }
}
