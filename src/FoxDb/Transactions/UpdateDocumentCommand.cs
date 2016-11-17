using System;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace FoxDb.Transactions
{
    internal class UpdateDocumentCommand<T> : IDocumentCommand<T>
        where T : class
    {
        private string _collectionName;
        private readonly Expression<Func<T, bool>> _filter;
        private Type _itemType;
        private string _itemData;

        public UpdateDocumentCommand(string collectionName, T item, Expression<Func<T, bool>> filter)
        {
            _collectionName = collectionName;
            _filter = filter;
            _itemType = typeof(T);
            _itemData = JsonConvert.SerializeObject(item);

        }

        public void Apply(ITransactionSource transactionSource)
        {
            var collection = transactionSource.GetCollection<T>(_collectionName);
            collection.Insert(JsonConvert.DeserializeObject<T>(_itemData));
        }

        public override string ToString()
        {
            return $"INSERT [{_itemData}] OF TYPE [{_itemType}] INTO [{_collectionName}]";
        }
    }
}
