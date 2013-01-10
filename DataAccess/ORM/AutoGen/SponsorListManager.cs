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
    public partial class SponsorListManager : ManagerBase<SponsorListManager, SponsorListResult, SponsorList, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(SponsorList record, SponsorListResult result)
        {
            record.SponsorName = result.SponsorName;
            record.NextActionDate = result.NextActionDate;
            record.ImageURL = result.ImageURL;
            record.NavigateURL = result.NavigateURL;
            record.HoverOverText = result.HoverOverText;
            record.UnderLogoText = result.UnderLogoText;
            record.Comment = result.Comment;
            record.CompanyDescriptionShort = result.CompanyDescriptionShort;
            record.CompanyDescriptionLong = result.CompanyDescriptionLong;
            record.CompanyAddressLine1 = result.CompanyAddressLine1;
            record.CompanyAddressLine2 = result.CompanyAddressLine2;
            record.CompanyCity = result.CompanyCity;
            record.CompanyState = result.CompanyState;
            record.CompanyZip = result.CompanyZip;
            record.CompanyPhoneNumber = result.CompanyPhoneNumber;
            record.CompanyImage = result.CompanyImage;
            record.CreationDate = result.CreationDate;
            record.ModifiedDate = result.ModifiedDate;
            record.CompanyImageType = result.CompanyImageType;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override SponsorList GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.SponsorList where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<SponsorListResult> GetBaseResultIQueryable(IQueryable<SponsorList> baseQuery)
        {
      IQueryable<SponsorListResult> results = (from myData in baseQuery orderby myData.Id select new SponsorListResult { Id= myData.Id,
            SponsorName = myData.SponsorName,
            NextActionDate = myData.NextActionDate == null ? null :  (DateTime?) new DateTime(myData.NextActionDate.Value.Ticks,DateTimeKind.Utc),
            ImageURL = myData.ImageURL,
            NavigateURL = myData.NavigateURL,
            HoverOverText = myData.HoverOverText,
            UnderLogoText = myData.UnderLogoText,
            Comment = myData.Comment,
            CompanyDescriptionShort = myData.CompanyDescriptionShort,
            CompanyDescriptionLong = myData.CompanyDescriptionLong,
            CompanyAddressLine1 = myData.CompanyAddressLine1,
            CompanyAddressLine2 = myData.CompanyAddressLine2,
            CompanyCity = myData.CompanyCity,
            CompanyState = myData.CompanyState,
            CompanyZip = myData.CompanyZip,
            CompanyPhoneNumber = myData.CompanyPhoneNumber,
            CompanyImage = myData.CompanyImage,
            CreationDate = myData.CreationDate == null ? null :  (DateTime?) new DateTime(myData.CreationDate.Value.Ticks,DateTimeKind.Utc),
            ModifiedDate = myData.ModifiedDate == null ? null :  (DateTime?) new DateTime(myData.ModifiedDate.Value.Ticks,DateTimeKind.Utc),
            CompanyImageType = myData.CompanyImageType
      });
		    return results;
        }
        
        public List<SponsorListResult> GetJustBaseTableColumns(SponsorListQuery query)
        {
            foreach (var info in typeof (SponsorListQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: SponsorList QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<SponsorList> baseQuery = from myData in meta.SponsorList select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<SponsorListResult> results = (from myData in baseQuery orderby myData.Id select new SponsorListResult { Id= myData.Id,
                        SponsorName = myData.SponsorName,
                        NextActionDate = myData.NextActionDate == null ? null :  (DateTime?) new DateTime(myData.NextActionDate.Value.Ticks,DateTimeKind.Utc),
                        ImageURL = myData.ImageURL,
                        NavigateURL = myData.NavigateURL,
                        HoverOverText = myData.HoverOverText,
                        UnderLogoText = myData.UnderLogoText,
                        Comment = myData.Comment,
                        CompanyDescriptionShort = myData.CompanyDescriptionShort,
                        CompanyDescriptionLong = myData.CompanyDescriptionLong,
                        CompanyAddressLine1 = myData.CompanyAddressLine1,
                        CompanyAddressLine2 = myData.CompanyAddressLine2,
                        CompanyCity = myData.CompanyCity,
                        CompanyState = myData.CompanyState,
                        CompanyZip = myData.CompanyZip,
                        CompanyPhoneNumber = myData.CompanyPhoneNumber,
                        CompanyImage = myData.CompanyImage,
                        CreationDate = myData.CreationDate == null ? null :  (DateTime?) new DateTime(myData.CreationDate.Value.Ticks,DateTimeKind.Utc),
                        ModifiedDate = myData.ModifiedDate == null ? null :  (DateTime?) new DateTime(myData.ModifiedDate.Value.Ticks,DateTimeKind.Utc),
                        CompanyImageType = myData.CompanyImageType
            });
            
            List<SponsorListResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<SponsorList> BaseQueryAutoGen(IQueryable<SponsorList> baseQuery, SponsorListQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.SponsorName != null) baseQuery = baseQuery.Where(a => a.SponsorName.ToLower().Equals(query.SponsorName.ToLower()));
            if (query.NextActionDate != null) baseQuery = baseQuery.Where(a => a.NextActionDate.Value.CompareTo(query.NextActionDate.Value) == 0);
            if (query.ImageURL != null) baseQuery = baseQuery.Where(a => a.ImageURL.ToLower().Equals(query.ImageURL.ToLower()));
            if (query.NavigateURL != null) baseQuery = baseQuery.Where(a => a.NavigateURL.ToLower().Equals(query.NavigateURL.ToLower()));
            if (query.HoverOverText != null) baseQuery = baseQuery.Where(a => a.HoverOverText.ToLower().Equals(query.HoverOverText.ToLower()));
            if (query.UnderLogoText != null) baseQuery = baseQuery.Where(a => a.UnderLogoText.ToLower().Equals(query.UnderLogoText.ToLower()));
            if (query.Comment != null) baseQuery = baseQuery.Where(a => a.Comment.ToLower().Equals(query.Comment.ToLower()));
            if (query.CompanyDescriptionShort != null) baseQuery = baseQuery.Where(a => a.CompanyDescriptionShort.ToLower().Equals(query.CompanyDescriptionShort.ToLower()));
            if (query.CompanyDescriptionLong != null) baseQuery = baseQuery.Where(a => a.CompanyDescriptionLong.ToLower().Equals(query.CompanyDescriptionLong.ToLower()));
            if (query.CompanyAddressLine1 != null) baseQuery = baseQuery.Where(a => a.CompanyAddressLine1.ToLower().Equals(query.CompanyAddressLine1.ToLower()));
            if (query.CompanyAddressLine2 != null) baseQuery = baseQuery.Where(a => a.CompanyAddressLine2.ToLower().Equals(query.CompanyAddressLine2.ToLower()));
            if (query.CompanyCity != null) baseQuery = baseQuery.Where(a => a.CompanyCity.ToLower().Equals(query.CompanyCity.ToLower()));
            if (query.CompanyState != null) baseQuery = baseQuery.Where(a => a.CompanyState.ToLower().Equals(query.CompanyState.ToLower()));
            if (query.CompanyZip != null) baseQuery = baseQuery.Where(a => a.CompanyZip.ToLower().Equals(query.CompanyZip.ToLower()));
            if (query.CompanyPhoneNumber != null) baseQuery = baseQuery.Where(a => a.CompanyPhoneNumber.ToLower().Equals(query.CompanyPhoneNumber.ToLower()));
            if (query.CreationDate != null) baseQuery = baseQuery.Where(a => a.CreationDate.Value.CompareTo(query.CreationDate.Value) == 0);
            if (query.ModifiedDate != null) baseQuery = baseQuery.Where(a => a.ModifiedDate.Value.CompareTo(query.ModifiedDate.Value) == 0);
            if (query.CompanyImageType != null) baseQuery = baseQuery.Where(a => a.CompanyImageType.ToLower().Equals(query.CompanyImageType.ToLower()));

            return baseQuery;
        }
        
    }
}
