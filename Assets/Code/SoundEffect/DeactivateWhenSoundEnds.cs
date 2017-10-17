
using UnityEngine;

public class DeactivateWhenSoundEnds : MonoBehaviour {

    [SerializeField]
    AudioSource source;

    void Start() {
        source.Play();
    }

	void Update () {
        if (source.isPlaying == false){
            gameObject.SetActive(false);
        }
	}

}
