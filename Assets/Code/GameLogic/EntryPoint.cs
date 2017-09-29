using UnityEngine;
using UnityEngine.SceneManagement;

namespace BotArena
{
	public class EntryPoint : MonoBehaviour {
	
    	static readonly string SCENE_AFTER_LOADING = "Match";
	
		void Start(){
			DataManager.Init();
            SceneManager.LoadScene(SCENE_AFTER_LOADING);
		}
	}
}
