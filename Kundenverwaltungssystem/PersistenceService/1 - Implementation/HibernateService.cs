using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;
using PersistenceService.Implementation;

namespace PersistenceService
{
    public class HibernateService : IPersistenceService, ITransactionService
    {
        private readonly ISession _session;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session">Session Objekt (für Testzwecke)</param>
        public HibernateService()
        {
            _session = HibernateSessionFactory.OpenSession();
        }

        public T Save<T>(T entity) where T : class
        {
            _session.SaveOrUpdate(entity);
            _session.Flush();
            return entity;
        }

        public T Refresh<T>(T entity) where T : class
        {
            _session.Refresh(entity);
            return entity;
        }

        public IList<T> SaveAll<T>(IList<T> entities) where T : class
        {
            using (var trans = _session.BeginTransaction())
            {
                entities.ForEach(x => _session.SaveOrUpdate(x));
                trans.Commit();
            }
            return entities;
        }

        public T GetById<T, TIdTyp>(TIdTyp id) where T : class
        {
            return _session.Get<T>(id);
        }

        public IList<T> GetAll<T>() where T : class
        {
            return _session.Query<T>().ToList();
        }

        public void Delete<T>(T entity) where T : class
        {
            _session.Delete(entity);
            _session.Flush();
        }

        public void DeleteRange<T>(IList<T> entities) where T : class
        {
            using (var trans = _session.BeginTransaction())
            {
                entities.ForEach(x => _session.Delete(x));
                trans.Commit();
            }
        }

        public void DeleteAll<T>() where T : class
        {
            string name = typeof(T).Name;
            using (var trans = _session.BeginTransaction())
            {
                _session.CreateQuery("DELETE from " + name).ExecuteUpdate();
                trans.Commit();
            }
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return _session.Query<T>();
        }

        public void ExecuteInTransaction(Action action)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    action.Invoke();
                    transaction.Commit();
                    _session.Flush();
                }
                catch(StaleObjectStateException e)
                {
                    transaction.Rollback();
                    throw e;
                }
                
            }
        }

        public T ExecuteInTransaction<T>(Func<T> func)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    var res = func.Invoke();
                    transaction.Commit();
                    return res;
                }
                catch (StaleObjectStateException e)
                {
                    Debug.WriteLine("{0} {1} {2}", e.EntityName, e.Identifier, e.Message);
                    transaction.Rollback();
                    throw e;
                }

            }
        }
    }
}
