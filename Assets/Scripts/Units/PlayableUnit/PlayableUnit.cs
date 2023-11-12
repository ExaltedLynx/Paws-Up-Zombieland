using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class PlayableUnit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEntity
{
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int defense;
    [SerializeField] private int enemiesBlocked = 0;
    [SerializeField] private int maxBlock;
    [SerializeField] private float actionTimer;
    [SerializeField] protected float actionTime;
    [SerializeField] private int cost;
    [SerializeField] protected Tile.TileType validTile;
    [SerializeField] protected UnitState state = UnitState.NotPlaced;
    [SerializeField] protected GameObject rangeCollider;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Material originalMaterial;

    Vector3 IEntity.position => transform.position;
    internal HealthBar healthBar;
    internal Tile tilePlacedOn;
    internal EnemyBehavior[] blockedEnemies;

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
        blockedEnemies = new EnemyBehavior[maxBlock];
        healthBar = HealthBarsManager.Instance.InitHealthBar(this);
    }

    protected virtual void FixedUpdate()
    {
        if (state == UnitState.NotPlaced) { return; }

        if (state == UnitState.Acting)
            Action();
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

    private void OnDestroy()
    {
        if(healthBar != null)
            Destroy(healthBar.gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToggleRangeVisibility();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToggleRangeVisibility();
    }

    private void Action()
    {
        if (actionTimer > 0)
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
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
        if (currentHealth <= 0)
        {
            GameManager.unitHasDied = true;
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
        Damage(currentHealth);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
    }

    public void SetAttackingEnemyOffset(EnemyBehavior enemy)
    {
        int index = AddToEmptyIndex(enemy);
        enemy.attackerIndex = index;
        Vector3 offset = DetermineOffsetDirection(enemy, index);
        if(maxBlock == 2 && index != -1)
        {
            switch (index)
            {
                case 0:
                    enemy.transform.position += offset;
                    break;

                case 1:
                    enemy.transform.position -= offset;
                    break;
            }
        }
        else if(index != -1)
        {
            switch (index)
            {
                case 0:
                    if(enemy.transform.position.y > transform.position.y || enemy.transform.position.x > transform.position.x)
                        enemy.transform.position -= offset;
                    else
                        enemy.transform.position += offset;
                    break;
                case 1:
                    enemy.transform.position += offset;
                    break;

                case 2:
                    enemy.transform.position -= offset;
                    break;
            }
        }
    }

    private Vector3 DetermineOffsetDirection(EnemyBehavior enemy, int index)
    {
        Vector3 offset;
        if(maxBlock == 2 || (maxBlock == 3 && index != 0))
        {
            if (enemy.transform.position.x == transform.position.x)
                offset = new Vector3(0.25f, 0, 0);
            else //y position is equal
                offset = new Vector3(0, 0.25f, 0);
        }
        else
        {
            if (enemy.transform.position.x == transform.position.x)
                offset = new Vector3(0, 0.1f, 0);
            else //y position is equal
                offset = new Vector3(0.1f, 0, 0);
        }
        return offset;
    }

    private int AddToEmptyIndex(EnemyBehavior enemy)
    {
        for(int i = 0; i < blockedEnemies.Length; i++)
        {
            if (blockedEnemies[i] == null)
            {
                blockedEnemies[i] = enemy;
                return i;
            }
        }
        return -1;
    }

    //returns true if the number of units attacking this unit is equal to max amount of units this unit can hold aggro
    public bool IsAtMaxBlock()
    {
        return enemiesBlocked == maxBlock;
    }

    public void IncreaseEnemiesBlocked()
    {
        enemiesBlocked++;
    }

    public void DecreaseEnemiesBlocked()
    {
        enemiesBlocked--;
    }

    public void ResetEnemiesBlocked()
    {
        enemiesBlocked = 0;
    }

    public int GetEnemyBlockCount()
    {
        return enemiesBlocked;
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

    public int GetUnitCost()
    {
        return cost;
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
