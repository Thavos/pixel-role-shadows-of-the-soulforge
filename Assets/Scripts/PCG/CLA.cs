using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLA : AbstractGenerator
{
    [SerializeField]
    [Range(0.1f, 0.9f)] // TODO na 100 to buguje
    float coveragePercentage = 0.1f;

    [SerializeField]
    int iterattions;

    [SerializeField]
    Vector2Int mapSize;

    public override void Generate()
    {
        int[,] mapArray = GenerateNoiseMap();

        for (int i = 0; i < iterattions; i++)
        {
            mapArray = RunCLA(mapArray);
        }

        HashSet<Vector2Int> map = ConvertToMap(mapArray);
        HashSet<Vector2Int> walls = WallsGenerator.GenerateWalls(map);

        tilemapGen.ClearAll();
        tilemapGen.GenerateTilemap(map);
        tilemapGen.GenerateWalls(walls);
    }

    private HashSet<Vector2Int> ConvertToMap(int[,] mapArray)
    {
        HashSet<Vector2Int> map = new HashSet<Vector2Int>();

        for (int x = 0; x < mapArray.GetUpperBound(0); x++)
        {
            for (int y = 0; y < mapArray.GetUpperBound(1); y++)
            {
                if (mapArray[x, y] == 1)
                    map.Add(new Vector2Int(x, y));
            }
        }

        return map;
    }

    private int[,] RunCLA(int[,] mapArray)
    {
        int[,] newMapArray = new int[mapSize.x, mapSize.y];

        for (int x = 0; x < mapArray.GetUpperBound(0); x++)
        {
            for (int y = 0; y < mapArray.GetUpperBound(1); y++)
            {
                int neighbourCount = 0;
                foreach (var dir in Directions2D.EightDir)
                {
                    int xPos = x + dir.x;
                    int yPos = y + dir.y;

                    if (xPos < 0 || xPos > mapArray.GetUpperBound(0) || yPos < 0 || yPos > mapArray.GetUpperBound(1))
                        continue;

                    else if (mapArray[xPos, yPos] == 1)
                        neighbourCount++;
                }

                if (neighbourCount > 4)
                    newMapArray[x, y] = 1;
                else
                    newMapArray[x, y] = 0;
            }
        }

        return newMapArray;
    }

    public int[,] GenerateNoiseMap()
    {
        int[,] map = new int[mapSize.x, mapSize.y];
        int cellsCount = Mathf.RoundToInt(mapSize.x * mapSize.y * coveragePercentage);

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                map[x, y] = 0;
            }
        }

        for (int i = 0; i < cellsCount; i++)
        {
            int x = rand.Next(0, map.GetUpperBound(0));
            int y = rand.Next(0, map.GetUpperBound(1));

            if (map[x, y] == 1)
                i--;
            else
                map[x, y] = 1;
        }

        return map;
    }
}
