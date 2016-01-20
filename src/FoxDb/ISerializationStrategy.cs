namespace FoxDb
{
    public interface ISerializationStrategy
    {

        void Serialize(object table);

        object Deserialize();

        T Deserialize<T>();

    }
}
