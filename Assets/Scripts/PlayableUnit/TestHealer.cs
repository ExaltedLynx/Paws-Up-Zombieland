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
        //finds the unit with the lowest health then heals them
        List<PlayableUnit> units = GetUnitsInRange();
        PlayableUnit minHealthUnit = units[0];
        for (int i = 1; i < units.Count; i++) 
        {
            if (minHealthUnit.GetCurrentHealth() > units[i].GetCurrentHealth())
                minHealthUnit = units[i];
        }
        minHealthUnit.Heal(healPower);
    }
}
