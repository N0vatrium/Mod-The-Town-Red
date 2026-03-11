using MTTR.Converters;
using MTTR.Converters.Newtonsoft.Json.UnityConverters.Mathematics;
using MTTR.Helpers;
using MTTR.Imports;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace MTTR.Importers
{
    public class JsonImporter
    {
        public static JsonImporter Instance { get; private set; }

        public static List<JsonConverter> EnabledConverters = [new Vector3Converter(), new QuaternionConverter()];
        public JsonImporter()
        {
            Instance ??= this;
        }

#nullable enable
        public BaseImport? ProcessJson(string jsonString)
        {
            var settings = new JsonSerializerSettings
            {
                Converters = EnabledConverters,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
            };

            return JsonConvert.DeserializeObject<BaseImport>(jsonString, settings);
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
