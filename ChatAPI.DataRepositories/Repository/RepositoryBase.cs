﻿using ChatAPI.Core.Repository_Interfaces;
using DomainChat.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ChatAPI.DataRepositories.Repository
{
    public class RepositoryBase<TEntity> : IRepository<TEntity>
          where TEntity : class
    {
        ChatDbContext context;
        public RepositoryBase(ChatDbContext context)
        {
            this.context = context;
        }

        /// <value>The database set.</value>
        protected DbSet<TEntity> DbSet => this.context.Set<TEntity>();

        public IQueryable<TEntity> Get()
        {
            return this.DbSet;
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return this.Get().Where(filter);
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            var get = this.Get();
            foreach (var include in includes)
            {
                get.Include(include);
            }

            return get.Where(filter);
        }

        public TEntity Find(params object[] keyValues)
        {
            return this.DbSet.Find(keyValues);
        }

        public void Add(TEntity entity)
        {
            this.DbSet.Add(entity);
        }

        public void Update(TEntity entityToUpdate)
        {
            if (this.context.Entry(entityToUpdate).State == EntityState.Detached)
            {
                this.DbSet.Attach(entityToUpdate);
                this.context.Entry(entityToUpdate).State = EntityState.Modified;
            }
        }

        public void Delete(params object[] keyValues)
        {
            TEntity entityToDelete = this.Find(keyValues);
            this.Delete(entityToDelete);
        }

        public void Delete(TEntity entityToDelete)
        {
            this.DbSet.Remove(entityToDelete);
        }

        public void DeleteRange(IEnumerable<TEntity> entityToDelete)
        {
            this.DbSet.RemoveRange(entityToDelete);
        }


    }
}