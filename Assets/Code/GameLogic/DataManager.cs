using System;
using System.Collections.Generic;
using UnityEngine;

namespace BotArena { 
    class DataManager 
    {
        static readonly string ROBOTS_PATH = @"./Libraries/";

        static DataManager _instance;
        static DataManager instance {
            get {
                if (_instance == null) {
                    Init();
                }
                return _instance;
            }
        }

        readonly GameConfig gameConfig;
        readonly List<PlayerMatchData> robotsMatchData;

		/**
		 * Private constructor
		 */
        DataManager(List<PlayerMatchData> robotsMatchData){
			this.robotsMatchData = robotsMatchData;
			this.gameConfig = GameConfig.instance;
		}

		public static void Init(){
			if (_instance == null) {
				List<PlayerMatchData> matchData = new List<PlayerMatchData>();
				_instance = new DataManager(matchData);
				GameConfig config = _instance.gameConfig;

				Time.timeScale = config.gameSpeed;
				Debug.Log(JsonUtility.ToJson(config));
				Debug.Log("Setting time scale to: " + config.gameSpeed);
				
				//Instantiating players and their robots
				foreach (GameConfig.PlayerConfig playerConfig in config.robots) {
					string robotLibrary = ROBOTS_PATH + playerConfig.filename;
					IRobot robot = DllUtil.CreateRobotFromDll(robotLibrary, playerConfig.robotToLoad);
					PlayerMatchData playerMatchData = new PlayerMatchData(playerConfig.playerNickname, robot, null);
					matchData.Add(playerMatchData);

					if (robot != null) {
						Debug.Log("Added robot '" + robot.GetName() + "' from player '" + playerConfig.playerNickname + "'");
					} else {
						Debug.Log("Added null robot from player '" + playerConfig.playerNickname + "'. Did you provide a valid dll file path?");
					}
				}
            } else {
                throw new Exception("DataManager has an instance initialized already!");
            }
		}      
	
		public static List<PlayerMatchData> GetRobotsMatchData() {
			return instance.robotsMatchData;
		}
    }
}