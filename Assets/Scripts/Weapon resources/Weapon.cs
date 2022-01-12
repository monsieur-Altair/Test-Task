using UnityEngine;

namespace Weapon_resources
{
    [CreateAssetMenu(fileName = "New weapon", menuName = "Resources/Weapon")]
    public class Weapon : ScriptableObject
    {
        public float damage;    //hp
        public float rapidity;  //bullet/s
        public float range;     //metres
        public float dispersion;//increases circle radius for each 100 metres
        public float cooldown;  //in seconds
        public int   clip;
        public float speed;     // m/s
    }
}