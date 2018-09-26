using System.Collections.Generic;
using UnityEngine;

namespace BotArena {
    public class SoundManager : MonoBehaviour{

        [SerializeField]
        GameObject audioSourcePrefab;

        static LinkedList<AudioClip> soundEffects;
        static Pool audioPool;

        void Awake(){
            audioPool = new Pool(audioSourcePrefab);
            soundEffects = new LinkedList<AudioClip>();
            foreach (AudioClip audioclip in
                     Resources.LoadAll("Sounds", typeof(AudioClip))) {
                soundEffects.AddLast(audioclip);
            }
        }

        public static void Play(string id, float volume) {
            Vector3 pos = Vector3.zero;
            Play(pos, id, volume, 0f);
        }

        public static void Play(Vector3 position, string id, float volume){
            Play(position, id, volume, 0.8f);
        }

        public static void Play(Vector3 position, string id, float volume, float spatialBlend) {
        	AudioClip clip = PickRandomClip(id);
        	if (clip != null) {
        		AudioSource source = audioPool.Get().GetComponent<AudioSource>();
        		source.transform.position = position;
        		source.clip = clip;
        		source.volume = volume;
                source.spatialBlend = spatialBlend;
        		source.pitch = Random.Range(0.8f, 1.2f);
        		source.gameObject.SetActive(true);
        	} else {
                Debug.LogWarning("Could not find audio clip with id: " + id);
            }
        }

        // TODO this can be improved
        static List<AudioClip> FindWithId(string id){
            List<AudioClip> res = new List<AudioClip>();
            foreach(AudioClip ac in soundEffects){
                if (ac.name.Contains(id)){
                    res.Add(ac);
                }
            }
            return res;
        }

        static AudioClip PickRandomClip(string id){
            List<AudioClip> audios = FindWithId(id);
            if (audios == null || audios.Count == 0){
                return null;
            }
            
            int random = Random.Range(0, audios.Count);
            return audios[random];
        }
    }
}
