using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType
    {
        Null = -1,
        Ground = 0,
        Elevated = 1,
    }

    [SerializeField] TileType type;
    private PlayableUnit placedUnit;
    private GameObject placedUnitObject;

    public TileType GetTileType()
    {
        return type;
    }

    public bool SetUnit(PlayableUnit unitToPlace)
    {
        bool placed = false;
        if (placedUnit == null && unitToPlace.GetValidTile() == type && unitToPlace.GetState() == PlayableUnit.UnitState.NotPlaced)
        {
            Debug.Log(unitToPlace);
            placedUnit = unitToPlace;
            placed = true;
        }
        return placed;
    }

    private void OnMouseOver()
    {
        if(GameManager.Instance.heldUnit != null && Input.GetMouseButtonDown(0))
        {
            if(SetUnit(GameManager.Instance.heldUnit.GetComponent<PlayableUnit>()))
            {
                placedUnitObject = GameManager.Instance.heldUnit;
                GameManager.Instance.heldUnit = null;
                placedUnit.transform.position = transform.position;
                placedUnit.transform.parent = transform;
                placedUnit.SetState(PlayableUnit.UnitState.Idle);
            }
        }
    }

}
