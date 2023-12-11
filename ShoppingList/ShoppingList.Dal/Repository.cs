using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Dal
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext ctx { get; set; }
        protected DbSet<T> dataSet { get; set; }

        public Repository(DbContext dbContext)
        {
            ctx = dbContext;
            dataSet = ctx.Set<T>();
        }

        public async Task<IQueryable<T>> GetAll()
        {
            return dataSet;
        }

        public IEnumerable<T> GetAllEnu()
        {
            return dataSet;
        }

        public IQueryable<T> GetAllFiltered(List<string> includes, params Expression<Func<T, bool>>[] filters)
        {
            IQueryable<T> result = dataSet;

            if (filters != null)
            {
                foreach (Expression<Func<T, bool>> filter in filters)
                {
                    result = result.Where(filter);
                }
            }
            if (includes != null)
            {
                foreach (string include in includes)
                {
                    result = result.Include(include);
                }
            }

            return result;
        }

        public IQueryable<T> GetAllFilteredNoTrack(List<string> includes, params Expression<Func<T, bool>>[] filters)
        {
            IQueryable<T> result = dataSet.AsNoTracking();

            foreach (Expression<Func<T, bool>> filter in filters)
            {
                result = result.Where(filter);
            }
            if (includes != null)
            {
                foreach (string include in includes)
                {
                    result = result.Include(include);
                }
            }

            return result;
        }

        public T GetByFiltered(List<string> includes, params Expression<Func<T, bool>>[] filters)
        {
            IQueryable<T> result = dataSet;

            foreach (Expression<Func<T, bool>> filter in filters)
            {
                result = result.Where(filter);
            }
            if (includes != null)
            {
                foreach (string include in includes)
                {
                    result = result.Include(include);
                }
            }

            return result.FirstOrDefault();
        }

        public T GetByID(Guid id)
        {
            return dataSet.Find(id);
        }

        public T GetByID(int id)
        {
            return dataSet.Find(id);
        }

        public void Add(T model)
        {
            var entryItem = ctx.Entry(model);
            if (entryItem.State == EntityState.Detached)
            {
                entryItem.State = EntityState.Added;
            }
            else
            {
                dataSet.Add(model);
            }
        }

        public void Detach(T model)
        {
            var entryItem = ctx.Entry(model);
            entryItem.State = EntityState.Detached;
        }

        public virtual void Update(T model)
        {
            var entryItem = ctx.Entry(model);
            if (entryItem.State == EntityState.Detached)
            {
                dataSet.Attach(model);
            }
            entryItem.State = EntityState.Modified;
        }

        public virtual void Update(T model, Guid id)
        {
            var entryItem = ctx.Entry(model);
            if (entryItem.State == EntityState.Detached)
            {
                T original = dataSet.Find(id);
                if (original != null)
                {
                    ctx.Entry<T>(original).CurrentValues.SetValues(model);
                }
                else
                {
                    dataSet.Attach(model);
                    ctx.Entry<T>(model).State = EntityState.Modified;
                }
            }
        }

        public virtual void Delete(T model)
        {
            var entryItem = ctx.Entry(model);
            if (entryItem.State != EntityState.Deleted)
            {
                entryItem.State = EntityState.Deleted;
            }
            else
            {
                dataSet.Attach(model);
                dataSet.Remove(model);
            }
        }

        public void DeleteFiltered(Expression<Func<T, bool>> filter)
        {
            var results = dataSet.Where(filter);
            foreach (var item in results)
            {
                var entryitem = ctx.Entry(item);
                if (entryitem.State != EntityState.Deleted)
                {
                    entryitem.State = EntityState.Deleted;
                }
                else
                {
                    dataSet.Attach(item);
                    dataSet.Remove(item);
                }
            }
        }

        public virtual void Delete(Guid id)
        {
            var model = GetByID(id);
            if (model != null)
            {
                Delete(model);
            }
        }
        public virtual void Delete(int id)
        {
            var model = GetByID(id);
            if (model != null)
            {
                Delete(model);
            }
        }

    }
}
