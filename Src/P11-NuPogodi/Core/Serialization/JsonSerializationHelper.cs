using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Win8.Core.Serialization
{
    /// <summary>
    /// Simple wrapper for serializing data into/from Json using <see cref="DataContractJsonSerializer"/>.
    /// </summary>
    public class JsonSerializationHelper
    {
        /// <summary>
        /// Serialize input object into Json stream.
        /// </summary>
        public static void Serialize(Stream streamObject, object objForSerialization)
        {
            if (objForSerialization == null || streamObject == null)
                return;

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(objForSerialization.GetType());
            serializer.WriteObject(streamObject, objForSerialization);
        }

        /// <summary>
        /// Deserialize input stream containing Json data into target object of given type.
        /// </summary>
        public static object Deserialize(Stream streamObject, Type serializedObjectType)
        {
            if (serializedObjectType == null || streamObject == null)
                return null;

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(serializedObjectType);
            return serializer.ReadObject(streamObject);
        }
    }
}
