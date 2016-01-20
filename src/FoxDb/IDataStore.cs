using System.IO;

namespace FoxDb
{
    public interface IDataStore
    {

        Stream OpenRead();

        Stream OpenWrite();

    }
}
