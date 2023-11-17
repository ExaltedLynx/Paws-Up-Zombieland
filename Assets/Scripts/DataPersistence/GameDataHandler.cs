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
        //save file is at "Application.persistentDataPath(changes based on OS, for windows its AppData/LocalLow)/Saves/dataFileName + saveSlot" 
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
            return null; //returns null when no save file for that slot
        }

        try
        {
            GameData loadedData = JsonUtility.FromJson<GameData>(File.ReadAllText(fullPath));
            //Debug.Log(loadedData.saveDate.dateTime + ", " +loadedData.unlockedLevels);
            return loadedData;
        }
        catch(Exception e)
        {
            Debug.LogError($"Error while trying to load game data from file: {fullPath} \n {e.Message} {e.StackTrace}");
            throw e;
        }
    }

    public bool Delete(int saveSlot)
    {
        currentSaveFileName = dataFileName + saveSlot;
        string fullPath = Path.Combine(dataDirectoryPath, currentSaveFileName);
        if(File.Exists(fullPath))
        {
            File.Delete(fullPath);
            Debug.Log("deleted save " + saveSlot);
            return true;
        }
        return false;
    }
}
