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

    SpriteRenderer rend;

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
    }

    void Start(){
        Wpoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();
        rend = GetComponent<SpriteRenderer>();
        Color c = rend.material.color;
        c.a = 0f;
        rend.material.color = c;
    }

    void Update(){
       if (waypoints == null || waypointIndex >= waypoints.Length)
        {
            // Handle error or destroy the enemy.
            Destroy(gameObject);
            return;
        }

        IEnumerator FadeOut()
        {
            for (float f = 1f; f>= -0.05f; f -= 0.05f)
            {
                Color c = rend.material.color;
                c.a = f;
                rend.material.color = c;
                yield return new WaitForSeconds (0.05f);
            }
        }

        IEnumerator FadeIn()
        {
            for (float f = 0.05f; f <= 1; f += 0.05f)
            {
                Color c = rend.material.color;
                c.a = f;
                rend.material.color = c;
                yield return new WaitForSeconds (0.05f);
            }
        }

        IEnumerator DelayDestroy()
        {
            yield return new WaitForSeconds(0.2f);
            Destroy(gameObject);
        }

        void startFadingOut()
        {
            StartCoroutine(FadeOut());
        }

        void startFadingIn()
        {
            StartCoroutine(FadeIn());
        }

        void startDelay()
        {
            StartCoroutine(DelayDestroy());
        }

        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, waypoints[waypointIndex].position) < 0.1f)
        {
            if (waypointIndex == 0)
            {
                startFadingIn();
            }

            if (waypointIndex < waypoints.Length - 1)
            {
                waypointIndex++;
            }
            else
                {
                WaveSpawner.onEnemyDestroy.Invoke();
                startFadingOut();
                startDelay();
                 // Change this later so that it takes away player's health
                }
            }
        }
    }

