using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealerUnit : PlayableUnit
{
    [SerializeField] private int healPower;

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
        PlayableUnit collidedUnit = collision.gameObject.GetComponentInParent<PlayableUnit>();
        if (state != UnitState.NotPlaced && collidedUnit.GetState() != UnitState.NotPlaced)
            Debug.Log(collision.gameObject + " entered heal range");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

            Debug.Log(collision.gameObject +" left heal range");
    }

    internal abstract void HealLogic();
}
