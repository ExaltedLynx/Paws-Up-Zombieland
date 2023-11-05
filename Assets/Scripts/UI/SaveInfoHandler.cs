using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveInfoHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] currentLevelText = new TextMeshProUGUI[5];
    [SerializeField] private TextMeshProUGUI[] lastPlayedText = new TextMeshProUGUI[5];

    void Start()
    {
        SetLastPlayedText();
        SetCurrentLevelText();
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
        }
    }

    private void SetCurrentLevelText()
    {
        for (int i = 0; i < lastPlayedText.Length; i++)
        {
            if (DataManager.SaveExists(i))
            {
                currentLevelText[i].SetText(currentLevelText[i].text + DataManager.Instance.GetSaves()[i].currentLevel);
            }
        }
            
    }

}
