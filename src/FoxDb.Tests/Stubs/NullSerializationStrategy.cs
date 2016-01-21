using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxDb.Tests.Stubs
{
    public class NullSerializationStrategy : ISerializationStrategy
    {

        public static NullSerializationStrategy Default = new NullSerializationStrategy();

        private NullSerializationStrategy()
        {
            
        }

        public void Serialize(object table)
        {
        }

        public object Deserialize()
        {
            return null;
        }

        public T Deserialize<T>()
        {
            return default(T);
        }
    }
}
