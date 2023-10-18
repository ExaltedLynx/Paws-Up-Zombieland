using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayableUnit : MonoBehaviour
{
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int Defense;
    [SerializeField] private float actionTimer;
    [SerializeField] protected float actionTime;
    [SerializeField] protected Tile.TileType validTile;
    [SerializeField] protected UnitState state = UnitState.NotPlaced;
    [SerializeField] protected GameObject rangeCollider;
    internal Tile tilePlacedOn;

    public enum UnitState {NotPlaced, Idle, Acting}

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        actionTimer = actionTime;
        state = UnitState.NotPlaced;
    }

    protected virtual void FixedUpdate()
    {
        if(state == UnitState.NotPlaced) { return; }

        if(state == UnitState.Acting)
            Action();

        state = UnitState.Idle;
    }

    protected virtual void Update()
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

    private void OnMouseEnter()
    {
        ToggleRangeVisibility();
    }
    private void OnMouseExit()
    {
        ToggleRangeVisibility();
    }

    private void Action()
    {
        if(actionTimer != 0)
        {
            actionTimer -= Time.fixedDeltaTime;
            return;
        }

        switch(this)
        {
            case FighterUnit fighter:
                fighter.AttackLogic();
                break;
            case HealerUnit healer:
                healer.HealLogic();
                break;
        }
        actionTimer = actionTime; //resets the timer
    }

    //moves the gameobject of the unit to where the mouse is
    private void DragUnit()
    {
        if (GameManager.Instance.heldUnit != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.z = 0;
            transform.localPosition = mousePos;
        }
    }

    [ContextMenu("Toggle Range Visibility")]
    public void ToggleRangeVisibility()
    {
        if (state != UnitState.NotPlaced && GameManager.Instance.heldUnit == null)
        {
            MeshRenderer[] rangeRenderers = rangeCollider.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mesh in rangeRenderers)
                mesh.enabled = !mesh.enabled;
        }
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

    public Tile.TileType GetValidTile()
    {
        return validTile;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public UnitState GetState()
    {
        return state;
    }

    public void SetState(UnitState newState)
    {
        state = newState;
    }
}
