using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BotArena
{
    internal class MatchManager : MonoBehaviour
    {
        private static readonly string SCENE_TO_LOAD = "Match";
        private static MatchManager instance = null;
        private static bool matchEnded = false;

        public List<PlayerMatchData> robotsMatchData;
        public int round;

        public static MatchManager Instance() {
            if (instance == null) {
                instance = (MatchManager) FindObjectOfType(typeof(MatchManager));
                DontDestroyOnLoad(instance.gameObject);
                instance.round = 0;
            }
            return instance;
        }

        void Awake() {
            NewRound();
            InstantiateRobots();
            Debug.Log(robotsMatchData.ToArray()[0].VictoriesCount() + " : " + robotsMatchData.ToArray()[1].VictoriesCount());
        }

        void FixedUpdate() {
            if (robotsMatchData.Count > 0
                    && TurnManager.IsTurnUpdate()
                    && matchEnded == false) {

                List<PlayerMatchData> aliveBots = new List<PlayerMatchData>();
                List<PlayerMatchData> deadBots = new List<PlayerMatchData>();

                foreach (PlayerMatchData robotMatchData in robotsMatchData){
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
                matchEnded = true;
                SceneManager.LoadScene(SCENE_TO_LOAD); //TODO: Make it so that it loads after X seconds (coroutine?)
            }
            if(aliveBots.Count == 0) {
                //It's a tie!
                deadBots.ForEach(x => x.AddMatch(MatchResult.DRAW));
                matchEnded = true;
                SceneManager.LoadScene(SCENE_TO_LOAD);
            }
        }

        private static void NewRound() {
            if(Instance().robotsMatchData == null) {
                Instance().robotsMatchData = new List<PlayerMatchData>();

                // hardcoded for now
                string robotLibrary1 = @".\Libraries\RandomAI.dll";
                string robotLibrary2 = @".\Libraries\RandomAI.dll";
                IRobot robot1 = DllUtil.LoadRobotFromDll(robotLibrary1);
                IRobot robot2 = DllUtil.LoadRobotFromDll(robotLibrary2);
                Instance().robotsMatchData.Add(new PlayerMatchData(robot1.GetName(), robot1, null));
                Instance().robotsMatchData.Add(new PlayerMatchData(robot2.GetName(), robot2, null));
            }

            matchEnded = false;
            instance.round++;
        }

        private static void InstantiateRobots() {
            foreach (PlayerMatchData r in Instance().robotsMatchData) {
                Vector3 randomPosition = RandomUtil.RandomPositionInsideMap();
                Vector3 randomRotation = RandomUtil.RandomRotationHorizontal();
                RobotController robotController = RobotPrefabFactory.Create(r.robot, randomPosition, Quaternion.Euler(randomRotation));
                r.controller = robotController;
            }
        }
    }
}