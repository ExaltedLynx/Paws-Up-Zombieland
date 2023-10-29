using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int unlockedLevels;
    DateTime saveDate;

    public GameData() 
    {
        saveDate = DateTime.Now;
        unlockedLevels = 1;
    }
    
}
