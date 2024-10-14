using System;
using System.IO;
using Newtonsoft.Json;

namespace backend.Services
{
    public class JsonCRUD
    {
        private readonly string _filePath;

        public JsonCRUD(string filePath)
        {
            _filePath = filePath;
        }

        public void WriteJsonObject<T>(T obj)
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public T ReadJsonObject<T>()
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("The specified file does not exist.");

            string json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }
        public void DeleteWrittenObject<T>()
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("The specified file does not exist.");

            var obj = ReadJsonObject<T>();
            if (obj != null)
            {
                File.Delete(_filePath);
            }
            else
            {
                throw new InvalidOperationException("The specified object does not exist in the file.");
            }
        }
    }
}