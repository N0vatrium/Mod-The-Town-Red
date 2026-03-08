using HarmonyLib;
using MTTR.Helpers;
using UnityEngine;
using UnityEngine.Events;
using static FinalMenuStandardButton;

#nullable enable
namespace MTTR.Patches
{
    [HarmonyPatch(typeof(FinalMenu), "Update")]
    internal static class Patch_mods_button
    {
        public static bool Loaded = false;

        private static FinalMenuTransition? PanelTransition;
        private static FinalMenuTransition? PanelMain;

        static void Postfix(FinalMenu __instance)
        {
            if (Loaded || !Timing.IsTick())
            {
                return;
            }

            var buttons = GameObject.Find("PanelMainButtonsLeft");
            if (buttons == null)
            {
                //Abort("Could not find buttons on menu");
                return;
            }

            var reference = buttons.transform.Find("ButtonMain-Multiplayer");
            if (reference == null)
            {
                return;
            }

            var modButton = Tools.CreateButton("Mods");
            modButton.transform.SetParent(buttons.transform);

            var rectT = modButton.GetComponent<RectTransform>();
            rectT.anchoredPosition = new Vector2(380, 0);
            rectT.localScale = Vector3.one;



            var seg = modButton.GetComponent<SEGButton>();

            void ModsPanelToggle()
            {
                if (PanelMain == null)
                {
                    PanelMain = GameObject.Find("Panel-MainMenu").GetComponent<FinalMenuTransition>();
                }

                if (PanelTransition == null)
                {
                    var panel = CreatePanel();
                    if (panel == null)
                    {
                        Abort("Failed to create mods panel");
                        return;
                    }

                    PanelTransition = panel.GetComponent<FinalMenuTransition>();
                   /* PanelTransition.show = false;
                    PanelTransition.overallT = 0f;*/


                    /*var panelBackButton = panel.GetComponentInChildren<FinalMenuStandardButton>();
                    panelBackButton.clickCallback = (ButtonClickedCallback)ModsPanelToggle;*/
                }

                if (PanelTransition.show)
                {
                    PanelTransition.TransitionOff();
                    PanelMain.TransitionOn();
                    return;
                }

                PanelMain.TransitionOff();
                PanelTransition.TransitionOn();
            }

            seg.onClick.AddListener((UnityAction)ModsPanelToggle);

            Loaded = true;
        }

        static GameObject? CreatePanel()
        {
            var ratioHelper = GameObject.Find("AspectRatioHelper");
            if (ratioHelper == null)
            {
                return null;
            }

            /*var optionsPanel = Tools.LoadAsset("aaa839b7b5e2f0a4081a98c127dcfe72");
            if (optionsPanel == null)
            {
                return null;
            }*/
            var optionsPanel = ratioHelper.transform.Find("Panel-CoopMenu").gameObject;
            Tools.WriteLog("count " + optionsPanel.transform.childCount);


            var instance = GameObject.Instantiate(optionsPanel.gameObject);
            var mpComp = instance.GetComponent<FinalMenuMultiplayer>();
            GameObject.Destroy(mpComp);

            instance.name = "Panel-Mods";

            instance.transform.SetParent(ratioHelper.transform, false);

            Tools.WriteLog("instance is " + instance.name);
            return instance;
        }


        private static void Abort(string message)
        {
            Tools.WriteLog(message, error: true);
            Loaded = true;
        }
    }

    [HarmonyPatch(typeof(FinalMenu), "Start")]
    internal static class Patch_mods_button_clear
    {
        static void Postfix()
        {
            Patch_mods_button.Loaded = false;
        }
    }
}
