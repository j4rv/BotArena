using System;
using UnityEngine;

namespace BotArena {
    public static class ParticleEffectFactory {

        public static GameObject Summon(String prefabName, float Strength, Vector3 position,
                                        Quaternion rotation) {
            GameObject effect = UnityEngine.Object.Instantiate(
                Resources.Load("Prefabs/ParticleSystems/"+prefabName), position,
                rotation) as GameObject;
            effect.transform.localScale *= Strength;
            return effect;
        }

    }
}
