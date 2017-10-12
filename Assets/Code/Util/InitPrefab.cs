using UnityEngine;

public class InitPrefab : MonoBehaviour {

    public GameObject prefab;

	void Awake () {
        // Instantiates the prefab, sets its parent to this gameObject's parent 
        // and this dummy gameObject kills itself
        Transform instantiatedPrefab = Instantiate(prefab, transform.position, transform.rotation).transform;
        instantiatedPrefab.parent = transform.parent;
        Destroy(gameObject);
	}

}