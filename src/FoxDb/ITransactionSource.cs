using System.Collections.Generic;

namespace FoxDb
{
    internal interface ITransactionSource
    {
        bool CollectionExists<T>(string name) where T : class ;

        IFoxCollection<T> GetCollection<T>(string name) where T : class;

        void Process(IList<ITransactionAction> actions);

    }

    internal interface ITransactionAction
    {

    }

    internal interface ITransactionAction<T> : ITransactionAction
        where T : class
    {
        void Apply(IFoxCollection<T> collection);
    }
}