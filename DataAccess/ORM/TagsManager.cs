using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampSV;
using System.ComponentModel;

namespace CodeCampSV
{
    public partial class TagsManager
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

        public List<TagsResult> Get(TagsQuery query)
        {

            var meta = new CodeCampDataContext();

            IQueryable<Tags> baseQuery = from myData in meta.Tags select myData;


            //  next is automated query formation from AutoGen Shared Class 
            //  (do not remove next line or filters will go away)
            baseQuery = BaseQueryAutoGen(baseQuery, query);

              if (query.CodeCampYearId.HasValue)
              {
                  var tagsIds =
                      meta.SessionTagsView.Where(data => data.CodeCampYearId == query.CodeCampYearId)
                          .Select(data => data.TagId);
                  baseQuery = baseQuery.Where(data => tagsIds.Contains(data.Id));
              }



            IQueryable<TagsResult> results = GetBaseResultIQueryable(baseQuery);

          


            List<TagsResult> resultList = GetFinalResults(results, query);

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
        public List<TagsResult> GetAll()
        {
            return Get(new TagsQuery {IsMaterializeResult = true});
        }
    }
}
