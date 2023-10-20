using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayableUnit : MonoBehaviour
{
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int defense;
    [SerializeField] private int enemiesBlocked = 0;
    [SerializeField] private int maxBlock;
    [SerializeField] private float actionTimer;
    [SerializeField] protected float actionTime;
    [SerializeField] protected Tile.TileType validTile;
    [SerializeField] protected UnitState state = UnitState.NotPlaced;
    [SerializeField] protected GameObject rangeCollider;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Material originalMaterial;


    internal Tile tilePlacedOn;

    public enum UnitState { NotPlaced, Idle, Acting }

    private IEnumerator FlashEffect()
    {
        // Apply the flash material to the Sprite Renderer.
        spriteRenderer.material = flashMaterial;
    
        // Wait for a short duration (e.g., 0.2 seconds).
        yield return new WaitForSeconds(0.2f);
    
        // Revert to the original material.
        spriteRenderer.material = originalMaterial;
    }


    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        originalMaterial = GetComponent<SpriteRenderer>().material;
        originalMaterial = spriteRenderer.material;
        currentHealth = maxHealth;
        actionTimer = actionTime;
        state = UnitState.NotPlaced;
    }

    protected virtual void FixedUpdate()
    {
        if (state == UnitState.NotPlaced) { return; }

        if (state == UnitState.Acting)
            Action();

        state = UnitState.Idle;
    }

    protected virtual void Update()
    {
        if (state == UnitState.NotPlaced)
        {
            DragUnit();
            if (Input.GetMouseButtonDown(1))
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
        if (actionTimer != 0)
        {
            actionTimer -= Time.fixedDeltaTime;
            return;
        }
        ActionLogic();
        actionTimer = actionTime; //resets the timer
    }

    protected abstract void ActionLogic();


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
        currentHealth -= amount - (int)(defense * 0.2); //lowers damage recieved by 20% of the unit's defense
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
        StartCoroutine(FlashEffect()); // Trigger the flash effect.
        }
    }

    [ContextMenu("Force Kill")]
    private void ForceKill()
    {
        Damage(GetCurrentHealth());
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    //returns true if the number of units attacking this unit is equal to max amount of units this unit can hold aggro
    public bool IsAtMaxBlock()
    {
        return enemiesBlocked == maxBlock;
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

    public void IncreaseBlockedCount()
    {
        enemiesBlocked++;
    }
    public void DecreaseBlockedCount()
    {
        enemiesBlocked--;
    }
}
