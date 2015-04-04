using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EFHelp.Abstract;

namespace EFHelp.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public GenericRepository(DbContext db)
        {
            this.m_db = db;
            m_table = db.Set<T>();
        }

        #region MEMBERS
        private readonly DbContext m_db;
        private readonly DbSet<T> m_table;
        #endregion

        #region INTERFACE
        public IEnumerable<T> SelectAll()
        {
            return m_table.ToList();
        }
        public T SelectByID(object id)
        {
            return m_table.Find(id);
        }
        public void Insert(T obj)
        {
            m_table.Add(obj);
        }
        public void Update(T obj)
        {
            m_table.Attach(obj);
            m_db.Entry(obj).State = EntityState.Modified;
        }
        public void Delete(object id)
        {
            T existing = m_table.Find(id);
            m_table.Remove(existing);
        }
        public void Save()
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
