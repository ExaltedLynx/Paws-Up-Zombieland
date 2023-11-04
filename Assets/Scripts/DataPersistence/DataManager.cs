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
    private static GameDataHandler dataHandler;
    private static GameData currentSave;
    private static int currentSaveSlot;
    private bool isDeleting = false;

    public static DataManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dataHandler = new GameDataHandler(Application.persistentDataPath, fileName);
    }

    public void SaveGame()
    {
        HandleSaveData();
        //Debug.Log(currentSaveSlot + 1);
        dataHandler.Save(currentSave, currentSaveSlot + 1);
    }

    public void LoadGame(int saveSlot)
    {
        currentSaveSlot = saveSlot;
        if(HandleDelete()) { return; }

        currentSave = dataHandler.Load(currentSaveSlot + 1);
        if (currentSave == null)
        {
            Debug.Log("No save data found, initializing new save.");
            NewGame(currentSaveSlot);
        }
        else
        {
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
        currentSave = dataHandler.Load(saveSlot);

        HandleLoadData(currentSave);
        SceneController.ChangeLevel(currentSave.currentLevel);

    }

    private void NewGame(int saveSlot)
    {
        currentSaveSlot = saveSlot;
        currentSave = new GameData();
        SceneController.ChangeLevel(1);
    }

    //will need a more versatile implementation if saving/loading data that is not in a singleton
    private void HandleSaveData()
    {
        currentSave.unlockedLevels = GameManager.unlockedLevels;
        currentSave.currentLevel = GameManager.currentLevel;
    }

    private void HandleLoadData(GameData data)
    {
        GameManager.unlockedLevels = data.unlockedLevels;
    }
    
    private bool HandleDelete()
    {
        bool deleted = false;
        if(isDeleting)
            deleted = dataHandler.Delete(currentSaveSlot + 1);

        isDeleting = false;
        return deleted;
    }

    private int FindEmptySaveSlot()
    {
        string savesPath = Path.Combine(Application.persistentDataPath, fileName);
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
        DirectoryInfo savesDir = new DirectoryInfo(Application.persistentDataPath);
        FileInfo mostRecent = savesDir.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
        return (int) char.GetNumericValue(mostRecent.Name.Last());
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
