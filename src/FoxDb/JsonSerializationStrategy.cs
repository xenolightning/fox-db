using System.IO;
using Newtonsoft.Json;

namespace FoxDb
{
    public class JsonSerializationStrategy : ISerializationStrategy
    {

        private static readonly JsonSerializerSettings DefaultSerializationSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private readonly IDataStore _dataStore;
        private readonly JsonSerializerSettings _serializerSettings;

        public JsonSerializationStrategy(IDataStore dataStore)
            : this(dataStore, DefaultSerializationSettings)
        {
        }

        public JsonSerializationStrategy(IDataStore dataStore, JsonSerializerSettings serializerSettings)
        {
            _dataStore = dataStore;
            _serializerSettings = serializerSettings;
        }

        public void Serialize(object table)
        {
            using (var stream = _dataStore.OpenWrite())
            using (var streamWriter = new StreamWriter(stream))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                var serializer = JsonSerializer.CreateDefault(_serializerSettings);
                serializer.Serialize(jsonWriter, table);
            }
        }

        public object Deserialize()
        {
            try
            {
                using (var stream = _dataStore.OpenRead())
                using (var streamReader = new StreamReader(stream))
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    var serializer = JsonSerializer.CreateDefault(_serializerSettings);
                    return serializer.Deserialize<object>(jsonReader);
                }
            }
            catch
            {
                return null;
            }
        }

        public T Deserialize<T>()
        {
            try
            {
                using (var stream = _dataStore.OpenRead())
                using (var streamReader = new StreamReader(stream))
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    var serializer = JsonSerializer.CreateDefault(_serializerSettings);
                    return serializer.Deserialize<T>(jsonReader);
                }
            }
            catch
            {
                return default(T);
            }
        }
    }
}
