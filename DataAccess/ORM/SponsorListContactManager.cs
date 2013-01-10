using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampSV;
using System.ComponentModel;

namespace CodeCampSV
{
    public partial class SponsorListContactManager
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

        public List<SponsorListContactResult> Get(SponsorListContactQuery query)
        {

            var meta = new CodeCampDataContext();

            IQueryable<SponsorListContact> baseQuery = from myData in meta.SponsorListContact select myData;


            //  next is automated query formation from AutoGen Shared Class 
            //  (do not remove next line or filters will go away)
            baseQuery = BaseQueryAutoGen(baseQuery, query);

            if (query.SponsorListIds != null && query.SponsorListIds.Count > 0)
            {
                baseQuery = baseQuery.Where(a => a.SponsorListId != null && query.SponsorListIds.Contains(a.SponsorListId.Value));
            }

            IQueryable<SponsorListContactResult> results = GetBaseResultIQueryable(baseQuery);


            List<SponsorListContactResult> resultList = GetFinalResults(results, query);

            List<string> emails = resultList.Select(a => a.EmailAddress).ToList();
            List <AttendeesResult> attendeesResults = AttendeesManager.I.Get(new AttendeesQuery()
                                                                  {
                                                                      Emails = emails
                                                                  });



            foreach (var r in resultList)
            {
                // this could be null which is OK
                r.UsernameAssociated =
                    attendeesResults.Where(a => a.Email == r.EmailAddress).Select(a => a.Username).FirstOrDefault();
            }

            return resultList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<SponsorListContactResult> GetAll()
        {
            return Get(new SponsorListContactQuery {IsMaterializeResult = true});
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<SponsorListContactResult> GetBySponsorListId(int sponsorListId)
        {
            return Get(new SponsorListContactQuery { IsMaterializeResult = true, SponsorListId = sponsorListId }).OrderBy(a => a.EmailAddress).ToList();
        }

       
    }
}
