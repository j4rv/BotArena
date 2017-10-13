using System.Collections.Generic;
using UnityEngine;

namespace BotArena
{
    class MatchManager : MonoBehaviour {
        static readonly int TURN_LIMIT = 500;                      //TODO Make it configurable
        static readonly float DAMAGE_PER_TURN_AFTER_LIMIT = 0.5f;  //TODO Make it configurable
        static readonly float SECONDS_BEFORE_NEW_ROUND = 2;        //TODO Make it configurable

        HashSet<PlayerMatchData> aliveBotsLastTurn;
        HashSet<PlayerMatchData> aliveBots;
        HashSet<PlayerMatchData> deadBots;
        public int round;
        public bool matchInProgress;

        public void Init() {
            aliveBotsLastTurn = new HashSet<PlayerMatchData>();
            aliveBots = new HashSet<PlayerMatchData>();
            deadBots = new HashSet<PlayerMatchData>();
            round = 0;
        }

        void Awake() {
            Init();
            NewRound();
        }

        void FixedUpdate() {
			if (DataManager.GetRobotsMatchData().Count > 0
                    && TurnManager.IsTurnUpdate()
                    && matchInProgress == true) {
                PreRobotTurns();
                RobotTurns();
                PostRobotTurns();
            }
        }        

        void PreRobotTurns() {
        	aliveBotsLastTurn.Clear();
        	aliveBotsLastTurn.UnionWith(aliveBots);
            aliveBots.Clear();
        }

        void RobotTurns() {
        	foreach (PlayerMatchData playerMatchData in DataManager.GetRobotsMatchData()){
                RobotController robotController = playerMatchData.controller;

                if (robotController) {
                    if (TurnManager.GetCurrentTurn() > TURN_LIMIT) {
                        robotController.TakeDamage(DAMAGE_PER_TURN_AFTER_LIMIT);
                    }

                    robotController.TurnUpdate();

                    if (robotController.IsAlive()) {
                        aliveBots.Add(playerMatchData);
                    } else {
                        deadBots.Add(playerMatchData);
                    }
                }
            }
        }

        void PostRobotTurns() {
        	CheckMatchEnd();
        }

        void CheckMatchEnd() {
            if(aliveBots.Count == 1) {
                //One robot lives, that robot wins!
                foreach (PlayerMatchData a in aliveBots) { a.AddMatch(MatchResult.VICTORY); }
                foreach (PlayerMatchData d in deadBots) { d.AddMatch(MatchResult.LOSS); }
                matchInProgress = false;
                MatchEnd();
            }
            if(aliveBots.Count == 0) {
                //It's a tie!
                foreach (PlayerMatchData d in aliveBotsLastTurn) { d.AddMatch(MatchResult.DRAW); }
                MatchEnd();
            }
        }

        void MatchEnd() {
            matchInProgress = false;
        	Invoke("NewRound", SECONDS_BEFORE_NEW_ROUND);
        }

        void NewRound() {
            if(round < GameConfig.instance.rounds) {
                DestroyAliveBots();
                InstantiateRobots();
                TurnManager.ResetTurns();
                matchInProgress = true;
                round++;
                Debug.Log(string.Format("Round {0} started!", round));
            } else {
                Debug.Log("Game ended!");
            }
        }

        void DestroyAliveBots(){
            foreach (PlayerMatchData playerMatchData in aliveBots){
                Destroy(playerMatchData.controller.gameObject);
            }
        }

        void InstantiateRobots() {
            foreach (PlayerMatchData playerMatchData in DataManager.GetRobotsMatchData()) {
                Vector3 randomPosition = RandomUtil.RandomPositionInsideMap();
                Vector3 randomRotation = RandomUtil.RandomRotationHorizontal();
                RobotController robotController = RobotPrefabFactory.Create(playerMatchData.robot, randomPosition, Quaternion.Euler(randomRotation));
                playerMatchData.controller = robotController;
            }
        }
    }
}