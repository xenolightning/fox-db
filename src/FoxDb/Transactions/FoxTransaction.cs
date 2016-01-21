using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FoxDb.Transactions
{
    internal class FoxTransaction<T> : IFoxTransaction<T>
        where T : new()
    {
        private readonly FoxCollection<T> _collection;
        private List<Action<Dictionary<string, T>>> _operations;

        public FoxTransaction(FoxCollection<T> collection)
        {
            _collection = collection;
            _operations = new List<Action<Dictionary<string, T>>>();
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
                    throw new Exception(String.Format("Object with key '{0}' does not exist in table", key));

                x[key] = value;
            });
        }

        public void Delete(string key)
        {
            _operations.Add(x =>
            {
                if (!x.ContainsKey(key))
                    throw new Exception(String.Format("Object with key '{0}' does not exist in table", key));

                x.Remove(key);
            });
        }

        public void Commit()
        {
            var itemsCopy = new Dictionary<string, T>(_collection.Items);
            for (int i = 0; i < _operations.Count; i++)
            {
                _operations[i](itemsCopy);
            }

            _collection.Items = itemsCopy;
        }

        public void Rollback()
        {
        }

        public void Dispose()
        {
            Commit();
        }
    }
}
