using System.Collections;
using System.Collections.Generic;

namespace FoxDb
{
    public class FoxCollection : ICollection<object>
    {

        private readonly ISerializationStrategy _serializationStrategy;
        private readonly List<object> _items;
        private readonly object _syncRoot = new object();

        public int Count => _items.Count;
        public bool IsReadOnly => false;

        public FoxCollection(ISerializationStrategy serializationStrategy)
        {
            _serializationStrategy = serializationStrategy;
            _items = _serializationStrategy.Deserialize<List<object>>() ?? new List<object>();
        }

        public void Save()
        {
            _serializationStrategy.Serialize(_items);
        }

        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            lock (_syncRoot)
            {
                return _items.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (_syncRoot)
            {
                return _items.GetEnumerator();
            }
        }

        public void Add(object item)
        {
            lock (_syncRoot)
            {
                _items.Add(item);
            }
        }

        public void Clear()
        {
            lock (_syncRoot)
            {
                _items.Clear();
            }
        }

        public bool Contains(object item)
        {
            lock (_syncRoot)
            {
                return _items.Contains(item);
            }
        }

        void ICollection<object>.CopyTo(object[] array, int arrayIndex)
        {
            lock (_syncRoot)
            {
                _items.CopyTo(array, arrayIndex);
            }
        }

        public bool Remove(object item)
        {
            lock (_syncRoot)
            {
                return _items.Remove(item);
            }
        }

    }
}
