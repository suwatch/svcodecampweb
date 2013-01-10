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
    public partial class AttendeeVolunteerManager : ManagerBase<AttendeeVolunteerManager, AttendeeVolunteerResult, AttendeeVolunteer, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(AttendeeVolunteer record, AttendeeVolunteerResult result)
        {
            record.AttendeeId = result.AttendeeId;
            record.VolunteerJobId = result.VolunteerJobId;
            record.Notes = result.Notes;
            record.CreatedDate = result.CreatedDate;
            record.LastUpdated = result.LastUpdated;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override AttendeeVolunteer GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.AttendeeVolunteer where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<AttendeeVolunteerResult> GetBaseResultIQueryable(IQueryable<AttendeeVolunteer> baseQuery)
        {
      IQueryable<AttendeeVolunteerResult> results = (from myData in baseQuery orderby myData.Id select new AttendeeVolunteerResult { Id= myData.Id,
            AttendeeId = myData.AttendeeId,
            VolunteerJobId = myData.VolunteerJobId,
            Notes = myData.Notes,
            CreatedDate = myData.CreatedDate == null ? null :  (DateTime?) new DateTime(myData.CreatedDate.Value.Ticks,DateTimeKind.Utc),
            LastUpdated = myData.LastUpdated == null ? null :  (DateTime?) new DateTime(myData.LastUpdated.Value.Ticks,DateTimeKind.Utc)
      });
		    return results;
        }
        
        public List<AttendeeVolunteerResult> GetJustBaseTableColumns(AttendeeVolunteerQuery query)
        {
            foreach (var info in typeof (AttendeeVolunteerQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: AttendeeVolunteer QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<AttendeeVolunteer> baseQuery = from myData in meta.AttendeeVolunteer select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<AttendeeVolunteerResult> results = (from myData in baseQuery orderby myData.Id select new AttendeeVolunteerResult { Id= myData.Id,
                        AttendeeId = myData.AttendeeId,
                        VolunteerJobId = myData.VolunteerJobId,
                        Notes = myData.Notes,
                        CreatedDate = myData.CreatedDate == null ? null :  (DateTime?) new DateTime(myData.CreatedDate.Value.Ticks,DateTimeKind.Utc),
                        LastUpdated = myData.LastUpdated == null ? null :  (DateTime?) new DateTime(myData.LastUpdated.Value.Ticks,DateTimeKind.Utc)
            });
            
            List<AttendeeVolunteerResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<AttendeeVolunteer> BaseQueryAutoGen(IQueryable<AttendeeVolunteer> baseQuery, AttendeeVolunteerQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.AttendeeId != null) baseQuery = baseQuery.Where(a => a.AttendeeId == query.AttendeeId);
            if (query.VolunteerJobId != null) baseQuery = baseQuery.Where(a => a.VolunteerJobId == query.VolunteerJobId);
            if (query.Notes != null) baseQuery = baseQuery.Where(a => a.Notes.ToLower().Equals(query.Notes.ToLower()));
            if (query.CreatedDate != null) baseQuery = baseQuery.Where(a => a.CreatedDate.Value.CompareTo(query.CreatedDate.Value) == 0);
            if (query.LastUpdated != null) baseQuery = baseQuery.Where(a => a.LastUpdated.Value.CompareTo(query.LastUpdated.Value) == 0);

            return baseQuery;
        }
        
    }
}
