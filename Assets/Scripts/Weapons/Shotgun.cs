using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    public class Shotgun : BaseWeapon
    {
        protected override void Shoot()
        {
            foreach (var point in spawnPoints)
            {
                var startPosition = point.transform.position;
                var bullet = ObjectPool.GetObject(Type.Shotgun, startPosition, Quaternion.identity).GetComponent<Bullet>();
                var bulletDirection = transform.TransformDirection(CalculateStartDirection());
                bullet.SetParameters(Damage, Range, startPosition);
                bullet.Launch(bulletDirection);
                CurrentBulletCount--;
            }
        }
    }
}