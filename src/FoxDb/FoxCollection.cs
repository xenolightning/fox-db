using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FoxDb
{
    internal sealed class FoxCollection<T> : IFoxCollection<T>
        where T : class
    {
        private readonly IFoxTransaction _transaction;
        private readonly List<T> _items;

        public Expression Expression => _items.AsQueryable().Expression;

        public Type ElementType => typeof(T);

        public IQueryProvider Provider => _items.AsQueryable().Provider;

        public FoxCollection(IFoxTransaction transaction)
        {
            _transaction = transaction;
            _items = new List<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _items)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Truncate()
        {
            _items.Clear();
        }

        public void Insert(T data)
        {
            _items.Add(data);
        }

        public T Find(Expression<Func<T, bool>> findExpression)
        {
            return _items.AsQueryable().SingleOrDefault(findExpression);
        }

        public IQueryable<T> FindMany(Expression<Func<T, bool>> findExpression)
        {
            return _items.AsQueryable().Where(findExpression);
        }

        public void Delete(Expression<Func<T, bool>> deleteExpression)
        {
            var item = _items.FirstOrDefault(deleteExpression.Compile());

            if (item != null)
                _items.Remove(item);
        }

        public int DeleteMany(Expression<Func<T, bool>> deleteExpression)
        {
            return _items.RemoveAll(new Predicate<T>(deleteExpression.Compile()));
        }
    }
}