using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FighterUnit : PlayableUnit
{
    protected List<EnemyBehavior> enemies = new List<EnemyBehavior>();
    [SerializeField] protected int attackStat;

    protected override void Start()
    {
        base.Start();
        WaveSpawner.onEnemyDestroy.AddListener(RemoveEnemyInRange);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void SetEnemyArray(EnemyBehavior enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemyInRange(EnemyBehavior enemy)
    {
        Debug.Log("enemy removed");
        enemies.Remove(enemy);
    }

}
