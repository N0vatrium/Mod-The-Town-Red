using AsImpL;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using MTTR.Helpers;
using MTTR.Importers;
using MTTR.Monos;
using System.Reflection;

namespace MTTR;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    public static Plugin Instance { get; private set; }
    public static Datastore DataStore = new Datastore();
    internal static new ManualLogSource Log;
    public JsonImporter JsonImporter { get; private set; }
    public ModelImporter ModelImporter { get; set; }

    public Plugin()
    {
        Instance ??= this;
    }

    public override void Load()
    {
        // Plugin startup logic
        Log = base.Log;
        PluginConfig.Init(Config);

        // register il2cpp comps
        AddComponent<BaseMono>();
        AddComponent<ModelReferences>();
        AddComponent<PathSettings>();

        AddComponent<StageEntity>();
        AddComponent<WorldButtonStage>();
        AddComponent<StageStatusDisplay>();

        AddComponent<MTTRPlayer>();

        JsonImporter = new JsonImporter();

        var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

        harmony.PatchAll(Assembly.GetExecutingAssembly());


        Tools.WriteLog($"Plugin loaded, asset store ready");
    }

}
