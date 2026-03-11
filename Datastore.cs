using BepInEx;
using MTTR.Helpers;
using MTTR.Importers;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MTTR
{
    public class Datastore
    {
        const ushort WEAPON_ID_OFFSET = 1000;
        const ushort STAGE_WEAPON_ID = 666;

        public static Datastore Instance { get; private set; }

        public static string BASE_PATH = (new Uri(new Uri(typeof(Plugin).Assembly.Location), relativeUri: ".")).LocalPath;

        public static string DATA_PATH_NAME = MyPluginInfo.PLUGIN_GUID + "_data";
        public static string DATA_PATH = Utility.CombinePaths(BASE_PATH, DATA_PATH_NAME);
        public static string MODEL_PATH = Utility.CombinePaths(DATA_PATH, "models");
        public static string DEF_PATH = Utility.CombinePaths(DATA_PATH, "defs");

        public static string STAGE_PATH_NAME = Utility.CombinePaths(DATA_PATH_NAME, "stage");
        public static string STAGE_PATH = Utility.CombinePaths(DATA_PATH, "stage");
        public static string STAGE_JSON_PATH = Utility.CombinePaths(STAGE_PATH, "asset.json");

        public Dictionary<string, DataStoreEntry> Store = [];
        public Dictionary<string, short> WeaponStore = [];

        public Datastore()
        {
            Instance ??= this;
        }
        public void Init()
        {
            foreach (var item in new string[] { DATA_PATH, MODEL_PATH, DEF_PATH, STAGE_PATH })
            {
                Directory.CreateDirectory(item);
            }

            var defs = Directory.GetFiles(DEF_PATH, "*.json", SearchOption.AllDirectories);
            Tools.WriteLog("Found " + defs.Length + " defs");

            Plugin.Instance.ModelImporter = new ModelImporter();

            var jsonImporter = Plugin.Instance.JsonImporter;
            var modelImporter = Plugin.Instance.ModelImporter;

            foreach (var def in defs)
            {
                var imported = jsonImporter.ProcessJsonFile(def);
                Tools.WriteLog("Imported JSON " + imported.Id + " with type " + imported.Type);
                if (imported.Type == "weapon")
                {
                    try
                    {
                        modelImporter.ImportModel(imported, Utility.CombinePaths(MODEL_PATH, imported.Weapon.Model));
                    }
                    catch (Exception ex)
                    {
                        Tools.WriteLog("Failed to generate weapon", error: true);
                        Tools.WriteLog(ex, error: true);
                    }
                }
            }
        }

        public void StoreItem(string id, GameObject gameObject, DataType type, bool isVanilla, bool force = false, bool stage = false)
        {
            if (Store.ContainsKey(id))
            {
                if (force)
                {
                    Store[id] = new DataStoreEntry(gameObject, type, isVanilla);
                    return;
                }

                Tools.WriteLog("Key '" + id + "' already present in store, set force to true if you want to overwrite it", error: true);
                return;
            }


            if (!isVanilla)
            {
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                gameObject = gameObject.transform.root.gameObject;
                gameObject.active = false;
            }

            Store.Add(id, new DataStoreEntry(gameObject, type, isVanilla));
        }

        public void DestroyItem(string id)
        {
            if (!Store.ContainsKey(id))
            {
                Tools.WriteLog("Tried to detroy " + id + " from datastore but isn't present");
                return;
            }

            if (WeaponStore.ContainsKey(id))
            {
                WeaponStore.Remove(id);
            }

            GameObject.Destroy(Store[id].GameObject);
            Store.Remove(id);
        }

#nullable enable
        public GameObject? TryGetGameObject(string id, bool instantiate = true)
        {
            if (Store.ContainsKey(id))
            {
                var gameObject = Store[id].GameObject;

                if (instantiate)
                {
                    gameObject = GameObject.Instantiate(gameObject);
                    gameObject.active = true;
                    Tools.ToggleObjectRenderers(gameObject, true);
                    Tools.TogglePhysic(gameObject, true);
                }
                

                return gameObject;
            }

            return null;
        }

        public short CreateWeaponId(string itemId)
        {
            if (WeaponStore.ContainsKey(itemId))
            {
                Tools.WriteLog("Reusing weapon ID " + WeaponStore[itemId] + " already attributed to " + itemId);

                return WeaponStore[itemId];
            }

            var weaponId = Convert.ToInt16(itemId == "mttr.stage" ? 666 : WEAPON_ID_OFFSET + WeaponStore.Count);
            WeaponStore[itemId] = weaponId;

            return weaponId;
        }

        public StageStatus GetStageStatus()
        {
            if (!File.Exists(STAGE_JSON_PATH))
            {
                return StageStatus.MISSING;
            }

            return Store.ContainsKey("mttr.stage") ? StageStatus.LOADED : StageStatus.UNLOADED;
        }

        public class DataStoreEntry
        {
            public DataType Type;
            public GameObject GameObject;
            public bool IsVanilla;

            public DataStoreEntry(GameObject gameObject, DataType type, bool isVanilla)
            {
                GameObject = gameObject;
                Type = type;
                IsVanilla = isVanilla;
            }
        }

        public enum DataType
        {
            ASSET,
            WEAPON
        }

        public enum StageStatus
        {
            MISSING,
            UNLOADED,
            LOADED,
            DATA_ERROR,
            MODEL_ERROR,
        }
    }
}
