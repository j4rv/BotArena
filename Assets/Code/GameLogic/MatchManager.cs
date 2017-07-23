using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BotArena
{
    internal class MatchManager : MonoBehaviour
    {
        private static readonly string SCENE_TO_LOAD                =   "Match";
        private static readonly int    TURN_LIMIT                   =   500;
        private static readonly float  DAMAGE_PER_TURN_AFTER_LIMIT  =   0.5f;

        private static MatchManager instance = null;

        HashSet<PlayerMatchData> aliveBots;
        HashSet<PlayerMatchData> deadBots;
        public List<PlayerMatchData> robotsMatchData;
        public int round;
        public bool matchInProgress = false;

        public static MatchManager Instance() {
            if (instance == null) {
                instance = (MatchManager) FindObjectOfType(typeof(MatchManager));
                DontDestroyOnLoad(instance.gameObject);

                instance.robotsMatchData = new List<PlayerMatchData>();
                instance.aliveBots = new HashSet<PlayerMatchData>();
                instance.deadBots = new HashSet<PlayerMatchData>();

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

        void Awake() {
            NewRound();
            matchInProgress = true;
            //Debug.Log(instance.robotsMatchData.ToArray()[0].VictoriesCount() + " : " + instance.robotsMatchData.ToArray()[1].VictoriesCount());
        }

        void FixedUpdate() {
            if (Instance().robotsMatchData.Count > 0
                    && TurnManager.IsTurnUpdate()
                    && matchInProgress == true) {

                instance.aliveBots.Clear();
                foreach (PlayerMatchData playerMatchData in instance.robotsMatchData){
                    RobotTurnOperations(playerMatchData);
                }

                CheckMatchEnd();
            }
        }        

        static void RobotTurnOperations(PlayerMatchData playerMatchData) {
            RobotController robotController = playerMatchData.controller;

            if (robotController) {
                if (TurnManager.GetCurrentTurn() > TURN_LIMIT) {
                    robotController.TakeDamage(DAMAGE_PER_TURN_AFTER_LIMIT);
                }

                robotController.TurnUpdate();

                if (robotController.IsAlive())
                    instance.aliveBots.Add(playerMatchData);
                else
                    instance.deadBots.Add(playerMatchData);
            }
        }

        static void CheckMatchEnd() {
            if(instance.aliveBots.Count == 1) {
                //One robot lives, that robot wins!
                foreach (PlayerMatchData a in instance.aliveBots) { a.AddMatch(MatchResult.VICTORY); }
                foreach (PlayerMatchData d in instance.deadBots) { d.AddMatch(MatchResult.LOSS); }
                instance.matchInProgress = false;
                SceneManager.LoadScene(SCENE_TO_LOAD); //TODO: Make it so that it loads after X seconds (coroutine?)
            }
            if(instance.aliveBots.Count == 0) {
                //It's a tie!
                foreach (PlayerMatchData d in instance.deadBots) { d.AddMatch(MatchResult.DRAW); }
                instance.matchInProgress = false;
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