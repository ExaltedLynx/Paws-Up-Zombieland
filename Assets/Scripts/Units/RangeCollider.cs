using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeCollider : MonoBehaviour
{
    [SerializeField] public FighterUnit fighter;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemy")
        {
            Debug.Log("in" + collision.gameObject);
            fighter.SetEnemyArray(collision.gameObject.GetComponent<EnemyBehavior>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "enemy")
        {
            fighter.RemoveEnemyInRange(collision.gameObject.GetComponent<EnemyBehavior>());
        }


    }
}
