using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefenderUnit : FighterUnit
{
    private int abilityDuration = 4;
    private bool abilityActive = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void ActionLogic()
    {
        if(abilityActive)
        {
            foreach (var enemy in enemiesInRange)
            {
                enemy.Damage(attackStat);
            }
        }
        else
            enemiesInRange[0].Damage(attackStat);
    }

    protected override IEnumerator AbilityLogic()
    {
        foreach(var enemy in enemiesInRange)
        {
            enemy.isStunned = true;
        }
        int baseAttackStat = attackStat;
        attackStat += (int)(attackStat * 0.3f);

        yield return new WaitForSecondsRealtime(abilityDuration);

        attackStat = baseAttackStat;
        foreach (var enemy in enemiesInRange)
        {
            enemy.isStunned = false;
        }
    }
}
