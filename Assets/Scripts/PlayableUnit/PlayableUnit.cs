using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableUnit : MonoBehaviour
{
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] protected int attackStat;
    [SerializeField] private float attackTimer;
    [SerializeField] protected float attackTime;
    [SerializeField] protected Tile.TileType validTile;
    [SerializeField] private UnitState state = UnitState.NotPlaced;

    public enum UnitState {NotPlaced, Idle, Attacking}

    void Start()
    {
        currentHealth = maxHealth;
        attackTimer = attackTime;
        state = UnitState.NotPlaced;
    }

    void FixedUpdate()
    {
        if(state == UnitState.NotPlaced) { return; }
        Attack();
        state = UnitState.Idle;
    }

    private void Update()
    {
        if(state == UnitState.NotPlaced)
        {
            DragUnit();
            if(Input.GetMouseButtonDown(1))
            {
                gameObject.transform.Rotate(0, 0, 90);
            }
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //collision.transform.gameObject.GetComponent<Enemy>();
    }

    //moves the gameobject of the unit to where the mouse is
    private void DragUnit()
    {
        if (GameManager.Instance.heldUnit != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.z = 0;
            Debug.Log(mousePos);
            transform.localPosition = mousePos;
        }
    }

    public Tile.TileType GetValidTile()
    {
        return validTile;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public UnitState GetState()
    {
        return state;
    }

    public void SetState(UnitState state)
    {
        this.state = state;
    }
}
