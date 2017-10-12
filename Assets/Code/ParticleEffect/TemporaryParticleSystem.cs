using UnityEngine;

namespace BotArena {
    public class TemporaryParticleSystem : MonoBehaviour {

        public ParticleSystem mainParticleSystem;
        public bool destroyIfOrphanAndZeroParticles;

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
