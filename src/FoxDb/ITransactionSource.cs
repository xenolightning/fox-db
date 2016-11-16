using System.Collections.Generic;
using FoxDb.Transactions;

namespace FoxDb
{
    internal interface ITransactionSource
    {
        bool CollectionExists<T>(string name) where T : class ;

        IFoxCollection<T> GetCollection<T>(string name) where T : class;

        void Apply(IList<ITransactionCommand> actions);

    }
}