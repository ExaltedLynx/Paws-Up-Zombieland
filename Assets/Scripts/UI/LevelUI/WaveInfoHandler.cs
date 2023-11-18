using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveInfoHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveInfoText;

    public void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        waveInfoText.SetText(GameManager.Instance.EnemiesSpawned + "/" + GameManager.Instance.TotalEnemies);
    }
}
