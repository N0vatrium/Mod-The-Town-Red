using System.ComponentModel;
using UnityEngine;

namespace MTTR.Imports
{
    public class WeaponImport : BaseImport
    {
        public string Model { get; set; }

        [DefaultValue("melee")]
        public string WeaponType { get; set; }

        [DefaultValue("one")]
        public string HoldType { get; set; }

        public Vector3? HandHold { get; set; }

        public Vector3? HandHoldRight { get; set; }

        public Vector3? HandHoldTwoHandRight { get; set; }

        public Vector3? HandHoldEnemy { get; set; }

        public Vector3? HandHoldEnemyRight { get; set; }

        public Vector3? HandHoldEnemyLeft { get; set; }

        public Vector3? HandHoldNonCombat { get; set; }

        public Quaternion? HandHoldNonCombatLocalRotationInverse { get; set; }

        public Quaternion? HandHoldNonCombatRightLocalRotationInverse { get; set; }

        public Quaternion? HandHoldRightTwoLocalRotationInverse { get; set; }

        public Quaternion? UseHandHoldLocalRotationInverse { get; set; }

        public Quaternion? HandHoldEnemyLocalRotationInverse { get; set; }

        public Quaternion? HandHoldEnemyRightLocalRotationInverse { get; set; }

        [DefaultValue(false)]
        public bool AttemptToFixStabPosition { get; set; }

        [DefaultValue(0f)]
        public float BreakChanceOnBlock { get; set; }

        [DefaultValue(0f)]
        public float BreakChanceOnBlockFullHealth { get; set; }

        [DefaultValue(0f)]
        public float BreakChanceOnHitIntoEnvMax { get; set; }

        [DefaultValue(0f)]
        public float BreakChanceOnHitIntoEnvMin { get; set; }

        [DefaultValue(0f)]
        public float BreakChanceOnHitIntoEnvVelocityMod { get; set; }

        [DefaultValue(0f)]
        public float BreakChanceOnSwing { get; set; }

        [DefaultValue(0f)]
        public float BreakChanceOnSwingFullHealth { get; set; }

        [DefaultValue(0f)]
        public float BreakChanceOnThrow { get; set; }

        [DefaultValue(0f)]
        public float BreakChanceOnThrowFullHealth { get; set; }

        [DefaultValue(0f)]
        public float BreakChanceOnThrust { get; set; }

        [DefaultValue(0f)]
        public float BreakChanceOnThrustFullHealth { get; set; }

        [DefaultValue(0f)]
        public float BreakChanceWhenHitFromOtherWeaponByDamageMod { get; set; }

        [DefaultValue(0f)]
        public float BreakChanceWhenHitFromOtherWeaponMax { get; set; }

        [DefaultValue(0f)]
        public float BreakChanceWhenHitFromOtherWeaponMin { get; set; }

        [DefaultValue(0f)]
        public float BreakChanceWhenShotMax { get; set; }

        [DefaultValue(0f)]
        public float BreakChanceWhenShotMin { get; set; }

        [DefaultValue(false)]
        public bool BreakOnAttack { get; set; }

        [DefaultValue(false)]
        public bool BreakWhenShot { get; set; }

        [DefaultValue(true)]
        public bool CanBePickedUpByEnemies { get; set; }

        [DefaultValue(false)]
        public bool CanBreakOnHitFromWeapon { get; set; }

        [DefaultValue(false)]
        public bool CanDamage { get; set; }

        [DefaultValue(false)]
        public bool Food { get; set; }

        [DefaultValue(0f)]
        public float Mass { get; set; }

        [DefaultValue(0f)]
        public float MaxThrowDamage { get; set; }

        [DefaultValue(0f)]
        public float MinThrowDamage { get; set; }

        [DefaultValue(0f)]
        public float MinThrowVelocity { get; set; }

        [DefaultValue(0f)]
        public float MinThrowVelocitySqr { get; set; }

        [DefaultValue(0)]
        public int NumBlocksBeforeDamage { get; set; }

        [DefaultValue(0)]
        public int NumHitsBeforeDamage { get; set; }

        [DefaultValue(false)]
        public bool PlayerUseTwoHandsWithoutShield { get; set; }

        //TODO add random torque throw
        //TODO add stick in logic

        [DefaultValue(0f)]
        public float BlockStrength { get; set; }

        [DefaultValue(false)]
        public bool CanCutHair { get; set; }

        [DefaultValue(false)]
        public bool CanHoldWithShield { get; set; }

        [DefaultValue(false)]
        public bool CanHoldWithShieldPlayer { get; set; }

