using System;
using Exceptions;
using UnityEngine;

namespace Weapons
{
    public class Bullet : MonoBehaviour
    {
        public float Damage { get; private set; }
        private float _range;
        private Vector3 _startPosition;
        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody == null)
            {
                throw new GameException("cannot get bullet rigidbody component");
            }
        }

        protected virtual void OnEnable()
        {
            _range=float.PositiveInfinity;
        }

        public void SetParameters(float damage, float range, Vector3 startPos)
        {
            Damage = damage;
            _range = range;
            _startPosition = startPos;
        }

        private void Update()
        {
            var distance = Vector3.Distance(_startPosition, transform.position);
            if (distance > _range)
            {
                DestroyBullet();
            }
        }

        private void DestroyBullet()
        {
            gameObject.SetActive(false);
        }

        public virtual void ApplyDamage(Characters.Character character)
        {
            character.ReceiveDamage(Damage);
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        public void Launch(Vector3 direction)
        {
            _rigidbody.AddForce(direction);
        }
        
    }
}