using System.Linq.Expressions;
using System.Reflection;
using Core.Interfaces;
using DataAccess.Context;
using Helpers.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using DataAccess.Schemas;
using System.Linq.Dynamic.Core;
using DataAccess.Models;

namespace Core
{

    public class ServiceProvider<T> : IServiceProvider<T> where T : class
    {
        protected readonly DbContext _context;
        internal DbSet<T> _dbSet;


        public ServiceProvider(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }
        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }
        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public T GetById(object id)
        {
            return _context.Set<T>().Find(id);
        }
        public T GetById(long Id)
        {
            return _context.Set<T>().Find(Id);
        }
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public T GetById(string id)
        {
            return _context.Set<T>().Find(id);
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public T GetByIdNoTrack(string id)
        {
            var data = _context.Set<T>().Find(id);
            _context.ChangeTracker.Clear();
            return data;
        }
        public T GetByIdNoTrack(int id)
        {
            var data = _context.Set<T>().Find(id);
            _context.ChangeTracker.Clear();
            return data;
        }

        public IEnumerable<T> LoadData()
        {
            return _context.Set<T>().ToList();
        }

        public void SoftRemove(T entity)
        {
            Common.TrySetProperty(entity, "Deleted", (short)1);
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }


        public IQueryable<T> GetQuery(
            string searchTemplate = "",
            string searchText = "",
            List<FilterSchema>? filterList = null)
        {
            IQueryable<T> query = _dbSet;

            if ((searchText != "") && (searchTemplate != ""))
            {
                query = query.Where(searchTemplate, searchText);
            }

            if (filterList != null)
            {
                foreach (var filterItem in filterList)
                {
                    if (string.Equals(filterItem.Operator,
                        "in",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        var valueList = filterItem.Value.Split(",");
                        var textSQL = "";
                        var parameterIndex = 0;

                        foreach (var valueItem in valueList)
                        {
                            if (textSQL != "")
                            {
                                textSQL += " OR ";
                            }

                            textSQL += (filterItem.Field + " == @" + parameterIndex);
                            parameterIndex++;
                        }

                        query = query.Where(textSQL, valueList);
                    }
                    else
                    {
                        query = query.Where(
                            (filterItem.Field
                            + " "
                            + filterItem.Operator
                            + " "
                            + "@0"),
                            filterItem.Value);
                    }
                }
            }

            return query;
        }
        public IQueryable<T> GetQueryAsNoTrack(
            string searchTemplate = "",
            string searchText = "",
            List<FilterSchema>? filterList = null)
        {
            IQueryable<T> query = _dbSet;

            if ((searchText != "") && (searchTemplate != ""))
            {
                query = query.Where(searchTemplate, searchText);
            }

            if (filterList != null)
            {
                foreach (var filterItem in filterList)
                {
                    if (string.Equals(filterItem.Operator,
                        "in",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        var valueList = filterItem.Value.Split(",");
                        var textSQL = "";
                        var parameterIndex = 0;

                        foreach (var valueItem in valueList)
                        {
                            if (textSQL != "")
                            {
                                textSQL += " OR ";
                            }

                            textSQL += (filterItem.Field + " == @" + parameterIndex);
                            parameterIndex++;
                        }

                        query = query.Where(textSQL, valueList);
                    }
                    else
                    {
                        query = query.Where(
                            (filterItem.Field
                            + " "
                            + filterItem.Operator
                            + " "
                            + "@0"),
                            filterItem.Value);
                    }
                }
            }

            _context.ChangeTracker.Clear();

            return query;
        }

        public async Task<IEnumerable<T>> GetByQueryAsync(
                IQueryable<T> query,
                string orderBy = "",
                int page = 0,
                int limit = 50)
        {
            if (orderBy == "")
            {
                orderBy = "id";
            }

            query = query.OrderBy(orderBy);

            if (limit > 0)
            {
                query = query.Skip(page * limit).Take(limit);
            }

            return await query.ToListAsync();
        }

        public IQueryable<T> GetByQueryReturnQueryAsync(
                IQueryable<T> query,
                string orderBy = "",
                int page = 0,
                int limit = 50)
        {
            if (orderBy == "")
            {
                orderBy = "createdat desc";
            }

            if (orderBy == "id asc")
            {
                orderBy = "createdat asc";
            }

            if (orderBy == "id desc")
            {
                orderBy = "createdat desc";
            }

            query = query.OrderBy(orderBy);

            if (limit > 0)
            {
                query = query.Skip(page * limit).Take(limit);
            }

            return query.AsQueryable();
        }

        public async Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int page = 0,
            int limit = 50)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);

                if (limit > 0)
                {
                    query = query.Skip(page * limit).Take(limit);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(
            string searchTemplate = "",
            string searchText = "",
            List<FilterSchema>? filterList = null,
            string orderBy = "",
            int page = 0,
            int limit = 50)
        {
            IQueryable<T> query = GetQuery(
                    searchTemplate,
                    searchText,
                    filterList);

            if (orderBy == "")
            {
                orderBy = "Id";
            }

            query = query.OrderBy(orderBy);

            if (limit > 0)
            {
                query = query.Skip(page * limit).Take(limit);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync((int)id);
        }

        public async Task<T?> GetByStringIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> Insert(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(long id)
        {
            T? entityToDelete = await _dbSet.FindAsync((int)id);

            if (entityToDelete != null)
            {
                Delete(entityToDelete);
            }
        }

        public void Delete(T entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void ToUpdate(T entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;

        }

        public async Task<T?> GetByFieldAsync(string fieldName, object value)
        {
            return await _dbSet.FirstOrDefaultAsync(e =>
                EF.Property<object>(e, fieldName).ToString() == value.ToString());
        }
    }
}