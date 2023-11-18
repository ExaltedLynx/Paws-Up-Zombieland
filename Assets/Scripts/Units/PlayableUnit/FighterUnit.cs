using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FighterUnit : PlayableUnit
{
    [SerializeField] protected int attackStat;
    public List<EnemyBehavior> enemiesInRange = new List<EnemyBehavior>();

    protected override void Start()
    {
        base.Start();
        WaveSpawner.onEnemyDestroy.AddListener(RemoveKilledEnemy);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
    }
    private void RemoveKilledEnemy(EnemyBehavior enemy)
    {
        enemiesInRange.Remove(enemy);
    }

    public int GetAttack()
    {
        return attackStat;
    }
}
