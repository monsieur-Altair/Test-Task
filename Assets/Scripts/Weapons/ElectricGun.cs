using Exceptions;
using UnityEngine;
using Weapon_resources;

namespace Weapons
{
    public class ElectricGun : BaseWeapon
    {
        public float ElectricDamage { get; private set; }
        public int ElectricHitCount { get; private set; }
        public float HitsDuration { get; private set; }
        protected override void Shoot()
        {
            foreach (var point in spawnPoints)
            {
                var startPosition = point.transform.position;
                var bullet = ObjectPool.GetObject(Type.ElectricGun, startPosition, Quaternion.identity)
                    .GetComponent<ElectricBullet>();
                var bulletDirection = CalculateStartDirection();
                bullet.SetParameters(Damage, Range, startPosition, ElectricDamage, ElectricHitCount, HitsDuration);
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
        
        public override void Initialize(Weapon resource, CalcDirFunc func)
        {
            base.Initialize(resource, func);
            var electricResource = resource as ElectricWeapon;
            if (electricResource == null)
            {
                throw new GameException("cannot get electric resources");
            }

            ElectricDamage = electricResource.electricDamage;
            ElectricHitCount = electricResource.electricHitCount;
            HitsDuration = electricResource.hitsDuration;
        }
        
        
    }
}