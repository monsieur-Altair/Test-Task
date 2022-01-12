using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon_resources;
using Random=UnityEngine.Random;

namespace Weapons
{
    public enum Type
    {
        Pistol,
        Shotgun,
        AssaultRifle,
        ElectricGun
    }
    
    
    public abstract class BaseWeapon : MonoBehaviour
    {
        public float Damage { get; private set; }
        public float Rapidity { get; private set; }
        public float Range { get; private set; }
        public float Dispersion { get; private set; }
        public float Cooldown { get; private set; }
        public int Clip { get; private set; }
        public float Speed { get; private set; }
        
        public bool IsOnCooldown { get; private set; }
        public int CurrentBulletCount { get; protected set; }
        
        [SerializeField] private Type gunType;
        [SerializeField] protected List<GameObject> spawnPoints;

        public delegate Vector3 CalcDirFunc(Vector3 weaponDirection);

        private CalcDirFunc _getRawDirection;
        public Type GunType { get; private set; }

        protected abstract void Shoot();
        protected Managers.ObjectPool ObjectPool;

        private float _fireDuration;
        private bool _isShooted;
        private const float DispersionGap = 100.0f;
        
        private void Awake() => GunType = gunType;

        private void OnDisable() => _isShooted = false;
        
        public virtual void Initialize(Weapon resource, CalcDirFunc func)
        {
            Damage = resource.damage;
            Rapidity = resource.rapidity;
            Range = resource.range;
            Dispersion = resource.dispersion;
            Cooldown = resource.cooldown;
            Clip = resource.clip;
            Speed = resource.speed;
            
            CurrentBulletCount = Clip;
            ObjectPool = Managers.ObjectPool.Instance;
            IsOnCooldown = false;
            _isShooted = false;
            _fireDuration = 1.0f / Rapidity;

            _getRawDirection = func;
        }

        public void StartShooting()
        {
            if (CurrentBulletCount == 0 || IsOnCooldown|| _isShooted)
            {
                return;
            }
            Shoot();
            StartCoroutine(WaitShootDuration());
        }

        private IEnumerator WaitShootDuration()
        {
            _isShooted = true;
            yield return new WaitForSeconds(_fireDuration);
            _isShooted = false;
        }

        protected Vector3 CalculateStartDirection()
        {
            var offsetY = Random.Range(0.0f, Dispersion / DispersionGap);
            var offsetX = Random.Range(0.0f, Dispersion / DispersionGap);
            
            var startDirection = _getRawDirection(transform.position);

            startDirection+= Vector3.right * offsetX + Vector3.up * offsetY;;
            return startDirection * Speed;
        }
        
        public void CooldownTheWeapon()
        {
            if(gameObject.activeSelf)
                StartCoroutine(CooldownCoroutine());
        }

        private IEnumerator CooldownCoroutine()
        {
            IsOnCooldown = true;
            yield return new WaitForSeconds(Cooldown);
            CurrentBulletCount = Clip;
            IsOnCooldown = false;
        }
        
        
    }
    
    
}