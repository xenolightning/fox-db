namespace FoxDb.Transactions
{
    internal interface ITransactionCommand
    {

    }

    internal interface IDocumentCommand<T> : ITransactionCommand
        where T : class
    {
        void Apply(ITransactionSource transactionSource);
    }

    internal interface IKeyValueCommand<TKey, TValue> : ITransactionCommand
            where TValue : class
            where TKey : struct
    {
        void Apply(ITransactionSource transactionSource);
    }
}