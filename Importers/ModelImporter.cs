using AsImpL;
using MTTR.Helpers;
using UnityEngine;

namespace MTTR.Importers
{
    public class ModelImporter
    {
        public static ModelImporter Instance { get; private set; }

        private GameObject _spawner;
        private ImportOptions _options = new();
        private ObjectImporter _objectImporter;

        public ModelImporter()
        {
            Instance ??= this;
            _spawner = new GameObject("Spawner");
            _spawner.AddComponent<BaseMono>();
            GameObject.DontDestroyOnLoad(_spawner);

            _objectImporter = new ObjectImporter(_spawner);
            //_options.hideWhileLoading = true;
        }

        public void ImportModel(string name, string path, Transform parent)
        {
            _objectImporter.ImportModelAsync(name, path, parent, _options);
        }

    }
}
