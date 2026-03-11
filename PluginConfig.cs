using BepInEx.Configuration;

namespace MTTR;
public class PluginConfig
{
    public static ConfigEntry<bool> DebugObjImporter;

    public static void Init(ConfigFile configFile)
    {
        DebugObjImporter = configFile.Bind("General", "DebugObjImporter", false, "Enable obj importer logging, true/false");
    }
}
