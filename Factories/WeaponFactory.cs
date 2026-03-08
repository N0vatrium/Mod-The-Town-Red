using MTTR.Helpers;
using MTTR.Imports;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MTTR.Factories
{
    public class WeaponFactory
    {
        public static readonly Dictionary<string, Weapon.WeaponHoldType> HoldTypes = new()
        {
            { "one", Weapon.WeaponHoldType.OneHanded },
            { "two_thin", Weapon.WeaponHoldType.TwoHandedThin },
            { "two_thick", Weapon.WeaponHoldType.TwoHandedThick }
        };

        public GameObject Generate(BaseImport import)
        {
            var weapon = new GameObject(import.Name);
            //weapon.active = false;

            var weaponComp = weapon.AddComponent<Weapon>();

            var importedWeapon = import.Weapon;

            if (importedWeapon == null)
            {
                FormatLog("Weapon is null", import);
            }

            if(importedWeapon.Model == null)
            {
                FormatLog("Weapon model is null", import);
            }


            if (!HoldTypes.ContainsKey(importedWeapon.HoldType)){
                FormatLog("HoldType should be one of " + String.Join(',', HoldTypes.Keys), import);
            }
            weaponComp.weaponHoldType = HoldTypes[importedWeapon.HoldType];

            weaponComp.attemptToFixStabPosition = importedWeapon.AttemptToFixStabPosition;
            weaponComp.breakChanceOnBlock = importedWeapon.BreakChanceOnBlock;
            weaponComp.breakChanceOnBlockFullHealth = importedWeapon.BreakChanceOnBlockFullHealth;
            weaponComp.breakChanceOnHitIntoEnvironmentMax = importedWeapon.BreakChanceOnHitIntoEnvMax;
            weaponComp.breakChanceOnHitIntoEnvironmentMin = importedWeapon.BreakChanceOnHitIntoEnvMin;
            weaponComp.breakChanceOnHitIntoEnvironmentVelocityMod = importedWeapon.BreakChanceOnHitIntoEnvVelocityMod;
            weaponComp.breakChanceOnSwing = importedWeapon.BreakChanceOnSwing;
            weaponComp.breakChanceOnSwingFullHealth = importedWeapon.BreakChanceOnSwingFullHealth;
            weaponComp.breakChanceOnThrow = importedWeapon.BreakChanceOnThrow;
            weaponComp.breakChanceOnThrowFullHealth = importedWeapon.BreakChanceOnThrowFullHealth;
            weaponComp.breakChanceOnThrust = importedWeapon.BreakChanceOnThrust;
            weaponComp.breakChanceOnThrustFullHealth = importedWeapon.BreakChanceOnThrustFullHealth;
            weaponComp.breakChanceWhenHitFromOtherWeaponByDamageMod = importedWeapon.BreakChanceWhenHitFromOtherWeaponByDamageMod;
            weaponComp.breakChanceWhenHitFromOtherWeaponMax = importedWeapon.BreakChanceWhenHitFromOtherWeaponMax;
            weaponComp.breakChanceWhenHitFromOtherWeaponMin = importedWeapon.BreakChanceWhenHitFromOtherWeaponMin;
            weaponComp.breakChanceWhenShotMax = importedWeapon.BreakChanceWhenShotMax;
            weaponComp.breakChanceWhenShotMin = importedWeapon.BreakChanceWhenShotMin;
            weaponComp.breakOnAttack = importedWeapon.BreakOnAttack;
            weaponComp.breakWhenShot = importedWeapon.BreakWhenShot;
            weaponComp.canBePickedUpByEnemies = importedWeapon.CanBePickedUpByEnemies;
            weaponComp.canBreakOnHitFromWeapon = importedWeapon.CanBreakOnHitFromWeapon;
            weaponComp.canDamage = importedWeapon.CanDamage;
            weaponComp.food = importedWeapon.Food;
            weaponComp.mass = importedWeapon.Mass;
            weaponComp.maxThrowDamage = importedWeapon.MaxThrowDamage;
            weaponComp.minThrowDamage = importedWeapon.MinThrowDamage;
            weaponComp.minThrowVelocity = importedWeapon.MinThrowVelocity;
            weaponComp.minThrowVelocitySqr = importedWeapon.MinThrowVelocitySqr;
            weaponComp.numBlocksBeforeDamage = importedWeapon.NumBlocksBeforeDamage;
            weaponComp.numHitsBeforeDamage = importedWeapon.NumHitsBeforeDamage;
            weaponComp.playerUseTwoHandsWithoutShield = importedWeapon.PlayerUseTwoHandsWithoutShield;
            weaponComp.blockStrength = importedWeapon.BlockStrength;
            weaponComp.canCutHair = importedWeapon.CanCutHair;
            weaponComp.canHoldWithShield = importedWeapon.CanHoldWithShield;
            weaponComp.canHoldWithShieldPlayer = importedWeapon.CanHoldWithShieldPlayer;
            weaponComp.canKillPursuers = importedWeapon.CanKillPursuers;
            weaponComp.canLungeStab = importedWeapon.CanLungeStab;
            weaponComp.canLungeSwing = importedWeapon.CanLungeSwing;
            weaponComp.canUseGunHold = importedWeapon.CanUseGunHold;
            weaponComp.damageAmountForEnemies = importedWeapon.DamageAmountForEnemies;
            weaponComp.deadHitForceMod = importedWeapon.DeadHitForceMod;
            weaponComp.explodeVoxelChance = importedWeapon.ExplodeVoxelChance;
            weaponComp.explodeVoxelForceMax = importedWeapon.ExplodeVoxelForceMax;
            weaponComp.explodeVoxelForceMin = importedWeapon.ExplodeVoxelForceMin;
            weaponComp.explodeVoxelSize = importedWeapon.ExplodeVoxelSize;
            weaponComp.extraWeaponRangeForEnemies = importedWeapon.ExtraWeaponRangeForEnemies;
            weaponComp.gold = importedWeapon.Gold;
            weaponComp.hitForce = importedWeapon.HitForce;
            weaponComp.impactForce = importedWeapon.ImpactForce;
            weaponComp.isGun = importedWeapon.IsGun;
            weaponComp.isShield = importedWeapon.IsShield;
            weaponComp.knockbackForce = importedWeapon.KnockbackForce;
            weaponComp.knockdownChance = importedWeapon.KnockdownChance;
            weaponComp.maxDamage = importedWeapon.MaxDamage;
            weaponComp.minDamage = importedWeapon.MinDamage;
            weaponComp.shock = importedWeapon.Shock;
            weaponComp.startAttackTime = importedWeapon.StartAttackTime;
            weaponComp.weaponDamageAmountOnBlock = importedWeapon.WeaponDamageAmountOnBlock;
            weaponComp.weaponDamageAmountOnHit = importedWeapon.WeaponDamageAmountOnHit;

            return weapon;
        }

        public void FormatLog(string message, BaseImport context, bool error= true)
        {
            message = "Factory error for \"" + context?.Name + "\": " + message;
            Tools.WriteLog(message, error: error);

            throw new System.Exception(message);
        }
    }
}
