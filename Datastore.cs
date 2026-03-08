using BepInEx;
using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using MTTR.Helpers;
using MTTR.Factories;
using MTTR.Importers;

namespace MTTR
{
    public class Datastore
    {
        public static Datastore Instance { get; private set; }

        public static string BASE_PATH = (new Uri(new Uri(typeof(Plugin).Assembly.Location), relativeUri: ".")).LocalPath;

        public static string DATA_PATH = Utility.CombinePaths(BASE_PATH, MyPluginInfo.PLUGIN_GUID + "_data");
        public static string MODEL_PATH = Utility.CombinePaths(DATA_PATH, "models");
        public static string DEF_PATH = Utility.CombinePaths(DATA_PATH, "defs");

        public Dictionary<string, DataStoreEntry> Store = [];

        public Datastore()
        {
            Instance ??= this;
        }
        public void Init()
        {
            foreach (var item in new string[] { DATA_PATH, MODEL_PATH, DEF_PATH })
            {
                Directory.CreateDirectory(item);
            }

            var defs = Directory.GetFiles(DEF_PATH, "*.json", SearchOption.AllDirectories);
            Tools.WriteLog("Found " + defs.Length + " defs");

            Plugin.Instance.ModelImporter = new ModelImporter();

            var jsonImporter = Plugin.Instance.JsonImporter;
            var modelImporter = Plugin.Instance.ModelImporter;
            var factory = new WeaponFactory();

            foreach (var def in defs)
            {
                var imported = jsonImporter.ProcessJsonFile(def);
                Tools.WriteLog("Imported JSON " + imported.Id + " with type " + imported.Type);
                if (imported.Type == "weapon")
                {
                    try
                    {
                        var generated = factory.Generate(imported);

                        // the lesser evil way to hide it frop the loading sceen camera, can't disabled, can't toggle renderers
                        //generated.transform.position = new Vector3(-1000, 0, 0);
                        modelImporter.ImportModel(imported.Name, Utility.CombinePaths(MODEL_PATH, imported.Weapon.Model), generated.transform);

                        StoreItem(imported.Id, generated, DataType.WEAPON, false);
                    }
                    catch (Exception ex)
                    {
                        Tools.WriteLog("Failed to generate weapon", error: true);
                        Tools.WriteLog(ex, error: true);
                    }
                }
            }
        }

        public void StoreItem(string id, GameObject gameObject, DataType type, bool isVanilla, bool force = false)
        {
            gameObject = gameObject.transform.root.gameObject;

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

            UnityEngine.Object.DontDestroyOnLoad(gameObject);

            Store.Add(id, new DataStoreEntry(gameObject, type, isVanilla));
        }

#nullable enable
        public GameObject? TryGetGameObject(string id)
        {
            if (Store.ContainsKey(id))
            {
                Tools.WriteLog("Cache hit");
                var gameObject = Store[id].GameObject;
                Tools.ToggleObjectRenderers(gameObject, true);

                return gameObject;
            }

            return null;
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
    }
}
