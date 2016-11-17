using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FoxDb
{
    public interface IFoxCollection
    {

    }

    public interface IReadOnlyDocumentCollection<T> : IFoxCollection
        where T : class
    {
        T Find(Expression<Func<T, bool>> filter);

        IEnumerable<T> FindMany(Expression<Func<T, bool>> filter);
    }

    public interface IDocumentCollection<T> : IReadOnlyDocumentCollection<T>
        where T : class
    {

        void Insert(T data);

        void Delete(Expression<Func<T, bool>> filter);

        int DeleteMany(Expression<Func<T, bool>> filter);

    }

    public interface IKeyValueCollection<in TKey, TValue> : IFoxCollection
        where TValue : class
        where TKey : struct 
    {

        void Insert(TKey key, TValue data);

        TValue FindByKey(TKey key);

        TValue Find(Expression<Func<TValue, bool>> filter);

        IEnumerable<TValue> FindMany(Expression<Func<TValue, bool>> filter);

        void DeleteByKey(TKey key);

        void Delete(Expression<Func<TValue, bool>> filter);

        int DeleteMany(Expression<Func<TValue, bool>> filter);

    }

}