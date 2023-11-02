using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private string fileName = "save";
    private GameDataHandler dataHandler;
    private GameData[] gameData = new GameData[5];
    private GameData currentSave;
    private int currentSaveSlot;
    public static DataManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        dataHandler = new GameDataHandler(Application.persistentDataPath, fileName);
    }

    public void SaveGame()
    {
        HandleSaveData();
        dataHandler.Save(currentSave, currentSaveSlot + 1);
    }

    //TODO use select save file scene to set save slot once its added
    public void LoadGame(int saveSlot)
    {
        currentSaveSlot = saveSlot;
        currentSave = dataHandler.Load(currentSaveSlot + 1);
        if (currentSave == null)
        {
            Debug.Log("No save data found, initializing new save.");
            NewGame(currentSaveSlot);
        }
        else
            HandleLoadData(currentSave);
    }

    private void NewGame(int saveSlot)
    {
        currentSaveSlot = saveSlot;
        gameData[currentSaveSlot] = new GameData();
        currentSave = gameData[currentSaveSlot];
    }

    //will need a more versatile implementation if saving/loading data that is not in a singleton
    private void HandleSaveData()
    {
        currentSave.unlockedLevels = GameManager.unlockedLevels;
        currentSave.saveDate = DateTime.Today;
        Debug.Log(currentSave.saveDate);
    }

    private void HandleLoadData(GameData data)
    {
        GameManager.unlockedLevels = data.unlockedLevels;
    }


    private void OnApplicationQuit()
    {
        SaveGame();
    }


}
