using System;
using System.Diagnostics;
using FoxDb;

namespace ConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();

            var store = new FoxTable(new JsonSerializationStrategy(new FileSystemStore("hello"))); //jit
            var store2 = new FoxTable(new JsonSerializationStrategy(new CompressedFileSystemStore("hello2"))); //jit


            sw.Restart();
            for (int i = 0; i < 1000; i++)
                store = new FoxTable(new JsonSerializationStrategy(new FileSystemStore("hello")));
            sw.Stop();

            Console.WriteLine("Uncompressed Load: {0}", (double)sw.ElapsedMilliseconds / 1000);

            sw.Restart();
            for (int i = 0; i < 1000; i++)
                store2 = new FoxTable(new JsonSerializationStrategy(new CompressedFileSystemStore("hello2")));
            sw.Stop();

            Console.WriteLine("Compressed Load: {0}", (double)sw.ElapsedMilliseconds / 1000);

            for (int i = 0; i < 1000; i++)
            {
                store[i + "T1"] = new Test1();
                store[i + "T2"] = new Test2();
                store2[i + "T1"] = new Test1();
                store2[i + "T2"] = new Test2();
            }

            store.Save(); //jit
            sw.Restart();
            for (int i = 0; i < 1000; i++)
                store.Save();
            sw.Stop();

            Console.WriteLine("Uncompressed Save: {0}", (double)sw.ElapsedMilliseconds / 1000);

            store2.Save(); //jit
            sw.Restart();
            for (int i = 0; i < 1000; i++)
                store2.Save();
            sw.Stop();

            Console.WriteLine("Compressed Save: {0}", (double)sw.ElapsedMilliseconds / 1000);

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
