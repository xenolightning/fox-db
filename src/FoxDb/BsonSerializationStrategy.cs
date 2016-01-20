using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace FoxDb
{
    public class BsonSerializationStrategy : ISerializationStrategy
    {

        private static readonly JsonSerializerSettings DefaultSerializationSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private readonly IDataStore _dataStore;
        private readonly JsonSerializerSettings _serializerSettings;

        public BsonSerializationStrategy(IDataStore dataStore)
            :this(dataStore, DefaultSerializationSettings)
        {
        }

        public BsonSerializationStrategy(IDataStore dataStore, JsonSerializerSettings serializerSettings)
        {
            _dataStore = dataStore;
            _serializerSettings = serializerSettings;
        }

        public void Serialize(object table)
        {
            using (var stream = _dataStore.OpenWrite())
            using (var bsonWriter = new BsonWriter(stream))
            {
                var serializer = JsonSerializer.CreateDefault(_serializerSettings);
                serializer.Serialize(bsonWriter, table);
            }
        }

        public object Deserialize()
        {
            using (var stream = _dataStore.OpenRead())
            using (var bsonReader = new BsonReader(stream))
            {
                var serializer = JsonSerializer.CreateDefault(_serializerSettings);
                return serializer.Deserialize<object>(bsonReader);
            }
        }

        public T Deserialize<T>()
        {
            using (var stream = _dataStore.OpenRead())
            using (var bsonReader = new BsonReader(stream))
            {
                var serializer = JsonSerializer.CreateDefault(_serializerSettings);
                return serializer.Deserialize<T>(bsonReader);
            }
        }
    }
}
