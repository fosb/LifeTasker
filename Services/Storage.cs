using System.Xml;
using LifeTasker.Models;
using Newtonsoft.Json;

namespace LifeTasker.Services
{
    public static class Storage
    {
        private static readonly string _filePath = "tasks.json";

        public static void SaveData(List<LifeTask> tasks)
        {
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(tasks, Newtonsoft.Json.Formatting.Indented));
        }

        public static List<LifeTask> LoadData()
        {
            if (!File.Exists(_filePath)) return new List<LifeTask>();
            return JsonConvert.DeserializeObject<List<LifeTask>>(File.ReadAllText(_filePath));
        }
    }
}