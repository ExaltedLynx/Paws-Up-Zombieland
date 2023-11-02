using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class GameDataHandler
{
    private string dataDirectoryPath = "";
    private string dataFileName = "";
    private string currentSaveFileName = "";


    public GameDataHandler(string dataDirectoryPath, string dataFileName) 
    {
        this.dataDirectoryPath = dataDirectoryPath;
        this.dataFileName = dataFileName;
    }

    public void Save(GameData gameData, int saveSlot)
    {
        currentSaveFileName = dataFileName + saveSlot;
        //save file is at "Application.persistentDataPath(changes based on OS, for windows its AppData/LocalLow) + dataFileName + saveSlot" 
        string fullPath = Path.Combine(dataDirectoryPath, currentSaveFileName);
        try
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            File.WriteAllText(fullPath, JsonUtility.ToJson(gameData)); //writes the gameData to the file in JSON format
        }
        catch (Exception e)
        {
            Debug.LogError($"Error while trying to load game data from file: {fullPath} \n {e.Message} {e.StackTrace}");
        }
    }

    public GameData Load(int saveSlot)
    {
        currentSaveFileName = dataFileName + saveSlot;
        string fullPath = Path.Combine(dataDirectoryPath, currentSaveFileName);
        if (!File.Exists(fullPath))
        {
            Debug.Log($"File at {fullPath} does not exist, cannot load game data.");
            return null;
        }

        try
        {
            GameData loadedData = JsonUtility.FromJson<GameData>(File.ReadAllText(fullPath));
            return loadedData;
        }
        catch(Exception e)
        {
            Debug.LogError($"Error while trying to load game data from file: {fullPath} \n {e.Message} {e.StackTrace}");
            throw e;
        }
    }
}
