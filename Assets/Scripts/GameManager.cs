using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject heldUnit;

    [SerializeField] private GameObject[] unitPrefabs;
    private PlayableUnit[] placedUnits;
    int sceneIndex = 0;

    public static GameManager Instance
    {
        get => instance;
    }

    private static GameManager instance;

    private void Awake()
    {
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

    }

    public void SetHeldUnit(int index)
    {
        if (placedUnits[index] == null)
        {
            heldUnit = Instantiate(unitPrefabs[index], transform);
            placedUnits[index] = heldUnit.GetComponent<PlayableUnit>();
        }
        else if (placedUnits[index].GetState() != PlayableUnit.UnitState.NotPlaced)
        {
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
}
