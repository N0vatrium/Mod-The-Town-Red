using HarmonyLib;

namespace MTTR.Patches
{
    [HarmonyPatch(typeof(SplashLoad), "Start")]
    internal static class Patch_splash_screen
    {
        static void Postfix(SplashLoad __instance)
        {
            
            //TODO increase loading time based on datastore

            Plugin.DataStore.Init();
        }
    }
}
