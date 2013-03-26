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
    public partial class SponsorRequestManager : ManagerBase<SponsorRequestManager, SponsorRequestResult, SponsorRequest, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(SponsorRequest record, SponsorRequestResult result)
        {
            record.ContactEmail = result.ContactEmail;
            record.Company = result.Company;
            record.PhoneNumber = result.PhoneNumber;
            record.EmailMe = result.EmailMe;
            record.ContactMeByPhone = result.ContactMeByPhone;
            record.AlsoAttending = result.AlsoAttending;
            record.PastSponsor = result.PastSponsor;
            record.SponsorSpecialNotes = result.SponsorSpecialNotes;
            record.SvccNotes = result.SvccNotes;
            record.SvccRespondedTo = result.SvccRespondedTo;
            record.SvccEnteredInSystem = result.SvccEnteredInSystem;
            record.CreateDate = result.CreateDate;
            record.FoothillWorkStudyCheckBox = result.FoothillWorkStudyCheckBox;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override SponsorRequest GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.SponsorRequest where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<SponsorRequestResult> GetBaseResultIQueryable(IQueryable<SponsorRequest> baseQuery)
        {
      IQueryable<SponsorRequestResult> results = (from myData in baseQuery orderby myData.Id select new SponsorRequestResult { Id= myData.Id,
            ContactEmail = myData.ContactEmail,
            Company = myData.Company,
            PhoneNumber = myData.PhoneNumber,
            EmailMe = myData.EmailMe,
            ContactMeByPhone = myData.ContactMeByPhone,
            AlsoAttending = myData.AlsoAttending,
            PastSponsor = myData.PastSponsor,
            SponsorSpecialNotes = myData.SponsorSpecialNotes,
            SvccNotes = myData.SvccNotes,
            SvccRespondedTo = myData.SvccRespondedTo,
            SvccEnteredInSystem = myData.SvccEnteredInSystem,
            CreateDate = myData.CreateDate == null ? null :  (DateTime?) new DateTime(myData.CreateDate.Value.Ticks,DateTimeKind.Utc),
            FoothillWorkStudyCheckBox = myData.FoothillWorkStudyCheckBox
      });
		    return results;
        }
        
        public List<SponsorRequestResult> GetJustBaseTableColumns(SponsorRequestQuery query)
        {
            foreach (var info in typeof (SponsorRequestQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: SponsorRequest QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<SponsorRequest> baseQuery = from myData in meta.SponsorRequest select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<SponsorRequestResult> results = (from myData in baseQuery orderby myData.Id select new SponsorRequestResult { Id= myData.Id,
                        ContactEmail = myData.ContactEmail,
                        Company = myData.Company,
                        PhoneNumber = myData.PhoneNumber,
                        EmailMe = myData.EmailMe,
                        ContactMeByPhone = myData.ContactMeByPhone,
                        AlsoAttending = myData.AlsoAttending,
                        PastSponsor = myData.PastSponsor,
                        SponsorSpecialNotes = myData.SponsorSpecialNotes,
                        SvccNotes = myData.SvccNotes,
                        SvccRespondedTo = myData.SvccRespondedTo,
                        SvccEnteredInSystem = myData.SvccEnteredInSystem,
                        CreateDate = myData.CreateDate == null ? null :  (DateTime?) new DateTime(myData.CreateDate.Value.Ticks,DateTimeKind.Utc),
                        FoothillWorkStudyCheckBox = myData.FoothillWorkStudyCheckBox
            });
            
            List<SponsorRequestResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<SponsorRequest> BaseQueryAutoGen(IQueryable<SponsorRequest> baseQuery, SponsorRequestQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.ContactEmail != null) baseQuery = baseQuery.Where(a => a.ContactEmail.ToLower().Equals(query.ContactEmail.ToLower()));
            if (query.Company != null) baseQuery = baseQuery.Where(a => a.Company.ToLower().Equals(query.Company.ToLower()));
            if (query.PhoneNumber != null) baseQuery = baseQuery.Where(a => a.PhoneNumber.ToLower().Equals(query.PhoneNumber.ToLower()));
            if (query.EmailMe != null) baseQuery = baseQuery.Where(a => a.EmailMe == query.EmailMe);
            if (query.ContactMeByPhone != null) baseQuery = baseQuery.Where(a => a.ContactMeByPhone == query.ContactMeByPhone);
            if (query.AlsoAttending != null) baseQuery = baseQuery.Where(a => a.AlsoAttending == query.AlsoAttending);
            if (query.PastSponsor != null) baseQuery = baseQuery.Where(a => a.PastSponsor == query.PastSponsor);
            if (query.SponsorSpecialNotes != null) baseQuery = baseQuery.Where(a => a.SponsorSpecialNotes.ToLower().Equals(query.SponsorSpecialNotes.ToLower()));
            if (query.SvccNotes != null) baseQuery = baseQuery.Where(a => a.SvccNotes.ToLower().Equals(query.SvccNotes.ToLower()));
            if (query.SvccRespondedTo != null) baseQuery = baseQuery.Where(a => a.SvccRespondedTo == query.SvccRespondedTo);
            if (query.SvccEnteredInSystem != null) baseQuery = baseQuery.Where(a => a.SvccEnteredInSystem == query.SvccEnteredInSystem);
            if (query.CreateDate != null) baseQuery = baseQuery.Where(a => a.CreateDate.Value.CompareTo(query.CreateDate.Value) == 0);
            if (query.FoothillWorkStudyCheckBox != null) baseQuery = baseQuery.Where(a => a.FoothillWorkStudyCheckBox == query.FoothillWorkStudyCheckBox);

            return baseQuery;
        }
        
    }
}
