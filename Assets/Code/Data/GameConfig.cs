using System.Collections.Generic;
using UnityEngine;

namespace BotArena {
    [System.Serializable]
    internal class GameConfig {

        private static GameConfig instance;

        public static GameConfig Instance() {
            if (instance == null) {
                Debug.Log("Creating GameConfig instance");
                instance = DataLoader.LoadGameConfig();
            }

            return instance;
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