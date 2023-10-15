using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int playerMaxHealth;
    [SerializeField] public int playerHealth;
    [SerializeField] private GameObject[] unitPrefabs;

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
        playerHealth = playerMaxHealth;
        placedUnits = new PlayableUnit[unitPrefabs.Length];
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

    }

    public void damagePlayer()
    {
        playerHealth -= 1;
    }

    public void SetHeldUnit(int index)
    {
        if (placedUnits[index] == null)
        {
            removePreviousHeldUnit();
            heldUnit = Instantiate(unitPrefabs[index], transform);
             placedUnits[index] = heldUnit.GetComponent<PlayableUnit>();
            
        }
        else if (placedUnits[index].GetState() != PlayableUnit.UnitState.NotPlaced)
        {
            removePreviousHeldUnit();
            PlayableUnit unit = placedUnits[index];
            unit.tilePlacedOn.removeUnit();
            unit.ToggleRangeVisibility();
            heldUnit = unit.gameObject;
            unit.SetState(PlayableUnit.UnitState.NotPlaced);
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

    private void removePreviousHeldUnit()
    {
        if (heldUnit != null)
        {
            int prevUnitIndex = Array.IndexOf(placedUnits, heldUnit.GetComponent<PlayableUnit>(), 0);
            placedUnits[prevUnitIndex] = null;
            Destroy(heldUnit);
            heldUnit = null;
        }
    }    
}
