using System.IO;

namespace FoxDb
{
    public class FileSystemStore : IDataStore
    {
        private readonly string _filePath;

        public FileSystemStore(string filePath)
        {
            _filePath = filePath;
        }

        public Stream OpenRead()
        {
            return File.Open(_filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
        }

        public Stream OpenWrite()
        {
            return File.Open(_filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
        }

    }
}
