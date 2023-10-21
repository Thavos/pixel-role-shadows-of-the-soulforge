using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    [SerializeField]
    Tilemap tilemap;

    [SerializeField]
    TileBase floor, wall;

    public void ClearAll()
    {
        tilemap.ClearAllTiles();
    }

    public void GenerateTilemap(HashSet<Vector2Int> map)
    {
        foreach (Vector2Int pos in map)
        {
            Vector3Int worldPos = tilemap.WorldToCell((Vector3Int)pos);
            tilemap.SetTile(worldPos, floor);
        }
    }

    public void GenerateWalls(HashSet<Vector2Int> walls)
    {
        foreach (Vector2Int pos in walls)
        {
            Vector3Int worldPos = tilemap.WorldToCell((Vector3Int)pos);
            tilemap.SetTile(worldPos, wall);
        }
    }
}
