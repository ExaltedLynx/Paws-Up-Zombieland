using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHealer : HealerUnit
{
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

    [ContextMenu("Force Heal")]
    protected override void ActionLogic()
    {
        List<PlayableUnit> units = GetUnitsInRange();
        if(units.Count == 0) { return; }

        //finds the unit with the lowest relative health then heals them
        PlayableUnit minHealthUnit = units[0];
        float lowestHealthPercent = (float)minHealthUnit.GetCurrentHealth() / minHealthUnit.GetMaxHealth();
        for (int i = 1; i < units.Count; i++) 
        {
            float healthPercent = (float)units[i].GetCurrentHealth() / units[i].GetMaxHealth();
            if (lowestHealthPercent > healthPercent)
            {
                lowestHealthPercent = healthPercent;
                minHealthUnit = units[i];
            }
        }
        minHealthUnit.Heal(healPower);
    }
}
