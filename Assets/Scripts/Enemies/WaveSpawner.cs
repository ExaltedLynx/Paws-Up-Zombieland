using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner main;
    public Transform startPoint;

    [Header ("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 5;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    [Header("Waypoints")]
    public Transform[] waypoints; // Reference to your waypoints.

    [Header("Wave Settings")]
    [SerializeField] private int maxWaves = 10; // Maximum number of waves.


    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private WaveSpawnerManager waveSpawnerManager;


    private void Awake(){
    waveSpawnerManager = FindObjectOfType<WaveSpawnerManager>();
    onEnemyDestroy.AddListener(EnemyDestroyed);
    }    

    private void Start(){
        StartCoroutine(StartWave());
    }


    private void Update(){
        if(!isSpawning) return;
        
        
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= 1f / enemiesPerSecond && enemiesLeftToSpawn > 0){
            SpawnEnemy();
            enemiesLeftToSpawn--;
            timeSinceLastSpawn = 0f;
        }

        if (spawnedEnemies.Count == 0 && enemiesLeftToSpawn == 0){
            EndWave();
        }

    }
    
  private void EnemyDestroyed()
    {
       if (spawnedEnemies.Count > 0)
    {
        // Remove the enemy from the associated WaveSpawner's list.
        waveSpawnerManager.RemoveEnemyFromSpawner(this, spawnedEnemies[0]);
        spawnedEnemies.RemoveAt(0);
    }
    }

    private IEnumerator StartWave(){
         // Check if the current wave exceeds the maximum number of waves.
        if (currentWave > maxWaves)
        {
            // You've reached the maximum number of waves, so no more waves will start.
            isSpawning = false;
            yield break;
        }

        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();

         // Reset the enemy count in the WaveSpawnerManager.
        spawnedEnemies.Clear();
    }

     private void EndWave(){
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;

        // Check if the current wave exceeds the maximum number of waves.
        if (currentWave > maxWaves)
        {
            // You've reached the maximum number of waves, so no more waves will start.
            return;
        }

        StartCoroutine(StartWave());
    }

    private void SpawnEnemy(){
        GameObject prefabToSpawn = enemyPrefabs[0];
        GameObject enemy = Instantiate(prefabToSpawn, startPoint.position, Quaternion.identity);
        enemy.GetComponent<EnemyBehavior>().SetWaypoints(waypoints);
        spawnedEnemies.Add(enemy);
        waveSpawnerManager.AddEnemyToSpawner(this, enemy);
    }

    private int EnemiesPerWave(){
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }


}
