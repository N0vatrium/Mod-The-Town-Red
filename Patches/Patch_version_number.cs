using HarmonyLib;
using MTTR.Helpers;
using TMPro;
using UnityEngine;

namespace MTTR.Patches
{
    [HarmonyPatch(typeof(FinalMenu), "Update")]
    internal static class Patch_version_number
    {
        public static bool Loaded = false;
        static void Postfix(FinalMenu __instance)
        {
            if (Loaded || !Timing.IsTick())
            {
                return;
            }

            var canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
               // Abort("Could not find main canvas on menu");
                return;
            }

            var reference = canvas.transform.Find("TopRight");
            if (reference == null)
            {
                Abort("Could not find reference on menu");
                return;
            }

            var referenceChild = reference.transform.Find("DiscordBG");
            if (referenceChild == null)
            {
                Abort("Could not find reference child on menu");
                return;
            }

            var topLeftCorner = Tools.FindOrCreate(canvas, "TopLeft");

            if (topLeftCorner.GetComponent<CanvasRenderer>() == null)
            {
                topLeftCorner.AddComponent<CanvasRenderer>();
            }

            var topLeftCornerRect = topLeftCorner.GetComponent<RectTransform>() ?? topLeftCorner.AddComponent<RectTransform>();

            topLeftCornerRect.anchorMin = new Vector2(0, 1);
            topLeftCornerRect.anchorMax = new Vector2(0, 1);
            topLeftCornerRect.pivot = new Vector2(0, 1);
            topLeftCornerRect.anchoredPosition = Vector2.zero;

            var versionNumber = Tools.FindOrCreate(topLeftCorner, "VersionNumber");

            var versionTextComp = versionNumber.GetComponent<TextMeshProUGUI>() ?? versionNumber.AddComponent<TextMeshProUGUI>();
            versionTextComp.text = "MTT<color=\"red\">R<color=\"white\"> version " + MyPluginInfo.PLUGIN_VERSION;
            versionTextComp.alignment = TextAlignmentOptions.TopLeft;
            versionTextComp.fontSize = 26f;

            var textRect = versionTextComp.rectTransform;

            textRect.anchorMin = new Vector2(0, 1);
            textRect.anchorMax = new Vector2(0, 1);
            textRect.pivot = new Vector2(0, 1);
            textRect.anchoredPosition = new Vector2(10, -10);
            textRect.sizeDelta = new Vector2(300, 50);

            topLeftCornerRect.localScale = Vector3.one;
            textRect.localScale = Vector3.one;

            Loaded = true;
        }

        private static void Abort(string message)
        {
            Tools.WriteLog(message, error: true);
            Loaded = true;
        }
    }


    [HarmonyPatch(typeof(FinalMenu), "Start")]
    internal static class Patch_version_number_clear
    {
        static void Postfix()
        {
            Patch_version_number.Loaded = false;
        }
    }
}
