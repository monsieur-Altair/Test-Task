using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Characters
{
    public class Enemy : BaseCharacter
    {
        private BaseCharacter _aim;
        private Transform _aimTransform;

        private const float MINTime = 2.0f;
        private const float MAXTime = 5.0f;

        private void Update() => transform.LookAt(_aimTransform);

        public void SetAim(BaseCharacter character)
        {
            _aim = character;
            _aimTransform = _aim.transform;
            StartCoroutine(AttackAim());
        }

        private IEnumerator AttackAim()
        {
            while (IsAlive)
            {
                var decisionTime = Random.Range(MINTime, MAXTime);
                yield return new WaitForSeconds(decisionTime);
                if (CurrentWeapon.CurrentBulletCount != 0)
                    CurrentWeapon.StartShooting();
                else
                {
                    StartCooldown();
                }

                yield return null;
            }
        }
    
        //calculate bullet direction for AI(enemies)
        public override Vector3 GetRawDirection(Vector3 weaponPosition)
        {
            var rawDirection = (_aim.transform.position - weaponPosition).normalized;
            rawDirection.y = 0;
            return rawDirection;
        }
    
    }
}