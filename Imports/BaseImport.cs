using UnityEngine;

namespace MTTR.Imports
{
    public class BaseImport
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Vector3? Scale { get; set; }

#nullable enable
        public WeaponImport? Weapon { get; set; }
    }
}
