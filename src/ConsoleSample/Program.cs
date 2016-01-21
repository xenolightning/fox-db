using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using FoxDb;

namespace ConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();

            var store = new FoxCollection<object>(new JsonSerializationStrategy(new FileSystemStore("hello"))); //jit
            var store2 = new FoxCollection<object>(new JsonSerializationStrategy(new CompressedDataStore(new FileSystemStore("hello2")))); //jit


            sw.Restart();

            //for (int i = 0; i < 1000; i++)
            store = new FoxCollection<object>(new JsonSerializationStrategy(new FileSystemStore("hello")));

            sw.Stop();

            Console.WriteLine("Uncompressed Load: {0}", (double)sw.ElapsedMilliseconds);

            sw.Restart();

            //for (int i = 0; i < 1000; i++)
            store2 = new FoxCollection<object>(new JsonSerializationStrategy(new CompressedDataStore(new FileSystemStore("hello2"))));

            sw.Stop();

            Console.WriteLine("Compressed Load: {0}", (double)sw.ElapsedMilliseconds);

            sw.Restart();
            using (var trans = store.BeginTransaction())
            {
                for (int i = 0; i < 10000; i++)
                {
                    trans.Insert(new Test1());
                    trans.Insert(new Test2());
                }

            }

            sw.Stop();

            Console.WriteLine("One Tran: {0}", (double)sw.ElapsedMilliseconds);



            sw.Restart();
            for (int i = 0; i < 10000; i++)
            {
                var tran = store2.BeginTransaction();
                try
                {

                    tran.Insert(new Test1());
                    tran.Insert(new Test2());
                    //tran.Update(Guid.NewGuid().ToString(), new Test1());


                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                }
            }

            sw.Stop();

            Console.WriteLine("Multi Tran: {0}", (double)sw.ElapsedMilliseconds);

            store.Save(); //jit

            sw.Restart();
            store.Save(); //jit
            //for (int i = 0; i < 1000; i++)
            //    store.Save();
            sw.Stop();

            Console.WriteLine("Uncompressed Save: {0}", (double)sw.ElapsedMilliseconds);

            store2.Save(); //jit

            sw.Restart();
            store2.Save(); //jit
            //for (int i = 0; i < 1000; i++)
            //    store2.Save();
            sw.Stop();

            Console.WriteLine("Compressed Save: {0}", (double)sw.ElapsedMilliseconds);

            Console.ReadKey(true);
        }
    }

    public class Test1
    {

    }

    public class Test2
    {

    }
}
