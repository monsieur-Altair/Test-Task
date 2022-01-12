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

        private const float HitPause = 0.3f;
        private BaseCharacter _hittedBaseCharacter;
        public event Action<ElectricBullet> Attacking;
        public void SetParameters(float damage, float range, Vector3 startPos, float electricDamage, int hitCount)
        {
            base.SetParameters(damage,range,startPos);
            ElectricDamage = electricDamage;
            ElectricHitCount = hitCount;
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
                yield return new WaitForSeconds(HitPause);
                _hittedBaseCharacter.ReceiveDamage(damageByHit);
            }
        }

        protected virtual void OnAttacking() => Attacking?.Invoke(this);
    }
}