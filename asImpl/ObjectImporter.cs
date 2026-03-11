using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BepInEx.Unity.IL2CPP.Utils;
using MTTR.Helpers;

namespace AsImpL
{
    /// <summary>
    /// Component that imports objects from a model, both at run-rime and as assets in Editor.
    /// </summary>
    /// <remarks></remarks>
    public class ObjectImporter
    {
        /// <summary>
        /// Import the model as a set of assets. This should be set only by the Editor window.
        /// </summary>
        public bool importAssets = false;

        /// <summary>
        /// Set the import path for assets. This should be set only by the Editor window.
        /// </summary>
        public string importAssetPath = "_ImportedOBJ";
        protected int numTotalImports = 0;
        public bool allLoaded = false;
        protected ImportOptions buildOptions;
        protected List<Loader> loaderList;

        // raw subdivision in percentages of the import phases (empirically computed importing a large sample OBJ file)
        // TODO: refine or change this method
        private static float TEX_PHASE_PERC = 13f;
        private static float OBJ_PHASE_PERC = 76f;
        //private static float ASSET_PHASE_PERC = 11f;

        private string importMessage = string.Empty;
        private ImportPhase importPhase = ImportPhase.Idle;

        /// <summary>
        /// Event triggered when starting to import.
        /// </summary>
        public event Action ImportingStart;

        /// <summary>
        /// Event triggered when finished importing.
        /// </summary>
        public event Action ImportingComplete;

        /// <summary>
        /// Event triggered when a single model has been created and before it is imported.
        /// </summary>
        public event Action<GameObject, string> CreatedModel;

        /// <summary>
        /// Event triggered when a single model has been successfully imported.
        /// </summary>
        public event Action<GameObject, string> ImportedModel;

        /// <summary>
        /// Event triggered when an error occurred importing a model.
        /// </summary>
        public event Action<string> ImportError;

        private enum ImportPhase { Idle, TextureImport, ObjLoad, AssetBuild, Done }

        private GameObject targetGameObject;

        public ObjectImporter(GameObject targetGameObject)
        {
            this.targetGameObject = targetGameObject;
        }

        /// <summary>
        /// Number of pending import activities.
        /// </summary>
        public int NumImportRequests
        {
            get { return numTotalImports; }
        }

        /// <summary>
        /// Create the proper loader component according to the file extension.
        /// </summary>
        /// <param name="absolutePath">path of the model to be imported</param>
        /// <returns>A proper loader or null if not available.</returns>
        private Loader CreateLoader(string absolutePath)
        {
            string ext = Path.GetExtension(absolutePath);
            if (string.IsNullOrEmpty(ext))
            {
                Debug.LogError("No extension defined, unable to detect file format");
                return null;
            }
            Loader loader = null;
            ext = ext.ToLower();

            if (ext.StartsWith(".php"))
            {
                if (!ext.EndsWith(".obj"))
                {
                    // TODO: other formats supported? Remark: often there are zip and rar archives without extension.
                    Debug.LogError("Unable to detect file format in " + ext);
                    return null;
                }
                loader = new LoaderObj();
            }
            else
            {
                switch (ext)
                {
                    case ".obj":
                        loader = new LoaderObj();
                        break;
                    // TODO: add mode formats here...
                    default:
                        Debug.LogErrorFormat("File format not supported ({0})", ext);
                        return null;
                }
            }
            loader.ModelCreated += OnModelCreated;
            loader.ModelLoaded += OnImported;
            loader.ModelError += OnImportError;

            return loader;
        }


        /// <summary>
        /// Request to load a file asynchronously.
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="filePath"></param>
        /// <param name="parentObj"></param>
        /// <param name="options"></param>
        public void ImportModelAsync(string objName, string filePath, Transform parentObj, ImportOptions options)
        {
            if (loaderList == null)
            {
                loaderList = new List<Loader>();
            }

            if (loaderList.Count == 0)
            {
                numTotalImports = 0;// files.Length;
                if (ImportingStart != null)
                {
                    ImportingStart();
                }
            }
            // If the given path is a URI leave it as it is, else get its absolute path
            string absolutePath = filePath.Contains("//") ? filePath : Path.GetFullPath(filePath);

            Loader loader = CreateLoader(absolutePath);
            if (loader == null)
            {
                return;
            }
            numTotalImports++;
            loaderList.Add(loader);
            loader.buildOptions = options;
            if (string.IsNullOrEmpty(objName))
            {
                objName = Path.GetFileNameWithoutExtension(absolutePath);
            }
            allLoaded = false;
            MonoBehaviourExtensions.StartCoroutine(self: targetGameObject.GetComponent<BaseMono>(), loader.Load(objName, absolutePath, parentObj));
        }


        /// <summary>
        /// Update the loading/importing status
        /// </summary>
        public virtual void UpdateStatus()
        {
            if (allLoaded) return;
            int num_loaded_files = numTotalImports - Loader.totalProgress.singleProgress.Count;

            bool loading = num_loaded_files < numTotalImports;
            if (!loading)
            {
                allLoaded = true;
                if (loaderList != null)
                {
                    for (int i = 0; i < loaderList.Count; i++)
                    {
                        loaderList[i] = null;
                    }
                    loaderList.Clear();
                }
                OnImportingComplete();
            }
        }


        protected virtual void Update()
        {
            UpdateStatus();
        }


        /// <summary>
        /// Called when finished importing. It triggers ImportingComplete event, if it was set.
        /// </summary>
        protected virtual void OnImportingComplete()
        {
            if (ImportingComplete != null)
            {
                ImportingComplete();
                ImportingComplete = null;
            }
        }


        /// <summary>
        /// Called when each model has been created and before it is imported. It triggers CreatedModel event, if it was set.
        /// </summary>
        protected virtual void OnModelCreated(GameObject obj, string absolutePath)
        {
            if (CreatedModel != null)
            {
                CreatedModel(obj, absolutePath);
            }
        }


        /// <summary>
        /// Called when each model has been imported. It triggers ImportedModel event, if it was set.
        /// </summary>
        protected virtual void OnImported(GameObject obj, string absolutePath)
        {
            if (ImportedModel != null)
            {
                ImportedModel(obj, absolutePath);
            }
        }


        /// <summary>
        /// Called when a model import fails. It triggers ImportError event, if it was set.
        /// </summary>
        protected virtual void OnImportError(string absolutePath)
        {
            if (ImportError != null)
            {
                ImportError(absolutePath);
            }
        }

    }
}
