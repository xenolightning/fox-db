using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxDb.Transactions
{
    public interface IFoxTransaction<in T> : IDisposable
        where T : new()
    {
        TransactionState State { get; }

        string Insert(T value);

        void Update(string key, T value);

        void Delete(string key);

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
