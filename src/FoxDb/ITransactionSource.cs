using System.Collections.Generic;
using FoxDb.Transactions;

namespace FoxDb
{
    internal interface ITransactionSource
    {
        bool CollectionExists<T>(string name) where T : class;

        bool CollectionExists<TKey, TValue>(string name)
            where TValue : class
            where TKey : struct;

        IFoxCollection GetCollection(string name);

        IDocumentCollection<T> GetCollection<T>(string name) where T : class;

        IKeyValueCollection<TKey, TValue> GetCollection<TKey, TValue>(string name)
            where TValue : class
            where TKey : struct;

        void Apply(IList<ITransactionCommand> actions);

    }
}