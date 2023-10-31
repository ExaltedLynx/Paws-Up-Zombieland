using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fighter : FighterUnit
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
        enemies[0].Damage(attackStat);
    }

}
