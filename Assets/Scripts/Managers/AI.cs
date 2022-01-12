using System.Collections.Generic;
using System.Linq;
using Characters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    [DefaultExecutionOrder(600)]
    public class AI : MonoBehaviour
    {
        private List<BaseCharacter> _allEnemies = new List<BaseCharacter>();
        [SerializeField] private GameObject enemiesLay;
        private WeaponManager _weaponManager;
        private void Start()
        {
            _weaponManager = WeaponManager.Instance;
            _allEnemies = enemiesLay.GetComponentsInChildren<BaseCharacter>().ToList();
            foreach (var enemy in _allEnemies)
            {
                var weaponIndex = Random.Range(0, _weaponManager.WeaponsCount);
                _weaponManager.AddWeaponToCharacter(weaponIndex, enemy);
                WeaponManager.SwitchWeapon(0,enemy);
                enemy.GetComponent<Enemy>().SetAim(_weaponManager.MainCharacter);
            }
        }
    
    
    }
}