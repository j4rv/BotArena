using UnityEngine;

namespace BotArena
{
    internal abstract class IWeaponController : MonoBehaviour
    {
        [SerializeField]
        public RobotController robot;
        [SerializeField]
        public GameObject projectileObject; //Optional, can be null for melee weapons.
        [SerializeField]
        public Transform bulletSpawner;    //Optional, can be null for melee weapons.

        public abstract void Attack(float power);
        public abstract float GetStaminaCost();

    }
}
