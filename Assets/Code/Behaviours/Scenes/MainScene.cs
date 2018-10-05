using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

namespace BotArena { 
	public class MainScene : MonoBehaviour {

		public Button editButton;
		public RobotSelector robotToEdit;

		void Start(){
			if(robotToEdit.HasNoOptions()){
				editButton.interactable = false;
			}
		}

		public void LoadMatchConfigScene(){
			SceneManager.LoadScene(Constants.MATCH_CONFIG_SCENE);
    }

		public void LoadNewRobot(){
			PlayerPrefs.DeleteKey(PlayerPrefKeys.SELECTED_ROBOT_TO_EDIT);
			SceneManager.LoadScene(Constants.COMPILER_SCENE);      
    }

		public void LoadEditRobot(){
			PlayerPrefs.SetString(PlayerPrefKeys.SELECTED_ROBOT_TO_EDIT, robotToEdit.GetSelectedRobot());
			SceneManager.LoadScene(Constants.COMPILER_SCENE);      
    }

	}
}