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
    public partial class CodeCampYearManager : ManagerBase<CodeCampYearManager, CodeCampYearResult, CodeCampYear, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(CodeCampYear record, CodeCampYearResult result)
        {
            record.Name = result.Name;
            record.CampStartDate = result.CampStartDate;
            record.CampEndDate = result.CampEndDate;
            record.ReadOnly = result.ReadOnly;
            record.CodeCampDateString = result.CodeCampDateString;
            record.CodeCampSaturdayString = result.CodeCampSaturdayString;
            record.CodeCampSundayString = result.CodeCampSundayString;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override CodeCampYear GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.CodeCampYear where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<CodeCampYearResult> GetBaseResultIQueryable(IQueryable<CodeCampYear> baseQuery)
        {
      IQueryable<CodeCampYearResult> results = (from myData in baseQuery orderby myData.Id select new CodeCampYearResult { Id= myData.Id,
            Name = myData.Name,
            CampStartDate = new DateTime(myData.CampStartDate.Ticks,DateTimeKind.Utc),
            CampEndDate = new DateTime(myData.CampEndDate.Ticks,DateTimeKind.Utc),
            ReadOnly = myData.ReadOnly,
            CodeCampDateString = myData.CodeCampDateString,
            CodeCampSaturdayString = myData.CodeCampSaturdayString,
            CodeCampSundayString = myData.CodeCampSundayString
      });
		    return results;
        }
        
        public List<CodeCampYearResult> GetJustBaseTableColumns(CodeCampYearQuery query)
        {
            foreach (var info in typeof (CodeCampYearQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: CodeCampYear QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<CodeCampYear> baseQuery = from myData in meta.CodeCampYear select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<CodeCampYearResult> results = (from myData in baseQuery orderby myData.Id select new CodeCampYearResult { Id= myData.Id,
                        Name = myData.Name,
                        CampStartDate = new DateTime(myData.CampStartDate.Ticks,DateTimeKind.Utc),
                        CampEndDate = new DateTime(myData.CampEndDate.Ticks,DateTimeKind.Utc),
                        ReadOnly = myData.ReadOnly,
                        CodeCampDateString = myData.CodeCampDateString,
                        CodeCampSaturdayString = myData.CodeCampSaturdayString,
                        CodeCampSundayString = myData.CodeCampSundayString
            });
            
            List<CodeCampYearResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<CodeCampYear> BaseQueryAutoGen(IQueryable<CodeCampYear> baseQuery, CodeCampYearQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.Name != null) baseQuery = baseQuery.Where(a => a.Name.ToLower().Equals(query.Name.ToLower()));
            if (query.CampStartDate != null) baseQuery = baseQuery.Where(a => a.CampStartDate.CompareTo(query.CampStartDate) == 0);
            if (query.CampEndDate != null) baseQuery = baseQuery.Where(a => a.CampEndDate.CompareTo(query.CampEndDate) == 0);
            if (query.ReadOnly != null) baseQuery = baseQuery.Where(a => a.ReadOnly == query.ReadOnly);
            if (query.CodeCampDateString != null) baseQuery = baseQuery.Where(a => a.CodeCampDateString.ToLower().Equals(query.CodeCampDateString.ToLower()));
            if (query.CodeCampSaturdayString != null) baseQuery = baseQuery.Where(a => a.CodeCampSaturdayString.ToLower().Equals(query.CodeCampSaturdayString.ToLower()));
            if (query.CodeCampSundayString != null) baseQuery = baseQuery.Where(a => a.CodeCampSundayString.ToLower().Equals(query.CodeCampSundayString.ToLower()));

            return baseQuery;
        }
        
    }
}
