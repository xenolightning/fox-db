﻿using System;
using System.Collections;
using System.Collections.Generic;
using FoxDb.Transactions;

namespace FoxDb
{
    public class FoxCollection<T> : IFoxTransactionSource<T>, IEnumerable<T>
        where T : new()
    {

        private readonly ISerializationStrategy _serializationStrategy;
        private Dictionary<string, T> _items;
        private readonly object _syncRoot = new object();

        public int Count => _items.Count;
        public bool IsReadOnly => false;
        public ICollection<string> Keys => _items.Keys;
        public ICollection<T> Values => _items.Values;

        public long Timestamp { get; private set; }

        IDictionary<string, T> IFoxTransactionSource<T>.Items
        {
            get
            {
                return _items;
            }
            set
            {
                lock (_syncRoot)
                {
                    _items = new Dictionary<string, T>(value);
                }
            }
        }

        IFoxTransaction<T> IFoxTransactionSource<T>.ActiveTransaction { get; set; }

        public FoxCollection(ISerializationStrategy serializationStrategy)
        {
            _serializationStrategy = serializationStrategy;
            _items = _serializationStrategy.Deserialize<Dictionary<string, T>>() ?? new Dictionary<string, T>();
        }

        public IFoxTransaction<T> BeginTransaction()
        {
            return new FoxTransaction<T>(this);
        }

        public T Get(string key)
        {
            lock (_syncRoot)
            {
                if (_items.ContainsKey(key))
                    return _items[key];
            }

            return default(T);
        }

        public void Save()
        {
            lock (_syncRoot)
            {
                Timestamp = DateTime.Now.AsUnixTimestamp();
                _serializationStrategy.Serialize(_items);
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            lock (_syncRoot)
            {
                return _items.Values.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (_syncRoot)
            {
                return _items.Values.GetEnumerator();
            }
        }
    }
}
