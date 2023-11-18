using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
 public Slider fontSizeSlider;

    void Start()
    {
        if (fontSizeSlider != null)
        {
        fontSizeSlider.minValue = 28f;
        fontSizeSlider.maxValue = 52f;
        fontSizeSlider.onValueChanged.AddListener(OnFontSizeChanged);
        }
    }

    void OnFontSizeChanged(float value)
    {
         GlobalSettings.dialogueFontSize = Mathf.Clamp(value, fontSizeSlider.minValue, fontSizeSlider.maxValue);
        Debug.Log("New Font Size: " + GlobalSettings.dialogueFontSize);
        // You can update the font size of the existing dialogue objects in other scenes here
    }
}
