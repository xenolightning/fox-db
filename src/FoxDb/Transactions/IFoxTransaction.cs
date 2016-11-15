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

    internal class FoxTransaction : IFoxTransaction, ITransactionSource
    {
        private readonly ITransactionSource _transactionSource;
    }
}