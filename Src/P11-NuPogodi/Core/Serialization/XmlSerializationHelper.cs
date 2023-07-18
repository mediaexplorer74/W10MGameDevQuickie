using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Win8.Core.Serialization
{
    /// <summary>
    /// Simple wrapper for serializing data into/from Json using <see cref="XmlSerializer"/>.
    /// </summary>
    public class XmlSerializationHelper
    {
        /// <summary>
        /// Serialize input object into Xml stream.
        /// </summary>
        public static void Serialize(Stream streamObject, object objForSerialization)
        {
            if (objForSerialization == null || streamObject == null)
                return;

            XmlSerializer serializer = new XmlSerializer(objForSerialization.GetType());
            serializer.Serialize(streamObject, objForSerialization);
        }

        /// <summary>
        /// Deserialize input stream containing Xml data into target object of given type.
        /// </summary>
        public static object Deserialize(Stream streamObject, Type serializedObjectType)
        {
            if (serializedObjectType == null || streamObject == null)
                return null;

            XmlSerializer serializer = new XmlSerializer(serializedObjectType);
            return serializer.Deserialize(streamObject);
        }

        /// <summary>
        /// Serialize input object into Xml string.
        /// </summary>
        public static void SerializeToXml<T>(string file, T movies)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (TextWriter textWriter = new StreamWriter(file))
            {
                serializer.Serialize(textWriter, movies);
                textWriter.Close();
            }
        }

        /// <summary>
        /// Deserialize input string containing Xml data into target object of given type.
        /// </summary>
        public static List<T> DeserializeFromXml<T>(string file)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<T>));
            using (TextReader textReader = new StreamReader(file))
            {
                List<T> data = (List<T>)deserializer.Deserialize(textReader);
                textReader.Close();
                return data;
            }
        }
    }
}