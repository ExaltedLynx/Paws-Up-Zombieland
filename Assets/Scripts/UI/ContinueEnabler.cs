using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ContinueEnabler : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;

    void Awake()
    {
        if(SavesExists())
            continueButton.SetActive(true);
        else
            continueButton.SetActive(false);
    }

    private bool SavesExists()
    {
        DirectoryInfo savesDir = new DirectoryInfo(Application.persistentDataPath);
        int numSaves = savesDir.GetFiles().Length;
        if (numSaves > 0)
        {
            return true;
        }
        return false;
    }

    private void OnEnable()
    {
        if (SavesExists())
            continueButton.SetActive(true);
        else
            continueButton.SetActive(false);
    }
}
