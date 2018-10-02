using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace BotArena { 
	public class MainScene : MonoBehaviour {

		public RobotSelector robotToEdit;

		public void LoadMatchConfigScene(){
			SceneManager.LoadScene(Constants.MATCH_CONFIG_SCENE);      
    }

		public void LoadNewRobot(){
			PlayerPrefs.SetString(null, robotToEdit.GetSelectedRobot());
			SceneManager.LoadScene(Constants.COMPILER_SCENE);      
    }

		public void LoadEditRobot(){
			PlayerPrefs.SetString(PlayerPrefKeys.SELECTED_ROBOT_TO_EDIT, robotToEdit.GetSelectedRobot());
			SceneManager.LoadScene(Constants.COMPILER_SCENE);      
    }

	}
}