using System;
using Newtonsoft.Json;

namespace FoxDb.Transactions
{
    internal class InsertDocumentCommand<T> : IDocumentCommand<T>
        where T : class
    {
        private string _collectionName;
        private Type _itemType;
        private string _itemData;

        public InsertDocumentCommand(string collectionName, T item)
        {
            _collectionName = collectionName;
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
