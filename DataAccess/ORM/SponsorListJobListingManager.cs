using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampSV;
using System.ComponentModel;

namespace CodeCampSV
{
    public partial class SponsorListJobListingManager
    {
        //  public override void Insert(LoadResult result)
        //  {
        //     base.Insert(result);
        //     if (result.Cargos != null && result.Cargos.Count > 0)
        //     {
        //        foreach (CargoResult c in result.Cargos)
        //        {
        //            c.LoadId = result.Id;
        //            CargoManager.I.Insert(c);
        //         }
        //      }
        //  }
        // 
        //  public override void Update(LoadResult result)
        //  {
        //      base.Update(result);
        //      if (result.Cargos != null && result.Cargos.Count > 0)
        //      {
        //          CargoManager.I.Update(result.Cargos);
        //      }
        //  }

        public List<SponsorListJobListingResult> Get(SponsorListJobListingQuery query)
        {

            var meta = new CodeCampDataContext();

            IQueryable<SponsorListJobListing> baseQuery = from myData in meta.SponsorListJobListing select myData;

            DateTime today = DateTime.UtcNow;

            //var result = meta.Meeting.Where(d => today >= start
            //                                   && d.MeetingDate <= end);

            if (query.Top5ActiveListings.HasValue && query.Top5ActiveListings.Value)
            {
                baseQuery =
                    baseQuery.Where(
                        data => today >= data.StartRunDate &&
                                today <= data.EndRunDate && data.HideListing == false).Take(5);
            }

            if (query.Top5ForTesting.HasValue && query.Top5ForTesting.Value)
            {
                baseQuery =
                    baseQuery.Where(
                        data => data.HideListing == false)
                             .OrderByDescending(a => a.StartRunDate)
                             .Take(5);
            }






            //  next is automated query formation from AutoGen Shared Class 
            //  (do not remove next line or filters will go away)
            baseQuery = BaseQueryAutoGen(baseQuery, query);

            IQueryable<SponsorListJobListingResult> results = GetBaseResultIQueryable(baseQuery);




            List<SponsorListJobListingResult> resultList = GetFinalResults(results, query);

            foreach (var job in resultList)
            {
                job.JobDateFriendly = String.Format("{0:mm/d/yy}", job.StartRunDate);
            }

            //var recs = from data in meta.SessionsJobListing
            //           select data.SessionId
            //           where 




            //  Put Stuff Here if you want to load another result
            //  The following is done AFTER GetFinalResults so that we don't waste machine cycles sucking in all the
            //  addresses for all results returned, just the ones that are actually being returned.
            //  if (query.WithAddress != null && query.WithAddress == true)
            //  {
            //     foreach (var r in companyResultList)
            //     {
            //         r.CompanyAddressResultList =
            //             CompanyAddressManager.I.Get(new CompanyAddressQuery { CompanyIds = query.Ids, WithAddress = true });
            //     }
            //  }
            //             
            return resultList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<SponsorListJobListingResult> GetAll()
        {
            return Get(new SponsorListJobListingQuery {IsMaterializeResult = true});
        }
    }
}
