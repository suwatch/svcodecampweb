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
    public partial class SponsorListJobListingManager : ManagerBase<SponsorListJobListingManager, SponsorListJobListingResult, SponsorListJobListing, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(SponsorListJobListing record, SponsorListJobListingResult result)
        {
            record.SponsorListId = result.SponsorListId;
            record.JobName = result.JobName;
            record.JobLocation = result.JobLocation;
            record.JobURL = result.JobURL;
            record.JobBrief = result.JobBrief;
            record.JobTagline = result.JobTagline;
            record.JobButtonToolTip = result.JobButtonToolTip;
            record.EnteredDate = result.EnteredDate;
            record.JobCompanyName = result.JobCompanyName;
            record.StartRunDate = result.StartRunDate;
            record.EndRunDate = result.EndRunDate;
            record.HideListing = result.HideListing;
            record.Notes = result.Notes;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override SponsorListJobListing GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.SponsorListJobListing where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<SponsorListJobListingResult> GetBaseResultIQueryable(IQueryable<SponsorListJobListing> baseQuery)
        {
      IQueryable<SponsorListJobListingResult> results = (from myData in baseQuery orderby myData.Id select new SponsorListJobListingResult { Id= myData.Id,
            SponsorListId = myData.SponsorListId,
            JobName = myData.JobName,
            JobLocation = myData.JobLocation,
            JobURL = myData.JobURL,
            JobBrief = myData.JobBrief,
            JobTagline = myData.JobTagline,
            JobButtonToolTip = myData.JobButtonToolTip,
            EnteredDate = new DateTime(myData.EnteredDate.Ticks,DateTimeKind.Utc),
            JobCompanyName = myData.JobCompanyName,
            StartRunDate = myData.StartRunDate == null ? null :  (DateTime?) new DateTime(myData.StartRunDate.Value.Ticks,DateTimeKind.Utc),
            EndRunDate = myData.EndRunDate == null ? null :  (DateTime?) new DateTime(myData.EndRunDate.Value.Ticks,DateTimeKind.Utc),
            HideListing = myData.HideListing,
            Notes = myData.Notes
      });
		    return results;
        }
        
        public List<SponsorListJobListingResult> GetJustBaseTableColumns(SponsorListJobListingQuery query)
        {
            foreach (var info in typeof (SponsorListJobListingQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: SponsorListJobListing QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<SponsorListJobListing> baseQuery = from myData in meta.SponsorListJobListing select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<SponsorListJobListingResult> results = (from myData in baseQuery orderby myData.Id select new SponsorListJobListingResult { Id= myData.Id,
                        SponsorListId = myData.SponsorListId,
                        JobName = myData.JobName,
                        JobLocation = myData.JobLocation,
                        JobURL = myData.JobURL,
                        JobBrief = myData.JobBrief,
                        JobTagline = myData.JobTagline,
                        JobButtonToolTip = myData.JobButtonToolTip,
                        EnteredDate = new DateTime(myData.EnteredDate.Ticks,DateTimeKind.Utc),
                        JobCompanyName = myData.JobCompanyName,
                        StartRunDate = myData.StartRunDate == null ? null :  (DateTime?) new DateTime(myData.StartRunDate.Value.Ticks,DateTimeKind.Utc),
                        EndRunDate = myData.EndRunDate == null ? null :  (DateTime?) new DateTime(myData.EndRunDate.Value.Ticks,DateTimeKind.Utc),
                        HideListing = myData.HideListing,
                        Notes = myData.Notes
            });
            
            List<SponsorListJobListingResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<SponsorListJobListing> BaseQueryAutoGen(IQueryable<SponsorListJobListing> baseQuery, SponsorListJobListingQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.SponsorListId != null) baseQuery = baseQuery.Where(a => a.SponsorListId == query.SponsorListId);
            if (query.JobName != null) baseQuery = baseQuery.Where(a => a.JobName.ToLower().Equals(query.JobName.ToLower()));
            if (query.JobLocation != null) baseQuery = baseQuery.Where(a => a.JobLocation.ToLower().Equals(query.JobLocation.ToLower()));
            if (query.JobURL != null) baseQuery = baseQuery.Where(a => a.JobURL.ToLower().Equals(query.JobURL.ToLower()));
            if (query.JobBrief != null) baseQuery = baseQuery.Where(a => a.JobBrief.ToLower().Equals(query.JobBrief.ToLower()));
            if (query.JobTagline != null) baseQuery = baseQuery.Where(a => a.JobTagline.ToLower().Equals(query.JobTagline.ToLower()));
            if (query.JobButtonToolTip != null) baseQuery = baseQuery.Where(a => a.JobButtonToolTip.ToLower().Equals(query.JobButtonToolTip.ToLower()));
            if (query.EnteredDate != null) baseQuery = baseQuery.Where(a => a.EnteredDate.CompareTo(query.EnteredDate) == 0);
            if (query.JobCompanyName != null) baseQuery = baseQuery.Where(a => a.JobCompanyName.ToLower().Equals(query.JobCompanyName.ToLower()));
            if (query.StartRunDate != null) baseQuery = baseQuery.Where(a => a.StartRunDate.Value.CompareTo(query.StartRunDate.Value) == 0);
            if (query.EndRunDate != null) baseQuery = baseQuery.Where(a => a.EndRunDate.Value.CompareTo(query.EndRunDate.Value) == 0);
            if (query.HideListing != null) baseQuery = baseQuery.Where(a => a.HideListing == query.HideListing);
            if (query.Notes != null) baseQuery = baseQuery.Where(a => a.Notes.ToLower().Equals(query.Notes.ToLower()));

            return baseQuery;
        }
        
    }
}
