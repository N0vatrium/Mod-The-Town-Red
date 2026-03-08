using UnityEngine;

namespace AsImpL
{
    [System.Serializable]
    /// <summary>
    /// Options to define how the model will be loaded and imported.
    /// </summary>
    public class ImportOptions
    {
        public bool zUp = true;

        public bool litDiffuse = false;

        public bool convertToDoubleSided = false;

        public float modelScaling = 1f;

        public bool reuseLoaded = false;

        public bool inheritLayer = false;

        public bool buildColliders = false;

        public bool colliderConvex = false;

        public bool colliderTrigger = false;

        public bool colliderInflate = false;

        public float colliderSkinWidth = 0.01f;

        public bool use32bitIndices = true;

        public bool hideWhileLoading = false;

        public bool splitByMaterial = false;

        public Vector3 localPosition = Vector3.zero;

        public Vector3 localEulerAngles = Vector3.zero;

        public Vector3 localScale = Vector3.one;
    }
}
