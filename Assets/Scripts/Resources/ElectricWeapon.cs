using UnityEngine;

namespace Resources
{    
    [CreateAssetMenu(fileName = "New electric weapon", menuName = "Resources/Electric weapon")]
    public class ElectricWeapon : Weapon
    {
        public float electricDamage;
        public int electricHitCount;
    }
}