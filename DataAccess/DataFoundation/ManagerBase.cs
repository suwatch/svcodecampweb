using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using Gurock.SmartInspect.LinqToSql;


namespace CodeCampSV
{

    

    /// <summary>
    /// This base class provide a simple singleton interface to all data access manager classes
    /// that inherit from it. 
    /// The singleton is not enforced since we cannot make the child class constructor private. 
    /// It does not hurt either if you do want to make your own instance when using the managers. 
    /// </summary>
    public abstract class ManagerBase<TManager, TResult, TEntity, TDataContext>
        where TResult : ResultBase
        where TEntity : class, new()
        where TDataContext : DataContext, new()
        where TManager : new()
    {

        private readonly bool _smartInspectEnabled = (ConfigurationManager.AppSettings["SmartInspectEnabled"] ?? "").Equals("true");

        private static readonly TManager instance = new TManager();

        public static TManager I
        {
            get { return instance; }
        }

        protected abstract void ApplyToDataModel(TEntity record, TResult result);

        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public virtual void Insert(TResult result)
        {
            var meta = new TDataContext();

            if (_smartInspectEnabled)
            {
                meta.Log = new SmartInspectLinqToSqlAdapter();
            }

            var record = new TEntity();
            ApplyToDataModel(record, result);
            meta.GetTable<TEntity>().InsertOnSubmit(record);
            meta.SubmitChanges();
            result.Id = (int) typeof (TEntity).GetProperty("Id").GetValue(record, null);
        }

        public void Insert(IEnumerable<TResult> results)
        {
            foreach (TResult result in results)
            {
                Insert(result);
            }
        }


        public virtual void Delete(TResult result)
        {
            Delete(new[] {result});
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public virtual void Delete(int id)
        {
            Delete(new[] {id});
        }

        public virtual void Delete(IEnumerable<TResult> list)
        {
            Delete((from l in list select l.Id));
        }

        public virtual void Delete(IEnumerable<int> Ids)
        {
            List<int> IdsList = Ids.ToList(); // materialize the list just once.
            if (IdsList.Count > 0)
            {
                var meta = new TDataContext();

                if (_smartInspectEnabled)
                {
                    meta.Log = new SmartInspectLinqToSqlAdapter();
                }

                var tableAttribute =
                    AttributeHelper.GetAttribute(typeof (TEntity), typeof (TableAttribute)) as TableAttribute;
                if (tableAttribute != null)
                {
                    meta.ExecuteCommand(string.Format("DELETE {0} WHERE Id IN ({1});", tableAttribute.Name,
                                                      IdsList.ToJoinedString(",")));
                }
            }
        }


        protected abstract TEntity GetEntityById(TDataContext meta, int id);


       
        public virtual void Update(IEnumerable<TResult> results)
        {
            foreach (TResult result in results)
            {
                Update(result);
            }
        }

        public virtual void Update(TResult result)
        {
            if (result.Id == 0)
            {
                string errorMessage = 
                    String.Format("ManagerBase Reports Update with result.Id==0.  Likely means {0} class does not have Id = r.Id in Get Results.", result.GetType().Name);
                throw new ApplicationException(errorMessage);
            }

            var meta = new TDataContext();

            if (_smartInspectEnabled)
            {
                meta.Log = new SmartInspectLinqToSqlAdapter();
            }

            //var table = meta.GetTable<TEntity>();
            //List<TEntity> entities = new List<TEntity>();

            TEntity record = GetEntityById(meta, result.Id);
            ApplyToDataModel(record, result);
            // entities.Add(record);

            //table.AttachAll(entities, true);
            meta.SubmitChanges(ConflictMode.ContinueOnConflict);
        }

        public void DeleteAll()
        {
            var meta = new TDataContext();

            if (_smartInspectEnabled)
            {
                meta.Log = new SmartInspectLinqToSqlAdapter();
            }

            var tableAttribute =
                AttributeHelper.GetAttribute(typeof (TEntity), typeof (TableAttribute)) as TableAttribute;
            //cannot use TRUNCATE because most tables have foreign key constraints.
            if (tableAttribute != null) meta.ExecuteCommand(string.Format("DELETE {0}", tableAttribute.Name));
        }

        protected List<TResult>     GetFinalResults(IQueryable<TResult> results, QueryBase query)
        {
            query.OutputTotal = results.Count();
            //if the starting point is negative, we start from the end
            if (query.Start < 0)
            {
                query.Start = query.OutputTotal + query.Start;
                if (query.Start < 0)
                {
                    query.Start = 0;
                }
            }

            if (query.Start > 0)
            {
                results = results.Skip(query.Start);
            }
            if (query.Limit > 0)
            {
                results = results.Take(query.Limit);
            }

            List<TResult> retList = null;
            if (query.IsMaterializeResult)
            {
                retList = results.ToList();
            }
            return retList;
        }
    }
}