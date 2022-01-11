using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public enum Type
    {
        Pistol,
        Shotgun,
        AssaultRifle
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
        public Type GunType { get; private set; }

        protected abstract void Fire();
        protected Managers.ObjectPool ObjectPool;
        
        private void Awake()
        {
            GunType = gunType;
           // Debug.Log(GunType);
        }

        public void Initialize(Resources.Weapon resource)
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
        }

        public void StartFiring()
        {
            if (CurrentBulletCount == 0 || IsOnCooldown)
            {
                return;
            }
            
            Fire();
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