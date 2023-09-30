using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class GameManager : MonoBehaviour
{
    public GameObject heldUnit;

    [SerializeField] private GameObject[] unitPrefabs;
    private PlayableUnit[] placedUnits;

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
}
