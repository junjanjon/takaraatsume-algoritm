using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var _tilemap = GetComponent<Tilemap>();

        foreach (var tile in _tilemap.GetTilesBlock(new BoundsInt(Vector3Int.zero, Vector3Int.one * 100)))
        {
            Debug.Log(tile);
        }
    }


    class MyTile : TileBase
    {
        
    }
}
