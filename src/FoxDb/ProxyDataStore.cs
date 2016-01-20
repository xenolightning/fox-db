using System;
using System.IO;

namespace FoxDb
{
    public class ProxyDataStore : MarshalByRefObject, IDataStore
    {
        private readonly IDataStore _localStore;

        public ProxyDataStore(IDataStore localStore)
        {
            _localStore = localStore;
        }

        public Stream OpenRead()
        {
            return _localStore.OpenRead();
        }

        public Stream OpenWrite()
        {
            return _localStore.OpenWrite();
        }
    }
}
