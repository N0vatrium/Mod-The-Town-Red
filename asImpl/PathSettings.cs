using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace AsImpL
{
    public enum RootPathEnum
    {
        Url,
        DataPath,
        DataPathParent,
        PersistentDataPath,
        StreamingAssetsPath,
        CurrentPath
    }

    public class PathSettings : MonoBehaviour
    {
        public RootPathEnum editorRootPath = RootPathEnum.Url;

        public RootPathEnum defaultRootPath = RootPathEnum.Url;

        public RootPathEnum mobileRootPath = RootPathEnum.Url;


        public string RootPath
        {
            get
            {
#if UNITY_EDITOR
                switch (editorRootPath)
#elif UNITY_STANDALONE
                switch (defaultRootPath)
#else
                switch (mobileRootPath)
#endif
                {
                    case RootPathEnum.DataPath:
                        return Application.dataPath + "/";
                    case RootPathEnum.DataPathParent:
                        return Application.dataPath + "/../";
                    case RootPathEnum.PersistentDataPath:
                        return Application.persistentDataPath + "/";
                    case RootPathEnum.StreamingAssetsPath:
                        return Application.streamingAssetsPath + "/";
                }
                return "";
            }
        }


        public static PathSettings FindPathComponent(GameObject obj)
        {
            PathSettings pathSettings = obj.GetComponent<PathSettings>();
            if (pathSettings == null)
            {
                pathSettings = FindObjectOfType<PathSettings>();
            }
            if (pathSettings == null)
            {
                pathSettings = obj.AddComponent<PathSettings>();
            }
            return pathSettings;
        }


        public string FullPath(string path)
        {
            string fullPath = path;
            if(!Path.IsPathRooted(path))
            {
                fullPath = RootPath + path;
            }
            return fullPath;
        }
    
    }
}
