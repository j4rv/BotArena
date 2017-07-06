using System;
using UnityEngine;

namespace BotArena
{
    internal class Cannon : IWeaponController
    {

        public override void Attack(float power) {
            GameObject res;

            res = Instantiate(projectileObject, bulletSpawner.position, bulletSpawner.rotation);
            res.GetComponent<Rigidbody>().velocity = transform.forward * power * 8;
            res.GetComponent<IProjectileController>().SetWeapon(this);
        }

        public override float GetStaminaCost() {
            return 20;
        }
    }
}
