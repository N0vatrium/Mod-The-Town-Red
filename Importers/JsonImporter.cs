using MTTR.Helpers;
using MTTR.Imports;
using Newtonsoft.Json;
using System.IO;

namespace MTTR.Importers
{
    public class JsonImporter
    {
        public static JsonImporter Instance { get; private set; }
        public JsonImporter()
        {
            Instance ??= this;
        }

#nullable enable
        public BaseImport? ProcessJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<BaseImport>(jsonString, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
        }

        public BaseImport? ProcessJsonFile(string path)
        {
            if (!File.Exists(path))
            {
                Tools.WriteLog("Trying to import but the JSON file is missing", error: true);
                return null;
            }

            var jsonString = File.ReadAllText(path);

            return ProcessJson(jsonString);
        }
    }
}
