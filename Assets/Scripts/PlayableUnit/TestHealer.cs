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
    internal override void HealLogic()
    {
        foreach(PlayableUnit unit in GetUnitsInRange()) 
        {
            //do healing stuff here
            Debug.Log(unit);
        }
    }
}
