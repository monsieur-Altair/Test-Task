using System;
using System.Collections.Generic;
using Characters;
using Exceptions;
using UnityEngine;
using Weapon_resources;
using Random = UnityEngine.Random;
using Type = Weapons.Type;

namespace Managers
{
    public class WeaponManager : MonoBehaviour
    {
        public static WeaponManager Instance { get; private set; }

        [SerializeField] private List<Weapon> weaponResources;
        [SerializeField] private List<GameObject> weaponsPrefab;
        [SerializeField] private GameObject player;
        public BaseCharacter MainCharacter { get; private set; }
        public int WeaponsCount { get; private set; }
        private readonly Vector3 _offset = new Vector3(0.6f, 1.5f, 0.35f);

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            WeaponsCount = weaponsPrefab.Count;
        }

        private void Start()
        {
            MainCharacter = player.GetComponent<BaseCharacter>();
            if (MainCharacter == null)
            {
                throw new GameException("cannot get character component");
            }
            PrepareMainCharacter();
        }

        private void PrepareMainCharacter()
        {
            for (var prefabIndex=0;prefabIndex<weaponsPrefab.Count;prefabIndex++)
            {
                AddWeaponToCharacter(prefabIndex,MainCharacter);
            }
            var index = Random.Range(0, weaponsPrefab.Count - 1);
            SwitchWeapon(index, MainCharacter);
        }

        public void AddWeaponToCharacter(int prefabIndex, BaseCharacter character)
        {
            var weapon = CreateWeapon(weaponsPrefab[prefabIndex]);
            var type = (int) weapon.GunType;
            weapon.Initialize(weaponResources[type], character.GetRawDirection);
            var weaponTransform=weapon.gameObject.transform;
            var characterTransform = character.transform;
            weaponTransform.parent = characterTransform;
            weaponTransform.position = characterTransform.position + _offset;
            character.Cooldown+=weapon.CooldownTheWeapon;
            character.WeaponsList.Add(weapon);
            weapon.gameObject.SetActive(false);
        }

        private static Weapons.BaseWeapon CreateWeapon(GameObject prefab)
        {
            var weapon = Instantiate(prefab);
            var type = weapon.GetComponent<Weapons.BaseWeapon>().GunType;
            
            return type switch
            {
                Type.Pistol => weapon.GetComponent<Weapons.Pistol>(),
                Type.Shotgun => weapon.GetComponent<Weapons.Shotgun>(),
                Type.AssaultRifle => weapon.GetComponent<Weapons.AssaultRifle>(),
                Type.ElectricGun => weapon.GetComponent<Weapons.ElectricGun>(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public static void SwitchWeapon(int index, BaseCharacter character)
        {
            var newWeapon = character.WeaponsList[index];
            character.WeaponsList[character.CurrentIndex].gameObject.SetActive(false);
            newWeapon.gameObject.SetActive(true);
            character.SetCurrentWeapon(newWeapon,index);
        }
    }
}