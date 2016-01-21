using System;
using System.Collections.Generic;
using System.Linq;
using FoxDb.Tests.Stubs;
using FoxDb.Transactions;
using Xunit;

namespace FoxDb.Tests
{
    public class TransactionTests
    {

        [Fact]
        public void Collection_Creates_Transaction()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            var tran = collection.BeginTransaction();

            Assert.NotNull(tran);
            Assert.NotNull(((IFoxTransactionSource<int>)collection).ActiveTransaction);
            Assert.Equal(TransactionState.Active, tran.State);
        }

        [Fact]
        public void Collection_In_Using_Block_Creates_Transaction()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            using (var tran = collection.BeginTransaction())
            {
                Assert.NotNull(tran);
                Assert.NotNull(((IFoxTransactionSource<int>)collection).ActiveTransaction);
            }

            Assert.Null(((IFoxTransactionSource<int>)collection).ActiveTransaction);
        }

        [Fact]
        public void Transaction_Can_Commit()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            var tran = collection.BeginTransaction();

            tran.Commit();
        }

        [Fact]
        public void Transaction_Commit_Changes_State()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            var tran = collection.BeginTransaction();

            tran.Commit();

            Assert.Equal(TransactionState.Committed, tran.State);
        }

        [Fact]
        public void Transaction_With_Operations_Can_Commit()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            var tran = collection.BeginTransaction();

            tran.Insert(1);

            tran.Commit();

            Assert.Equal(TransactionState.Committed, tran.State);
        }

        [Fact]
        public void Transaction_In_Using_Block_Commits()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            IFoxTransaction<int> tran;

            using (tran = collection.BeginTransaction())
            {
                tran.Insert(1);
            }

            Assert.Equal(TransactionState.Committed, tran.State);
        }

        [Fact]
        public void Transaction_Rollback_Changes_State_To_Aborted()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            var tran = collection.BeginTransaction();

            tran.Insert(1);

            tran.Rollback();

            Assert.Equal(TransactionState.Aborted, tran.State);
            Assert.Null(((IFoxTransactionSource<int>)collection).ActiveTransaction);
        }

        [Fact]
        public void Transaction_Failed_Commit_Sets_State_To_Aborted()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            var tran = collection.BeginTransaction();
            try
            {
                tran.Update("NON EXISTENT KEY", 1);

                tran.Commit();
            }
            catch
            {
                //ignored
            }

            Assert.Equal(TransactionState.Aborted, tran.State);
        }

        [Fact]
        public void Transaction_Rollback_Sets_State_To_Aborted_Clears_Active_Transaction()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            var tran = collection.BeginTransaction();

            tran.Rollback();

            Assert.Equal(TransactionState.Aborted, tran.State);
            Assert.Null(((IFoxTransactionSource<int>)collection).ActiveTransaction);
        }

        [Fact]
        public void Transaction_Rollback_Cannot_Commit()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            var tran = collection.BeginTransaction();

            tran.Rollback();

            Assert.Equal(TransactionState.Aborted, tran.State);
            Assert.Null(((IFoxTransactionSource<int>)collection).ActiveTransaction);
            Assert.ThrowsAny<Exception>(() => tran.Commit());
        }

        [Fact]
        public void Transaction_Rollback_Can_Rollback()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            var tran = collection.BeginTransaction();

            tran.Rollback();
            tran.Rollback();

            Assert.Equal(TransactionState.Aborted, tran.State);
            Assert.Null(((IFoxTransactionSource<int>)collection).ActiveTransaction);
        }

        [Fact]
        public void Transaction_Insertion_On_Commit()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            using (var tran = collection.BeginTransaction())
            {
                tran.Insert(1);
                tran.Insert(2);
                tran.Insert(3);
            }

            Assert.Contains(1, collection);
            Assert.Contains(2, collection);
            Assert.Contains(3, collection);
        }

        [Fact]
        public void Transaction_Insertion_On_Commit_Contains_Keys()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);
            var keys = new List<string>();

            using (var tran = collection.BeginTransaction())
            {
                keys.Add(tran.Insert(1));
                keys.Add(tran.Insert(2));
                keys.Add(tran.Insert(3));
            }

            Assert.True(!keys.Except(collection.Keys).Any() && keys.Count == collection.Keys.Count);
        }

    }
}
