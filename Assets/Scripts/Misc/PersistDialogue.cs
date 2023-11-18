using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistDialogue : MonoBehaviour
{
    public static PersistDialogue Instance;

    public bool AlreadyReadDialogue = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
