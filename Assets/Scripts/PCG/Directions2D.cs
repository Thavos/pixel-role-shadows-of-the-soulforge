using System.Collections.Generic;
using UnityEngine;

public static class Directions2D
{
    public static List<Vector2Int> CardinalDir = new List<Vector2Int> {
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.up
    };

    public static List<Vector2Int> EightDir = new List<Vector2Int> {
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.up,
        new Vector2Int(1,1),  // up - right
        new Vector2Int(-1,1), // up - left
        new Vector2Int(1,-1), // down - right
        new Vector2Int(-1,-1),// down - left
    };


    public static Vector2Int GetRandomCardinalDir(System.Random rand)
    {
        return CardinalDir[rand.Next(0, CardinalDir.Count)];
    }
}
