using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
    public Transform startPoint;
    public GameObject InactiveWaypoint;
    public GameObject WaypointToDeactivate;

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 5;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    [Header("Events")]
    public static UnityEvent<EnemyBehavior> onEnemyDestroy = new UnityEvent<EnemyBehavior>(); 

    [Header("Waypoints")]
    public Transform[] waypoints; // Reference to your waypoints.

    [Header("Wave Settings")]
    [SerializeField] private int maxWaves = 10; // Maximum number of waves.
    [SerializeField] private int waveToActivateObject = 3; // Adjust the wave number as needed


    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;
    private List<GameObject> spawnedEnemies = new List<GameObject>(); // Not static

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
        
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= 1f / enemiesPerSecond && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            timeSinceLastSpawn = 0f;
        }


        // Check if the current wave matches the wave number to activate the object
         if (currentWave == waveToActivateObject)
        {
        // Activate the object
        InactiveWaypoint.SetActive(true);
        }


         // Remove empty elements from spawnedEnemies
        RemoveEmptyElements();

         if (spawnedEnemies.Count == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void EnemyDestroyed(EnemyBehavior enemy)
    {
            if (spawnedEnemies.Count > 0)
            {
                GameObject destroyedEnemy = spawnedEnemies[0];
                spawnedEnemies.RemoveAt(0);
            }
        }

         private void RemoveEmptyElements()
    {
        // Create a new list without empty elements
        List<GameObject> newSpawnedEnemies = new List<GameObject>();

        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                newSpawnedEnemies.Add(enemy);
            }
        }

        // Replace the old list with the new one
        spawnedEnemies = newSpawnedEnemies;
    }
    

    private IEnumerator StartWave()
    {
        if (currentWave > maxWaves)
        {
            isSpawning = false;
            yield break;
        }

        GameManager.Instance.enemyCounter += EnemiesPerWave();

        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;

        if (currentWave > maxWaves)
        {
              // Check if there are no more enemies left to spawn
        if (enemiesLeftToSpawn == 0 && spawnedEnemies.Count == 0)
        {
            // All waves have been completed, and no more enemies are left
            Debug.Log("All waves completed, and no more enemies left.");
            // Deactivate the object
            WaypointToDeactivate.SetActive(false);
            GameManager.Instance.WinPoints += 1;

        }

        return;
        }

        StartCoroutine(StartWave());
    }

    private void SpawnEnemy()
    {
        // Randomly select an enemy prefab from the available types
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[randomIndex];

        GameObject enemy = Instantiate(prefabToSpawn, startPoint.position, Quaternion.identity);
        enemy.GetComponent<EnemyBehavior>().SetWaypoints(waypoints);
        spawnedEnemies.Add(enemy); 
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }
}
