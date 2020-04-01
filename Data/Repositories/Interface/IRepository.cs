using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repositories.Interface {
    public interface IRepository<TEntity, TGUID> where TEntity : class {
        // get objects
        Task<TEntity> Get (TGUID id);
        Task<IEnumerable<TEntity>> GetAll ();
        IEnumerable<TEntity> Find (Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> SingleOrDefault (Expression<Func<TEntity, bool>> predicate);

        // create objects
        Task Add (TEntity entity);
        Task AddRange (IEnumerable<TEntity> entities);

        // remove objects 
        void Remove (TEntity entity);
        void RemoveRange (IEnumerable<TEntity> entities);
    }
}