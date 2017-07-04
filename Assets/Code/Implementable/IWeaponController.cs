using UnityEngine;

namespace BotArena
{
    public abstract class IWeaponController : MonoBehaviour
    {
        
        public RobotController robot;
        public GameObject projectileObject; //Optional, can be null for melee weapons.
        public Transform bulletSpawner;    //Optional, can be null for melee weapons.

        public abstract void Attack(float power);
        public abstract float GetStaminaCost();

    }
}
