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

    [SerializeField] private TextMeshProUGUI placementPointsText;
    [SerializeField] public int placementPoints;
    [SerializeField] private int winPointsRequirement;

    private float timePerPoint = 1;
    private float timer;

    public GameObject heldUnit;
    private PlayableUnit[] placedUnits;

    public static int unlockedLevels = 1;
    public static int currentLevel = 1;
    private int winPoints;

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
        placementPointsText.SetText(placementPoints.ToString());
        playerHealthText.SetText(playerHealth.ToString());
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(playerMaxHealth == 0)
        {
            //load failed level scene
        }

        if(timer < 0 && placementPoints < 99)
        {
            placementPoints++;
            placementPointsText.SetText(placementPoints.ToString());
            timer = timePerPoint;
        }
        timer -= Time.deltaTime;

        if(winPoints == winPointsRequirement)
        {
            Debug.Log("You Win!");
            winPoints++;
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
        placementPointsText.SetText(placementPoints.ToString());
    }

    public static void ChangeLevel(int sceneIndex)
    {
        currentLevel = sceneIndex;
        DataManager.Instance.StartCoroutine(LoadLevelScene());
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

    //Using the async load scene function lets us add loading screens later
    private static IEnumerator LoadLevelScene()
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(currentLevel);
        while (!asyncOp.isDone)
        {
            yield return null;
        }
    }
}
