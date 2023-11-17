using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static MainMenuRefs;

public class DataManager : MonoBehaviour
{
    private static string fileName = "save";
    private static string savePath;
    private static GameDataHandler dataHandler;
    private static GameData[] allSaves = new GameData[5];
    private static GameData currentSave;
    private static int currentSaveSlot = -1;
    private bool isDeleting = false;

    public static DataManager Instance { get; private set; }

    private void Awake()
    {
        savePath = Application.persistentDataPath + "\\Saves";
        if(!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);

        dataHandler = new GameDataHandler(savePath, fileName);
        Instance = this;
    }

    public void SaveGame()
    {
        if(currentSaveSlot != -1)
        {
            HandleSaveData();
            dataHandler.Save(currentSave, currentSaveSlot + 1);
        }
    }

    public void LoadGame(int saveSlot)
    {
        currentSaveSlot = saveSlot;
        if(HandleDelete()) { return; }

        if (allSaves[currentSaveSlot] == null)
        {
            Debug.Log("No save data found, initializing new save.");
            NewGame(currentSaveSlot);
        }
        else
        {
            currentSave = allSaves[currentSaveSlot];
            HandleLoadData(currentSave);
            SwitchMenus(LevelSelectMenu, LoadGameMenu);
        }

    }

    //will immediately go to level 1's scene as long as there is an available save slot, will enable the load game menu if there isn't
    public void HandleNewGame()
    {
        int emptySlot = FindEmptySaveSlot();
        if(emptySlot == -1)
        {
            SwitchMenus(LoadGameMenu, MainMenu);
            return;
        }
        //Debug.Log(emptySlot);
        NewGame(emptySlot);
    }
    
    public void ContinueGame()
    {
        int saveSlot = GetMostRecentSave();
        //Debug.Log(saveSlot);
        currentSaveSlot = saveSlot - 1;
        currentSave = allSaves[currentSaveSlot];

        HandleLoadData(currentSave);
        SceneController.ChangeLevel(currentSave.currentLevel);

    }

    private void NewGame(int saveSlot)
    {
        currentSaveSlot = saveSlot;
        currentSave = new GameData();
        allSaves[currentSaveSlot] = currentSave;
        SceneController.ChangeLevel(1);
    }

    //will need a more versatile implementation if saving/loading data that is not in a singleton
    private void HandleSaveData()
    {
        currentSave.unlockedLevels = GameManager.unlockedLevels;
        currentSave.starsObtained = GameManager.starsObtained;

        //these don't need to be loaded
        currentSave.currentLevel = GameManager.currentLevel;
        currentSave.saveDate.dateTime = DateTime.Today;
    }

    private void HandleLoadData(GameData data)
    {
        GameManager.unlockedLevels = data.unlockedLevels;
        GameManager.starsObtained = data.starsObtained;
    }
    
    private bool HandleDelete()
    {
        bool deleted = false;
        if (isDeleting)
        {
            deleted = dataHandler.Delete(currentSaveSlot + 1);
            allSaves[currentSaveSlot] = null;
            currentSaveSlot = -1;
            SaveInfoHandler.Instance.RefreshAllSaveInfo();
        }
        isDeleting = false;
        return deleted;
    }

    private int FindEmptySaveSlot()
    {
        string savesPath = Path.Combine(savePath, fileName);
        for (int i = 0; i < 5; i++)
        {
            string savePath  = savesPath + (i + 1);
            //Debug.Log(savePath);
            if(!File.Exists(savePath))
            {
                return i;
            }
        }
        return -1;
    }

    private int GetMostRecentSave()
    {
        DirectoryInfo savesDir = new DirectoryInfo(savePath);
        FileInfo mostRecent = savesDir.GetFiles().OrderByDescending(f => f.LastWriteTime).First(); //finds the save file that was written to the most recently
        return (int) char.GetNumericValue(mostRecent.Name.Last());
    }

    //runs when menu scene loads to be able to display game data on load game menu
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void LoadAllSaves()
    {
        for(int i = 0; i < allSaves.Length; i++)
            allSaves[i] = dataHandler.Load(i + 1);
    }

    public DateTime GetSaveSlotDate(int saveSlot)
    {
        return allSaves[saveSlot].saveDate.dateTime;
    }

    public static bool SaveExists(int saveSlot)
    {
        if(allSaves[saveSlot] != null)
        {
            return true;
        }
        return false;     
    }

    public GameData[] GetSaves()
    {
        return allSaves;
    }

    public void SetDeleting()
    {
        isDeleting = true;
    }

    private void OnApplicationQuit()
    {
        //Debug.Log(currentSave);
        if (currentSave != null)
            SaveGame();
    }
}
