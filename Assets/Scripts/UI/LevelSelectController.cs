using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons = new Button[5];

    void Start()
    {
        EnableUnlockedLevels();
    }
    
    private void EnableUnlockedLevels()
    {
        for(int i = 0; i < GameManager.unlockedLevels; i++)
        {
            levelButtons[i].interactable = true;
        }
    }
}
