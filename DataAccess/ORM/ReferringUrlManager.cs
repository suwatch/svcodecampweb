using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampSV;
using System.ComponentModel;

namespace CodeCampSV
{
    public partial class ReferringUrlManager
    {
        public List<ReferringUrlResult> Get(ReferringUrlQuery query)
        {

            var meta = new CodeCampDataContext();

            IQueryable<ReferringUrl> baseQuery = from myData in meta.ReferringUrl select myData;


            if (query.SkipAllInReferringUrlGroup != null)
            {
                // get list from group, then exclude from query
                var recs = (from data in meta.ReferringUrlGroup
                            select data.ArticleName).Distinct();
                baseQuery =  baseQuery.Where(a => recs.Contains(a.ReferringUrlName));
            }

            //  next is automated query formation from AutoGen Shared Class 
            //  (do not remove next line or filters will go away)
            baseQuery = BaseQueryAutoGen(baseQuery, query);

           

            IQueryable<ReferringUrlResult> results = GetBaseResultIQueryable(baseQuery);


            List<ReferringUrlResult> resultList = GetFinalResults(results, query);

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
        public List<ReferringUrlResult> GetAll()
        {
            return Get(new ReferringUrlQuery {IsMaterializeResult = true});
        }
    }
}
