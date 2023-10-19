using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
   // How fast enemy moves
    [SerializeField] public float speed;
    private Waypoints Wpoints;
    private PlayableUnit playableUnit;

    private int waypointIndex;

    private Transform[] waypoints; // Reference to the waypoints array.

    SpriteRenderer rend;
    
    private bool isMoving = true; // A flag to control movement
    private bool isColliding = false; // A flag to control collision

    private float damageTimer = 0f;
    private float damageDelay = 0.5f; // Adjust this value to set the desired delay between damage. 

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
    }


    void Start(){
        Wpoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();
        playableUnit = FindObjectOfType<PlayableUnit>();
        rend = GetComponent<SpriteRenderer>();
        Color c = rend.material.color;
        c.a = 0f;
        rend.material.color = c;
    }

    private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Unit")
            {
                isMoving = false;
                isColliding = true;
            } 
        }
           private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Unit")
            {
                isColliding = false;
                isMoving = true;
            } 
        }
        
    void Update(){

       if (waypoints == null || waypointIndex >= waypoints.Length)
        {
            // Handle error or destroy the enemy.
            Destroy(gameObject);
            return;
        }

           if (isMoving)
        {
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

            if (isColliding)
    {
     if (damageTimer <= 0f)
        {
            if (playableUnit != null)
            {
                playableUnit.Damage(10);
            }
            damageTimer = damageDelay; // Reset the timer after applying damage.
        }
        else
        {
            damageTimer -= Time.deltaTime; // Decrease the timer.
        }
    }

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

       
}

