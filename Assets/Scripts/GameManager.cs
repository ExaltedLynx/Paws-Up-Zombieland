using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private int playerMaxHealth;
    [SerializeField] private int playerHealth;
    [SerializeField] private GameObject[] unitPrefabs;
    [SerializeField] private CreditsTextHandler placementPointsText;

    public Dialogue dialogue;
    public int PlacementPoints
    {
        get => placementPoints;
        set
        {
            placementPoints = value;
            placementPointsText.UpdateText();
        }
    }
    [SerializeField] private int placementPoints = 0;

    private float timePerPoint = 1;
    private float timer;

    public GameObject heldUnit;
    private PlayableUnit[] placedUnits;

    public static int unlockedLevels = 1;
    public static int currentLevel = 1;
    [SerializeField] private int winPointsRequirement;
    private int winPoints;

    [SerializeField] private WaveInfoHandler waveInfoText;
    public int totalEnemies = 0;

    [SerializeField] private GameObject PauseScreen;
    private bool gameIsPaused = false;
    private bool isDoubleSpeed = false;

    public int EnemiesSpawned
    {
        get => enemiesSpawned;
        set
        {
            enemiesSpawned = value;
            waveInfoText.UpdateText();
        }
    }
    private int enemiesSpawned = 0;

    public static GameManager Instance
    {
        get => instance;
    }

      public int WinPoints
    {
        get => winPoints;
        set => winPoints = value;
    }
    
    private static GameManager instance;

    private void Awake()
    {
        playerHealth = playerMaxHealth;
        placedUnits = new PlayableUnit[unitPrefabs.Length];
        timer = timePerPoint;
        playerHealthText.SetText(playerHealth.ToString());
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }

        if (gameIsPaused) return;

        if (playerHealth == 0)
        {
            Debug.Log("Game Over");
            SceneController.RestartLevel();
        }

        if (timer < 0 && placementPoints < 99 && !dialogue.gameObject.activeSelf)
        {
            PlacementPoints++;
            timer = timePerPoint;
        }
        timer -= Time.deltaTime;

        if (winPoints == winPointsRequirement && playerHealth > 0)
        {
            Debug.Log("You Win!");
            winPoints++;

            SceneManager.LoadScene("menu"); 
        }
    }

    public void DamagePlayer()
    {
        playerHealth--;
        playerHealthText.SetText(playerHealth.ToString());
        //if(playerHealth == 0)
            //game over scene
    }

    public void SetHeldUnit(int index)
    {
        PlayableUnit unit;
        if(placedUnits[index] == null)
        {
            unit = unitPrefabs[index].GetComponent<PlayableUnit>();
            if(CanAffordUnit(unit))
            {
                removeCurrentlyHeldUnit();
                heldUnit = Instantiate(unitPrefabs[index], transform);
                placedUnits[index] = heldUnit.GetComponent<PlayableUnit>();

            }
        }
        else if(placedUnits[index].GetState() != PlayableUnit.UnitState.NotPlaced)
        {
            removeCurrentlyHeldUnit();
            unit = placedUnits[index];
            unit.tilePlacedOn.removeUnit();
            unit.ToggleRangeVisibility();
            unit.SetState(PlayableUnit.UnitState.NotPlaced);
            unit.ResetEnemiesBlocked();
            heldUnit = unit.gameObject; 
            if (unit is FighterUnit fighter) //clears the array for storing enemies in the unit's range when being re-placed;
                fighter.enemiesInRange.Clear();
        }
    }

    public void UsePlacementPoints(PlayableUnit unit)
    {
        placementPoints -= unit.GetUnitCost();
    }

    public void TogglePauseGame()
    {
        if(gameIsPaused)
            Time.timeScale = isDoubleSpeed ? 2 : 1;
        else
            Time.timeScale = 0;
        
        gameIsPaused = !gameIsPaused;
        PauseScreen.SetActive(gameIsPaused);
    }

    public void ToggleDoubleSpeed()
    {
        Time.timeScale = isDoubleSpeed ? 2 : 1;
        isDoubleSpeed = !isDoubleSpeed;
    }
    internal void ResetTimeScale()
    {
        Time.timeScale = 1;
        gameIsPaused = false;
        isDoubleSpeed = false;
    }

    private void removeCurrentlyHeldUnit()
    {
        if (heldUnit != null)
        {
            int currentUnitIndex = Array.IndexOf(placedUnits, heldUnit.GetComponent<PlayableUnit>(), 0);
            placedUnits[currentUnitIndex] = null;
            Destroy(heldUnit);
            heldUnit = null;
        }
    }

    //returns true if player has enough points to place the unit, false otherwise
    private bool CanAffordUnit(PlayableUnit unit)
    {
        if (unit.GetUnitCost() <= placementPoints)
            return true;

        return false;
    }
}
