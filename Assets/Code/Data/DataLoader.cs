using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BotArena {
    static class DataLoader {

        static readonly string CONFIG_PATH = @"./config.json";
        static readonly string LOADED = "Loaded configuration file.";
        static readonly string CREATED = "Created a default configuration file.";

        public static GameConfig LoadGameConfig() {
            if (File.Exists(CONFIG_PATH)) {
                StreamReader reader = new StreamReader(CONFIG_PATH);
                string jsonString = reader.ReadToEnd();
                reader.Close();

                Debug.Log(LOADED);
                return JsonUtility.FromJson<GameConfig>(jsonString);
            }
            else {
                GameConfig gameConfig = new GameConfig();
                GameConfig.PlayerConfig r1 = new GameConfig.PlayerConfig();
                GameConfig.PlayerConfig r2 = new GameConfig.PlayerConfig();

                r1.filename = "DefaultBots.dll";
                r1.robotToLoad = "(RANDOM)";
                r1.nickname = "Default bot 1";
                r2.filename = "DefaultBots.dll";
                r2.robotToLoad = "(RANDOM)";
                r2.nickname = "Default bot 2";
                gameConfig.players = new List<GameConfig.PlayerConfig>();
                gameConfig.players.Add(r1);
                gameConfig.players.Add(r2);

                StreamWriter gameConfigFile = File.CreateText(CONFIG_PATH);
                gameConfigFile.WriteLine(gameConfig.Serialize());
                gameConfigFile.Close();

                Debug.Log(CREATED);
                return gameConfig;
            }
        }
    }
}
