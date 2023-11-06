using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveInfoHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveInfoText;
    private int totalEnemies = 0;
    void Start()
    {
        totalEnemies = GameManager.Instance.totalEnemies;
        waveInfoText.SetText("0/" + totalEnemies);
    }

    public void UpdateText()
    {
        waveInfoText.SetText(GameManager.Instance.EnemiesSpawned + "/" + totalEnemies);
    }
}
