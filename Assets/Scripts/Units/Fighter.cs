using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fighter : FighterUnit
{
    private int enhancedAttacks = 8;
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
        enemiesInRange[0].Damage(attackStat);
        if (abilityActive)
        {
            GameManager.Instance.PlacementPoints += 1;
            enhancedAttacks--;
        }
    }

    protected override IEnumerator AbilityLogic()
    {
        int baseAttackStat = attackStat;
        attackStat += (int)(attackStat * 0.30f);
        abilityActive = true;

        yield return new WaitUntil(() => enhancedAttacks == 0);

        abilityActive = false;
        enhancedAttacks = 8;
        attackStat = baseAttackStat;
    }
}
