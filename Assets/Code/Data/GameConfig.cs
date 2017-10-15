using System.Collections.Generic;
using UnityEngine;

namespace BotArena {
    [System.Serializable]
    class GameConfig {

        static GameConfig _instance;

        public static GameConfig instance {
            get {
                if (_instance == null) {
                    _instance = DataLoader.LoadGameConfig();
                }
                return _instance;
            }
        }

        // Match properties
        public List<PlayerConfig> players = new List<PlayerConfig>();
        public int rounds = 10;

        // Game properties
        public float gameSpeed =                1;
        public int timestepsPerTurn =           2;
        public float lostTurnPenalty =          5;
        public int turnLimit =                  500;
        public float damagePerTurnAfterLimit =  0.5f;

        [System.Serializable]
        public class PlayerConfig
        {
            public string nickname;
            public string filename;
            public string robotToLoad = "(RANDOM)";
        }

        public string Serialize() {
            return JsonUtility.ToJson(this, true);
        }

    }
}