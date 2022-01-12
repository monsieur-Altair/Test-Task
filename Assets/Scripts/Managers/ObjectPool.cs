﻿using System;
using System.Collections.Generic;
using Exceptions;
using UnityEngine;

namespace Managers
{
    public class ObjectPool : MonoBehaviour
    {
        private Dictionary<int, Queue<GameObject>> _poolDictionary;

        [Serializable]
        public class Pool
        {
            public Weapons.Type type;
            public GameObject prefab;
            public int size;
        }
        public List<Pool> pools;
        public static ObjectPool Instance;
        public void Awake()
        {
            if (Instance == null)
                Instance = this;
            _poolDictionary = new Dictionary<int, Queue<GameObject>>();
        }

        public void Start()
        {
            foreach (var pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>(); 
                for (var i = 0; i < pool.size; i++)
                {
                    var unit = Instantiate(pool.prefab,transform);
                    unit.SetActive(false);
                    objectPool.Enqueue(unit);
                }
                _poolDictionary.Add(pool.type.GetHashCode(),objectPool);
            }
        }

        public GameObject GetObject(Weapons.Type type, Vector3 position, Quaternion rotation)
        {
            var hash=type.GetHashCode();
            if (_poolDictionary.ContainsKey(hash) == false)
            {
                throw new GameException("doesn't exist key value");
            }

            GameObject obj = _poolDictionary[hash].Dequeue();
            obj.SetActive(true);
            obj.transform.position = position;
            obj.transform.rotation = rotation;

            _poolDictionary[hash].Enqueue(obj);

            return obj;
        }
    }
}