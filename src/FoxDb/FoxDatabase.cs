using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoxDb
{
    public sealed class FoxDatabase : ITransactionCache
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

        public bool CollectionExists<T>(string name) where T : class
        {
            return _collectionIndex.ContainsKey(name) && _collectionIndex[name] == typeof(T);
        }

        public IFoxCollection<T> GetCollection<T>(string name) where T : class
        {
            throw new NotImplementedException();
        }
    }

    internal interface ITransactionCache
    {
        bool CollectionExists<T>(string name) where T : class ;

        IFoxCollection<T> GetCollection<T>(string name) where T : class;

    }
}
