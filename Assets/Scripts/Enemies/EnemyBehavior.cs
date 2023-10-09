using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
   // How fast enemy moves
    [SerializeField] public float speed;
    private Waypoints Wpoints;

    private int waypointIndex;

    private Transform[] waypoints; // Reference to the waypoints array.

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
    }

    void Start(){
        Wpoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();
    }

    void Update(){
       if (waypoints == null || waypointIndex >= waypoints.Length)
        {
            // Handle error or destroy the enemy.
            Destroy(gameObject);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, waypoints[waypointIndex].position) < 0.1f)
        {
            if (waypointIndex < waypoints.Length - 1)
            {
                waypointIndex++;
            }
            else
                {
                WaveSpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                }
            }
        }
    }

