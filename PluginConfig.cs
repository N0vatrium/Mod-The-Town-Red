using BepInEx.Configuration;

namespace MTTR;
public class PluginConfig
{
    public static ConfigEntry<bool> ConfigGunEnabled;
    public static ConfigEntry<bool> ConfigMeleeEnabled;
    public static ConfigEntry<bool> ConfigBigKatanaEnabled;

    public static ConfigEntry<bool> ConfigUnlimitedAmmo;
    public static ConfigEntry<float> ConfigCdr;

    public static ConfigEntry<float> ConfigMeleeSpeed;
    public static ConfigEntry<float> ConfigMeleeSpeedMaximum;

    public static ConfigEntry<float> ConfigMeleeAtkSpeed;
    public static ConfigEntry<float> ConfigMeleeAtkSpeedMaximum;

    public static void Init(ConfigFile configFile)
    {
        ConfigGunEnabled = configFile.Bind("General", "GunModule", true, "Enable gun modification, true/false");
        ConfigMeleeEnabled = configFile.Bind("General", "MeleeModule", true, "Enable melee modification, true/false");
        ConfigBigKatanaEnabled = configFile.Bind("General", "GiveBigKatana", false, "Spawn an unbreakable big katana with the player, true/false");

        ConfigUnlimitedAmmo = configFile.Bind("GunModule", "UnlimitedAmmo", true, "Set unlimited ammo for all weapons, true/false");
        ConfigCdr = configFile.Bind("GunModule", "CooldownPercent", 0.4f, "Set weapon cooldown (ex 0.4 = 40% of initial cd OR 60% faster), 0-1");

        ConfigMeleeSpeed = configFile.Bind("MeleeModule", "MeleeMoveSpeed", .2f, "Set move speed gained after dealing melee damage, this will happen A LOT be careful (ex 0.02 = 2%), 0-3.402823E+38");
        ConfigMeleeSpeedMaximum = configFile.Bind("MeleeModule", "MeleeMoveSpeedMaximum", 10f, "Set the maximum speed boost, the animation can glitch if it's too high, 0-3.402823E+38");

        ConfigMeleeAtkSpeed = configFile.Bind("MeleeModule", "MeleeAtkSpeed", .2f, "Set attack speed gained after dealing melee damage, this will happen A LOT be careful , 0-3.402823E+38");
        ConfigMeleeAtkSpeedMaximum = configFile.Bind("MeleeModule", "MeleeAtkSpeedMaximum", 10f, "Set the maximum attack speed boost, the animation can glitch if it's too high, 0-3.402823E+38");
    }
}