        [DefaultValue(false)]
        public bool CanKillPursuers { get; set; }

        [DefaultValue(false)]
        public bool CanLungeStab { get; set; }

        [DefaultValue(false)]
        public bool CanLungeSwing { get; set; }

        [DefaultValue(false)]
        public bool CanUseGunHold { get; set; }

        [DefaultValue(0f)]
        public float DamageAmountForEnemies { get; set; }

        [DefaultValue(1f)]
        public float DamageMultiplierForDownedEnemies { get; set; }

        [DefaultValue(1f)]
        public float DeadHitForceMod { get; set; }

        [DefaultValue(0f)]
        public float ExplodeVoxelChance { get; set; }

        [DefaultValue(500f)]
        public float ExplodeVoxelForceMax { get; set; }

        [DefaultValue(200f)]
        public float ExplodeVoxelForceMin { get; set; }

        [DefaultValue(200f)]
        public float ExplodeVoxelSize { get; set; }

        [DefaultValue(0.2f)]
        public float ExtraWeaponRangeForEnemies { get; set; }

        [DefaultValue(0)]
        public ushort Gold { get; set; }

        [DefaultValue(0f)]
        public float HitForce { get; set; }

        [DefaultValue(0f)]
        public float ImpactForce { get; set; }

        [DefaultValue(false)]
        public bool IsGun { get; set; }

        [DefaultValue(false)]
        public bool IsShield { get; set; }

        [DefaultValue(0f)]
        public float KnockbackForce { get; set; }

        [DefaultValue(0f)]
        public float KnockdownChance { get; set; }

        [DefaultValue(0f)]
        public float MaxDamage { get; set; }

        [DefaultValue(0f)]
        public float MinDamage { get; set; }

        [DefaultValue(false)]
        public bool Shock { get; set; }

        [DefaultValue(0f)]
        public float StartAttackTime { get; set; }

        [DefaultValue(0f)]
        public float WeaponDamageAmountOnBlock { get; set; }

        [DefaultValue(0f)]
        public float WeaponDamageAmountOnHit { get; set; }


        // Gun specific

        [DefaultValue(0f)]
        public float AccuracyChangeMaxLeftRight { get; set; }

        [DefaultValue(0f)]
        public float AccuracyChangeMaxUpDown { get; set; }

        [DefaultValue(0f)]
        public float AccuracyImproveSpeed { get; set; }

        [DefaultValue(0f)]
        public float AccuracyReducePerShot { get; set; }

        [DefaultValue(0)]
        public int Ammo { get; set; }

        // ak47 bullet
        [DefaultValue("73e9ac69cf296a1469e316922d91ef7e")]
        public string Bullet { get; set; }

        [DefaultValue(true)]
        public bool ChangeAccuracy { get; set; }

        [DefaultValue(false)]
        public bool ChargedShot { get; set; }

        //TODO gun anims

        [DefaultValue(false)]
        public bool HasAdjustedAmmo { get; set; }

        [DefaultValue(false)]
        public bool HasLoadAnimation { get; set; }

        [DefaultValue(false)]
        public bool HasSwitchedToMelee { get; set; }

        [DefaultValue(false)]
        public bool IsMageStaff { get; set; }

        [DefaultValue(0f)]
        public float LoadDelay { get; set; }

        [DefaultValue(0)]
        public int NumBullets { get; set; }

        public Vector3? ShootPoint { get; set; }

        [DefaultValue(0)]
        public int NumShootPoints { get; set; }

        //TODO add seeker logic

        // PumpActionSmoke
        [DefaultValue("8ce8f0a655ee50542840cc4b5a4486f5")]
        public string ShootEffect { get; set; }

        [DefaultValue(0f)]
        public float ShotFrequency { get; set; }

        [DefaultValue(0f)]
        public float SpreadAngle { get; set; }

        [DefaultValue(0f)]
        public float SpreadAngleVertical { get; set; }

        [DefaultValue(true)]
        public bool StartCombatOnShoot { get; set; }

        [DefaultValue(0f)]
        public float SwitchToMeleeDelay { get; set; }

        [DefaultValue(false)]
        public bool SyncShots { get; set; }

        [DefaultValue(false)]
        public bool UnlimitedAmmo { get; set; }

        [DefaultValue(false)]
        public bool Use3rdPersonOffset1 { get; set; }

        [DefaultValue(false)]
        public bool Use3rdPersonOffset2 { get; set; }

        [DefaultValue(false)]
        public bool UseNonGunHitEnemyCheck { get; set; }
    }
}
