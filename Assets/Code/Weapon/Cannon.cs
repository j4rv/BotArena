using System;
using UnityEngine;

namespace BotArena
{
    internal class Cannon : IWeaponController
    {

        public override void Attack(float power) {
            GameObject res;
            SoundManager.Play(transform.position, "shoot", power/4);
            ParticleEffectFactory.Summon("ShootSmoke", 0.5f, projectileSpawner.position, Quaternion.identity);
            res = Instantiate(projectileObject, projectileSpawner.position, projectileSpawner.rotation);
            res.GetComponent<Rigidbody>().velocity = transform.forward * power * 5;
            res.GetComponent<IProjectileController>().SetWeapon(this);
        }

        public override int GetCooldown() {
            return 20;
        }

        public override float GetStaminaCost() {
            return 20;
        }
    }
}
