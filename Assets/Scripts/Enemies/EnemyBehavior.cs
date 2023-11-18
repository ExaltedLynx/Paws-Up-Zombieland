using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBehavior : MonoBehaviour, IEntity
{
   // How fast enemy moves
    [SerializeField] public float speed;
    [SerializeField] public int damage;
    [SerializeField] public int currentHealth { get; private set; }

    [SerializeField] public int maxHealth;
    private Waypoints Wpoints;
    private PlayableUnit targetedUnit;
    private WaveSpawner waveSpawner;
    private int waypointIndex;

    private Transform[] waypoints; // Reference to the waypoints array.

    SpriteRenderer rend;
    
    private bool isMoving = true; // A flag to control movement
    private bool isColliding = false; // A flag to control collision

    private float damageTimer = 0f;
    private float damageDelay = 2f; // Adjust this value to set the desired delay between damage. 

    private static int count = 0;

    Vector3 IEntity.position => transform.position;
    internal HealthBar healthBar;
    internal int attackerIndex = 0;
    internal Direction direction;

    public enum Direction { Up, Down, Left, Right }
    public bool isStunned = false;

    [SerializeField] private Sprite[] sprites = new Sprite[3];

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
    }

    public Transform GetNextWaypoint()
    {
        return waypoints[waypointIndex];
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
        waveSpawner = FindObjectOfType<WaveSpawner>();
        healthBar = HealthBarsManager.Instance.InitHealthBar(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Unit") && targetedUnit == null)
        {
            PlayableUnit temp = collision.GetComponent<PlayableUnit>();
            if (temp.GetState() != PlayableUnit.UnitState.NotPlaced && !temp.IsAtMaxBlock())
            {
                targetedUnit = temp;
                targetedUnit.IncreaseEnemiesBlocked();
                targetedUnit.SetAttackingEnemyOffset(this);
                isMoving = false;
                isColliding = true;
            }
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Unit") && targetedUnit != null)
        {
            if(targetedUnit.GetComponent<PlayableUnit>() == collision.GetComponent<PlayableUnit>())
            {
                targetedUnit.blockedEnemies[attackerIndex] = null;
                targetedUnit = null;
                isColliding = false;
                isMoving = true;
            }
        }
    }

    private void OnDestroy()
    {
        
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

            if (transform.position == waypoints[waypointIndex].position)
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
                    // WaveSpawner.onEnemyDestroy.Invoke();
                    Destroy(healthBar.gameObject);
                    startFadingOut();
                    startDelay();
                    GameManager.Instance.DamagePlayer(); // Reduce Player Health
                    isMoving = false;
                }

                if (transform.position.x == waypoints[waypointIndex].position.x)
                {
                    if (transform.position.y > waypoints[waypointIndex].position.y)
                    {
                        direction = Direction.Down;
                        rend.sprite = sprites[0];
                    }
                    else
                    {
                        direction = Direction.Up;
                        rend.sprite = sprites[0];
                    }
                }
                else
                {
                    if (transform.position.x < waypoints[waypointIndex].position.x)
                    {
                        direction = Direction.Left;
                        rend.sprite = sprites[1];
                    }
                    else
                    {
                        direction = Direction.Right;
                        rend.sprite = sprites[2];
                    }
                }

            }
        }

        if (isColliding && targetedUnit != null && !isStunned)
        {
            if (damageTimer <= 0f && targetedUnit.GetState() != PlayableUnit.UnitState.NotPlaced)
            {
                targetedUnit.AddSkillPoint(1);
                targetedUnit.Damage(damage);
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
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
        if(currentHealth <= 0)
        {
            if(targetedUnit != null) 
                targetedUnit.DecreaseEnemiesBlocked();

            WaveSpawner.onEnemyDestroy.Invoke(this);

            Destroy(healthBar.gameObject);

            if(gameObject != null)
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

