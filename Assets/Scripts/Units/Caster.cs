using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Caster : FighterUnit
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

    protected override void ActionLogic()
    {
        //attacks all the enemies in their range
        /*
        for(int i = 0; i < enemiesInRange.Count; i++)
        {
            enemiesInRange[i].Damage(attackStat);
        }
        */
    }
}
