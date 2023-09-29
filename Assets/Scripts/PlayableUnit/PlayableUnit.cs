using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableUnit : MonoBehaviour
{
    //TODO figure out how to add multiple tile range that isn't just a square around the unit 
    [SerializeField] private int currentHealth;
    [SerializeField] public int maxHealth { get; }
    [SerializeField] public int attackStat { get; }
    [SerializeField] public Tile.TileType validTile { get; }
    [SerializeField] public float attackTime { get; }
    [SerializeField] private float attackTimer;
    public UnitState state = UnitState.NotPlaced;

    public enum UnitState {NotPlaced, Idle, Attacking}

    void Start()
    {
        currentHealth = maxHealth;
        attackTimer = attackTime;
        state = UnitState.Idle;
    }


    void FixedUpdate()
    {
        if(state == UnitState.NotPlaced) { return; }
        Attack();
        state = UnitState.Idle;
    }

    //add parameter for a list of enemies once they are implemented
    protected void Attack(/*List<Enemy> enemiesInRange*/)
    {
        if(attackTimer != 0)
        {
            attackTimer -= Time.fixedDeltaTime;
            return;
        }
        state = UnitState.Attacking;
        //deal damage to the passed enemies
        attackTimer = attackTime;
    }

    public void Damage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    private void FindEnemiesInRange()
    {

    }
}
