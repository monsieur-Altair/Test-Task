using UnityEngine;

namespace Weapons
{
    public class Pistol : BaseWeapon
    {
        protected override void Fire()
        {
            foreach (var point in spawnPoints)
            {
                var startPosition = point.transform.position;
                var bullet = ObjectPool.GetObject(Type.Pistol, startPosition, Quaternion.identity).GetComponent<Bullet>();
                var bulletDirection = transform.TransformDirection(Vector3.forward * Speed);
                bullet.SetParameters(Damage, Range, startPosition);
                bullet.Launch(bulletDirection);
                CurrentBulletCount--;

            }
        }
    }
}