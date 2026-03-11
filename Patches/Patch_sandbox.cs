using AsImpL;
using HarmonyLib;
using MTTR.Helpers;
using System.Collections;
using UnityEngine;

namespace MTTR.Patches
{
    [HarmonyPatch(typeof(PTTRPlayer), "Start")]
    internal static class Patch_sandbox
    {
        public static bool Loaded = false;
        private static ObjectImporter importer;
        static void Postfix(PTTRPlayer __instance)
        {
            if (GameManager.CurrentLevel != "Sandbox")
            {
                return;
            }
            var spawnPoint = new Vector3(__instance.gameObject.transform.position.x, __instance.gameObject.transform.position.y, __instance.gameObject.transform.position.z - 2);
            //var katana = Tools.SpawnItemAt("9766a4851c58f484d97fcacf8df71c46", spawnPoint);
            Tools.SpawnItemAt("novatrium.diamond_sword", spawnPoint);

            /*var import = Plugin.Instance.Importer.ProcessJsonFile(Utility.CombinePaths(Datastore.MODEL_PATH, "sword.json"));

            Tools.WriteLog("imported " + import.Id + " with model " + import.Weapon?.Model + " damage is " + import.Weapon.HitForce);
            var factory = new WeaponFactory();
            var generated = factory.Generate(import);
            generated.transform.position = spawnPoint;*/

            /* var hilt = katana.transform.Find("Katana_Hilt");
             Tools.WriteLog("hilt is " + hilt.name);

             var weaponChild = hilt.GetComponent<WeaponChild>();

             var filePath = Utility.CombinePaths(Datastore.MODEL_PATH, "espada_hacha_minecraftSebas.obj");
             Tools.WriteLog("obj path is " + filePath);


             var options = new ImportOptions();
             var spawner = new GameObject("spawner");
             spawner.AddComponent<BaseMono>();

             var spawned = new GameObject("spawned");
             spawned.transform.position = spawnPoint;
             
             Tools.WriteLog("comps ready");
             importer = new ObjectImporter(spawner);

             importer.ImportModelAsync("sphere", filePath, spawned.transform, options);*/

            /*var body = spawned.transform.Find("espada_hacha_minecraftSebas");
            Tools.WriteLog("sword body is " + body.name);

            var newSword = GameObject.Instantiate(body);
            newSword.transform.SetParent(hilt.parent);*/
        }

        static IEnumerator CheckStatus()
        {
            yield return new WaitForSeconds(1);
            importer.UpdateStatus();
        }
    }
}
