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
    public partial class SponsorListCodeCampYearManager : ManagerBase<SponsorListCodeCampYearManager, SponsorListCodeCampYearResult, SponsorListCodeCampYear, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(SponsorListCodeCampYear record, SponsorListCodeCampYearResult result)
        {
            record.CodeCampYearId = result.CodeCampYearId;
            record.SponsorListId = result.SponsorListId;
            record.DonationAmount = result.DonationAmount;
            record.SortIndex = result.SortIndex;
            record.Visible = result.Visible;
            record.Status = result.Status;
            record.NextActionDate = result.NextActionDate;
            record.WhoOwns = result.WhoOwns;
            record.TableRequired = result.TableRequired;
            record.AttendeeBagItem = result.AttendeeBagItem;
            record.ItemSentToUPS = result.ItemSentToUPS;
            record.ItemSentToFoothill = result.ItemSentToFoothill;
            record.Comments = result.Comments;
            record.ItemsShippingDescription = result.ItemsShippingDescription;
            record.NoteFromCodeCamp = result.NoteFromCodeCamp;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override SponsorListCodeCampYear GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.SponsorListCodeCampYear where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<SponsorListCodeCampYearResult> GetBaseResultIQueryable(IQueryable<SponsorListCodeCampYear> baseQuery)
        {
      IQueryable<SponsorListCodeCampYearResult> results = (from myData in baseQuery orderby myData.Id select new SponsorListCodeCampYearResult { Id= myData.Id,
            CodeCampYearId = myData.CodeCampYearId,
            SponsorListId = myData.SponsorListId,
            DonationAmount = myData.DonationAmount,
            SortIndex = myData.SortIndex,
            Visible = myData.Visible,
            Status = myData.Status,
            NextActionDate = myData.NextActionDate == null ? null :  (DateTime?) new DateTime(myData.NextActionDate.Value.Ticks,DateTimeKind.Utc),
            WhoOwns = myData.WhoOwns,
            TableRequired = myData.TableRequired,
            AttendeeBagItem = myData.AttendeeBagItem,
            ItemSentToUPS = myData.ItemSentToUPS,
            ItemSentToFoothill = myData.ItemSentToFoothill,
            Comments = myData.Comments,
            ItemsShippingDescription = myData.ItemsShippingDescription,
            NoteFromCodeCamp = myData.NoteFromCodeCamp
      });
		    return results;
        }
        
        public List<SponsorListCodeCampYearResult> GetJustBaseTableColumns(SponsorListCodeCampYearQuery query)
        {
            foreach (var info in typeof (SponsorListCodeCampYearQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: SponsorListCodeCampYear QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<SponsorListCodeCampYear> baseQuery = from myData in meta.SponsorListCodeCampYear select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<SponsorListCodeCampYearResult> results = (from myData in baseQuery orderby myData.Id select new SponsorListCodeCampYearResult { Id= myData.Id,
                        CodeCampYearId = myData.CodeCampYearId,
                        SponsorListId = myData.SponsorListId,
                        DonationAmount = myData.DonationAmount,
                        SortIndex = myData.SortIndex,
                        Visible = myData.Visible,
                        Status = myData.Status,
                        NextActionDate = myData.NextActionDate == null ? null :  (DateTime?) new DateTime(myData.NextActionDate.Value.Ticks,DateTimeKind.Utc),
                        WhoOwns = myData.WhoOwns,
                        TableRequired = myData.TableRequired,
                        AttendeeBagItem = myData.AttendeeBagItem,
                        ItemSentToUPS = myData.ItemSentToUPS,
                        ItemSentToFoothill = myData.ItemSentToFoothill,
                        Comments = myData.Comments,
                        ItemsShippingDescription = myData.ItemsShippingDescription,
                        NoteFromCodeCamp = myData.NoteFromCodeCamp
            });
            
            List<SponsorListCodeCampYearResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<SponsorListCodeCampYear> BaseQueryAutoGen(IQueryable<SponsorListCodeCampYear> baseQuery, SponsorListCodeCampYearQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.CodeCampYearId != null) baseQuery = baseQuery.Where(a => a.CodeCampYearId == query.CodeCampYearId);
            if (query.SponsorListId != null) baseQuery = baseQuery.Where(a => a.SponsorListId == query.SponsorListId);
            if (query.DonationAmount != null) baseQuery = baseQuery.Where(a => a.DonationAmount == query.DonationAmount);
            if (query.SortIndex != null) baseQuery = baseQuery.Where(a => a.SortIndex == query.SortIndex);
            if (query.Visible != null) baseQuery = baseQuery.Where(a => a.Visible == query.Visible);
            if (query.Status != null) baseQuery = baseQuery.Where(a => a.Status.ToLower().Equals(query.Status.ToLower()));
            if (query.NextActionDate != null) baseQuery = baseQuery.Where(a => a.NextActionDate.Value.CompareTo(query.NextActionDate.Value) == 0);
            if (query.WhoOwns != null) baseQuery = baseQuery.Where(a => a.WhoOwns.ToLower().Equals(query.WhoOwns.ToLower()));
            if (query.TableRequired != null) baseQuery = baseQuery.Where(a => a.TableRequired == query.TableRequired);
            if (query.AttendeeBagItem != null) baseQuery = baseQuery.Where(a => a.AttendeeBagItem == query.AttendeeBagItem);
            if (query.ItemSentToUPS != null) baseQuery = baseQuery.Where(a => a.ItemSentToUPS == query.ItemSentToUPS);
            if (query.ItemSentToFoothill != null) baseQuery = baseQuery.Where(a => a.ItemSentToFoothill == query.ItemSentToFoothill);
            if (query.Comments != null) baseQuery = baseQuery.Where(a => a.Comments.ToLower().Equals(query.Comments.ToLower()));
            if (query.ItemsShippingDescription != null) baseQuery = baseQuery.Where(a => a.ItemsShippingDescription.ToLower().Equals(query.ItemsShippingDescription.ToLower()));
            if (query.NoteFromCodeCamp != null) baseQuery = baseQuery.Where(a => a.NoteFromCodeCamp.ToLower().Equals(query.NoteFromCodeCamp.ToLower()));

            return baseQuery;
        }
        
    }
}
