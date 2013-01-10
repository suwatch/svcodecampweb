using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampSV;
using System.ComponentModel;
using System.Web;

namespace CodeCampSV
{
    public partial class SessionTagsManager
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
        //public static object Olock = new Object();

        //public List<SessionTagsResult> GetByCodeCampYearIdCached(int codeCampYearId,int cacheTimeOutSeconds)
        //{
        //    string cacheString = "SessionTagsManagerCacheAll-" + codeCampYearId.ToString();
        //    var o = (List<SessionTagsResult>)HttpContext.Current.Cache[cacheString];
        //    if (o == null)
        //    {
        //        lock (Olock)
        //        {
        //            o = Get(new SessionTagsQuery() { CodeCampYearId = codeCampYearId });
        //            HttpContext.Current.Cache.Insert(cacheString, o,
        //                                             null,
        //                                             DateTime.Now.Add(new TimeSpan(0, 0, cacheTimeOutSeconds)),
        //                                             TimeSpan.Zero);
        //        }
        //    }
        //    return o;
        //}

        public List<SessionTagsResult> Get(SessionTagsQuery query)
        {

            var meta = new CodeCampDataContext();

            IQueryable<SessionTags> baseQuery = from myData in meta.SessionTags select myData;


            //  next is automated query formation from AutoGen Shared Class 
            //  (do not remove next line or filters will go away)
            baseQuery = BaseQueryAutoGen(baseQuery, query);

            if (query.CodeCampYearId != null)
            {
                int codeCampYearId = query.CodeCampYearId.Value;
                var tagsCodeCampYear =
                    meta.SessionTagsView.Where(data => data.CodeCampYearId == codeCampYearId).Select(data => data.TagId);

                baseQuery = baseQuery.Where(data => tagsCodeCampYear.Contains(data.TagId));
            }


            IQueryable<SessionTagsResult> results = GetBaseResultIQueryable(baseQuery);


            List<SessionTagsResult> resultList = GetFinalResults(results, query);

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
        public List<SessionTagsResult> GetAll()
        {
            return Get(new SessionTagsQuery { IsMaterializeResult = true });
        }
    }
}
