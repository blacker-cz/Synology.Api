using System.IO;

namespace Blacker.Synology.Api.Client
{
    interface ISerializer
    {
        T Deserialize<T>(Stream stream) where T : class;
        void Serialize(object graph, Stream stream);
    }
}
