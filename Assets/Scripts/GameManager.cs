using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private int playerMaxHealth;
    [SerializeField] private int playerHealth;
    [SerializeField] private UnitCooldownUI[] spawnButtonCooldownUI;
    [SerializeField] private GameObject[] unitPrefabs;
    public GameObject heldUnit;
    private PlayableUnit[] placedUnits;
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
    [SerializeField] private CreditsTextHandler placementPointsText;
    private float timePerPoint = 1;
    private float timer;

    public static int unlockedLevels = 1;
    public static int currentLevel { get => SceneManager.GetActiveScene().buildIndex; }
    public static int[] starsObtained = new int[5];
    public static bool unitHasDied = false;
    public static bool lostHealth = false;

    [SerializeField] private int winPointsRequirement;
    private int winPoints;

    [SerializeField] private WaveInfoHandler waveInfoText;
    public int TotalEnemies
    {
        get => totalEnemies;
        set
        {
            totalEnemies = value;
            waveInfoText.UpdateText();
        }
    }
    private int totalEnemies = 0;

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

    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private Button ChangeSpeedButton;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject failScreen;
    [SerializeField] private StarsObtainedUIHandler starsObtainedUI;
    private bool gameIsPaused = false;
    private bool isDoubleSpeed = false;

    public int WinPoints
    {
        get => winPoints;
        set => winPoints = value;
    }

    public static GameManager Instance
    {
        get => instance;
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
            ChangeSpeedButton.interactable = !ChangeSpeedButton.interactable;
        }

        if (timer < 0 && placementPoints < 99 && !dialogue.gameObject.activeSelf)
        {
            PlacementPoints++;
            timer = timePerPoint;
        }
        timer -= Time.deltaTime;

        if (winPoints == winPointsRequirement && playerHealth > 0 && !victoryScreen.activeSelf)
        {
            Debug.Log("You Win!");
            winPoints++;

            if (unlockedLevels < currentLevel + 1 && unlockedLevels < 5)
                unlockedLevels = currentLevel + 1;

            starsObtained[currentLevel - 1] = GetStarsAchieved();
            victoryScreen.SetActive(true);
            starsObtainedUI.UpdateStarImages(starsObtained[currentLevel - 1]);
            Time.timeScale = 0;
        }
    }

    public void DamagePlayer()
    {
        playerHealth--;
        lostHealth = true;
        playerHealthText.SetText(playerHealth.ToString());
        if (playerHealth == 0)
        {
            failScreen.SetActive(true);
            Time.timeScale = 0;
        }
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
                placedUnits[index].cooldownUI = spawnButtonCooldownUI[index];
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
        Time.timeScale = isDoubleSpeed ? 1 : 2;
        isDoubleSpeed = !isDoubleSpeed;
    }
    internal void ResetLevel()
    {
        Time.timeScale = 1;
        gameIsPaused = false;
        isDoubleSpeed = false;
        unitHasDied = false;
        lostHealth = false;
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

    private int GetStarsAchieved()
    {
        int stars;
        if(unitHasDied && lostHealth)
            stars = 1;
        else if(unitHasDied || lostHealth)
            stars = 2;
        else
            stars = 3;

        return stars;
    }
}
