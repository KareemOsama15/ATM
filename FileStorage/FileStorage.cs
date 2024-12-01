using System.Text.Json;
using ATM.Utils;

namespace ATM.FileStorage
{
    public static class FileStorageService
    {
        public static string ATMFilePath = "./FileStorage/ATM.json";

        public static void SaveToFile<T>(string filePath, List<T> data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
            };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json);

        }

        public static List<T> LoadFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath)) return new List<T>();

            string json = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
            };

            return JsonSerializer.Deserialize<List<T>>(json, options) ?? new List<T>();
        }
    }
}
