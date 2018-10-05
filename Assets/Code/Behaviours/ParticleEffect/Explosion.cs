using UnityEngine;

namespace BotArena {
    public class Explosion : TemporaryParticleSystem {
        
        public float shakeDuration = 1.0f;
        public float shakeAmount = 0.2f;
        public float decreaseFactor = 1.0f;

        Transform cameraToShake;
        Vector3 originalPos;

        void Awake() {
            mainParticleSystem = GetComponent<ParticleSystem>();
        }

        void Start() {
            // TODO add flash effect?
            SoundManager.Play(transform.position, "explosion", 1);
            cameraToShake = GameObject.Find("Main Camera").transform;
            originalPos = cameraToShake.localPosition;
            shakeDuration = transform.localScale.x;
        }

        new void Update() {
            base.Update();
            if (shakeDuration > 0) {
                cameraToShake.localPosition = originalPos + Random.insideUnitSphere * shakeAmount * shakeDuration;
                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
        }
    }
}