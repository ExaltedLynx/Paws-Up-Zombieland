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
    //add a field to store the unit on the tile once they are implemented
    //private PlayableUnit unit;

    public TileType GetTileType()
    {
        return type;
    }

    public void SetUnit()
    {

    }

    private void OnMouseDown()
    {
        Debug.Log(type);
    }

}
