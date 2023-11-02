using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public DateTime saveDate = DateTime.Today; //TODO serializer for DateTime;
    public int unlockedLevels;
    
    public GameData()
    {
        unlockedLevels = 1;
    }
    
}
