using System;
using Newtonsoft.Json;

namespace FoxDb.Transactions
{
    internal class InsertDocumentAction<T> : ITransactionAction<T>
    {
        private readonly string _collectionName;
        private readonly Type _itemType;
        private string _itemData;

        public InsertDocumentAction(T item, string collectionName)
        {
            _collectionName = collectionName;
            _itemType = typeof(T);
            _itemData = JsonConvert.SerializeObject(item);
        }
    }
}
