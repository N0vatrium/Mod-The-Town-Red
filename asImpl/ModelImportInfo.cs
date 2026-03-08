using UnityEngine;

namespace AsImpL
{
    /// <summary>
    /// Model import settings, used for batch importing.
    /// </summary>
    [System.Serializable]
    public class ModelImportInfo
    {
        public string name;

        public string path;

        public bool skip = false;

        public ImportOptions loaderOptions;

        // Default constructor needed by XmlSerializer
        public ModelImportInfo()
        {
        }
    }
}
