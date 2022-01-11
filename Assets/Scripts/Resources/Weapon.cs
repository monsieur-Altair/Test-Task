using UnityEngine;

namespace Resources
{
    [CreateAssetMenu(fileName = "New weapon", menuName = "Resources/Weapon")]
    public class Weapon : ScriptableObject
    {
        public float damage;
        public float rapidity;
        public float range;
        public float dispersion;
        public float cooldown;
        public int clip;
        public float speed;
    }
}