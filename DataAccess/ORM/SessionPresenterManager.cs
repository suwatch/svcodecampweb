using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampSV;
using System.ComponentModel;

namespace CodeCampSV
{
    public partial class SessionPresenterManager
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

        public List<SessionPresenterResult> Get(SessionPresenterQuery query)
        {

            var meta = new CodeCampDataContext();

            
            // add to codecampyearids
            if (query.CodeCampYearId.HasValue)
            {
                if (query.CodeCampYearIds != null && query.CodeCampYearIds.Count > 0)
                {
                    if (!query.CodeCampYearIds.Contains(query.CodeCampYearId.Value))
                    {
                        query.CodeCampYearIds.Add(query.CodeCampYearId.Value);
                    }
                }
                else
                {
                    query.CodeCampYearIds = new List<int> { query.CodeCampYearId.Value };
                }
            }

            IQueryable<SessionPresenter> baseQuery = from myData in meta.SessionPresenter select myData;

            if (query.CodeCampYearIds != null && query.CodeCampYearIds.Count > 0)
            {
                // sessionpresenter Ids for this year codecamp
                var sessionPresenterIds = (from data in meta.SessionPresenter
                                           join data1 in meta.Sessions on data.SessionId equals data1.Id
                                           where query.CodeCampYearIds.Contains(data1.CodeCampYearId)
                                           select data.Id).ToList();
                baseQuery = baseQuery.Where(data => sessionPresenterIds.Contains(data.Id));
            }




            //  next is automated query formation from AutoGen Shared Class 
            //  (do not remove next line or filters will go away)
            baseQuery = BaseQueryAutoGen(baseQuery, query);

            IQueryable<SessionPresenterResult> results = GetBaseResultIQueryable(baseQuery);

           


            List<SessionPresenterResult> resultList = GetFinalResults(results, query);

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
        public List<SessionPresenterResult> GetAll()
        {
            return Get(new SessionPresenterQuery {IsMaterializeResult = true});
        }
    }
}
