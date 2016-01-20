using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace FoxDb
{
    /// <summary>
    /// GZip Compresses the contents before writing to disk. Is about 10-30% slower on reads/writes, but may save up to 95% on disk space.
    /// Use at your own will. 
    /// </summary>
    public class CompressedFileSystemStore : FileSystemStore
    {

        public CompressedFileSystemStore(string filePath)
            :base(filePath)
        {
        }

        public override Stream OpenRead()
        {
            return new GZipStream(base.OpenRead(), CompressionMode.Decompress, false);
        }

        public override Stream OpenWrite()
        {
            return new GZipStream(base.OpenWrite(), CompressionLevel.Optimal, false);
        }

    }
}
