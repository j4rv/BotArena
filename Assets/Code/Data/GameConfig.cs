using System.Collections.Generic;
using UnityEngine;

namespace BotArena {
    [System.Serializable]
    class GameConfig {

        static GameConfig _instance;

        public static GameConfig instance {
            get {
                if (_instance == null) {
                    Debug.Log("Creating GameConfig instance");
                    _instance = DataLoader.LoadGameConfig();
                }
                return _instance;
            }
        }

        public List<PlayerConfig> robots = new List<PlayerConfig>();
        public int rounds = 10;
        public float gameSpeed = 1;

        [System.Serializable]
        public class PlayerConfig
        {
            public string filename;
            public string playerNickname;
            public string robotToLoad = "(RANDOM)";
        }

        public string ToJson() {
            return JsonUtility.ToJson(this);
        }

    }
}