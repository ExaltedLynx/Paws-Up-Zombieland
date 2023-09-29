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

    public TileType GetTileType()
    {
        return type;
    }

    public void SetUnit(PlayableUnit unitToPlace)
    {
        if (placedUnit == null && unitToPlace.validTile == type)
        {
            placedUnit = unitToPlace;
        }
    }

    private void OnMouseDown()
    {
        Debug.Log(type);
    }

}
