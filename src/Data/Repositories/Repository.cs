﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories {
    public class Repository<TEntity, TGUID> : IRepository<TEntity, TGUID> where TEntity : class {
        protected readonly DbContext Context;

        public Repository (DbContext context) {
            Context = context;
        }

        public async Task<TEntity> Get (TGUID id) {
            return await Context.Set<TEntity> ().FindAsync (id);
        }

        public async Task<IEnumerable<TEntity>> GetAll () {
            return await Context.Set<TEntity> ().ToListAsync ();
        }

        public IEnumerable<TEntity> Find (Expression<Func<TEntity, bool>> predicate) {
            return Context.Set<TEntity> ().Where (predicate);
        }

        public async Task<TEntity> SingleOrDefault (Expression<Func<TEntity, bool>> predicate) {
            return await Context.Set<TEntity> ().SingleOrDefaultAsync (predicate);
        }

        public async Task Add (TEntity entity) {
            await Context.Set<TEntity> ().AddAsync (entity);
        }

        public async Task AddRange (IEnumerable<TEntity> entities) {
            await Context.Set<TEntity> ().AddRangeAsync (entities);
        }

        public void Remove (TEntity entity) {
            Context.Set<TEntity> ().Remove (entity);
        }

        public void RemoveRange (IEnumerable<TEntity> entities) {
            Context.Set<TEntity> ().RemoveRange (entities);
        }
    }
}