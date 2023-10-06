using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
   // How fast enemy moves
    [SerializeField] public float speed;
    private Waypoints Wpoints;

    private int waypointIndex;

    void Start(){
        Wpoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();
    }

    void Update(){
        transform.position = Vector2.MoveTowards(transform.position, Wpoints.waypoints[waypointIndex].position, speed * Time.deltaTime);

        if(Vector2.Distance(transform.position, Wpoints.waypoints[waypointIndex].position) < 0.1f){
           
            if(waypointIndex < Wpoints.waypoints.Length - 1){
                waypointIndex++; 
            } else{
               Destroy(gameObject); // Finish the trajectory, this is where the tower takes damage if enemy hits last waypoint
            }
        }
    }
}
