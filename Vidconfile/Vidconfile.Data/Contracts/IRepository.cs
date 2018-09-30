using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Vidconfile.Data.Contracts
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IQueryable<T> All();

        T GetById<TId>(TId id);

        void Add(T entity);

        void Update(T entity);

        IQueryable<T> All<TProperty>(Expression<Func<T, TProperty>> includes);

        IQueryable<T> All(string include);

        void Delete(T entity);

        void Delete<TId>(TId id);

        void Detach(T entity);
    }
}
