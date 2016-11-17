using System;
using System.Collections.Generic;

namespace FoxDb.Transactions
{
    public interface IFoxTransaction : IDisposable
    {

        Guid TransactionId { get; }

        IFoxCollection GetCollection(string name);

        IDocumentCollection<T> GetCollection<T>(string name) where T : class;

        IKeyValueCollection<TKey, TValue> GetCollection<TKey, TValue>(string name)
            where TValue : class
            where TKey : struct;

        IFoxTransaction BeginTransaction();

        void Commit();

        void Rollback();

    }

    internal class FoxTransaction : IFoxTransaction, ITransactionSource
    {
        private readonly ITransactionSource _transactionSource;
        private readonly Guid _transactionId;

        Guid IFoxTransaction.TransactionId => _transactionId;

        public FoxTransaction(ITransactionSource transactionSource)
        {
            _transactionId = Guid.NewGuid();
            _transactionSource = transactionSource;
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        bool ITransactionSource.CollectionExists<T>(string name)
        {
            throw new NotImplementedException();
        }

        bool ITransactionSource.CollectionExists<TKey, TValue>(string name)
        {
            throw new NotImplementedException();
        }

        IFoxCollection ITransactionSource.GetCollection(string name)
        {
            throw new NotImplementedException();
        }

        IDocumentCollection<T> ITransactionSource.GetCollection<T>(string name)
        {
            throw new NotImplementedException();
        }

        IKeyValueCollection<TKey, TValue> ITransactionSource.GetCollection<TKey, TValue>(string name)
        {
            throw new NotImplementedException();
        }

        void ITransactionSource.Apply(IList<ITransactionCommand> actions)
        {
            throw new NotImplementedException();
        }

        IFoxCollection IFoxTransaction.GetCollection(string name)
        {
            throw new NotImplementedException();
        }

        IDocumentCollection<T> IFoxTransaction.GetCollection<T>(string name)
        {
            throw new NotImplementedException();
        }

        IKeyValueCollection<TKey, TValue> IFoxTransaction.GetCollection<TKey, TValue>(string name)
        {
            throw new NotImplementedException();
        }

        IFoxTransaction IFoxTransaction.BeginTransaction()
        {
            throw new NotImplementedException();
        }

        void IFoxTransaction.Commit()
        {
            throw new NotImplementedException();
        }

        void IFoxTransaction.Rollback()
        {
            throw new NotImplementedException();
        }
    }
}