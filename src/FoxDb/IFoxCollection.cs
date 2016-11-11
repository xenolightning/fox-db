using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FoxDb
{
    public interface IFoxCollection<T> : IFoxCollection, IQueryable<T>
        where T : class
    {

        void Insert(T data);

        T Find(Expression<Func<T, bool>> findExpression);

        IQueryable<T> FindMany(Expression<Func<T, bool>> findExpression);

        void Delete(Expression<Func<T, bool>> deleteExpression);

        int DeleteMany(Expression<Func<T, bool>> deleteExpression);

    }

    public interface IFoxCollection : IQueryable
    {

    }
}