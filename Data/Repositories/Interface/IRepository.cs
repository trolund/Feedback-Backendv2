using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Feedback.Data.Repositories {
    public interface IRepository<TEntity> where TEntity : class {
        // get objects
        Task<TEntity> Get (int id);
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