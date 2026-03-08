namespace MTTR.Imports
{
    public class BaseImport
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

#nullable enable
        public WeaponImport? Weapon { get; set; }
    }
}
