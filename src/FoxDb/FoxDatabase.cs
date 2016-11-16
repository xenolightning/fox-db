using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FoxDb.Transactions;

namespace FoxDb
{
    public sealed class FoxDatabase : ITransactionSource
    {

        private readonly Dictionary<string, IFoxCollection> _collections;
        private readonly Dictionary<string, Type> _collectionIndex;

        public FoxDatabase()
        {
            _collectionIndex = new Dictionary<string, Type>();
            _collections = new Dictionary<string, IFoxCollection>();
        }

        public IFoxTransaction BeginTransaction()
        {
            return new FoxTransaction(this);
        }

        bool ITransactionSource.CollectionExists<T>(string name)
        {
            return _collectionIndex.ContainsKey(name) && _collectionIndex[name] == typeof(T);
        }

        IFoxCollection<T> ITransactionSource.GetCollection<T>(string name)
        { 
            throw new NotImplementedException();
        }

        void ITransactionSource.Apply(IList<ITransactionCommand> actions)
        {
            throw new NotImplementedException();
        }
    }
}
