﻿using System;
using System.Collections.Generic;

namespace FoxDb.Transactions
{
    internal class FoxTransaction<T> : IFoxTransaction<T>
        where T : new()
    {
        private readonly IFoxTransactionSource<T> _source;
        private readonly List<Action<Dictionary<string, T>>> _operations;
        private TransactionState _state;

        public FoxTransaction(IFoxTransactionSource<T> source)
        {
            if (source.ActiveTransaction != null)
                throw new Exception($"Source has an open transaction. Commit or Rollback active transaction before creating another transaction.");

            _source = source;
            _source.ActiveTransaction = this;
            _operations = new List<Action<Dictionary<string, T>>>();

            _state = TransactionState.Active;
        }

        public TransactionState State
        {
            get
            {
                return _state;
            }
        }

        public string Insert(T value)
        {
            string key = Guid.NewGuid().ToString();

            _operations.Add(x =>
            {
                x.Add(key, value);
            });

            return key;
        }

        public void Update(string key, T value)
        {
            _operations.Add(x =>
            {
                if (!x.ContainsKey(key))
                    throw new Exception($"Object with key '{key}' does not exist in table");

                x[key] = value;
            });
        }

        public void Delete(string key)
        {
            _operations.Add(x =>
            {
                if (!x.ContainsKey(key))
                    throw new Exception($"Object with key '{key}' does not exist in table");

                x.Remove(key);
            });
        }

        public void Commit()
        {
            if (_state != TransactionState.Active)
                throw new Exception(String.Format("Transaction is in an invalid state. Expected {0} but is {1}", TransactionState.Active, _state));

            try
            {

                var itemsCopy = new Dictionary<string, T>(_source.Items);
                for (int i = 0; i < _operations.Count; i++)
                {
                    _operations[i](itemsCopy);
                }

                _source.Items = itemsCopy;
                _state = TransactionState.Committed;
                _source.ActiveTransaction = null;
            }
            catch
            {
                _state = TransactionState.Aborted;
                throw;
            }
        }

        public void Rollback()
        {
            _source.ActiveTransaction = null;
            _state = TransactionState.Aborted;
        }

        public void Dispose()
        {
            Commit();
        }
    }
}
