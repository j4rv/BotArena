using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BotArena {
    static class DataManager 
    {
        
        public static readonly string ROBOTS_PATH = @"./Bots/";

        static GameConfig gameConfig = GameConfig.instance;
        static List<PlayerMatchData> robotsMatchData;

		public static void SetRounds(int rounds){
			gameConfig.rounds = rounds;
		}
		
		public static void SetRobots(List<string> robotNames){
			robotsMatchData = new List<PlayerMatchData>();
			var compiler = new Compiler<IRobot>();
			foreach(string robotName in robotNames){
				var robot = compiler.CompileAndCreateFromFilename(robotName);
				robotsMatchData.Add(new PlayerMatchData(robot.name, robot, null));
			}			
		}
	
		public static List<PlayerMatchData> GetRobotsMatchData() {
			return robotsMatchData;
		}
    }
}