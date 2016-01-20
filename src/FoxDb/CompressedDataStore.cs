using System.IO;
using System.IO.Compression;

namespace FoxDb
{
    /// <summary>
    /// GZip Compresses the contents to the base data store. Introduces about an extra 10-30% on CPU time, but dramatically decreases the payload size.
    /// </summary>
    public class CompressedDataStore : IDataStore
    {
        private readonly IDataStore _baseDataStore;

        public CompressedDataStore(IDataStore baseDataStore)
        {
            _baseDataStore = baseDataStore;
        }

        public Stream OpenRead()
        {
            return new GZipStream(_baseDataStore.OpenRead(), CompressionMode.Decompress, false);
        }

        public Stream OpenWrite()
        {
            return new GZipStream(_baseDataStore.OpenWrite(), CompressionLevel.Fastest, false);
        }

    }
}
