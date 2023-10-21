using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallsGenerator
{
    public static HashSet<Vector2Int> GenerateWalls(HashSet<Vector2Int> map)
    {
        HashSet<Vector2Int> walls = new HashSet<Vector2Int>();

        foreach (var pos in map)
        {
            foreach (var dir in Directions2D.EightDir)
            {
                Vector2Int nextPos = pos + dir;
                if (map.Contains(nextPos) == false)
                    walls.Add(nextPos);
            }
        }

        return walls;
    }
}
