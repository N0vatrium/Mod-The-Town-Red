using AsImpL;
using BepInEx.Unity.IL2CPP.Utils;
using MTTR.Factories;
using MTTR.Helpers;
using MTTR.Imports;
using UnityEngine;
using System.Collections;
using static MTTR.Datastore;
using System;

namespace MTTR.Importers
{
    public class ModelImporter
    {
        public static ModelImporter Instance { get; private set; }

        private GameObject _spawner;
        private ObjectImporter _objectImporter;
        private ImportOptions _options = new();
        private WeaponFactory _weaponFactory = new();

        public ModelImporter()
        {
            Instance ??= this;
            _spawner = new GameObject("Spawner");
            _spawner.AddComponent<BaseMono>();
            GameObject.DontDestroyOnLoad(_spawner);

            _objectImporter = new ObjectImporter(_spawner);
        }

#nullable enable
        public void ImportModel(BaseImport import, string path, Action<GameObject>? callback = null, bool stage = false)
        {
            Transform parent = new GameObject("ImportHolder").transform;

            _objectImporter.ImportingComplete += () =>
            {
                var model = parent.GetChild(0).gameObject;
                parent.DetachChildren();
                GameObject.Destroy(parent.gameObject);

                if (import.Scale != null)
                {
                    model.transform.localScale = (Vector3)import.Scale;
                }

                var generated = AddProperties(import, model);

                Datastore.Instance.StoreItem(import.Id, generated, import.Type == "weapon" ? DataType.WEAPON : DataType.ASSET, false, stage: stage);

                callback?.Invoke(stage ? generated : GameObject.Instantiate(generated));
            };

            _objectImporter.ImportError += (string path) =>
            {
                Tools.WriteLog("Import error for " + path, error: true);
            };

            _objectImporter.ImportModelAsync(import.Name, path, parent, _options);
            MonoBehaviourExtensions.StartCoroutine(self: _spawner.GetComponent<BaseMono>(), CheckStatus());
        }

        public GameObject AddProperties(BaseImport import, GameObject gameObject)
        {
            return _weaponFactory.Generate(import, gameObject);
        }

        IEnumerator CheckStatus()
        {
            while (!_objectImporter.allLoaded)
            {
                yield return new WaitForFixedUpdate();
                _objectImporter.UpdateStatus();
            }
        }

    }
}
