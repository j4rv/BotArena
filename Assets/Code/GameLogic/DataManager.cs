using System;
using System.Collections.Generic;
using UnityEngine;

namespace BotArena { 
    class DataManager 
    {
        
        public static readonly string ROBOTS_PATH = @"./Bots/";

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
				Debug.Log("Setting time scale to: " + config.gameSpeed);
				
				//Instantiating players and their robots
				foreach (GameConfig.PlayerConfig playerConfig in config.players) {
					IRobot robot = RobotFactory.Create(playerConfig.filename);
					PlayerMatchData playerMatchData = new PlayerMatchData(playerConfig.nickname, robot, null);
					matchData.Add(playerMatchData);

					if (robot != null) {
                        robot.player = playerConfig.nickname;
						Debug.Log("Added robot '" + robot.name + "' from player '" + playerConfig.nickname + "'");
					} else {
                        Debug.LogError("Added null robot from player '" + playerConfig.nickname + "'. Did you provide a valid dll file path?");
					}
				}
            } else {
                Debug.LogError("Tried to create a second DataManager!");
            }
		}      
	
		public static List<PlayerMatchData> GetRobotsMatchData() {
			return instance.robotsMatchData;
		}
    }
}