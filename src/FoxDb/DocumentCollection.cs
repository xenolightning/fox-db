using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FoxDb
{
    internal sealed class DocumentCollection<T> : IDocumentCollection<T>
        where T : class
    {
        private readonly List<T> _items;

        public DocumentCollection()
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

        public T Find(Expression<Func<T, bool>> filter)
        {
            return _items.AsQueryable().SingleOrDefault(filter);
        }

        public IEnumerable<T> FindMany(Expression<Func<T, bool>> filter)
        {
            return _items.AsQueryable().Where(filter);
        }

        public void Delete(Expression<Func<T, bool>> filter)
        {
            var item = _items.FirstOrDefault(filter.Compile());

            if (item != null)
                _items.Remove(item);
        }

        public int DeleteMany(Expression<Func<T, bool>> filter)
        {
            return _items.RemoveAll(new Predicate<T>(filter.Compile()));
        }
    }
}