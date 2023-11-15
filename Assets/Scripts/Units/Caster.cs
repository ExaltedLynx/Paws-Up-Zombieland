using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Caster : FighterUnit
{
    private int abilityDuration = 3;
    private int intervalModifier = 20;

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

        for(int i = 0; i < enemiesInRange.Count; i++)
        {
            enemiesInRange[i].Damage(attackStat);
        }
    }

    protected override IEnumerator AbilityLogic()
    {
        float baseActionInterval = actionTime;
        float finalActionInterval = 100 / ((100 + intervalModifier) / baseActionInterval); //this lowers the time between each action by intervalModifier%
        actionTime = finalActionInterval;
        yield return new WaitForSecondsRealtime(abilityDuration);
        actionTime = baseActionInterval;
    }
}
