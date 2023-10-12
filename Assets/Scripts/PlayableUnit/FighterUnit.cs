using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FighterUnit : PlayableUnit
{
    [SerializeField] protected int attackStat;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //state = UnitState.Acting;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if no enemies
            //state = UnitState.Idle;
    }

    internal abstract void AttackLogic();
}
