using System.Collections.Generic;

namespace FoxDb.Transactions
{
    internal interface IFoxTransactionSource<T>
        where T : new()
    {

        IDictionary<string, T> Items { get; set; }

        IFoxTransaction<T> ActiveTransaction { get; set; } 

    }
}
