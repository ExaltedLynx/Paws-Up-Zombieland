using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ranger : FighterUnit
{
    private int abilityDuration = 20;
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
            //each attack will hit one additional target for 130% attack while ability is active
            enemiesInRange[0].Damage(attackStat);
            if (enemiesInRange[1] != null)
            {
                int buffedAttack = (int)(attackStat * 1.3f);
                enemiesInRange[1].Damage(buffedAttack);
            }
        }
        else
        {
            enemiesInRange[0].Damage(attackStat);
        }
    }

    protected override IEnumerator AbilityLogic()
    {
        int baseAttackStat = attackStat;
        attackStat += (int)(baseAttackStat * 0.2f);
        abilityActive = true;
        yield return new WaitForSecondsRealtime(abilityDuration);
        attackStat = baseAttackStat;
    }
}
