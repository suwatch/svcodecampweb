using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampSV;
using System.ComponentModel;

namespace CodeCampSV
{
    public partial class SponsorListManager
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

        public List<SponsorListResult> GetAllCurrentYear7()
        {
            return Get(new SponsorListQuery { CodeCampYearId = 7 });
        }

        public List<SponsorListResult> Get(SponsorListQuery query)
        {

            var meta = new CodeCampDataContext();

            IQueryable<SponsorList> baseQuery = from myData in meta.SponsorList select myData;


            //  next is automated query formation from AutoGen Shared Class 
            //  (do not remove next line or filters will go away)
            baseQuery = BaseQueryAutoGen(baseQuery, query);

            if (query.CodeCampYearId.HasValue)
            {
                if (query.Visible.HasValue && query.Visible.Value)
                {

                    var rr = from data in meta.SponsorListCodeCampYear
                             where data.CodeCampYearId == query.CodeCampYearId.Value &&
                                   data.Visible
                             select data.SponsorListId;
                    baseQuery = baseQuery.Where(a => rr.Contains(a.Id));
                }
                else
                {
                    var rr = from data in meta.SponsorListCodeCampYear
                             where data.CodeCampYearId == query.CodeCampYearId.Value
                             select data.SponsorListId;
                    baseQuery = baseQuery.Where(a => rr.Contains(a.Id));
                }
            }

            IQueryable<SponsorListResult> results = GetBaseResultIQueryable(baseQuery);


            List<SponsorListResult> resultList = GetFinalResults(results, query);


            List<SponsorListCodeCampYearResult> sponsorListCodeCampYearResults = null;
            if (query.WithCodeCampYears != null && query.WithCodeCampYears.Value && 
                query.PlatinumLevel.HasValue && query.SilverLevel.HasValue && query.GoldLevel.HasValue && query.BronzeLevel.HasValue)
            {
                var sponsorListCodeCampYearQuery =
                    new SponsorListCodeCampYearQuery();
                if (query.CodeCampYearId != null)
                {
                    sponsorListCodeCampYearQuery.CodeCampYearId = query.CodeCampYearId;
                }
                sponsorListCodeCampYearResults =
                    SponsorListCodeCampYearManager.I.Get(sponsorListCodeCampYearQuery);
            }

           

            List<SponsorListContactResult> sponsorListContactResults = new List<SponsorListContactResult>();
            if (query.WithContacts != null && query.WithContacts.Value)
            {

                sponsorListContactResults = SponsorListContactManager.I.Get(new SponsorListContactQuery()
                                                                                {

                                                                                });
            }

            List<SponsorListNoteResult> sponsorListNoteResults = new List<SponsorListNoteResult>();
            if (query.WithNotes != null && query.WithNotes.Value)
            {

                sponsorListNoteResults = SponsorListNoteManager.I.Get(new SponsorListNoteQuery()
                                                                          {

                                                                          });
            }

            var sponsorListJobListingResults = new List<SponsorListJobListingResult>();
            if (query.WithJobListings != null && query.WithJobListings.Value)
            {

                sponsorListJobListingResults = SponsorListJobListingManager.I.Get(new SponsorListJobListingQuery()
                {

                });
            }

            if (resultList != null)
            {
                double platinum = 0;
                double gold = 0;
                double silver = 0;
                double bronze = 0;

                // this only works if a year is specified
                var sponsorLevelDictionary = new Dictionary<int, double>();
                if (query.IncludeSponsorLevel.HasValue && query.IncludeSponsorLevel.Value && query.CodeCampYearId.HasValue &&
                    query.PlatinumLevel.HasValue && query.SilverLevel.HasValue && query.GoldLevel.HasValue && query.BronzeLevel.HasValue)
                {
                      platinum = Convert.ToDouble(query.PlatinumLevel);
                      gold = Convert.ToDouble(query.GoldLevel);
                      silver = Convert.ToDouble(query.SilverLevel);
                      bronze = Convert.ToDouble(query.BronzeLevel);


                    var recs = (from data in meta.SponsorListCodeCampYear
                                where data.CodeCampYearId == query.CodeCampYearId.Value
                                select data).ToList();
                    foreach (var sponsorListCodeCampYear in recs)
                    {
                        // need to figure out why we have dupes and fix
                        if (!sponsorLevelDictionary.ContainsKey(sponsorListCodeCampYear.SponsorListId))
                        {
                            sponsorLevelDictionary.Add(sponsorListCodeCampYear.SponsorListId,sponsorListCodeCampYear.DonationAmount);
                        }
                    }
                }

               


                foreach (var rec in resultList)
                {
                    rec.ImageURL = String.Format("/sponsorimage/{0}.jpg", rec.Id);

                    if (query.IncludeSponsorLevel.HasValue && query.IncludeSponsorLevel.Value && query.CodeCampYearId.HasValue &&
                         query.PlatinumLevel.HasValue && query.SilverLevel.HasValue && query.GoldLevel.HasValue && query.BronzeLevel.HasValue)
                    {
                        if (sponsorLevelDictionary.ContainsKey(rec.Id))
                        {

                            double dollarAmount = sponsorLevelDictionary[rec.Id];
                            if (dollarAmount >= platinum)
                            {
                                rec.SponsorSupportLevel = "Platinum";
                                rec.SponsorSupportLevelOrder = 0;
                            }
                            else if (dollarAmount >= gold)
                            {
                                rec.SponsorSupportLevel = "Gold";
                                rec.SponsorSupportLevelOrder = 1;
                            }
                            else if (dollarAmount >= silver)
                            {
                                rec.SponsorSupportLevel = "Silver";
                                rec.SponsorSupportLevelOrder = 2;
                            }
                            else if (dollarAmount >= bronze)
                            {
                                rec.SponsorSupportLevel = "Gold";
                                rec.SponsorSupportLevelOrder = 3;
                            }
                            else if (Math.Abs(dollarAmount - 0.0) < .01)
                            {
                                rec.SponsorSupportLevel = "Community";
                                rec.SponsorSupportLevelOrder = 4;
                            }
                            else
                            {
                                rec.SponsorSupportLevel = "Unknown";
                                rec.SponsorSupportLevelOrder = 999;
                            }
                        }
                    }

                    if (sponsorListCodeCampYearResults != null)
                    {
                        rec.SponsorListCodeCampYearResults =
                            sponsorListCodeCampYearResults.Where(a => a.SponsorListId == rec.Id).ToList();
                    }

                    // this is fast if withNotes false
                    rec.SponsorListNoteResults =
                        sponsorListNoteResults.Where(a => a.SponsorListId == rec.Id).ToList();

                    // this is fast if withContact is false
                    rec.SponsorListContactResults =
                        sponsorListContactResults.Where(a => a.SponsorListId == rec.Id).ToList();

                    // this is fast if withJobListing is false
                    rec.SponsorListJobListingResults =
                        sponsorListJobListingResults.Where(a => a.SponsorListId == rec.Id).ToList();
                }
            }
            return resultList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<SponsorListResult> GetAll()
        {
            return Get(new SponsorListQuery {IsMaterializeResult = true}).OrderBy(a => a.SponsorName).ToList();
        }
    }
}
