using System;
using Exceptions;
using UnityEngine;

namespace Weapons
{
    public class Bullet : MonoBehaviour
    {
        private float _damage;
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

        private void OnEnable()
        {
            _range=float.PositiveInfinity;
        }

        public void SetParameters(float damage, float range, Vector3 startPos)
        {
            _damage = damage;
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
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            gameObject.SetActive(false);
        }

        public void Launch(Vector3 direction)
        {
            _rigidbody.AddForce(direction);
        }
        
        
        
    }
}