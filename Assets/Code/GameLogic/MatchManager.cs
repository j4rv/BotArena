using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BotArena
{
    public class MatchManager : MonoBehaviour
    {
        private static List<RobotMatchData> robotsMatchData;
        private static MatchManager instance = null;
        private static bool matchEnded = false;

        private static MatchManager Get() {
            if (instance == null) {
                instance = (MatchManager) FindObjectOfType(typeof(MatchManager));
                DontDestroyOnLoad(instance.gameObject);
                InitRobots();
            }
            return instance;
        }

        void Awake() {
            Get();
            matchEnded = false;
            InstantiateRobots();
            Debug.Log(robotsMatchData.ToArray()[0].VictoriesCount() + " : " + robotsMatchData.ToArray()[1].VictoriesCount());
        }

        void FixedUpdate() {
            if (robotsMatchData.Count > 0
                    && TurnManager.IsTurnUpdate()
                    && matchEnded == false) {

                List<RobotMatchData> aliveBots = new List<RobotMatchData>();
                List<RobotMatchData> deadBots = new List<RobotMatchData>();

                foreach (RobotMatchData robotMatchData in robotsMatchData){
                    RobotController robotController = robotMatchData.controller;

                    if (robotController) { 
                        robotController.TurnUpdate();

                        if (robotController.IsAlive())
                            aliveBots.Add(robotMatchData);
                        else
                            deadBots.Add(robotMatchData);
                    }
                }

                CheckMatchEnd(aliveBots, deadBots);
            }
        }        

        void CheckMatchEnd(List<RobotMatchData> aliveBots, List<RobotMatchData> deadBots) {
            if(aliveBots.Count == 1) {
                //One robot lives, that robot wins!
                aliveBots.ForEach(x => x.AddMatch(1));
                deadBots.ForEach(x => x.AddMatch(-1));
                matchEnded = true;
                SceneManager.LoadScene("Match");
            }
            if(aliveBots.Count == 0) {
                //It's a tie!
                aliveBots.ForEach(x => x.AddMatch(0));
                deadBots.ForEach(x => x.AddMatch(0));
                matchEnded = true;
                SceneManager.LoadScene("Match");
            }
        }

        private static void InitRobots() {
            if(robotsMatchData == null) { 
                robotsMatchData = new List<RobotMatchData>();

                // hardcoded for now
                string robotLibrary1 = @".\Libraries\RandomAI.dll";
                string robotLibrary2 = @".\Libraries\RandomAI.dll";
                IRobot robot1 = DllUtil.LoadRobotFromDll(robotLibrary1);
                IRobot robot2 = DllUtil.LoadRobotFromDll(robotLibrary2);
                robotsMatchData.Add(new RobotMatchData(robot1, null));
                robotsMatchData.Add(new RobotMatchData(robot2, null));
            }
        }

        private static void InstantiateRobots() {
            foreach (RobotMatchData r in robotsMatchData) {
                Vector3 randomPosition = RandomUtil.RandomPositionInsideMap();
                Vector3 randomRotation = RandomUtil.RandomRotationHorizontal();
                RobotController robotController = RobotPrefabFactory.Create(r.robot, randomPosition, Quaternion.Euler(randomRotation));
                r.controller = robotController;
            }
        }
    }
}