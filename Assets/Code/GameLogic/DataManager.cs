using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BotArena { 
    internal class DataManager : MonoBehaviour
    {
        private static readonly string ROBOTS_PATH = @"./Libraries/";
        private static readonly string LOAD_SCENE = "LoadScene";
        private static readonly string SCENE_AFTER_LOADING = "Match";

        private GameConfig gameConfig = GameConfig.Instance();
        public static List<PlayerMatchData> robotsMatchData;

        void Start () {
            if (SceneManager.GetActiveScene().name == LOAD_SCENE) { 
                DataManager dataManager = (DataManager) FindObjectOfType(typeof(DataManager));
                DontDestroyOnLoad(dataManager.gameObject);

                robotsMatchData = new List<PlayerMatchData>();
                Time.timeScale = gameConfig.gameSpeed;
                Debug.Log(JsonUtility.ToJson(gameConfig));
                Debug.Log("Setting time scale to: " + gameConfig.gameSpeed);
                foreach (GameConfig.PlayerConfig playerConfig in gameConfig.robots) {
                    string robotLibrary = ROBOTS_PATH + playerConfig.filename;
                    IRobot robot = DllUtil.CreateRobotFromDll(robotLibrary, playerConfig.robotToLoad);
                    PlayerMatchData playerMatchData = new PlayerMatchData(playerConfig.playerNickname, robot, null);
                    robotsMatchData.Add(playerMatchData);

                    if (robot != null)
                        Debug.Log("Added robot '" + robot.GetName() + "' from player '" + playerConfig.playerNickname + "'");
                    else
                        Debug.Log("Added null robot from player '" + playerConfig.playerNickname + "'. Did you provide a valid dll file path?");
                }

                SceneManager.LoadScene(SCENE_AFTER_LOADING);
            }
        }       
	
    }
}