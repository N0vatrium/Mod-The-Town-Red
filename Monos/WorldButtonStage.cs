using BepInEx;
using MTTR.Extensions;
using MTTR.Helpers;
using MTTR.Imports;
using MTTR.Patches;
using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MTTR.Monos
{
    public class WorldButtonStage : WorldButtonAction
    {
        private WorldButton _button;
        private string _text;

        public override void Init(WorldButton worldButton)
        {
            _button = worldButton;
        }
        public override bool DoPressAction()
        {
            _button.tempCantUse = true;
            _text = _button.label;

            var player = GameObject.Find("PTTRPlayer");
            var pttrPlayer = player.GetComponent<PTTRPlayer>();
            var playerFaction = pttrPlayer.GetFaction();

            if (pttrPlayer.HasWeapon())
            {
                // is the player is hold a stage weapon
                var heldWeapons = pttrPlayer.GetComponentsInChildren<Weapon>();
                foreach (var heldWeapon in heldWeapons)
                {
                    if (heldWeapon.weaponTypeID == 666)
                    {
                        pttrPlayer.DropAllWeaponsAndShields();
                    }
                }
            }


            var entities = GameObject.FindObjectsOfType<StageEntity>();
            foreach (var entity in entities)
            {
                GameObject.Destroy(entity.gameObject);
            }


            var enemies = GameObject.FindObjectsOfType<Enemy>();
            foreach (var enemy in enemies)
            {
                if (enemy.faction == playerFaction)
                {
                    GameObject.Destroy(enemy.gameObject);
                }
            }

            var bodyParts = GameObject.FindObjectsOfType<Bodypart>();
            foreach (var part in bodyParts)
            {
                GameObject.Destroy(part.gameObject);
            }

            var seekers = GameObject.FindObjectsOfType<Seeker>();
            foreach (var seeker in seekers)
            {
                seeker.CancelCurrentPathRequest();
                seeker.ReleaseClaimedPath();
            }

            _button.SetText("Reloading...");
            Patch_sandbox_stage.Display.SetStatus(Datastore.StageStatus.UNLOADED);
            var datastore = Datastore.Instance;

            if (datastore.Store.ContainsKey("mttr.stage"))
            {
                datastore.DestroyItem("mttr.stage");
            }

            var jsonImporter = Plugin.Instance.JsonImporter;
            var modelImporter = Plugin.Instance.ModelImporter;

            BaseImport imported = null;

            try
            {
                imported = jsonImporter.ProcessJsonFile(Datastore.STAGE_JSON_PATH);
            }
            catch (JsonReaderException ex)
            {
                Patch_sandbox_stage.Display.SetStatus(Datastore.StageStatus.DATA_ERROR);
                ResetButton();
            }
            catch (Exception ex)
            {
                Patch_sandbox_stage.Display.SetStatus(Datastore.StageStatus.MODEL_ERROR);
                ResetButton();
            }

            imported.Id = "mttr.stage";

            modelImporter.ImportModel(imported, Utility.CombinePaths(Datastore.STAGE_PATH, imported.Weapon.Model), stage: true, callback: (GameObject generated) =>
            {
                if (datastore.Store.ContainsKey("mttr.stage"))
                {
                    generated = Tools.SpawnItemAt("mttr.stage", new Vector3(10f, 1, 8.04f));
                    generated.AddComponent<StageEntity>();

                    /*var biker = Tools.SpawnItemAt("75ddc4b88dfea034285d8901a9e0c91b", new Vector3(10, 1, 10));

                    var bikerEnemy = biker.GetComponent<Enemy>();
                    bikerEnemy.canTarget = false;
                    bikerEnemy.SetRotation(Quaternion.Euler(0, 180, 0));
                    bikerEnemy.combatSpeedMod = 0;

                    biker.AddComponent<StageEntity>();*/

                    //Assets/_PaintTheTownRed/Content/EnemySettings/SettingsStandard.asset
                    var handle = Addressables.LoadAssetAsync<EnemySettings>("3aa02dcec2fbfe0499e5a3f115a00c34");
                    handle.WaitForCompletion();

                    Patch_sandbox_stage.SpawnArea.SpawnEnemies(handle.Result, 1, new Enemy.AdditionalSpawnInfo(faction: (short)playerFaction));

                    var enemies = GameObject.FindObjectsOfType<Enemy>();
                    foreach (var enemy in enemies)
                    {
                        if (enemy.faction == playerFaction)
                        {
                            enemy.canMove = false;
                            enemy.SetRotation(Quaternion.Euler(0, 180, 0));

                            var enemyWeapon = Tools.SpawnItemAt("mttr.stage", enemy.gameObject.transform.position);
                            enemyWeapon.AddComponent<StageEntity>();
                            enemy.PickUpWeapon(enemyWeapon.GetComponent<Weapon>());
                        }
                    }
                }

                ResetButton();

                Patch_sandbox_stage.Display.LoadedName = imported.Name;
                Patch_sandbox_stage.Display.UpdateStatus();
            }, errorCallback: (bool isJson) =>
            {
                Tools.WriteLog("??2");
                Patch_sandbox_stage.Display.SetStatus(Datastore.StageStatus.DATA_ERROR);
                ResetButton();
            });

            return true;
        }

        public void ResetButton()
        {
            _button.tempCantUse = false;
            _button.PressButton();
            _button.SetText(_text);
        }
    }
}
