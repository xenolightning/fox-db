namespace FoxDb.Transactions
{
    internal interface ITransactionCommand
    {

    }

    internal interface ITransactionCommand<T> : ITransactionCommand
        where T : class
    {
        void Apply(ITransactionSource transactionSource);
    }
}