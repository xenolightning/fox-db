using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxDb;
using Newtonsoft.Json;

namespace ConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {

            var store = new FoxTable(new JsonSerializationStrategy(new FileSystemStore("hello")));

            var store2 = new FoxTable(new BsonSerializationStrategy(new FileSystemStore("hello2")));

            store["test1"] = new Test1();
            store["test2"] = new Test2();
            store["test12"] = new Test1();
            store["test22"] = new Test2();
            store2["test1"] = new Test1();
            store2["test2"] = new Test2();

            store.Save();
            store2.Save();
        }
    }

    public class Test1
    {
        
    }

    public class Test2
    {

    }
}
