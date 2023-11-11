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
        waveInfoText.SetText("0/" + totalEnemies);
    }

    void Update()
    {
        totalEnemies = GameManager.Instance.totalEnemies;
    }
    public void UpdateText()
    {
        waveInfoText.SetText(GameManager.Instance.EnemiesSpawned + "/" + totalEnemies);
    }
}
