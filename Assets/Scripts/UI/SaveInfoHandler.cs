using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SaveInfoHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] currentLevelText = new TextMeshProUGUI[5];
    [SerializeField] private TextMeshProUGUI[] lastPlayedText = new TextMeshProUGUI[5];
    [SerializeField] private TextMeshProUGUI[] starsObtainedText = new TextMeshProUGUI[5];

    public static SaveInfoHandler Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RefreshAllSaveInfo();
    }

    public void RefreshAllSaveInfo()
    {
        SetLastPlayedText();
        SetCurrentLevelText();
        SetStarsObtainedText();
    }

    private void SetLastPlayedText()
    {
        for(int i = 0; i < lastPlayedText.Length; i++)
        {
            if (DataManager.SaveExists(i))
            {
                DateTime lastPlayed = DataManager.Instance.GetSaveSlotDate(i);
                lastPlayedText[i].SetText(lastPlayedText[i].text + lastPlayed.ToString("M/d/yy"));
            }
            else
                lastPlayedText[i].SetText("Last Played: ");
        }
    }

    private void SetCurrentLevelText()
    {
        for (int i = 0; i < lastPlayedText.Length; i++)
        {
            if (DataManager.SaveExists(i))
            {
                currentLevelText[i].SetText("Level " + DataManager.Instance.GetSaves()[i].currentLevel);
            }
            else
                currentLevelText[i].SetText("Level ");
        }  
    }

    private void SetStarsObtainedText()
    {
        for (int i = 0; i < lastPlayedText.Length; i++)
        {
            if (DataManager.SaveExists(i))
            {
                int totalStars = 0;
                Array.ForEach(DataManager.Instance.GetSaves()[i].starsObtained, stars => totalStars += stars);
                starsObtainedText[i].SetText("Stars Obtained: " + totalStars);
            }
            else
                starsObtainedText[i].SetText("Stars Obtained: ");
        }
    }
}
