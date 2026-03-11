using HarmonyLib;
using MTTR.Monos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTTR.Patches
{
    [HarmonyPatch(typeof(PTTRPlayer), "Start")]
    internal static class Patch_mttr_player
    {
        static void Postfix(PTTRPlayer __instance)
        {
            var mttr = __instance.gameObject.GetComponent<MTTRPlayer>();
            if(mttr == null)
            {
                __instance.gameObject.AddComponent<MTTRPlayer>();
            }
        }
    }
}
