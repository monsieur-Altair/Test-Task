using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Type = Weapons.Type;

namespace Managers
{
    public class WeaponManager : MonoBehaviour
    {
        public static WeaponManager Instance { get; private set; }

        [SerializeField] private List<Resources.Weapon> weaponResources;
        [SerializeField] private List<GameObject> weaponsPrefab;
        [SerializeField] private GameObject player;
        
        private readonly Vector3 _offset = new Vector3(0.6f, 1.5f, 0.35f);
        public int CurrentIndex { get; private set; }
        public List<Weapons.BaseWeapon> WeaponsList { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            WeaponsList = new List<Weapons.BaseWeapon>();
            foreach (var prefab in weaponsPrefab)
            {
                var weapon = CreateWeapon(prefab);
                var type = (int) weapon.GunType;
                weapon.Initialize(weaponResources[type]);
                weapon.gameObject.transform.parent = player.transform;//////////////////////////////////////////////
                weapon.gameObject.transform.position = _offset;/////////////////////////////////////////////////////
                player.GetComponent<Player>().Cooldown+=weapon.CooldownTheWeapon;
                WeaponsList.Add(weapon);
                weapon.gameObject.SetActive(false);
            }

            var index = Random.Range(0, weaponsPrefab.Count - 1);
            SwitchWeapon(index);
        }

        private Weapons.BaseWeapon CreateWeapon(GameObject prefab)
        {
            var weapon = Instantiate(prefab);
            var type = weapon.GetComponent<Weapons.BaseWeapon>().GunType;
//            Debug.Log("type = "+type);
            return type switch
            {
                Type.Pistol => weapon.GetComponent<Weapons.Pistol>(),
                Type.Shotgun => weapon.GetComponent<Weapons.Shotgun>(),
                Type.AssaultRifle => weapon.GetComponent<Weapons.AssaultRifle>(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public void SwitchWeapon(int index)
        {
            var newWeapon = WeaponsList[index];
            WeaponsList[CurrentIndex].gameObject.SetActive(false);
            newWeapon.gameObject.SetActive(true);
            CurrentIndex = index;
            player.GetComponent<Player>().SetWeapon(newWeapon);
        }
    }
}