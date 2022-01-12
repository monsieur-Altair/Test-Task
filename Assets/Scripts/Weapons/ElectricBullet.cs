using System;
using System.Collections;
using Characters;
using UnityEngine;

namespace Weapons
{
    public class ElectricBullet : Bullet
    {
        public float ElectricDamage { get; private set; }
        public int ElectricHitCount { get; private set; }

        private const float HitPause = 0.3f;
        private Character _hittedCharacter;
        public event Action<ElectricBullet> Attacking;
        public void SetParameters(float damage, float range, Vector3 startPos, float electricDamage, int hitCount)
        {
            base.SetParameters(damage,range,startPos);
            ElectricDamage = electricDamage;
            ElectricHitCount = hitCount;
        }

        public override void ApplyDamage(Character character)
        {
            _hittedCharacter = character;
            //StartCoroutine(AttackedByElectricity());
            base.ApplyDamage(character);
            OnAttacking();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _hittedCharacter = null;
        }

        public IEnumerator AttackedByElectricity()
        {
            var damageByHit = ElectricDamage / ElectricHitCount;
            for (var i = 0; i < ElectricHitCount; i++)
            {
                if(_hittedCharacter.IsAlive==false)
                    yield break;
                yield return new WaitForSeconds(HitPause);
                _hittedCharacter.ReceiveDamage(damageByHit);
            }
        }

        protected virtual void OnAttacking()
        {
            Attacking?.Invoke(this);
        }
    }
}