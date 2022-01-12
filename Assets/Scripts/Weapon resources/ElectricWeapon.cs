using UnityEngine;

namespace Weapon_resources
{    
    [CreateAssetMenu(fileName = "New electric weapon", menuName = "Resources/Electric weapon")]
    public class ElectricWeapon : Weapon
    {
        public float electricDamage; //additional damage by electricity
        public int electricHitCount; //hit count after main damage
    }
}