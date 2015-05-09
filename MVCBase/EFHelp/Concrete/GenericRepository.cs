using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EFHelp.Abstract;

namespace EFHelp.Concrete
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        public GenericRepository(DbContext db)
        {
            this.m_db = db;
            m_table = db.Set<TEntity>();
        }

        #region MEMBERS
        private readonly DbContext m_db;
        private readonly DbSet<TEntity> m_table;
        #endregion

        #region INTERFACE
        public IEnumerable<TEntity> DataEnumerable()
        {
            return m_table;
        }
        public IQueryable<TEntity> DataQueryable()
        {
            return m_table;
        }
        public IEnumerable<TEntity> SelectAll()
        {
            return m_table.ToList();
        }
        public TEntity SelectByID(object id)
        {
            return m_table.Find(id);
        }
        public void Insert(TEntity obj)
        {
            m_table.Add(obj);
        }
        public void Insert4ID(TEntity obj, Func<TEntity, int> getter, Action<TEntity, int> setter)
        {
            var items = SelectAll();
            int maxID = items.Max(item => getter(item));
            setter(obj, maxID + 1);
            Insert(obj);
        }
        public void Update(TEntity obj)
        {
            m_table.Attach(obj);
            m_db.Entry(obj).State = EntityState.Modified;
        }
        public void Delete(object id)
        {
            TEntity existing = m_table.Find(id);
            m_table.Remove(existing);
        }
        public void SaveChanges()
        {
            m_db.SaveChanges();
        }
        public void Dispose()
        {
            m_db.Dispose();
        }
        #endregion
    }
}
