using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealerUnit : PlayableUnit
{
    [SerializeField] protected int healPower;
    private ContactFilter2D filter = new ContactFilter2D();

    protected override void Start()
    {
        base.Start();
        filter.SetLayerMask(LayerMask.GetMask("Player Collision"));
        filter.useTriggers = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
    }

    //checks for colliders at the center of each range marker filtered through the Player Collision layer.
    protected List<PlayableUnit> GetUnitsInRange()
    {
        List<Collider2D> results = new List<Collider2D>();
        List<PlayableUnit> unitsInRange = new List<PlayableUnit>();
        Transform[] rangeMarkers = rangeCollider.GetComponentsInChildren<Transform>();

        for (int i = 1; i < rangeMarkers.Length; i++) // i is set to one to skip the transform attached to this unit
        {
            int output = Physics2D.OverlapPoint(rangeMarkers[i].position, filter, results); 
            if (output == 1)
                unitsInRange.Add(results[0].GetComponent<PlayableUnit>());
        }
        return unitsInRange;
    }

    public int GetHealPower()
    {
        return healPower;
    }
}
