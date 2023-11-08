using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreditsTextHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveInfoText;
    void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        waveInfoText.SetText("Credits: " + GameManager.Instance.PlacementPoints);
    }
}
