using System;
using UnityEngine;

namespace BotArena
{
    class CannonBall : IProjectileController
    {

        protected override float GetDamage() {
			return 6f;
        }

        protected override void OnHit() {
        	//TODO: Instantiate some kind of collision effect
            ParticleEffectFactory.Summon("Explosion", 1.5f, transform.position, Quaternion.identity);
            ChildUtil.SeparateParticleSystems(gameObject);
            Destroy(gameObject);
        }
    }
}
