using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FoxDb
{
    internal class StreamingDocumentCollection<T> : IReadOnlyDocumentCollection<T>
        where T : class
    {
        private readonly IReadOnlyDocumentCollection<T> _sourceCollection;

        public StreamingDocumentCollection(IReadOnlyDocumentCollection<T> sourceCollection)
        {
            _sourceCollection = sourceCollection;
        }

        public T Find(Expression<Func<T, bool>> filter)
        {
            return _sourceCollection.Find(filter)?.Clone();
        }

        public IEnumerable<T> FindMany(Expression<Func<T, bool>> filter)
        {
            return _sourceCollection.FindMany(filter).Select(x => x?.Clone());
        }
    }
}
