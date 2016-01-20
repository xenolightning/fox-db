using System;
using System.Collections;
using System.Collections.Generic;

namespace FoxDb
{
    public class FoxTable : IDictionary<string, object>
    {

        private readonly ISerializationStrategy _serializationStrategy;
        private readonly Dictionary<string, object> _items;
        private readonly object _syncRoot = new object();

        public int Count => _items.Count;
        public bool IsReadOnly => false;
        public ICollection<string> Keys => _items.Keys;
        public ICollection<object> Values => _items.Values;

        public object this[string key]
        {
            get
            {
                lock (_syncRoot)
                {
                    return _items[key];
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _items[key] = value;
                }
            }
        }

        public FoxTable(ISerializationStrategy serializationStrategy)
        {
            _serializationStrategy = serializationStrategy;
            _items = _serializationStrategy.Deserialize<Dictionary<string, object>>() ?? new Dictionary<string, object>();
        }

        public void Save()
        {
            lock (_syncRoot)
            {
                _serializationStrategy.Serialize(_items);
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            lock (_syncRoot)
            {
                _items.Add(item.Key, item.Value);
            }
        }

        public void Clear()
        {
            lock (_syncRoot)
            {
                _items.Clear();
            }
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> keyValuePair)
        {
            lock (_syncRoot)
            {
                if (!_items.ContainsKey(keyValuePair.Key))
                    return false;

                if (EqualityComparer<object>.Default.Equals(_items[keyValuePair.Key], keyValuePair.Value))
                {
                    return true;
                }
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

            lock (_syncRoot)
            {
                foreach (var entry in _items)
                {
                    if (entry.GetHashCode() >= 0)
                    {
                        array[arrayIndex++] = new KeyValuePair<string, object>(entry.Key, entry.Value);
                    }
                }
            }
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            lock (_syncRoot)
            {
                if (_items.ContainsKey(item.Key))
                    return _items.Remove(item.Key);
            }

            return false;
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
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

        public bool ContainsKey(string key)
        {
            lock (_syncRoot)
            {
                return _items.ContainsKey(key);
            }
        }

        public void Add(string key, object value)
        {
            lock (_syncRoot)
            {
                _items.Add(key, value);
            }
        }

        public bool Remove(string key)
        {
            lock (_syncRoot)
            {
                return _items.Remove(key);
            }
        }

        public bool TryGetValue(string key, out object value)
        {
            lock (_syncRoot)
            {
                return _items.TryGetValue(key, out value);
            }
        }
    }
}
