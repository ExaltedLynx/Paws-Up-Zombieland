using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSizer : MonoBehaviour
{
    private TextMeshProUGUI textComponent;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            // Set the font size based on the global font size
        textComponent.fontSize = GlobalSettings.dialogueFontSize; // Set initial font size
        }
        else
        {
            Debug.LogError("TextSizer script requires TextMeshProUGUI component.");
        }
    }
}
