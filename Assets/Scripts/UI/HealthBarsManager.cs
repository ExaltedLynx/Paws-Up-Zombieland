using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarsManager : MonoBehaviour
{
    [SerializeField] GameObject healthBarPrefab;
    [SerializeField] GameObject enemiesParent;
    [SerializeField] GameObject unitsParent;

    public static HealthBarsManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public HealthBar InitHealthBar(IEntity entity)
    {
        HealthBar healthBar;
        GameObject healthBarObj = null;
        switch(entity)
        {
            case PlayableUnit:
                healthBarObj = Instantiate(healthBarPrefab, unitsParent.transform);
                break;
            case EnemyBehavior:
                healthBarObj = Instantiate(healthBarPrefab, enemiesParent.transform);
                break;
        }
        healthBar = healthBarObj.GetComponent<HealthBar>();
        healthBar.Entity = entity;
        return healthBar;
    }
}