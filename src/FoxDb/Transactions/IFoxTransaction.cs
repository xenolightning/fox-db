using System;

namespace FoxDb.Transactions
{
    public interface IFoxTransaction
    {

        Guid TransactionId { get; }

        IFoxCollection GetCollection(string name);

        IFoxCollection<T> GetCollection<T>(string name) where T : class;

        IFoxTransaction BeginTransaction();

        void Commit();

        void Rollback();

    }

    internal class FoxTransaction : IFoxTransaction, ITransactionSource
    {
        private readonly ITransactionSource _transactionSource;

        public FoxTransaction(ITransactionSource transactionSource)
        {
            _transactionSource = transactionSource;
        }
    }
}