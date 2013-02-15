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
    public partial class AttendeesCodeCampYearManager : ManagerBase<AttendeesCodeCampYearManager, AttendeesCodeCampYearResult, AttendeesCodeCampYear, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(AttendeesCodeCampYear record, AttendeesCodeCampYearResult result)
        {
            record.AttendeesId = result.AttendeesId;
            record.CodeCampYearId = result.CodeCampYearId;
            record.AttendSaturday = result.AttendSaturday;
            record.AttendSunday = result.AttendSunday;
            record.Volunteer = result.Volunteer;
            record.CreateDate = result.CreateDate;
            record.AttendingDaysChoice = result.AttendingDaysChoice;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override AttendeesCodeCampYear GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.AttendeesCodeCampYear where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<AttendeesCodeCampYearResult> GetBaseResultIQueryable(IQueryable<AttendeesCodeCampYear> baseQuery)
        {
      IQueryable<AttendeesCodeCampYearResult> results = (from myData in baseQuery orderby myData.Id select new AttendeesCodeCampYearResult { Id= myData.Id,
            AttendeesId = myData.AttendeesId,
            CodeCampYearId = myData.CodeCampYearId,
            AttendSaturday = myData.AttendSaturday,
            AttendSunday = myData.AttendSunday,
            Volunteer = myData.Volunteer,
            CreateDate = myData.CreateDate == null ? null :  (DateTime?) new DateTime(myData.CreateDate.Value.Ticks,DateTimeKind.Utc),
            AttendingDaysChoice = myData.AttendingDaysChoice
      });
		    return results;
        }
        
        public List<AttendeesCodeCampYearResult> GetJustBaseTableColumns(AttendeesCodeCampYearQuery query)
        {
            foreach (var info in typeof (AttendeesCodeCampYearQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: AttendeesCodeCampYear QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<AttendeesCodeCampYear> baseQuery = from myData in meta.AttendeesCodeCampYear select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<AttendeesCodeCampYearResult> results = (from myData in baseQuery orderby myData.Id select new AttendeesCodeCampYearResult { Id= myData.Id,
                        AttendeesId = myData.AttendeesId,
                        CodeCampYearId = myData.CodeCampYearId,
                        AttendSaturday = myData.AttendSaturday,
                        AttendSunday = myData.AttendSunday,
                        Volunteer = myData.Volunteer,
                        CreateDate = myData.CreateDate == null ? null :  (DateTime?) new DateTime(myData.CreateDate.Value.Ticks,DateTimeKind.Utc),
                        AttendingDaysChoice = myData.AttendingDaysChoice
            });
            
            List<AttendeesCodeCampYearResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<AttendeesCodeCampYear> BaseQueryAutoGen(IQueryable<AttendeesCodeCampYear> baseQuery, AttendeesCodeCampYearQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.AttendeesId != null) baseQuery = baseQuery.Where(a => a.AttendeesId == query.AttendeesId);
            if (query.CodeCampYearId != null) baseQuery = baseQuery.Where(a => a.CodeCampYearId == query.CodeCampYearId);
            if (query.AttendSaturday != null) baseQuery = baseQuery.Where(a => a.AttendSaturday == query.AttendSaturday);
            if (query.AttendSunday != null) baseQuery = baseQuery.Where(a => a.AttendSunday == query.AttendSunday);
            if (query.Volunteer != null) baseQuery = baseQuery.Where(a => a.Volunteer == query.Volunteer);
            if (query.CreateDate != null) baseQuery = baseQuery.Where(a => a.CreateDate.Value.CompareTo(query.CreateDate.Value) == 0);
            if (query.AttendingDaysChoice != null) baseQuery = baseQuery.Where(a => a.AttendingDaysChoice.ToLower().Equals(query.AttendingDaysChoice.ToLower()));

            return baseQuery;
        }
        
    }
}
