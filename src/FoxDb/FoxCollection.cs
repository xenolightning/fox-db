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
        private readonly List<T> _items;

        public FoxCollection()
        {
            _items = new List<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _items)
                yield return item;
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