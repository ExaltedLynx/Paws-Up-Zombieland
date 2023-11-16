using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class PlayableUnit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEntity
{
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int defense;
    [SerializeField] private int enemiesBlocked = 0;
    [SerializeField] private int maxBlock;
    [SerializeField] private float actionTimer;
    [SerializeField] protected float actionTime;
    [SerializeField] private int currentSkillPoints;
    [SerializeField] protected int abilityCost;
    [SerializeField] private int cost;
    [SerializeField] private int placementCooldown;
    [SerializeField] protected Tile.TileType validTile;
    [SerializeField] protected UnitState state = UnitState.NotPlaced;
    [SerializeField] protected GameObject rangeCollider;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Material originalMaterial;

    private Vector3 offsetDown;
    private Vector3 offsetRight;
    private Vector3 offsetLeft;
    Vector3 IEntity.position => transform.position;
    internal HealthBar healthBar;
    internal SkillBar skillBar;
    internal Tile tilePlacedOn;
    internal EnemyBehavior[] blockedEnemies;
    internal UnitCooldownUI cooldownUI;

    public enum UnitState { NotPlaced, Idle, Acting }

    public UnityEvent onUnitDeath = new UnityEvent();

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
        cooldownUI.InitCooldownUI(this);
        healthBar = HealthBarsManager.Instance.InitHealthBar(this);
        skillBar = SkillBarManager.Instance.InitSkillBar(this);
    }

    protected virtual void FixedUpdate()
    {
        if (state == UnitState.NotPlaced) { return; }

        ChargeAbility();
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

        if(skillBar != null) 
            Destroy(skillBar.gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentSkillPoints == abilityCost && eventData.button == PointerEventData.InputButton.Left)
        {
            StartCoroutine(AbilityLogic());
            currentSkillPoints = 0;
        }
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
        AddSkillPoint(1);
        actionTimer = actionTime; //resets the timer
    }

    protected abstract void ActionLogic();

    private float second = 1;
    private void ChargeAbility()
    {
        if (currentSkillPoints < abilityCost)
        {
            if (second <= 0)
            {
                currentSkillPoints++;
                second = 1;
            }
            second -= Time.fixedDeltaTime;
        }
    }

    protected abstract IEnumerator AbilityLogic();

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
            onUnitDeath.Invoke();
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
        Damage(9999);
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
        if(index != -1)
        {
            switch (index)
            {
                case 0:
                case 1:
                    enemy.transform.position = offset;
                    Debug.Log(offset);
                break;
            }
        }
    }

    private Vector3 DetermineOffsetDirection(EnemyBehavior enemy, int index)
    {
        Vector3 offset = Vector3.zero;
        offsetDown = transform.localPosition + new Vector3(0.15f, 0.65f);
        offsetLeft = transform.localPosition + new Vector3(-0.65f, 0.15f);
        offsetRight = transform.localPosition + new Vector3(0.65f, 0.15f);
        Debug.Log(enemy.direction);
        switch (enemy.direction)
        {
            case EnemyBehavior.Direction.Down:
                if (index == 1)
                    offset = Vector3.Reflect(offsetDown, Vector3.left); //mirrors the vector
                else
                    offset = offsetDown;
                break;

            case EnemyBehavior.Direction.Left:
                if(index == 1)
                    offset = Vector3.Reflect(offsetLeft, Vector3.down); //mirrors the vector
                else
                    offset = offsetLeft;
                break;

            case EnemyBehavior.Direction.Right:
                if (index == 1)
                    offset = Vector3.Reflect(offsetRight, Vector3.down); //mirrors the vector
                else
                    offset = offsetRight;
                break;
        }

        Debug.Log(offset);
        Vector3 offsetWorldPos = TransformPointIgnoreRot(offset); //changes the offset from local position to world position
        Debug.Log(offsetWorldPos);
        return offsetWorldPos;
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

    //added this because Transform.TransformPoint uses rotation in its calculation which causes enemy's to be set to the wrong world position.
    private Vector3 TransformPointIgnoreRot(Vector3 vector)
    { 
        return Vector3.Scale(vector, transform.localScale) + transform.position;
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

    public void AddSkillPoint(int skillPoints)
    {
        if(currentSkillPoints < abilityCost)
            currentSkillPoints += skillPoints;
    }

    public int GetSkillPoints()
    {
        return currentSkillPoints;
    }

    public int GetAbilityCost()
    {
        return abilityCost;
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

    public int GetPlacementCooldown()
    {
        return placementCooldown;
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
