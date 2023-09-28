using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

[SerializeField] private float range;
[SerializeField] private float damage;
[SerializeField] private float timeBetweenShots; // Delay in seconds

private float nextTimeToShoot;

private GameObject currentTarget;

private void Start()
{
    nextTimeToShoot = Time.time;
}

private void updateNearestEnemy()
{
    GameObject currentNearestEnemy = null;

    float distance = Mathf.Infinity;

    foreach(GameObject enemy in EnemiesCounter.enemiesCounter)
    {
        if(enemy != null)
        {
             float _distance = (transform.position - enemy.transform.position).magnitude;

        if (_distance < distance)
        {
            distance = _distance;
            currentNearestEnemy = enemy;
            }
        }
       
    }

    if (distance <= range)
    {
        currentTarget = currentNearestEnemy;
    }
    else
    {
        currentTarget = null;
    }
}

private void shoot()
{
    Enemy enemyScript = currentTarget.GetComponent<Enemy>();
    enemyScript.takeDamage(damage);
}

private void Update()
{
    updateNearestEnemy();

    if (Time.time >= nextTimeToShoot)
    {
        if (currentTarget != null)
        {
            shoot();
            nextTimeToShoot = Time.time + timeBetweenShots;
            }
        }
    }
}
