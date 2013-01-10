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
    public partial class VolunteerJobManager : ManagerBase<VolunteerJobManager, VolunteerJobResult, VolunteerJob, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(VolunteerJob record, VolunteerJobResult result)
        {
            record.CodeCampYearId = result.CodeCampYearId;
            record.Description = result.Description;
            record.JobStartTime = result.JobStartTime;
            record.JobEndTime = result.JobEndTime;
            record.NumberNeeded = result.NumberNeeded;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override VolunteerJob GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.VolunteerJob where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<VolunteerJobResult> GetBaseResultIQueryable(IQueryable<VolunteerJob> baseQuery)
        {
      IQueryable<VolunteerJobResult> results = (from myData in baseQuery orderby myData.Id select new VolunteerJobResult { Id= myData.Id,
            CodeCampYearId = myData.CodeCampYearId,
            Description = myData.Description,
            JobStartTime = new DateTime(myData.JobStartTime.Ticks,DateTimeKind.Utc),
            JobEndTime = new DateTime(myData.JobEndTime.Ticks,DateTimeKind.Utc),
            NumberNeeded = myData.NumberNeeded
      });
		    return results;
        }
        
        public List<VolunteerJobResult> GetJustBaseTableColumns(VolunteerJobQuery query)
        {
            foreach (var info in typeof (VolunteerJobQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: VolunteerJob QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<VolunteerJob> baseQuery = from myData in meta.VolunteerJob select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<VolunteerJobResult> results = (from myData in baseQuery orderby myData.Id select new VolunteerJobResult { Id= myData.Id,
                        CodeCampYearId = myData.CodeCampYearId,
                        Description = myData.Description,
                        JobStartTime = new DateTime(myData.JobStartTime.Ticks,DateTimeKind.Utc),
                        JobEndTime = new DateTime(myData.JobEndTime.Ticks,DateTimeKind.Utc),
                        NumberNeeded = myData.NumberNeeded
            });
            
            List<VolunteerJobResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<VolunteerJob> BaseQueryAutoGen(IQueryable<VolunteerJob> baseQuery, VolunteerJobQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.CodeCampYearId != null) baseQuery = baseQuery.Where(a => a.CodeCampYearId == query.CodeCampYearId);
            if (query.Description != null) baseQuery = baseQuery.Where(a => a.Description.ToLower().Equals(query.Description.ToLower()));
            if (query.JobStartTime != null) baseQuery = baseQuery.Where(a => a.JobStartTime.CompareTo(query.JobStartTime) == 0);
            if (query.JobEndTime != null) baseQuery = baseQuery.Where(a => a.JobEndTime.CompareTo(query.JobEndTime) == 0);
            if (query.NumberNeeded != null) baseQuery = baseQuery.Where(a => a.NumberNeeded == query.NumberNeeded);

            return baseQuery;
        }
        
    }
}
