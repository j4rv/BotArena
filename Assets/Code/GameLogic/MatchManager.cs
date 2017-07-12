using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BotArena
{
    internal class MatchManager : MonoBehaviour
    {
        private static readonly string SCENE_TO_LOAD = "Match";
        private static MatchManager instance = null;

        public List<PlayerMatchData> robotsMatchData;
        public int round;
        public bool matchInProgress = false;

        public static MatchManager Instance() {
            if (instance == null) {
                instance = (MatchManager) FindObjectOfType(typeof(MatchManager));
                DontDestroyOnLoad(instance.gameObject);

                instance.robotsMatchData = new List<PlayerMatchData>();

                // hardcoded for now
                string robotLibrary1 = @".\Libraries\Defaultbots.dll";
                string robotLibrary2 = @".\Libraries\Defaultbots.dll";
                IRobot robot1 = DllUtil.CreateRandomRobotFromDll(robotLibrary1);
                IRobot robot2 = DllUtil.CreateRandomRobotFromDll(robotLibrary2);
                instance.robotsMatchData.Add(new PlayerMatchData(robot1.GetName(), robot1, null));
                instance.robotsMatchData.Add(new PlayerMatchData(robot2.GetName(), robot2, null));
                instance.round = 0;
            }
            return instance;
        }

        void Start() {
            NewRound();
            matchInProgress = true;
            //Debug.Log(instance.robotsMatchData.ToArray()[0].VictoriesCount() + " : " + instance.robotsMatchData.ToArray()[1].VictoriesCount());
        }

        void FixedUpdate() {
            if (Instance().robotsMatchData.Count > 0
                    && TurnManager.IsTurnUpdate()
                    && matchInProgress == true) {

                List<PlayerMatchData> aliveBots = new List<PlayerMatchData>();
                List<PlayerMatchData> deadBots = new List<PlayerMatchData>();

                foreach (PlayerMatchData robotMatchData in instance.robotsMatchData){
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

        void CheckMatchEnd(List<PlayerMatchData> aliveBots, List<PlayerMatchData> deadBots) {
            if(aliveBots.Count == 1) {
                //One robot lives, that robot wins!
                aliveBots.ForEach(x => x.AddMatch(MatchResult.VICTORY));
                deadBots.ForEach(x => x.AddMatch(MatchResult.LOSS));
                matchInProgress = false;
                SceneManager.LoadScene(SCENE_TO_LOAD); //TODO: Make it so that it loads after X seconds (coroutine?)
            }
            if(aliveBots.Count == 0) {
                //It's a tie!
                deadBots.ForEach(x => x.AddMatch(MatchResult.DRAW));
                matchInProgress = false;
                SceneManager.LoadScene(SCENE_TO_LOAD);
            }
        }

        private static void NewRound() {
            InstantiateRobots();

            instance.round++;
            instance.matchInProgress = true;
        }

        private static void InstantiateRobots() {
            foreach (PlayerMatchData playerMatchData in Instance().robotsMatchData) {
                Vector3 randomPosition = RandomUtil.RandomPositionInsideMap();
                Vector3 randomRotation = RandomUtil.RandomRotationHorizontal();
                RobotController robotController = RobotPrefabFactory.Create(playerMatchData.robot, randomPosition, Quaternion.Euler(randomRotation));
                playerMatchData.controller = robotController;
            }
        }
    }
}