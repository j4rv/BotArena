using UnityEngine;
namespace BotArena {
    public class Explosion : TemporaryParticleSystem {

        public Light flash;

        public float shakeDuration = 1.5f;
        public float shakeAmount = 0.1f;
        public float decreaseFactor = 1.0f;

        Transform cameraToShake;
        Vector3 originalPos;

        public static GameObject Instantiate(float Strength, Vector3 position, Quaternion rotation) {
            GameObject explosion = Instantiate(Resources.Load("Prefabs/ParticleSystems/Explosion"), position, rotation) as GameObject;
            explosion.transform.localScale *= Strength;
            explosion.GetComponent<Explosion>().shakeDuration *= Strength;
            return explosion;
        }

        void Awake() {
            mainParticleSystem = GetComponent<ParticleSystem>();
        }

        void OnEnable() {
            // TODO add flash effect?
            cameraToShake = GameObject.Find("Main Camera").transform;
            originalPos = cameraToShake.localPosition;
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