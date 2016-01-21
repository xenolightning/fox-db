using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FoxDb.Transactions
{
    public interface IFoxTransaction<T> : IDisposable
        where T : new()
    {
        TransactionState State { get; }

        string Insert(T value);

        void Update(string key, T value);

        void Delete(string key);

        void Delete(Func<T, bool> predicate);

        void Commit();

        void Rollback();

    }

    public enum TransactionState
    {
        Active,
        Committed,
        Aborted
    }
}
