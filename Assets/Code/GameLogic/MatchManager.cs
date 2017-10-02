using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BotArena
{
    class MatchManager : MonoBehaviour
    {
        static readonly string SCENE_TO_LOAD                =   "Match";
        static readonly int    TURN_LIMIT                   =   500;
        static readonly float  DAMAGE_PER_TURN_AFTER_LIMIT  =   0.5f;

        static MatchManager instance = null;

        HashSet<PlayerMatchData> aliveBots;
        HashSet<PlayerMatchData> deadBots;
        public int round;
        public bool matchInProgress = false;
        
        public static MatchManager Instance() {
            if (instance == null) {
                instance = (MatchManager) FindObjectOfType(typeof(MatchManager));
                DontDestroyOnLoad(instance.gameObject);

                instance.aliveBots = new HashSet<PlayerMatchData>();
                instance.deadBots = new HashSet<PlayerMatchData>();
                
                instance.round = 0;
            }
            return instance;
        }

        void Awake() {
            if(Instance().round < GameConfig.Instance().rounds) {
                NewRound();
                matchInProgress = true;
            } else {
                Debug.Log("Game ended!");
            }
        }

        void FixedUpdate() {
			if (DataManager.GetRobotsMatchData().Count > 0
                    && TurnManager.IsTurnUpdate()
                    && matchInProgress == true) {

                Instance().aliveBots.Clear();
                foreach (PlayerMatchData playerMatchData in DataManager.GetRobotsMatchData()){
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
                    Instance().aliveBots.Add(playerMatchData);
                else
                    Instance().deadBots.Add(playerMatchData);
            }
        }

        static void CheckMatchEnd() {
            if(Instance().aliveBots.Count == 1) {
                //One robot lives, that robot wins!
                foreach (PlayerMatchData a in instance.aliveBots) { a.AddMatch(MatchResult.VICTORY); }
                foreach (PlayerMatchData d in instance.deadBots) { d.AddMatch(MatchResult.LOSS); }
                instance.matchInProgress = false;
                SceneManager.LoadScene(SCENE_TO_LOAD); //TODO: Make it so that it loads after X seconds (coroutine?)
            }
            if(Instance().aliveBots.Count == 0) {
                //It's a tie!
                foreach (PlayerMatchData d in instance.deadBots) { d.AddMatch(MatchResult.DRAW); }
                instance.matchInProgress = false;
                SceneManager.LoadScene(SCENE_TO_LOAD);
            }
        }

        static void NewRound() {
            InstantiateRobots();

            Instance().round++;
            Instance().matchInProgress = true;
        }

        static void InstantiateRobots() {
            foreach (PlayerMatchData playerMatchData in DataManager.GetRobotsMatchData()) {
                Vector3 randomPosition = RandomUtil.RandomPositionInsideMap();
                Vector3 randomRotation = RandomUtil.RandomRotationHorizontal();
                RobotController robotController = RobotPrefabFactory.Create(playerMatchData.robot, randomPosition, Quaternion.Euler(randomRotation));
                playerMatchData.controller = robotController;
            }
        }
    }
}