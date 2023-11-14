using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class CharacterPortraitInfo
{
    public int lineIndex;
    public int characterPortraitIndex;
}

public class CharacterPortraitManager : MonoBehaviour
{
    public Image characterPortraitImage;
    public Sprite[] characterPortraits;
    public List<CharacterPortraitInfo> portraitInfoList;

    // Use this function to set a character portrait
    public void SetCharacterPortrait(int currentLineIndex)
{
    int portraitIndex = -1;
    
    foreach (CharacterPortraitInfo portraitInfo in portraitInfoList)
    {
        if (portraitInfo.lineIndex == currentLineIndex)
        {
            portraitIndex = portraitInfo.characterPortraitIndex;
            break;
        }
    }

    if (characterPortraitImage != null && portraitIndex >= 0 && portraitIndex < characterPortraits.Length)
    {
        characterPortraitImage.sprite = characterPortraits[portraitIndex];
    }
    else
    {
        characterPortraitImage.sprite = null; // No portrait to display
    }
}
}