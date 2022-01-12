using System;
using System.Collections;
using Characters;
using UnityEngine;

namespace Weapons
{
    public class ElectricBullet : BaseBullet
    {
        public float ElectricDamage { get; private set; }
        public int ElectricHitCount { get; private set; }
        public float HitsDuration { get; private set; }


        private float _hitPause;
        private BaseCharacter _hittedBaseCharacter;
        public event Action<ElectricBullet> Attacking;
        public void SetParameters(float damage, 
            float range, 
            Vector3 startPos, 
            float electricDamage, 
            int hitCount,
            float hitsDuration)
        {
            base.SetParameters(damage,range,startPos);
            ElectricDamage = electricDamage;
            ElectricHitCount = hitCount;
            HitsDuration = hitsDuration;
            _hitPause = HitsDuration / ElectricHitCount;
        }

        public override void ApplyDamage(BaseCharacter baseCharacter)
        {
            _hittedBaseCharacter = baseCharacter;
            base.ApplyDamage(baseCharacter);
            OnAttacking();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _hittedBaseCharacter = null;
        }

        public IEnumerator AttackedByElectricity()
        {
            var damageByHit = ElectricDamage / ElectricHitCount;
            for (var i = 0; i < ElectricHitCount; i++)
            {
                if(_hittedBaseCharacter.IsAlive==false)
                    yield break;
                yield return new WaitForSeconds(_hitPause);
                _hittedBaseCharacter.ReceiveDamage(damageByHit);
            }
        }

        protected virtual void OnAttacking() => Attacking?.Invoke(this);
    }
}