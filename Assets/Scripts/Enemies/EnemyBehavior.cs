using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBehavior : MonoBehaviour
{
   // How fast enemy moves
    [SerializeField] public float speed;
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    private Waypoints Wpoints;
    private PlayableUnit targetedUnit;

    private int waypointIndex;

    private Transform[] waypoints; // Reference to the waypoints array.

    SpriteRenderer rend;
    
    private bool isMoving = true; // A flag to control movement
    private bool isColliding = false; // A flag to control collision

    private float damageTimer = 0f;
    private float damageDelay = 1.7f; // Adjust this value to set the desired delay between damage. 

    private static int count = 0;

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
    }


    void Start()
    {
        Wpoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();
        rend = GetComponent<SpriteRenderer>();
        Color c = rend.material.color;
        c.a = 0f;
        rend.material.color = c;
        currentHealth = maxHealth;
        name = "enemy " + count++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Unit"))
        {
            PlayableUnit temp = collision.GetComponent<PlayableUnit>();
            if (temp.GetState() != PlayableUnit.UnitState.NotPlaced && !temp.IsAtMaxBlock())
            {
                targetedUnit = temp;
                targetedUnit.IncreaseEnemiesBlocked();
                isMoving = false;
                isColliding = true;
            }
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //this only happens when the unit the enemy is attacking is destroyed
        if (collision.gameObject.CompareTag("Unit"))
        {
            targetedUnit = null;
            isColliding = false;
            isMoving = true;
        }
    }
        
    void Update()
    {
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
                    WaveSpawner.onEnemyDestroy.Invoke(this);
                    startFadingOut();
                    startDelay();
                    GameManager.Instance.DamagePlayer();
                    isMoving = false;
                }
            }
        }

        if (isColliding && targetedUnit != null)
        {
            if (damageTimer <= 0f && targetedUnit.GetState() != PlayableUnit.UnitState.NotPlaced)
            {
                targetedUnit.Damage(15);
                damageTimer = damageDelay; // Reset the timer after applying damage.
            }
            else
            {
                damageTimer -= Time.deltaTime; // Decrease the timer.
            }
        }

    }

    public void Damage(int amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            if(targetedUnit != null) 
                targetedUnit.DecreaseEnemiesBlocked();

            WaveSpawner.onEnemyDestroy.Invoke(this);
            Destroy(gameObject);
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

