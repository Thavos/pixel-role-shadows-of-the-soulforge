using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    public static HashSet<Vector2Int> FindBasicWalls(HashSet<Vector2Int> map, List<Vector2Int> direction)
    {
        HashSet<Vector2Int> walls = new HashSet<Vector2Int>();

        foreach (var pos in map)
        {
            foreach (var dir in direction)
            {
                Vector2Int nextPos = pos + dir;
                if (map.Contains(nextPos) == false)
                    walls.Add(nextPos);
            }
        }

        return walls;
    }

    public static void GenerateBasicWalls(TilemapGenerator tilemapGen, HashSet<Vector2Int> map, HashSet<Vector2Int> walls)
    {
        foreach (Vector2Int pos in walls)
        {
            string binaryValue = "";

            foreach (var dir in Directions2D.CardinalDir)
            {
                var nextPos = pos + dir;
                if (map.Contains(nextPos))
                {
                    binaryValue += "1";
                }
                else
                {
                    binaryValue += "0";
                }
            }

            tilemapGen.GenerateBasicWall(pos, binaryValue);
        }
    }

    public static void GenerateComplexWalls(TilemapGenerator tilemapGen, HashSet<Vector2Int> map, HashSet<Vector2Int> walls)
    {
        foreach (Vector2Int pos in walls)
        {
            string binaryValue = "";

            foreach (var dir in Directions2D.EightDir)
            {
                var nextPos = pos + dir;
                if (map.Contains(nextPos))
                {
                    binaryValue += "1";
                }
                else
                {
                    binaryValue += "0";
                }
            }

            tilemapGen.GenerateComplexWall(pos, binaryValue);
        }
    }
}
