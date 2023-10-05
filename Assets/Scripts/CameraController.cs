using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera levelCamera;
    [SerializeField] private Tilemap levelTilemap;

    void Start()
    {
        //correctly fits the tilemap bounding box if the tiles that were changed should have made it smaller instead of bigger
        levelTilemap.CompressBounds(); 
        levelCamera.transform.position = new Vector3(levelTilemap.cellBounds.center.x, levelTilemap.cellBounds.center.y, -10);
        if(levelCamera.orthographicSize < levelTilemap.size.y / 2f)
            levelCamera.orthographicSize = (levelTilemap.size.y / 2f) + 1; // the +1 adds space for UI elements above or below the level itself
    }
}
