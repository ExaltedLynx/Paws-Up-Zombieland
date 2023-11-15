using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefenderUnit : FighterUnit
{
    private int abilityDuration = 5;

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
        enemiesInRange[0].Damage(attackStat);
    }

    protected override IEnumerator AbilityLogic()
    {
        int baseAttackStat = attackStat;
        attackStat += (int)(attackStat * 0.5f);
        yield return new WaitForSecondsRealtime(abilityDuration);
        attackStat = baseAttackStat;
    }
}
