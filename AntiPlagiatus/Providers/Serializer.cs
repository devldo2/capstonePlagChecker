using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace AntiPlagiatus.Providers
{
    public static class DataContractSerializer
    {
        private static Stream GetStream(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value));
        }
        private static string GetString(MemoryStream value)
        {
            var buffer = value.ToArray();
            return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }
        public static T DeserializeObject<T>(string content)
        {
            var result = default(T);

            using (var stream = GetStream(content))
            {
                try
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(T));
                        result = (T)serializer.ReadObject(stream);
                    }
                }

                catch (Exception e)
                {
                }
            }

            return result;
        }
        public static string SerializeObject<T>(T value)
        {
            string result = null;
            using (var stream = new MemoryStream())
            {
                try
                {
                    using (var sr = new StreamWriter(stream))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(T));
                        serializer.WriteObject(stream, value);
                    }

                    result = GetString(stream);
                }
                catch (Exception e)
                {
                    // Data in file can be broken.
                }
            }
            return result;
        }
    }
}
