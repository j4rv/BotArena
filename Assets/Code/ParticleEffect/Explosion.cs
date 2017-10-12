using UnityEngine;
namespace BotArena {
    public class Explosion : TemporaryParticleSystem {

        public Light flash;

        public float shakeDuration = 1.5f;
        public float shakeAmount = 0.1f;
        public float decreaseFactor = 1.0f;

        Transform cameraToShake;
        Vector3 originalPos;

        void Awake() {
            mainParticleSystem = GetComponent<ParticleSystem>();
        }

        void OnEnable() {
            // TODO add flash effect?
            cameraToShake = GameObject.Find("Main Camera").transform;
            originalPos = cameraToShake.localPosition;
            shakeDuration = transform.localScale.x;
        }

        new void Update() {
            base.Update();
            if (shakeDuration > 0) {
                cameraToShake.localPosition = originalPos + Random.insideUnitSphere * shakeAmount * shakeDuration;

                flash.intensity -= Time.deltaTime * 10;
                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
        }
    }
}