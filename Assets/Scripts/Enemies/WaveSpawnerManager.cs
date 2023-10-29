using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnerManager : MonoBehaviour
{
     public static WaveSpawnerManager instance;

    private Dictionary<WaveSpawner, List<GameObject>> spawnedEnemiesBySpawner = new Dictionary<WaveSpawner, List<GameObject>>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void AddEnemyToSpawner(WaveSpawner spawner, GameObject enemy)
    {
        if (!spawnedEnemiesBySpawner.ContainsKey(spawner))
        {
            spawnedEnemiesBySpawner[spawner] = new List<GameObject>();
        }

        spawnedEnemiesBySpawner[spawner].Add(enemy);
    }

    public void RemoveEnemyFromSpawner(WaveSpawner spawner, GameObject enemy)
    {
        if (spawnedEnemiesBySpawner.ContainsKey(spawner))
        {
            spawnedEnemiesBySpawner[spawner].Remove(enemy);
        }
    }
}
