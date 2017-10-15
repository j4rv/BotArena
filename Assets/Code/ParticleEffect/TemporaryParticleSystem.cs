using UnityEngine;

namespace BotArena {
    public class TemporaryParticleSystem : MonoBehaviour {

        public ParticleSystem mainParticleSystem;
        public bool randomSeedOnAwake;
        public bool destroyIfOrphanAndZeroParticles;

        void Awake(){
            if(mainParticleSystem == null){
                mainParticleSystem = GetComponent<ParticleSystem>();
            }
            if(randomSeedOnAwake){
                // Workaround for bug: 782232 (Looks like it's fixed in 5.4.1+)
                mainParticleSystem.Stop();
                mainParticleSystem.randomSeed = RandomUtil.RandomSeed();
                mainParticleSystem.Play();
            }
        }

        protected void Update() {
            if (mainParticleSystem.IsAlive() == false) {
                Destroy(gameObject);
            }
            DestroyInactiveOrphan();
        }

        void DestroyInactiveOrphan(){
            if (destroyIfOrphanAndZeroParticles &&
                mainParticleSystem.transform.parent == null &&
                mainParticleSystem.particleCount == 0){
                Destroy(gameObject);
            }
        }

    }
}
