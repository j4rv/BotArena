using UnityEngine;

namespace BotArena {
    [ExecuteInEditMode]
    class InitPrefab : MonoBehaviour {

        public GameObject prefab;
        public Vector3 scale = new Vector3(1, 1, 1);
        public bool rescaleToParentSize = true; // overrides manual scale.

        void Start() {
            // Instantiates the prefab, sets its parent to this gameObject's parent 
            // and this dummy gameObject kills itself
            Transform instantiatedPrefab = Instantiate(prefab, transform.position, transform.rotation).transform;
            instantiatedPrefab.parent = transform.parent;
            Rescale(instantiatedPrefab);
            Destroy(gameObject);
        }

        void Rescale(Transform toRescale) {
            if (rescaleToParentSize) {
                toRescale.transform.localScale = transform.root.localScale;
            } else {
                toRescale.transform.localScale = scale;
            }
        }

    }
}