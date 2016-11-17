using System.Collections;
using System.Linq;
using FoxDb.Tests.Stubs;
using Xunit;

namespace FoxDb.Tests
{
    public class CollectionTests
    {

        [Fact]
        public void Collection_Insert_Increases_Count()
        {
            var collection = new DocumentCollection<int>(NullSerializationStrategy.Default);

            using (var tran = collection.BeginTransaction())
            {
                tran.Insert(1);

                tran.Commit();
            }

            Assert.Equal(1, collection.Count);
            Assert.Equal(1, collection.Keys.Count);
        }


        [Fact]
        public void Collection_Insert_Increases_Adds_A_Value()
        {
            var collection = new DocumentCollection<int>(NullSerializationStrategy.Default);

            using (var tran = collection.BeginTransaction())
            {
                tran.Insert(1);

                tran.Commit();
            }

            Assert.Equal(1, collection.Count);
            Assert.Equal(1, collection.Keys.Count);
            Assert.Equal(1, collection.Values.Count);
            Assert.Equal(1, collection.First());
        }

        [Fact]
        public void Collection_Delete_Decreases_Count()
        {
            var collection = new DocumentCollection<int>(NullSerializationStrategy.Default);

            using (var tran = collection.BeginTransaction())
            {
                tran.Insert(1);

                tran.Commit();
            }

            Assert.Equal(1, collection.Count);

            using (var tran = collection.BeginTransaction())
            {
                tran.Delete(x => x == 1);

                tran.Commit();
            }

            Assert.Equal(0, collection.Count);
        }

        [Fact]
        public void Collection_Is_Not_ReadOnly()
        {
            var collection = new DocumentCollection<int>(NullSerializationStrategy.Default);

            Assert.False(collection.IsReadOnly);
        }

        [Fact]
        public void Collection_As_Enumerable()
        {
            var collection = new DocumentCollection<int>(NullSerializationStrategy.Default) as IEnumerable;

            Assert.NotNull(collection);

            foreach (var v in collection)
            {
                //Assert that it's still enumerable
            }

        }

    }
}
