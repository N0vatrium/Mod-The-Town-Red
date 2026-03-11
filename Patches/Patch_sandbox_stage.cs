using HarmonyLib;
using MTTR.Extensions;
using MTTR.Helpers;
using MTTR.Monos;
using UnityEngine;

namespace MTTR.Patches
{
    [HarmonyPatch(typeof(PTTRPlayer), "Start")]
    internal static class Patch_sandbox_stage
    {
        internal static StageStatusDisplay Display;
        internal static SpawnArea SpawnArea;

        static void Postfix(PTTRPlayer __instance)
        {
            if (GameManager.CurrentLevel != "Sandbox")
            {
                return;
            }

            var buttonPos = new Vector3(11.34f, 0, 8.04f);
            var button = Tools.SpawnItemAt("481e0c9bd37f9c74c8581292e35eada0", buttonPos);
            button.name = "MTTR stage reset";

            var action = button.AddComponent<WorldButtonStage>();

            var buttonComp = button.GetComponent<WorldButton>();
            buttonComp.buttonAction = action;
            buttonComp.stayPressed = false;

            buttonComp.SetText("Reload stage");

            var statusText = new GameObject("MTTR stage status");
            statusText.transform.position = new Vector3(16f, .01f, buttonPos.z);
            statusText.transform.eulerAngles = new Vector3(90f, 0, 0);

            Display = statusText.AddComponent<StageStatusDisplay>();
            Display.UpdateStatus();

            var refArea = GameObject.Find("BoxerSpawn");
            if (refArea == null)
            {
                Tools.WriteLog("Could not find BoxerSpawn in sandbox", error: true);
                return;
            }

            var stageArea = GameObject.Instantiate(refArea);
            stageArea.name = "StageSpawn";
            var oldPos = stageArea.transform.position;
            stageArea.transform.position = new Vector3(10, 0.05f, 11.71f);

            SpawnArea = stageArea.GetComponent<SpawnArea>();
            
            var areaRenderer = stageArea.GetComponent<Renderer>();
            SpawnArea.bounds = areaRenderer.bounds;

            action.Init(buttonComp);
        }
    }
}
