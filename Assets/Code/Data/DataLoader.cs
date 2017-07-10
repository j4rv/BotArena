using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    private static readonly string CONFIG_PATH = @"./Libraries/config.json";
    private static readonly string LOADED_LOG = "Loaded configuration file.";
    private static readonly string NO_FILE_ERROR = "Error: No config file!";

    public GameData gameData
    {
        get { return gameData; }
        private set { gameData = value; }
    }

    public void LoadGameData()
    {
        if (File.Exists(CONFIG_PATH))
        {
            Debug.Log(LOADED_LOG);
            string jsonString = File.ReadAllText(CONFIG_PATH);
            GameData gameData = JsonUtility.FromJson<GameData>(jsonString);
        }
        else
        {
            Debug.LogError(NO_FILE_ERROR);
        }
    }
}
