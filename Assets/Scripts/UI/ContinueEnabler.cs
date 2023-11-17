using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ContinueEnabler : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;

    void Start()
    {
        if(SavesExists())
            continueButton.SetActive(true);
        else
            continueButton.SetActive(false);
    }

    private bool SavesExists()
    {
        if (!Directory.Exists(Application.persistentDataPath + "\\Saves"))
            Directory.CreateDirectory(Application.persistentDataPath + "\\Saves");

        DirectoryInfo savesDir = new DirectoryInfo(Application.persistentDataPath + "\\Saves");
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
