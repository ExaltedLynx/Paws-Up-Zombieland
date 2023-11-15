using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.heldUnit != null && eventData.button == PointerEventData.InputButton.Left)
        {
            if (SetUnit(GameManager.Instance.heldUnit.GetComponent<PlayableUnit>()))
            {
                placedUnitObject = GameManager.Instance.heldUnit;
                GameManager.Instance.heldUnit = null;
                GameManager.Instance.UsePlacementPoints(placedUnit);
                placedUnit.transform.parent = transform;
                placedUnit.transform.position = transform.position;
                placedUnit.tilePlacedOn = this;
                if (placedUnit is HealerUnit) //easy way to avoid rewriting UnitsInRange() to also work when a healer unit is placed
                    placedUnit.SetState(PlayableUnit.UnitState.Acting);
                else
                    placedUnit.SetState(PlayableUnit.UnitState.Idle);

                placedUnit.ToggleRangeVisibility();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HighlightTile();
    }

    public void OnPointerExit(PointerEventData eventData)
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
