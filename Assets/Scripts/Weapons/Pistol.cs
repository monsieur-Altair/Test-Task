using UnityEngine;

namespace Weapons
{
    public class Pistol : BaseWeapon
    {
        protected override void Shoot()
        {
            foreach (var point in spawnPoints)
            {
                var startPosition = point.transform.position;
                var bullet = ObjectPool.GetObject(Type.Pistol, startPosition, Quaternion.identity)
                    .GetComponent<BaseBullet>();
                var bulletDirection = CalculateStartDirection();
                bullet.SetParameters(Damage, Range, startPosition);
                bullet.Launch(bulletDirection);
                CurrentBulletCount--;
            }
        }
    }
}