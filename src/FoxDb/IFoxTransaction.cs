using System.Collections.Generic;

namespace FoxDb
{
    public interface IFoxTransaction
    {

        IFoxCollection GetCollection(string name);

        IFoxCollection<T> GetCollection<T>(string name) where T : class;

        IFoxTransaction BeginTransaction();

        void Commit();

        void Rollback();

    }

    internal class FoxTransaction : IFoxTransaction, ITransactionCache
    {
        private readonly ITransactionCache _transactionCache;

        public FoxTransaction(ITransactionCache transactionCache)
        {
            _transactionCache = transactionCache;
        }

        IFoxCollection IFoxTransaction.GetCollection(string name)
        {
            throw new System.NotImplementedException();
        }

        bool ITransactionCache.CollectionExists<T>(string name)
        {
            throw new System.NotImplementedException();
        }

        IFoxCollection<T> ITransactionCache.GetCollection<T>(string name)
        {
            throw new System.NotImplementedException();
        }

        IFoxCollection<T> IFoxTransaction.GetCollection<T>(string name)
        {
            throw new System.NotImplementedException();
        }

        IFoxTransaction IFoxTransaction.BeginTransaction()
        {
            throw new System.NotImplementedException();
        }

        void IFoxTransaction.Commit()
        {
            throw new System.NotImplementedException();
        }

        void IFoxTransaction.Rollback()
        {
            throw new System.NotImplementedException();
        }
    }
}