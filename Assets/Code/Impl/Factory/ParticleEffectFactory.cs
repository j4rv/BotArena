using System;
using UnityEngine;

namespace BotArena {
    public static class ParticleEffectFactory {

        public static GameObject Summon(String prefabName, float Scale, Vector3 position,
                                        Quaternion rotation) {
            GameObject effect = UnityEngine.Object.Instantiate(
                Resources.Load("Prefabs/ParticleSystems/"+prefabName), position,
                rotation) as GameObject;
            effect.transform.localScale *= Scale;
            return effect;
        }

    }
}
