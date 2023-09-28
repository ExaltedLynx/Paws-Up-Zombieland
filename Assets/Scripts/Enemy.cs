using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float enemyHealth;

    [SerializeField]
    private float movementSpeed;

    private int killReward; // Amount of money player gets when enemy is killed
    private int damage;  // The amount of damage enemy deals at the end

    private GameObject targetTile;

    private void Start()
    {
        initializeEnemy();
    }

    private void initializeEnemy()
    {
        targetTile = mapGenerator.startTile;
    }

    public void takeDamage(float amount)
    {
        enemyHealth -= amount;

        if(enemyHealth <= 0)
        {
            die();
        }
    }

    private void die()
    {
        Destroy(transform.gameObject);
    }

    private void moveEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetTile.transform.position, movementSpeed * Time.deltaTime);
    }

    private void checkPosition()
    {
        if(targetTile != null && targetTile != mapGenerator.endTile)
        {
            float distance = (transform.position-targetTile.transform.position).magnitude;

            if (distance < 0.001f)
            {
                int currentIndex = mapGenerator.pathTiles.IndexOf(targetTile);

                targetTile = mapGenerator.pathTiles[currentIndex + 1];
            }
        }

    }

    private void Update()
    {
        checkPosition();
        moveEnemy();

        takeDamage(0);
    }


}
