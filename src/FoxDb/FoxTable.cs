using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FoxDb
{
    public class FoxTable : IDictionary<string, object>
    {

        private readonly ISerializationStrategy _serializationStrategy;
        private readonly Dictionary<string, object> _items;

        public int Count => _items.Count;
        public bool IsReadOnly => false;
        public ICollection<string> Keys => _items.Keys;
        public ICollection<object> Values => _items.Values;

        public object this[string key]
        {
            get { return _items[key]; }
            set { _items[key] = value; }
        }

        public FoxTable(ISerializationStrategy serializationStrategy)
        {
            _serializationStrategy = serializationStrategy;
            _items = _serializationStrategy.Deserialize<Dictionary<string, object>>() ?? new Dictionary<string, object>();
        }

        public void Save()
        {
            _serializationStrategy.Serialize(_items);
        }

        public void Add(KeyValuePair<string, object> item)
        {
            _items.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _items.Clear();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> keyValuePair)
        {
            if (!_items.ContainsKey(keyValuePair.Key))
                return false;

            if (EqualityComparer<object>.Default.Equals(_items[keyValuePair.Key], keyValuePair.Value))
            {
                return true;
            }

            return false;
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0 || arrayIndex > array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("Array too small", nameof(array));

            foreach (var entry in _items)
            {
                if (entry.GetHashCode() >= 0)
                {
                    array[arrayIndex++] = new KeyValuePair<string, object>(entry.Key, entry.Value);
                }
            }
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            if (_items.ContainsKey(item.Key))
                return _items.Remove(item.Key);

            return false;
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool ContainsKey(string key)
        {
            return _items.ContainsKey(key);
        }

        public void Add(string key, object value)
        {
            _items.Add(key, value);
        }

        public bool Remove(string key)
        {
            return _items.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _items.TryGetValue(key, out value);
        }

    }
}
