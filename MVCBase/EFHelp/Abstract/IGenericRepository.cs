using System;
using System.Collections.Generic;
using System.Linq;

namespace EFHelp.Abstract
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> DataSet();
        IEnumerable<TEntity> SelectAll();
        TEntity SelectByID(object id);
        void Insert4ID(TEntity obj, Func<TEntity, int> getter, Action<TEntity, int> setter);
        void Insert(TEntity obj);
        void Update(TEntity obj);
        void Delete(object id);
        void SaveChanges();
        void Dispose();
    }
}