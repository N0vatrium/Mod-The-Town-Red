using System.Reflection.Metadata;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable enable
namespace MTTR.Helpers
{
    static class Tools
    {
        public static string GUID_BIG_KATANA = "9766a4851c58f484d97fcacf8df71c46";

        public static void WriteLog(object log, bool warning = false, bool error = false)
        {
            var level = BepInEx.Logging.LogLevel.Message;
            if (warning)
            {
                level = BepInEx.Logging.LogLevel.Warning;
            }

            if (error)
            {
                level = BepInEx.Logging.LogLevel.Error;
            }

            Plugin.Log.Log(level, log);
        }

        public static GameObject FindOrCreate(GameObject parent, string childName)
        {
            var childTransform = parent.transform.Find(childName);

            if (childTransform == null)
            {
                var child = new GameObject(childName);
                child.transform.parent = parent.transform;

                childTransform = child.transform;
            }

            return childTransform.gameObject;
        }

        public static GameObject? SpawnItemAt(string guid, Vector3 position)
        {
            var asset = LoadAsset(guid);

            if (asset == null)
            {
                return null;
            }

            var child = GameObject.Instantiate(asset);
            child.transform.position = position;

            return child;
        }

        public static GameObject? LoadAsset(string id)
        {
            var cached = Datastore.Instance.TryGetGameObject(id);
            if (cached != null)
            {
                return cached;
            }

            var handle = Addressables.LoadAssetAsync<GameObject>(id);
            handle.WaitForCompletion();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                CacheGameObject(id, handle.Result, true);
                return handle.Result;
            }
            else
            {
                WriteLog("LOADED failed GUID " + id, error: true);

                return null;
            }
        }

        public static void CacheGameObject(string id, GameObject gameObject, bool isVanilla)
        {
            var isWeapon = gameObject.GetComponent<Weapon>() != null;

            Datastore.Instance.StoreItem(id, gameObject, isWeapon ? Datastore.DataType.WEAPON : Datastore.DataType.ASSET, isVanilla);
        }

        public static GameObject CreateButton(string text)
        {
            var loaded = LoadAsset("5f23ed8e39ba41b4589ed5dff02c6eec");
            if (loaded != null)
            {
                loaded = GameObject.Instantiate(loaded);
                loaded.name = "MTTR-button";

                var textComp = loaded.GetComponent<FinalMenuStandardButton>();
                textComp.SetLabel(text);
                textComp.text1TMP.alignment = TextAlignmentOptions.Left;
                textComp.text2TMP.alignment = TextAlignmentOptions.Left;

                return loaded;
            }

            return new GameObject("MTTR-button");
        }

        public static void ToggleObjectRenderers(GameObject gameObject, bool enabled)
        {
            var renderers = gameObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = enabled;
            }
        }
    }
}
