using EPOOutline;
using MTTR.Imports;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TNet;
using UnityEngine;
using static Weapon.WeaponHoldType;
using Tools = MTTR.Helpers.Tools;

namespace MTTR.Factories
{
    public class WeaponFactory
    {
        public static readonly Dictionary<string, Weapon.WeaponHoldType> HoldTypes = new()
        {
            { "one", OneHanded },
            { "two_thin", TwoHandedThin },
            { "two_thick", TwoHandedThick }
        };

        public GameObject Generate(BaseImport import, GameObject weapon)
        {
            var importedWeapon = import.Weapon;

            if (importedWeapon == null)
            {
                FormatLog("Weapon is null", import);
            }

            if (importedWeapon.Model == null)
            {
                FormatLog("Weapon model is null", import);
            }


            if (!HoldTypes.ContainsKey(importedWeapon.HoldType))
            {
                FormatLog("HoldType should be one of " + String.Join(',', HoldTypes.Keys), import);
            }

            // check weapon body and end
            var weaponBody = weapon.transform.Find("Body");
            if (weaponBody == null)
            {
                FormatLog("Imported model is missing a child named Body", import);
            }

            var endFormat = "Body_end";
            var weaponEnd = weapon.transform.Find(endFormat);
            if (weaponEnd == null)
            {
                FormatLog("Imported model is missing a child named Body_end", import);
            }
            weaponEnd.transform.SetParent(weaponBody);

            SetupWeaponChild(weaponBody.gameObject);
            SetupWeaponChild(weaponEnd.gameObject);

            var rigidbody = weapon.AddComponent<Rigidbody>();
            Tools.TogglePhysic(weapon, false);

            var weaponVoxelHolder = new GameObject("VoxelWeapon");
            weaponVoxelHolder.transform.SetParent(weapon.transform);
            var weaponVoxel = weaponVoxelHolder.AddComponent<VoxelWeapon>();
            weaponVoxel.enabled = false;

            var tnObject = weapon.AddComponent<TNObject>();
            var impactEfects = weapon.AddComponent<ImpactEffects>();

            Tools.WriteLog("Ignore the following Unity error, I need to decomp the game to find the source because I'm assigning the values a few lines after this", warning: true);
            var weaponComp = importedWeapon.IsGun ? weapon.AddComponent<WeaponGun>() : weapon.AddComponent<Weapon>();

            weaponComp.breakStickInChild = weaponEnd;


            weaponComp.weaponHoldType = HoldTypes[importedWeapon.HoldType];

            weaponComp.handHold = CreateHandPosOrFail(weapon, "HandHold", importedWeapon);
            weaponComp.useHandHold = weaponComp.handHold;
            weaponComp.handHoldNonCombat = CreateHandPosOrFail(weapon, "HandHoldNonCombat", importedWeapon);

            weaponComp.handHoldEnemyLocalRotationInverse = CreateRotation(importedWeapon.HandHoldEnemyLocalRotationInverse);
            weaponComp.handHoldNonCombatLocalRotationInverse = CreateRotation(importedWeapon.HandHoldNonCombatLocalRotationInverse);
            weaponComp.handHoldRightTwoLocalRotationInverse = CreateRotation(importedWeapon.HandHoldRightTwoLocalRotationInverse);
            weaponComp.useHandHoldLocalRotationInverse = CreateRotation(importedWeapon.UseHandHoldLocalRotationInverse);


            switch (weaponComp.weaponHoldType)
            {
                case TwoHandedThin:
                case TwoHandedThick:
                    weaponComp.handHoldTwoHandedRight = CreateHandPosOrFail(weapon, "HandHoldTwoHandRight", importedWeapon);
                    weaponComp.handHoldTwoHandRightIK = weaponComp.handHoldTwoHandedRight;

                    weaponComp.handHoldEnemy = CreateHandPosOrFail(weapon, "HandHoldEnemyLeft", importedWeapon);
                    weaponComp.handHoldEnemyRight = CreateHandPosOrFail(weapon, "HandHoldEnemy", importedWeapon);

                    weaponComp.handHoldEnemyRightLocalRotationInverse = CreateRotation(importedWeapon.HandHoldEnemyRightLocalRotationInverse);
                    weaponComp.handHoldNonCombatRightLocalRotationInverse = CreateRotation(importedWeapon.HandHoldNonCombatRightLocalRotationInverse);

                    break;
                case OneHanded:
                    weaponComp.handHoldEnemy = CreateHandPosOrFail(weapon, "HandHoldEnemy", importedWeapon);
                    break;
            }

            weaponComp.isGun = importedWeapon.IsGun;

            if (importedWeapon.IsGun)
            {
                var gunComp = (WeaponGun)weaponComp;

                var loadedAmmo = GetAssetOrfail(importedWeapon.Bullet, "Ammo");
                var loadedEffect = GetAssetOrfail(importedWeapon.ShootEffect, "ShootEffect");

                gunComp.accuracyChangeMaxLeftRight = importedWeapon.AccuracyChangeMaxLeftRight;
                gunComp.accuracyChangeMaxUpDown = importedWeapon.AccuracyChangeMaxUpDown;
                gunComp.accuracyImproveSpeed = importedWeapon.AccuracyImproveSpeed;
                gunComp.accuracyReducePerShot = importedWeapon.AccuracyReducePerShot;
                gunComp.ammo = importedWeapon.Ammo;
                gunComp.bullet = loadedAmmo;
                gunComp.changeAccuracy = importedWeapon.ChangeAccuracy;
                gunComp.chargedShot = importedWeapon.ChargedShot;
                gunComp.hasAdjustedAmmo = importedWeapon.HasAdjustedAmmo;
                gunComp.hasLoadAnimation = importedWeapon.HasLoadAnimation;
                gunComp.hasSwitchedToMelee = importedWeapon.HasSwitchedToMelee;
                gunComp.isMageStaff = importedWeapon.IsMageStaff;
                gunComp.loadDelay = importedWeapon.LoadDelay;
                gunComp.numBullets = importedWeapon.NumBullets;
                gunComp.shootPoint = CreateHandPosOrFail(weapon, "ShootPoint", importedWeapon);
                gunComp.numShootPoints = 1; //TODO add multishot
                gunComp.shootEffect = loadedEffect;
                gunComp.shotFrequency = importedWeapon.ShotFrequency;
                gunComp.spreadAngle = importedWeapon.SpreadAngle;
                gunComp.spreadAngleVertical = importedWeapon.SpreadAngleVertical;
                gunComp.startCombatOnShoot = importedWeapon.StartCombatOnShoot;
                gunComp.switchToMeleeDelay = importedWeapon.SwitchToMeleeDelay;
                gunComp.syncShots = importedWeapon.SyncShots;
                gunComp.unlimitedAmmo = importedWeapon.UnlimitedAmmo;
                gunComp.use3rdPersonOffset1 = importedWeapon.Use3rdPersonOffset1;
                gunComp.use3rdPersonOffset2 = importedWeapon.Use3rdPersonOffset2;
                gunComp.useNonGunHitEnemyCheck = importedWeapon.UseNonGunHitEnemyCheck;
                gunComp.shootSound = "PistolShoot";
            }

            // apply all properties
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

            // static stuff
            weaponComp.additionalLeftHandRotation = new Vector3(180, 0, 0);
            weaponComp.cameraRelativeForwardDirectionOnThrow = new Vector3(-1, 0, 0);
            weaponComp.cameraRelativeUpDirectionOnThrow = new Vector3(0, 0, -1);
            weaponComp.weaponTypeID = Datastore.Instance.CreateWeaponId(import.Id);

            weaponComp.FillChildRendererArray();
            weaponComp.FillVoxelWeapons();
            weaponComp.PostSpawn();
            return weapon;
        }

