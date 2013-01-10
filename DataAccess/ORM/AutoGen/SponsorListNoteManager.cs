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
    public partial class SponsorListNoteManager : ManagerBase<SponsorListNoteManager, SponsorListNoteResult, SponsorListNote, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(SponsorListNote record, SponsorListNoteResult result)
        {
            record.SponsorListId = result.SponsorListId;
            record.TimeStampOfNote = result.TimeStampOfNote;
            record.NoteAuthor = result.NoteAuthor;
            record.Note = result.Note;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override SponsorListNote GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.SponsorListNote where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<SponsorListNoteResult> GetBaseResultIQueryable(IQueryable<SponsorListNote> baseQuery)
        {
      IQueryable<SponsorListNoteResult> results = (from myData in baseQuery orderby myData.Id select new SponsorListNoteResult { Id= myData.Id,
            SponsorListId = myData.SponsorListId,
            TimeStampOfNote = new DateTime(myData.TimeStampOfNote.Ticks,DateTimeKind.Utc),
            NoteAuthor = myData.NoteAuthor,
            Note = myData.Note
      });
		    return results;
        }
        
        public List<SponsorListNoteResult> GetJustBaseTableColumns(SponsorListNoteQuery query)
        {
            foreach (var info in typeof (SponsorListNoteQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: SponsorListNote QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<SponsorListNote> baseQuery = from myData in meta.SponsorListNote select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<SponsorListNoteResult> results = (from myData in baseQuery orderby myData.Id select new SponsorListNoteResult { Id= myData.Id,
                        SponsorListId = myData.SponsorListId,
                        TimeStampOfNote = new DateTime(myData.TimeStampOfNote.Ticks,DateTimeKind.Utc),
                        NoteAuthor = myData.NoteAuthor,
                        Note = myData.Note
            });
            
            List<SponsorListNoteResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<SponsorListNote> BaseQueryAutoGen(IQueryable<SponsorListNote> baseQuery, SponsorListNoteQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.SponsorListId != null) baseQuery = baseQuery.Where(a => a.SponsorListId == query.SponsorListId);
            if (query.TimeStampOfNote != null) baseQuery = baseQuery.Where(a => a.TimeStampOfNote.CompareTo(query.TimeStampOfNote) == 0);
            if (query.NoteAuthor != null) baseQuery = baseQuery.Where(a => a.NoteAuthor.ToLower().Equals(query.NoteAuthor.ToLower()));
            if (query.Note != null) baseQuery = baseQuery.Where(a => a.Note.ToLower().Equals(query.Note.ToLower()));

            return baseQuery;
        }
        
    }
}
