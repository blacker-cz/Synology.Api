using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Blacker.Synology.Api.Client
{
    class JsonSerializer : ISerializer
    {
        public void Serialize(object graph, Stream stream)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            GetSerializer(graph.GetType()).WriteObject(stream, graph);
        }

        public T Deserialize<T>(Stream stream) where T : class
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            return GetSerializer(typeof (T)).ReadObject(stream) as T;
        }

        private XmlObjectSerializer GetSerializer(Type type)
        {
            return new DataContractJsonSerializer(type, new DataContractJsonSerializerSettings()
                                                              {
                                                                  UseSimpleDictionaryFormat = true
                                                              });
        }
    }
}