        public void SetupWeaponChild(GameObject child)
        {
            child.AddComponent<PTTRDecalSystem>();
            var collider = child.AddComponent<MeshCollider>();
            collider.convex = true;

            child.AddComponent<TargetStateListener>();
        }

        public void FormatLog(string message, BaseImport context, bool error = true)
        {
            message = "Factory error for \"" + context?.Name + "\": " + message;
            Tools.WriteLog(message, error: error);

            throw new System.Exception(message);
        }

        private Quaternion CreateRotation(Quaternion? quaternion)
        {
            return quaternion == null ? new Quaternion() : (Quaternion)quaternion;
        }

        private Transform CreateHandPosOrFail(GameObject parent, string name, WeaponImport importData)
        {
            var child = parent.transform.Find(name);
            if (child != null)
            {
                var message = "Child " + name + " already present in " + parent.name + " using HoldType " + HoldTypes[importData.HoldType];
                LogCrash(message);
            }

            Vector3? pos = (Vector3?)importData.GetType().GetProperty(name).GetValue(importData, null);
            if (pos == null)
            {
                var message = "Missing position named " + name + " in JSON config using HoldType " + HoldTypes[importData.HoldType];
                LogCrash(message);
            }

            child = new GameObject(name).transform;
            child.SetParent(parent.transform);
            child.localPosition = (Vector3)pos;

            return child;
        }

        private GameObject GetAssetOrfail(string assetId, string category)
        {
            var asset = Tools.LoadAsset(assetId, false);
            if (asset == null)
            {
                LogCrash(category + " " + assetId + " could not be found, aborting");
            }

            return asset;
        }

        private void LogCrash(string message)
        {
            Tools.WriteLog(message, error: true);

            throw new JsonReaderException(message);
        }
    }
}
