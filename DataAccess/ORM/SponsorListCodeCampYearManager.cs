using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CodeCampSV;
using System.ComponentModel;

namespace CodeCampSV
{
    public partial class SponsorListCodeCampYearManager
    {

        //
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void  UpdateSponsorSimple1(SponsorListCodeCampYearResult sponsorListCodeCampYearResult)
        {
              var meta = new CodeCampDataContext();

            var rec = (from data in meta.SponsorListCodeCampYear
                       where data.Id == sponsorListCodeCampYearResult.Id
                       select data).First();

            if (rec != null)
            {
                rec.TableRequired = sponsorListCodeCampYearResult.TableRequired;
                rec.AttendeeBagItem = sponsorListCodeCampYearResult.AttendeeBagItem;
                rec.ItemSentToFoothill = sponsorListCodeCampYearResult.ItemSentToFoothill;
                rec.ItemsShippingDescription = sponsorListCodeCampYearResult.ItemsShippingDescription;
                rec.NoteFromCodeCamp = sponsorListCodeCampYearResult.NoteFromCodeCamp;
                rec.Comments = sponsorListCodeCampYearResult.Comments;
                meta.SubmitChanges();
            }

        }

        public List<SponsorListCodeCampYearResult> Get(SponsorListCodeCampYearQuery query)
        {

            var meta = new CodeCampDataContext();

            //Dictinary<int, string> sponsorNameById = (from data in meta.SponsorList
            //                                          select data).ToDictionary(k => k.Id, v => v.SponsorName);

            //Dictinary<int, string> sponsorCommentById = (from data in meta.SponsorList
            //                                             select data).ToDictionary(k => k.Id, v => v.Comment);


            IQueryable<SponsorListCodeCampYear> baseQuery = from myData in meta.SponsorListCodeCampYear select myData;

           
            //  next is automated query formation from AutoGen Shared Class 
            //  (do not remove next line or filters will go away)
            baseQuery = BaseQueryAutoGen(baseQuery, query);

            if (query.DonationAmtMin != null)
            {
                baseQuery = baseQuery.Where(a => a.DonationAmount >= query.DonationAmtMin.Value);
            }

            if (query.DonationAmtMax != null)
            {
                baseQuery = baseQuery.Where(a => a.DonationAmount <= query.DonationAmtMax.Value);
            }


            IQueryable<SponsorListCodeCampYearResult> results = GetBaseResultIQueryable(baseQuery);




            List<SponsorListCodeCampYearResult> resultList = GetFinalResults(results, query);

            var recs = (from data in meta.SponsorList
                            select data).ToList();
            
            foreach (var rec in resultList)
            {
                rec.CurrentCodeCampYearId =
                    Convert.ToInt32(ConfigurationManager.AppSettings["CurrentCodeCampYearId"] ?? "0");

                rec.SponsorName = recs.Where(a => a.Id == rec.SponsorListId).Select(a => a.SponsorName).FirstOrDefault();
            }

            //List<SponsorListCodeCampYearNoteResult> sponsorListCodeCampYearNoteResults = null;
            //if (query.WithNotes != null && query.WithNotes.Value)
            //{
            //    var sponsorListCodeCampYearNoteQuery =
            //        new SponsorListCodeCampYearNoteQuery();
            //    if (query.CodeCampYearId != null)
            //    {
            //        sponsorListCodeCampYearNoteQuery.SponsorListCodeCampYearId = query.CodeCampYearId;
            //    }

            //    sponsorListCodeCampYearNoteResults =
            //        SponsorListCodeCampYearNoteManager.I.Get(sponsorListCodeCampYearNoteQuery);


            //    foreach (var rec in resultList)
            //    {
            //        rec.SponsorListNoteResults =
            //            sponsorListCodeCampYearNoteResults.Where(a => a.SponsorListCodeCampYearId == rec.Id).ToList();
            //    }
            //}


            //List<SponsorListCodeCampYearContactResult> sponsorListCodeCampYearCodeCampYearContactResults = null;
            //if (query.WithContacts != null && query.WithContacts.Value)
            //{
            //    var sponsorListCodeCampYearContactQuery =
            //        new SponsorListCodeCampYearContactQuery();
            //    if (query.CodeCampYearId != null)
            //    {
            //        sponsorListCodeCampYearContactQuery.SponsorListCodeCampYearId = query.CodeCampYearId;
            //    }

            //    sponsorListCodeCampYearCodeCampYearContactResults =
            //        SponsorListCodeCampYearContactManager.I.Get(sponsorListCodeCampYearContactQuery);


            //    foreach (var rec in resultList)
            //    {
            //        rec.SponsorListContactResults =
            //            sponsorListCodeCampYearCodeCampYearContactResults.Where(
            //                a => a.SponsorListCodeCampYearId == rec.Id).ToList();
            //    }
            //}

            return resultList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SponsorListCodeCampYearResult> GetByParams(double donationAmtMin,double donationAmtMax,int codeCampYearId)
        {
            SponsorListCodeCampYearQuery query = new SponsorListCodeCampYearQuery()
            {
                DonationAmtMin = donationAmtMin,
                DonationAmtMax = donationAmtMax,
                CodeCampYearId = codeCampYearId,
                IsMaterializeResult = true
            };

            return Get(query).OrderBy(a => a.SponsorName).ToList();
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<SponsorListCodeCampYearResult> GetAll()
        {
            return Get(new SponsorListCodeCampYearQuery {IsMaterializeResult = true});
        }
    }
}
