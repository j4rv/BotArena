using UnityEngine;
using UnityEngine.SceneManagement;

namespace BotArena { 
	public class MainScene : MonoBehaviour {


		void Start () {
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void LoadMatchConfigScene(){
			SceneManager.LoadScene(Constants.MATCH_CONFIG_SCENE);      
    }

		public void LoadNewRobot(){
			SceneManager.LoadScene(Constants.COMPILER_SCENE);      
    }

	}
}