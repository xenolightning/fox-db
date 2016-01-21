using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxDb.Tests.Stubs;
using Xunit;

namespace FoxDb.Tests
{
    public class CollectionTests
    {

        [Fact]
        public void Collection_Insert_Increases_Count()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            using (var tran = collection.BeginTransaction())
            {
                tran.Insert(1);
            }

            Assert.Equal(1, collection.Count);
            Assert.Equal(1, collection.Keys.Count);
        }


        [Fact]
        public void Collection_Insert_Increases_Adds_A_Value()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            using (var tran = collection.BeginTransaction())
            {
                tran.Insert(1);
            }

            Assert.Equal(1, collection.Count);
            Assert.Equal(1, collection.Keys.Count);
            Assert.Equal(1, collection.Values.Count);
            Assert.Equal(1, collection.First());
        }

        [Fact]
        public void Collection_Delete_Decreases_Count()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            using (var tran = collection.BeginTransaction())
            {
                tran.Insert(1);
            }

            Assert.Equal(1, collection.Count);

            using (var tran = collection.BeginTransaction())
            {
                tran.Delete(x => x == 1);
            }

            Assert.Equal(0, collection.Count);
        }

        [Fact]
        public void Collection_Is_Not_ReadOnly()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default);

            Assert.False(collection.IsReadOnly);
        }

        [Fact]
        public void Collection_As_Enumerable()
        {
            var collection = new FoxCollection<int>(NullSerializationStrategy.Default) as IEnumerable;

            Assert.NotNull(collection);

            foreach (var v in collection)
            {
                //Assert that it's still enumerable
            }

        }

    }
}
