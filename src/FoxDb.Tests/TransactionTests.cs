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
            IFoxTransaction<int> tran;

            using (tran = collection.BeginTransaction())
            {
                Assert.NotNull(tran);
                Assert.NotNull(((IFoxTransactionSource<int>)collection).ActiveTransaction);
            }

            Assert.Null(((IFoxTransactionSource<int>)collection).ActiveTransaction);
            Assert.Equal(TransactionState.Aborted, tran.State);
        }

        [Fact]
        public void Transaction_Dispose_Aborts_Uncommited()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);
            IFoxTransaction<int> tran;

            using (tran = collection.BeginTransaction())
            {
                Assert.NotNull(tran);
                Assert.NotNull(((IFoxTransactionSource<int>)collection).ActiveTransaction);
            }

            Assert.Null(((IFoxTransactionSource<int>)collection).ActiveTransaction);
            Assert.Equal(TransactionState.Aborted, tran.State);
        }

        [Fact]
        public void Transaction_Dispose_After_Commit()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);
            IFoxTransaction<int> tran;

            using (tran = collection.BeginTransaction())
            {
                Assert.NotNull(tran);
                Assert.NotNull(((IFoxTransactionSource<int>)collection).ActiveTransaction);
                tran.Commit();
            }

            Assert.Null(((IFoxTransactionSource<int>)collection).ActiveTransaction);
            Assert.Equal(TransactionState.Committed, tran.State);
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
                tran.Commit();
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

                Assert.DoesNotContain(1, collection);
                Assert.DoesNotContain(2, collection);
                Assert.DoesNotContain(3, collection);

                tran.Commit();
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

                tran.Commit();
            }

            Assert.True(!keys.Except(collection.Keys).Any() && keys.Count == collection.Keys.Count);
        }

        [Fact]
        public void Transaction_Cannot_Have_Nested_Transaction()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            collection.BeginTransaction();

            Assert.ThrowsAny<Exception>(() => collection.BeginTransaction());
        }

        [Fact]
        public void Transaction_Can_Update_Record()
        {

            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);
            string itemKey;

            using (var tran = collection.BeginTransaction())
            {
                itemKey = tran.Insert(1);

                tran.Update(itemKey, 2);

                tran.Commit();
            }

            Assert.Equal(2, collection.Get(itemKey));
        }

        [Fact]
        public void Transaction_Can_Update_Record_Multi_Transaction()
        {

            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);
            string itemKey;

            using (var tran = collection.BeginTransaction())
            {
                itemKey = tran.Insert(1);

                tran.Commit();
            }

            Assert.Equal(1, collection.Get(itemKey));

            using (var tran = collection.BeginTransaction())
            {
                tran.Update(itemKey, 2);

                tran.Commit();
            }

            Assert.Equal(2, collection.Get(itemKey));
        }

        [Fact]
        public void Transaction_Can_Delete_Record()
        {

            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);
            string itemKey;

            using (var tran = collection.BeginTransaction())
            {
                itemKey = tran.Insert(1);
                tran.Delete(itemKey);
            }

            Assert.Equal(default(int), collection.Get(itemKey));
            Assert.False(collection.Keys.Contains(itemKey));
        }

        [Fact]
        public void Transaction_Can_Delete_Record_Multi_Tran()
        {

            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);
            string itemKey;

            using (var tran = collection.BeginTransaction())
            {
                itemKey = tran.Insert(1);

                tran.Commit();
            }

            Assert.Equal(1, collection.Get(itemKey));
            Assert.True(collection.Keys.Contains(itemKey));

            using (var tran = collection.BeginTransaction())
            {
                tran.Delete(itemKey);

                tran.Commit();
            }

            Assert.Equal(default(int), collection.Get(itemKey));
            Assert.False(collection.Keys.Contains(itemKey));
        }
        [Fact]
        public void Transaction_Delete_Non_Existent_Item_Throws()
        {

            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            var tran = collection.BeginTransaction();

            tran.Delete("NON EXISTENT KEY");

            Assert.ThrowsAny<Exception>(() => tran.Commit());

        }

    }
}
