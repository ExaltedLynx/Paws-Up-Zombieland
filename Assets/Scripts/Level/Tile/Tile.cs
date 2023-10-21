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

    [SerializeField] private TileType type;
    private PlayableUnit placedUnit;
    private GameObject placedUnitObject;
    private SpriteRenderer highlighter;

    private void Awake()
    {
        highlighter = GameObject.FindGameObjectWithTag("Highlighter").GetComponent<SpriteRenderer>();
        highlighter.enabled = false;
    }

    public TileType GetTileType()
    {
        return type;
    }

    private bool SetUnit(PlayableUnit unitToPlace)
    {
        bool placed = false;
        if (placedUnit == null && unitToPlace.GetValidTile() == type && unitToPlace.GetState() == PlayableUnit.UnitState.NotPlaced)
        {
            placedUnit = unitToPlace;
            placed = true;
        }
        return placed;
    }

    public void removeUnit()
    {
        placedUnitObject.transform.parent = GameManager.Instance.transform;
        placedUnit = null;
        placedUnitObject = null;
    }

    private void OnMouseDown()
    {
        if(GameManager.Instance.heldUnit != null)
        {
            if(SetUnit(GameManager.Instance.heldUnit.GetComponent<PlayableUnit>()))
            {
                placedUnitObject = GameManager.Instance.heldUnit;
                GameManager.Instance.heldUnit = null;
                GameManager.Instance.UsePlacementPoints(placedUnit);
                placedUnit.transform.position = transform.position;
                placedUnit.transform.parent = transform;
                placedUnit.tilePlacedOn = this;
                placedUnit.SetState(PlayableUnit.UnitState.Idle);
                placedUnit.ToggleRangeVisibility();
            }
        }
    }

    private void OnMouseEnter()
    {
        HighlightTile();
    }

    private void OnMouseExit()
    {
        highlighter.enabled = false;
    }

    private void HighlightTile()
    {
        if(GameManager.Instance.heldUnit != null)
        {
            if (GameManager.Instance.heldUnit.GetComponent<PlayableUnit>().GetValidTile() == type)
            {
                highlighter.transform.position = transform.position; //moves highlighter gameobject on top of the respective tile
                highlighter.enabled = true;
            }
        }
    }
}
