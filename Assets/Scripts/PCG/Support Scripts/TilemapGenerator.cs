using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class TilemapGenerator : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private TileBase floor;

    [SerializeField]
    private List<TileBase> wallTiles;

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

        HashSet<Vector2Int> walls = WallGenerator.FindBasicWalls(map, Directions2D.CardinalDir);
        WallGenerator.GenerateBasicWalls(this, map, walls);

        walls = WallGenerator.FindBasicWalls(map, Directions2D.EightDir);
        WallGenerator.GenerateComplexWalls(this, map, walls);
    }

    public void GenerateBasicWall(Vector2Int wallPos, string binaryValue)
    {
        int typeAsInt = Convert.ToInt32(binaryValue, 2);
        TileBase tile = null;

        if (WallBitValues.TopWall.Contains(typeAsInt))
            tile = wallTiles[0];
        else if (WallBitValues.RightWall.Contains(typeAsInt))
            tile = wallTiles[1];
        else if (WallBitValues.BottomWall.Contains(typeAsInt))
            tile = wallTiles[2];
        else if (WallBitValues.LeftWall.Contains(typeAsInt))
            tile = wallTiles[3];

        PaintSingleTile(tile, wallPos);
    }

    public void GenerateComplexWall(Vector2Int wallPos, string binaryValue)
    {
        int typeAsInt = Convert.ToInt32(binaryValue, 2);
        TileBase tile = null;

        if (WallBitValues.LeftWall8Dir.Contains(typeAsInt))
            tile = wallTiles[1];
        else if (WallBitValues.RightWall8Dir.Contains(typeAsInt))
            tile = wallTiles[3];
        else if (WallBitValues.RightTopCorner.Contains(typeAsInt))
            tile = wallTiles[4];
        else if (WallBitValues.LeftTopCorner.Contains(typeAsInt))
            tile = wallTiles[5];
        else if (WallBitValues.RightBottomCorner.Contains(typeAsInt))
            tile = wallTiles[6];
        else if (WallBitValues.LeftBottomCorner.Contains(typeAsInt))
            tile = wallTiles[7];
        else if (WallBitValues.LCornerRight.Contains(typeAsInt))
            tile = wallTiles[8];
        else if (WallBitValues.LCornerLeft.Contains(typeAsInt))
            tile = wallTiles[9];
        else if (WallBitValues.SinglePeak.Contains(typeAsInt))
            tile = wallTiles[10];
        else if (WallBitValues.SinglePeakConnector.Contains(typeAsInt))
            tile = wallTiles[11];

        if (tile != null)
            PaintSingleTile(tile, wallPos);
    }

    private void PaintSingleTile(TileBase tile, Vector2Int pos)
    {
        Vector3Int worldPos = tilemap.WorldToCell((Vector3Int)pos);
        tilemap.SetTile(worldPos, tile);
    }
}
