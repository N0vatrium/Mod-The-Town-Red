using HarmonyLib;
using MTTR.Helpers;
using UnityEngine;

namespace MTTR.Patches
{
    [HarmonyPatch(typeof(LevelEditor), "SaveUGCLevelToFile")]
    internal static class Patch_editor_save
    {
        static bool Prefix(UGCLevel ugcLevel, string file, LevelEditor __instance)
        {
            Tools.WriteLog("Saving level " + ugcLevel.levelName);

            var proxies = GameObject.FindObjectsOfType<LevelEditorProxy>();
            GameObject target = null;
            foreach ( var proxy in proxies )
            {
                if (proxy.name.Contains("Character"))
                {
                    target = proxy.gameObject;
                }
            }

            Tools.WriteLog("Using proxy " + target.name);

            for ( var i = 0; i<300; i++)
            {
                var child = GameObject.Instantiate(target);
                child.transform.position = Vector3.zero;
            }

            return true;
        }
    }
}
