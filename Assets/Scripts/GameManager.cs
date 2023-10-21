using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int playerMaxHealth;
    [SerializeField] private int playerHealth;
    [SerializeField] private GameObject[] unitPrefabs;

    [SerializeField] private TextMeshProUGUI placementPointsText;
    public int placementPoints = 0;
    private float timePerPoint = 1;
    private float timer;

    public GameObject heldUnit;
    private PlayableUnit[] placedUnits;
    private int sceneIndex = 0;

    public static GameManager Instance
    {
        get => instance;
    }

    private static GameManager instance;

    private void Awake()
    {
        placementPointsText.SetText(placementPoints.ToString());
        playerHealth = playerMaxHealth;
        placedUnits = new PlayableUnit[unitPrefabs.Length];
        timer = timePerPoint;
        instance = this; 
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LoadLevelScene());
        }

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

    }

    public void damagePlayer()
    {
        playerHealth -= 1;
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
            heldUnit = unit.gameObject;
        }
    }

    //Using the async load scene function lets us add loading screens later
    IEnumerator LoadLevelScene()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (sceneIndex == SceneManager.sceneCountInBuildSettings)
            sceneIndex = 0;
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncOp.isDone)
        {
            yield return null;
        }
    }

    private void removeCurrentlyHeldUnit()
    {
        if (heldUnit != null)
        {
            int prevUnitIndex = Array.IndexOf(placedUnits, heldUnit.GetComponent<PlayableUnit>(), 0);
            placedUnits[prevUnitIndex] = null;
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

    public void UsePlacementPoints(PlayableUnit unit)
    {
        placementPoints -= unit.GetUnitCost();
    }
}
