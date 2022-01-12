﻿using Exceptions;
using UnityEngine;

namespace Weapons
{
    public class ElectricGun : BaseWeapon
    {
        public float ElectricDamage { get; private set; }
        public int ElectricHitCount { get; private set; }
        protected override void Shoot()
        {
            foreach (var point in spawnPoints)
            {
                var startPosition = point.transform.position;
                var bullet = ObjectPool.GetObject(Type.ElectricGun, startPosition, Quaternion.identity)
                    .GetComponent<ElectricBullet>();
                var bulletDirection = CalculateStartDirection();
                bullet.SetParameters(Damage, Range, startPosition, ElectricDamage, ElectricHitCount);
                bullet.Launch(bulletDirection);
                bullet.Attacking += StartAttacking;
                CurrentBulletCount--;
            }
        }

        private void StartAttacking(ElectricBullet bullet)
        {
            StartCoroutine(bullet.AttackedByElectricity());
            bullet.Attacking -= StartAttacking;
        }
        
        public override void Initialize(Resources.Weapon resource, CalcDirFunc func)
        {
            base.Initialize(resource, func);
            var electricResource = resource as Resources.ElectricWeapon;
            if (electricResource == null)
            {
                throw new GameException("cannot get electric resources");
            }

            ElectricDamage = electricResource.electricDamage;
            ElectricHitCount = electricResource.electricHitCount;
        }
        
        
    }
}