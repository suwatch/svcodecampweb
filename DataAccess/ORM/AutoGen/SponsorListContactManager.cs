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
    public partial class SponsorListContactManager : ManagerBase<SponsorListContactManager, SponsorListContactResult, SponsorListContact, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(SponsorListContact record, SponsorListContactResult result)
        {
            record.SponsorListId = result.SponsorListId;
            record.SponsorListContactTypeId = result.SponsorListContactTypeId;
            record.ContactType = result.ContactType;
            record.EmailAddress = result.EmailAddress;
            record.FirstName = result.FirstName;
            record.LastName = result.LastName;
            record.PhoneNumberOffice = result.PhoneNumberOffice;
            record.PhoneNumberCell = result.PhoneNumberCell;
            record.Comment = result.Comment;
            record.DateCreated = result.DateCreated;
            record.GeneralCCMailings = result.GeneralCCMailings;
            record.SponsorCCMailings = result.SponsorCCMailings;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override SponsorListContact GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.SponsorListContact where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<SponsorListContactResult> GetBaseResultIQueryable(IQueryable<SponsorListContact> baseQuery)
        {
      IQueryable<SponsorListContactResult> results = (from myData in baseQuery orderby myData.Id select new SponsorListContactResult { Id= myData.Id,
            SponsorListId = myData.SponsorListId,
            SponsorListContactTypeId = myData.SponsorListContactTypeId,
            ContactType = myData.ContactType,
            EmailAddress = myData.EmailAddress,
            FirstName = myData.FirstName,
            LastName = myData.LastName,
            PhoneNumberOffice = myData.PhoneNumberOffice,
            PhoneNumberCell = myData.PhoneNumberCell,
            Comment = myData.Comment,
            DateCreated = new DateTime(myData.DateCreated.Ticks,DateTimeKind.Utc),
            GeneralCCMailings = myData.GeneralCCMailings,
            SponsorCCMailings = myData.SponsorCCMailings
      });
		    return results;
        }
        
        public List<SponsorListContactResult> GetJustBaseTableColumns(SponsorListContactQuery query)
        {
            foreach (var info in typeof (SponsorListContactQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: SponsorListContact QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<SponsorListContact> baseQuery = from myData in meta.SponsorListContact select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<SponsorListContactResult> results = (from myData in baseQuery orderby myData.Id select new SponsorListContactResult { Id= myData.Id,
                        SponsorListId = myData.SponsorListId,
                        SponsorListContactTypeId = myData.SponsorListContactTypeId,
                        ContactType = myData.ContactType,
                        EmailAddress = myData.EmailAddress,
                        FirstName = myData.FirstName,
                        LastName = myData.LastName,
                        PhoneNumberOffice = myData.PhoneNumberOffice,
                        PhoneNumberCell = myData.PhoneNumberCell,
                        Comment = myData.Comment,
                        DateCreated = new DateTime(myData.DateCreated.Ticks,DateTimeKind.Utc),
                        GeneralCCMailings = myData.GeneralCCMailings,
                        SponsorCCMailings = myData.SponsorCCMailings
            });
            
            List<SponsorListContactResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<SponsorListContact> BaseQueryAutoGen(IQueryable<SponsorListContact> baseQuery, SponsorListContactQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.SponsorListId != null) baseQuery = baseQuery.Where(a => a.SponsorListId == query.SponsorListId);
            if (query.SponsorListContactTypeId != null) baseQuery = baseQuery.Where(a => a.SponsorListContactTypeId == query.SponsorListContactTypeId);
            if (query.ContactType != null) baseQuery = baseQuery.Where(a => a.ContactType.ToLower().Equals(query.ContactType.ToLower()));
            if (query.EmailAddress != null) baseQuery = baseQuery.Where(a => a.EmailAddress.ToLower().Equals(query.EmailAddress.ToLower()));
            if (query.FirstName != null) baseQuery = baseQuery.Where(a => a.FirstName.ToLower().Equals(query.FirstName.ToLower()));
            if (query.LastName != null) baseQuery = baseQuery.Where(a => a.LastName.ToLower().Equals(query.LastName.ToLower()));
            if (query.PhoneNumberOffice != null) baseQuery = baseQuery.Where(a => a.PhoneNumberOffice.ToLower().Equals(query.PhoneNumberOffice.ToLower()));
            if (query.PhoneNumberCell != null) baseQuery = baseQuery.Where(a => a.PhoneNumberCell.ToLower().Equals(query.PhoneNumberCell.ToLower()));
            if (query.Comment != null) baseQuery = baseQuery.Where(a => a.Comment.ToLower().Equals(query.Comment.ToLower()));
            if (query.DateCreated != null) baseQuery = baseQuery.Where(a => a.DateCreated.CompareTo(query.DateCreated) == 0);
            if (query.GeneralCCMailings != null) baseQuery = baseQuery.Where(a => a.GeneralCCMailings == query.GeneralCCMailings);
            if (query.SponsorCCMailings != null) baseQuery = baseQuery.Where(a => a.SponsorCCMailings == query.SponsorCCMailings);

            return baseQuery;
        }
        
    }
}
