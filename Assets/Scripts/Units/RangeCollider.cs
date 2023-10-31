using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayableUnit;

public class RangeCollider : MonoBehaviour
{
    [SerializeField] public FighterUnit fighter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemy")
        {
            Debug.Log("in " + collision.gameObject);
            fighter.enemiesInRange.Add(collision.gameObject.GetComponent<EnemyBehavior>());
            if (fighter.GetState() == UnitState.Idle)
                fighter.SetState(UnitState.Acting);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "enemy")
        {
            Debug.Log("out " + collision.gameObject);
            fighter.enemiesInRange.Remove(collision.gameObject.GetComponent<EnemyBehavior>());
            if (fighter.enemiesInRange.Count == 0 && fighter.GetState() == UnitState.Acting)
                fighter.SetState(UnitState.Idle);
        }
    }
}
