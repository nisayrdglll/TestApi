﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IServiceProvider<T> where T : class
    {
        T GetById(int id);
        T GetById(string id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void Update(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void SoftRemove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
