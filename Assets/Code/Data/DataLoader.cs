using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BotArena {
    internal static class DataLoader {

        private static readonly string CONFIG_PATH = @"./config.json";
        private static readonly string LOADED = "Loaded configuration file.";
        private static readonly string CREATED = "Created a default configuration file.";

        public static GameConfig LoadGameConfig() {
            if (File.Exists(CONFIG_PATH)) {
                Debug.Log(LOADED);
                StreamReader reader = new StreamReader(CONFIG_PATH);
                string jsonString = reader.ReadToEnd();
                reader.Close();

                return JsonUtility.FromJson<GameConfig>(jsonString);
            }
            else {
                GameConfig gameConfig = new GameConfig();
                GameConfig.PlayerConfig r1 = new GameConfig.PlayerConfig();
                GameConfig.PlayerConfig r2 = new GameConfig.PlayerConfig();

                r1.filename = "DefaultBots.dll";
                r1.robotToLoad = "(RANDOM)";
                r1.playerNickname = "Default bot 1";
                r2.filename = "DefaultBots.dll";
                r2.robotToLoad = "(RANDOM)";
                r2.playerNickname = "Default bot 2";
                gameConfig.robots = new List<GameConfig.PlayerConfig>();
                gameConfig.robots.Add(r1);
                gameConfig.robots.Add(r2);

                StreamWriter gameConfigFile = File.CreateText(CONFIG_PATH);
                gameConfigFile.WriteLine(JsonUtility.ToJson(gameConfig));
                gameConfigFile.Close();

                Debug.Log(CREATED);
                Debug.Log(JsonUtility.ToJson(gameConfig));

                return gameConfig;
            }
        }
    }
}
