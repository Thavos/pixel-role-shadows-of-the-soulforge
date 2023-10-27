using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Directions2D
{
    public static List<Vector2Int> CardinalDir = new List<Vector2Int> {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    public static List<Vector2Int> EightDir = new List<Vector2Int> {
        Vector2Int.up,
        new Vector2Int(1,1),  // up - right
        Vector2Int.right,
        new Vector2Int(1,-1), // right - down
        Vector2Int.down,
        new Vector2Int(-1,-1),// down - left
        Vector2Int.left,
        new Vector2Int(-1,1), // left - up
    };


    public static Vector2Int GetRandomCardinalDir(System.Random rand)
    {
        return CardinalDir[rand.Next(0, CardinalDir.Count)];
    }
}
