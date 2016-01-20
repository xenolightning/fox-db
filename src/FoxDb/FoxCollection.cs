using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxDb
{
    public class FoxCollection : ICollection<object>
    {

        private readonly ISerializationStrategy _serializationStrategy;
        private readonly List<object> _items;

        public int Count => _items.Count;
        public bool IsReadOnly => false;

        public FoxCollection(ISerializationStrategy serializationStrategy)
        {
            _serializationStrategy = serializationStrategy;
            _items = _serializationStrategy.Deserialize<List<object>>() ?? new List<object>();
        }

        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public void Add(object item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(object item)
        {
            return _items.Contains(item);
        }

        void ICollection<object>.CopyTo(object[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public bool Remove(object item)
        {
            return _items.Remove(item);
        }

    }
}
