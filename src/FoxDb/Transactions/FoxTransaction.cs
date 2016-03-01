using System;
using System.Collections.Generic;
using System.Linq;

namespace FoxDb.Transactions
{
    internal class FoxTransaction<T> : IFoxTransaction<T>
        where T : new()
    {
        private readonly IFoxTransactionSource<T> _source;
        private readonly List<Action<Dictionary<string, T>>> _operations;

        public TransactionState State
        {
            get;
            private set;
        }

        public FoxTransaction(IFoxTransactionSource<T> source)
        {
            if (source.ActiveTransaction != null)
                throw new Exception("Source has an open transaction. Commit or Rollback active transaction before creating another transaction.");

            _source = source;
            _source.ActiveTransaction = this;
            _operations = new List<Action<Dictionary<string, T>>>();

            State = TransactionState.Active;
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

        public void Delete(Func<T, bool> predicate)
        {
            _operations.Add(x =>
            {
                foreach (var s in x.Where(kv => predicate(kv.Value)).ToList())
                {
                    x.Remove(s.Key);
                }
            });
        }

        public void Commit()
        {
            if (State != TransactionState.Active)
                throw new Exception($"Transaction is in an invalid state. Expected {TransactionState.Active} but is {State}");

            try
            {
                var itemsCopy = new Dictionary<string, T>(_source.Items);

                foreach (var t in _operations)
                {
                    t(itemsCopy);
                }

                State = TransactionState.Committed;
                _source.ActiveTransaction = null;
                _source.Items = itemsCopy;
            }
            catch
            {
                State = TransactionState.Aborted;
                throw;
            }
        }

        public void Rollback()
        {
            _source.ActiveTransaction = null;
            State = TransactionState.Aborted;
        }

        public void Dispose()
        {
            if (State == TransactionState.Active)
                State = TransactionState.Aborted;

            _source.ActiveTransaction = null;
            _operations.Clear();
        }
    }
}
