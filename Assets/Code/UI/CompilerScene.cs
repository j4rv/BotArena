using UnityEngine;
using UnityEngine.SceneManagement;

namespace BotArena { 
	public class CompilerScene : MonoBehaviour {

		public void GoBack(){
			SceneManager.LoadScene(Constants.MAIN_SCENE);      
    }

	}
}