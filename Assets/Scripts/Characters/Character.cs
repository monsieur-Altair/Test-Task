using System;
using System.Collections;
using System.Collections.Generic;
using Exceptions;
using UnityEngine;

namespace Characters
{
    public class Character : MonoBehaviour
    {
        public float Hp { get; private set; }
        public Weapons.BaseWeapon CurrentWeapon { get; private set; }
        private Material _currentMaterial;
        private static readonly int Color1 = Shader.PropertyToID("_Color");
        public event Action Cooldown;
        public bool IsAlive { get; private set; }
        public List<Weapons.BaseWeapon> WeaponsList { get; private set; }
        public int CurrentIndex { get; private set; }



        private void Awake()
        {
            _currentMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;
            if (_currentMaterial == null)
            {
                throw new GameException("cannot get character material");
            }

            WeaponsList = new List<Weapons.BaseWeapon>();
        }

        private void Start()
        {
            IsAlive = true;
            Hp = 100.0f;
            CurrentIndex = 0;
        }

        public void SetCurrentWeapon(Weapons.BaseWeapon weapon, int index)
        {
            CurrentIndex = index;
            CurrentWeapon = weapon;
        }

        public void Shoot()
        {
            CurrentWeapon.StartShooting();
        }

        public void StartCooldown()
        {
            OnCooldown();
        }

        private void OnCooldown()
        {
            Cooldown?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsAlive == false)
                return;

            var bullet = other.GetComponent<Weapons.Bullet>();
            if (bullet == null)
            {
                throw new GameException("cannot get bullet component");
            }
            bullet.ApplyDamage(this);
        }

        private void ChangeColor(float colorDamage)
        {
            var currentColor = _currentMaterial.color;
            var additionalRed = 1.0f - currentColor.r - colorDamage;
            if (additionalRed > 0)
                currentColor.r += colorDamage;
            else
            {
                currentColor.r = 1.0f;
                currentColor.g -= colorDamage;
                if (currentColor.g < 0)
                    currentColor.g = 0;
            }
            _currentMaterial.SetColor(Color1,currentColor);
        }

        private IEnumerator DeleteCharacter()
        {
            Hp = 0;
            IsAlive = false;
            yield return new WaitForSeconds(1.0f);
            gameObject.SetActive(false);
        }
    
        public virtual Vector3 GetRawDirection(Vector3 weaponPosition)
        {
            var rawDirection =  transform.TransformDirection(Vector3.forward);
            var cameraTransform = Camera.main.transform;
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hitInfo, 100.0f))
            {
                rawDirection = (hitInfo.point-weaponPosition).normalized;
                rawDirection.y = 0.0f;
            }

            return rawDirection;
        }

        public void ReceiveDamage(float damage)
        {
            Hp -= damage;
            var colorDamage = 2.0f * damage/100.0f;//depend on start color
            ChangeColor(colorDamage);
            if (Hp <= 0)
                StartCoroutine(DeleteCharacter());
        }
    }
}