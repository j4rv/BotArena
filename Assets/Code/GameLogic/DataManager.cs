using System.Collections.Generic;
using UnityEngine;

namespace BotArena { 
    class DataManager 
    {
        static readonly string ROBOTS_PATH = @"./Libraries/";
        static readonly string LOAD_SCENE = "LoadScene";

		static DataManager instance;

		GameConfig gameConfig;
        List<PlayerMatchData> robotsMatchData;

		/**
		 * Private constructor
		 */
        DataManager(List<PlayerMatchData> robotsMatchData){
			this.robotsMatchData = robotsMatchData;
			this.gameConfig = GameConfig.Instance();
		}

		public static void Init(){
			if (instance == null) {
				List<PlayerMatchData> robotsMatchData = new List<PlayerMatchData>();
				instance = new DataManager(robotsMatchData);
				GameConfig gameConfig = instance.gameConfig;

				Time.timeScale = gameConfig.gameSpeed;
				Debug.Log(JsonUtility.ToJson(gameConfig));
				Debug.Log("Setting time scale to: " + gameConfig.gameSpeed);
				
				//Instantiating players and their robots
				foreach (GameConfig.PlayerConfig playerConfig in gameConfig.robots) {
					string robotLibrary = ROBOTS_PATH + playerConfig.filename;
					IRobot robot = DllUtil.CreateRobotFromDll(robotLibrary, playerConfig.robotToLoad);
					PlayerMatchData playerMatchData = new PlayerMatchData(playerConfig.playerNickname, robot, null);
					robotsMatchData.Add(playerMatchData);

					if (robot != null) {
						Debug.Log("Added robot '" + robot.GetName() + "' from player '" + playerConfig.playerNickname + "'");
					} else {
						Debug.Log("Added null robot from player '" + playerConfig.playerNickname + "'. Did you provide a valid dll file path?");
					}
				}
			}
		}

        static DataManager GetInstance () {
            if (instance == null) {
					Init();
			}
			return instance;
        }       
	
		public static List<PlayerMatchData> GetRobotsMatchData() {
			return GetInstance().robotsMatchData;
		}
    }
}