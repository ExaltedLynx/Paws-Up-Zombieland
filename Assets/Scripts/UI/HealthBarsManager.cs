using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void InitUnitHealthBar()
    {

    }

    public void InitEnemyHealthBar()
    {

    }
}
